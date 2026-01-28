/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors
 * Portions Copyright (C) 2015-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2023-6-21
 */
using Microsoft.CSharp;
using SanteDB.Core.Exceptions;
using SanteDB.Core.i18n;
using SanteDB.Core.Model.Map.Builder;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SanteDB.Core.Model.Map
{
    /// <summary>
    /// Model mapper
    /// </summary>
    public sealed class ModelMapper
    {
        private ConcurrentDictionary<Type, IModelMapper> m_mappers = new ConcurrentDictionary<Type, Builder.IModelMapper>();
        private static ConcurrentDictionary<String, Assembly> m_loadedMaps = new ConcurrentDictionary<string, Assembly>();

        /// <summary>
        /// When true, disables code generation for all model mapping activities. This value overrides the per-map setting.
        /// </summary>
        public static bool UseReflectionOnly { get; set; } = false;

        //private static Dictionary<Type, Dictionary<String, PropertyInfo[]>> s_modelPropertyCache = new Dictionary<Type, Dictionary<String, PropertyInfo[]>>();

        //private static Dictionary<Type, String> m_domainClassPropertyName = new Dictionary<Type, string>();

        /// <summary>
        /// Maps a model property at a root level only
        /// </summary>
        public PropertyInfo MapModelProperty(Type tmodel, PropertyInfo propertyInfo)
        {
            return this.MapModelProperty(tmodel, null, propertyInfo);
        }

        /// <summary>
        /// Maps a model property at a root level only
        /// </summary>
        public PropertyInfo MapModelProperty(Type tmodel, Type tdomain, PropertyInfo propertyInfo)
        {
            var classMap = this.m_mapFile.GetModelClassMap(tmodel, tdomain);
            tdomain = tdomain ?? classMap?.DomainType;
            PropertyMap propMap = null;
            if (classMap?.TryGetModelProperty(propertyInfo.Name, out propMap) == true)
            {
                return tdomain?.GetRuntimeProperty(propMap.DomainName);
            }
            else
            {
                return tdomain?.GetRuntimeProperty(propertyInfo.Name);
            }
        }

        // The map file
        private ModelMap m_mapFile;


        /// <summary>
        /// Creates a new mapper from source stream
        /// </summary>
        public ModelMapper(Stream sourceStream, String name, Assembly preGeneratedAssembly = null, bool useReflectionOnly = false)
        {
            this.Load(sourceStream, name, useReflectionOnly || UseReflectionOnly, preGeneratedAssembly);
        }

        /// <summary>
        /// Load mapping from a stream
        /// </summary>
        private void Load(Stream sourceStream, String name, bool useReflectionOnly, Assembly loadedAssembly = null)
        {
            this.m_mapFile = ModelMap.Load(sourceStream);
            if (loadedAssembly != null || m_loadedMaps.TryGetValue(name, out loadedAssembly))
            {
                this.ProcessAssembly(loadedAssembly);
            }
            else
            {
                if (!useReflectionOnly)
                {
                    try
                    {
                        var csProvider = new CSharpCodeProvider();
                        var compileUnit = new CodeCompileUnit();

                        // Add namespace
                        compileUnit.Namespaces.Add(new Builder.ModelMapBuilder().CreateCodeNamespace(this.m_mapFile, name));

                        var referencedassemblies = new List<string>();

                        referencedassemblies.Add(typeof(Type).Assembly.Location);
                        referencedassemblies.Add(typeof(HashSet<>).Assembly.Location);
                        referencedassemblies.AddRange(this.m_mapFile.Class.Select(o => o.ModelType.Assembly.Location).Distinct().ToArray());
                        referencedassemblies.AddRange(this.m_mapFile.Class.Select(o => o.DomainType.Assembly.Location).Distinct().ToArray());

                        compileUnit.ReferencedAssemblies.AddRange(referencedassemblies.Distinct().ToArray());

                        foreach (var casm in compileUnit.ReferencedAssemblies.OfType<String>().ToArray().Select(o => Assembly.LoadFile(o)).SelectMany(o => o.GetReferencedAssemblies()))
                        {
                            var asmC = Assembly.Load(casm);
                            compileUnit.ReferencedAssemblies.Add(asmC.Location);
                        }

                        // Compiler parameters
                        var compileParameters = new CompilerParameters();
                        compileParameters.ReferencedAssemblies.AddRange(compileUnit.ReferencedAssemblies.OfType<String>().ToArray());
                        compileParameters.GenerateInMemory = true;
                        compileParameters.GenerateExecutable = false;

#if DEBUG
                        using (var sw = new StringWriter())
                        {
                            csProvider.GenerateCodeFromCompileUnit(compileUnit, sw, new CodeGeneratorOptions() { });
                            String codeReview = sw.ToString();
                        }
#endif
                        var results = csProvider.CompileAssemblyFromDom(compileParameters, compileUnit);

                        if (results.Errors.HasErrors)
                        {
                            throw new ModelMapCompileException(results.Errors);
                        }

                        this.ProcessAssembly(results.CompiledAssembly);
                        m_loadedMaps.TryAdd(name, results.CompiledAssembly);
                    }
                    catch (ModelMapCompileException e)
                    {
                        throw new Exception($"Could not compile model map {name}", e);
                    }
                    catch (ModelMapValidationException e)
                    {
                        throw new Exception($"Could not validate model map {name}", e);
                    }
                    catch (Exception)
                    {
                        this.InitializeReflection(this.m_mapFile);
                    }
                }
                else
                {
                    this.InitializeReflection(this.m_mapFile);
                }
            }
        }

        /// <summary>
        /// Creates a reflection map only for the map file
        /// </summary>
        private void InitializeReflection(ModelMap mapFile)
        {
            foreach (var map in mapFile.Class)
            {
                var mapper = new ReflectionModelMapper(map, this);
                this.m_mappers.TryAdd(map.DomainType, mapper);
                this.m_mappers.TryAdd(map.ModelType, mapper);
            }
        }

        /// <summary>
        /// Process the provided assembly for mappers
        /// </summary>
        private void ProcessAssembly(Assembly asm)
        {
            // Load the types
            foreach (var t in asm.GetExportedTypesSafe().Where(t => typeof(IModelMapper).IsAssignableFrom(t)))
            {
                var map = Activator.CreateInstance(t, this) as IModelMapper;
                this.m_mappers.TryAdd(map.SourceType, map);
                this.m_mappers.TryAdd(map.TargetType, map);
            }
        }

        /// <summary>
        /// Gets a model mapper that can handle the specified type
        /// </summary>
        public IModelMapper GetModelMapper(Type forType)
        {
            if (this.m_mappers.TryGetValue(forType, out IModelMapper retVal))
            {
                return retVal;
            }

            return null;
        }

        /// <summary>
        /// Add a class map definition <paramref name="map"/> to this model mapper context
        /// </summary>
        /// <param name="map">The map to be added</param>
        public void AddClassMap(ClassMap map)
        {
            this.m_mapFile.Class.Add(map);
            var mapper = new ReflectionModelMapper(map, this);
            this.m_mappers.TryAdd(map.DomainType, mapper);
            this.m_mappers.TryAdd(map.ModelType, mapper);
        }
        /// <summary>
        /// Maps a cast to appropriate path
        /// </summary>
        public Expression MapTypeCast(UnaryExpression sourceExpression, Expression accessExpression)
        {
            // First we find the map for the specified type
            ClassMap classMap = this.m_mapFile.GetModelClassMap(sourceExpression.Operand.Type);

            PropertyMap castMap = classMap.Cast?.Find(o => o.ModelType == sourceExpression.Type);
            if (castMap == null)
            {
                throw new InvalidCastException();
            }

            Expression accessExpr = Expression.MakeMemberAccess(accessExpression, accessExpression.Type.GetRuntimeProperty(castMap.DomainName));

            return accessExpr;
        }

        /// <summary>
        /// Map member
        /// </summary>
        public Expression MapModelMember(MemberExpression memberExpression, Expression accessExpression, Type modelType = null)
        {
            if (accessExpression.Type.StripNullable() == (modelType ?? memberExpression.Expression.Type).StripNullable())
            {
                return accessExpression;
            }

            ClassMap classMap = this.m_mapFile.GetModelClassMap(modelType ?? memberExpression.Expression.Type.StripNullable());

            if (classMap == null)
            {
                // Special case for interface
                if (memberExpression.Expression.Type.IsInterface)
                {
                    classMap = new ClassMap()
                    {
                        DomainClass = accessExpression.Type.AssemblyQualifiedName,
                        ModelClass = memberExpression.Expression.Type.AssemblyQualifiedName
                    };
                }
                else
                {
                    // Is there a different map we could use
                    classMap = this.m_mapFile.Class.FirstOrDefault(o => o.DomainType == accessExpression.Type);
                    if (classMap == null || !classMap.ModelType.IsAssignableFrom(modelType ?? memberExpression.Expression.Type))
                    {
                        throw new InvalidOperationException(string.Format(ErrorMessages.MAP_NOT_FOUND, modelType ?? memberExpression.Type, accessExpression.Type));
                    }
                }
            }

            // Expression is the same class? Collapse if it is a key
            MemberExpression accessExpressionAsMember = accessExpression as MemberExpression;
            CollapseKey collapseKey = null;
            PropertyMap propertyMap = null;

            if (memberExpression.Member.Name == "Key" && classMap.TryGetCollapseKey(accessExpressionAsMember?.Member.Name, out collapseKey))
            {
                return Expression.MakeMemberAccess(accessExpressionAsMember.Expression, accessExpressionAsMember.Expression.Type.GetRuntimeProperty(collapseKey.KeyName));
            }
            else if (classMap.TryGetModelProperty(memberExpression.Member.Name, out propertyMap))
            {
                return Expression.MakeMemberAccess(accessExpression, this.ExtractDomainType(accessExpression.Type).GetRuntimeProperty(propertyMap.DomainName));
            }
            else
            {
                // look for idenical named property
                Type domainType = this.MapModelType(modelType ?? memberExpression.Expression.Type);

                // Get domain member and map
                MemberInfo domainMember = accessExpression.Type.GetRuntimeProperty(memberExpression.Member.Name);
                if (domainMember != null)
                {
                    return Expression.MakeMemberAccess(accessExpression, domainMember);
                }
                else if (accessExpression is ParameterExpression pe)
                {
                    // Try on the base?
                    var baseType = modelType?.BaseType ?? memberExpression.Expression.Type.BaseType;
                    var scanType = this.MapModelType(baseType);
                    if (baseType == scanType) // No mapping
                    {
                        return null;
                    }
                    else
                    {  // try to map
                        return MapModelMember(memberExpression, Expression.Parameter(scanType, "o"), baseType);
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Extracts a domain type from a generic if needed
        /// </summary>
        public Type ExtractDomainType(Type domainType)
        {
            if (!domainType.IsConstructedGenericType)
            {
                return domainType;
            }
            else if (domainType.GenericTypeArguments.Length == 1)
            {
                return this.ExtractDomainType(domainType.GenericTypeArguments[0]);
            }
            else
            {
                throw new InvalidOperationException("Cannot determine domain model type");
            }
        }

        /// <summary>
        /// Gets the domain type for the specified model type
        /// </summary>
        public Type MapModelType(Type modelType)
        {
            // Just a value type - don't look for a map
            if (modelType?.BaseType == typeof(ValueType) || modelType == null)
            {
                return modelType;
            }

            ClassMap classMap = this.m_mapFile.GetModelClassMap(modelType);
            // No class mapping so go up the tree
            if (modelType.BaseType == null)
            {
                return null;
            }
            else if (classMap == null && modelType.BaseType != typeof(Object))
            {
                return MapModelType(modelType.BaseType);
            }
            else if (classMap == null)
            {
                throw new InvalidOperationException($"Cannot map {modelType.FullName} to a domain type");
            }

            Type domainType = classMap.DomainType;
            if (domainType == null)
            {
                throw new InvalidOperationException(String.Format("Cannot find class {0}", classMap.DomainClass));
            }

            return domainType;
        }

        /// <summary>
        /// Gets the model type for the specified domain type
        /// </summary>
        public Type MapDomainType(Type domainType)
        {
            ClassMap classMap = this.m_mapFile.Class.FirstOrDefault(o => o.DomainType == domainType);
            if (classMap == null)
            {
                return domainType;
            }

            Type modelType = classMap.ModelType;
            if (domainType == null)
            {
                throw new InvalidOperationException(String.Format("Cannot find class {0}", classMap.DomainClass));
            }

            return modelType;
        }

        /// <summary>
        /// Create a traversal expression for a lambda expression
        /// </summary>
        public Expression CreateLambdaMemberAdjustmentExpression(Expression rootExpression, ParameterExpression lambdaParameterExpression)
        {
            if (rootExpression is MemberExpression) // Property map based re-write
            {
                var propertyExpression = rootExpression as MemberExpression;
                ClassMap classMap = this.m_mapFile.GetModelClassMap(this.ExtractDomainType(propertyExpression.Expression.Type));

                if (classMap == null)
                {
                    return lambdaParameterExpression;
                }

                // Expression is the same class? Collapse if it is a key
                PropertyMap propertyMap = null;
                while (propertyMap == null && classMap != null)
                {
                    classMap.TryGetModelProperty(propertyExpression.Member.Name, out propertyMap);
                    if (propertyMap == null)
                    {
                        classMap = this.m_mapFile.GetModelClassMap(classMap.ModelType.BaseType);
                        //                    var tDomain = rootExpression.Expression.Type.GetRuntimeProperty(classMap.ParentDomainProperty.DomainName);
                    }
                }


                return lambdaParameterExpression;
            }
            else
            {
                return lambdaParameterExpression;
            }
        }

        /// <summary>
        /// Automap the model expression using the configured mapper type
        /// </summary>
        public LambdaExpression MapModelExpression<TFrom, TReturn>(Expression<Func<TFrom, TReturn>> expression, Type toType, bool throwOnError = true)
        {
            try
            {
                var parameter = Expression.Parameter(toType, expression.Parameters[0].Name);

                Expression expr = new ModelExpressionVisitor(this, toType, parameter).Visit(expression.Body);
                if (expr == null && throwOnError)
                {
                    throw new ArgumentException(ErrorMessages.MAP_EXPRESSION_NOT_POSSIBLE);
                }
                else if (expr == null)
                {
                    return null;
                }
                else
                {
                    if (typeof(TReturn) != expr.Type)
                    {
                        expr = Expression.Convert(expr, typeof(TReturn));
                    }

                    var retVal = Expression.Lambda(expr, parameter);
#if VERBOSE_DEBUG
                Debug.WriteLine("Map Expression: {0} > {1}", expression, retVal);
#endif
                    return retVal;
                }
            }
            catch
            {
#if VERBOSE_DEBUG
                Debug.WriteLine("Error converting {0}. {1}", expression, e);
#endif
                if (throwOnError)
                {
                    throw;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert the specified lambda expression from model into query
        /// </summary>
        /// <param name="expression">The expression to be converted</param>
        /// <param name="throwOnError">When true, throw an exception of the expression can't be converted, otherwise return null when expression cannot be parsed</param>
        public Expression<Func<TTo, TReturn>> MapModelExpression<TFrom, TTo, TReturn>(Expression<Func<TFrom, TReturn>> expression, bool throwOnError = true) => (Expression<Func<TTo, TReturn>>)this.MapModelExpression(expression, typeof(TTo), throwOnError);

        /// <summary>
        /// Map model instance
        /// </summary>
        public object MapModelInstance<TModel>(TModel modelInstance)
            where TModel : new()
        {
            if (this.m_mappers.TryGetValue(modelInstance.GetType(), out IModelMapper modelMapper))
            {
                return modelMapper.MapToTarget(modelInstance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Map model instance
        /// </summary>
        public TDomain MapModelInstance<TModel, TDomain>(TModel modelInstance)
            where TDomain : new()
            where TModel : new()
        {
            if (modelInstance == null)
            {
                return default(TDomain);
            }

            // Can we find a map between the type and another?
            if (this.m_mappers.TryGetValue(typeof(TModel), out IModelMapper modelMapper) && modelMapper is IModelMapper<TModel, TDomain> preferredMapper)
            {
                return preferredMapper.MapToTarget(modelInstance);
            }
            else if (this.m_mappers.TryGetValue(typeof(TDomain), out modelMapper) ||
                this.m_mappers.TryGetValue(modelInstance.GetType(), out modelMapper))
            {
                if (modelMapper is IModelMapper<TModel, TDomain> smodelMapper)
                {
                    return smodelMapper.MapToTarget(modelInstance);
                }
                else
                {
                    return (TDomain)modelMapper.MapToTarget(modelInstance);
                }
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.MAP_NOT_FOUND, typeof(TModel), typeof(TDomain)));
            }
            /*
            ClassMap classMap = this.m_mapFile.GetModelClassMap(typeof(TModel), typeof(TDomain));
            if (classMap == null)
                classMap = this.m_mapFile.GetModelClassMap(typeof(TModel));

            if (classMap == null || modelInstance == null)
                return default(TDomain);

            // Now the property maps
            TDomain retVal = new TDomain();

            // Properties
            PropertyInfo[] properties = null;
            Dictionary<String, PropertyInfo[]> propertyClassMap = null;
            if (!s_modelPropertyCache.TryGetValue(typeof(TModel), out propertyClassMap))
            {
                lock (s_modelPropertyCache)
                {
                    if (!s_modelPropertyCache.TryGetValue(typeof(TModel), out propertyClassMap))
                        propertyClassMap = new Dictionary<string, PropertyInfo[]>();
                    if (!s_modelPropertyCache.ContainsKey(typeof(TModel)))
                        s_modelPropertyCache.Add(typeof(TModel), propertyClassMap);
                }
            }

            if (!propertyClassMap.TryGetValue(String.Empty, out properties))
            {
                lock (s_modelPropertyCache)
                {
                    properties = typeof(TModel).GetRuntimeProperties().Where(m => m != null &&
                    m.GetCustomAttribute<DataIgnoreAttribute>() == null &&
                    (primitives.Contains(m.PropertyType) || m.PropertyType.IsEnum) &&
                    m.CanWrite).ToArray();
                    if (!propertyClassMap.ContainsKey(String.Empty))
                        propertyClassMap.Add(String.Empty, properties);
                }
            }

            // Iterate through properties
            foreach (var propInfo in properties)
            {
                var propValue = propInfo.GetValue(modelInstance);
                // Property info
                if (propValue == null)
                    continue;

                if (!propInfo.PropertyType.IsPrimitive && propInfo.PropertyType != typeof(Guid) &&
                    (!propInfo.PropertyType.IsGenericType || propInfo.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) &&
                    propInfo.PropertyType != typeof(String) &&
                    propInfo.PropertyType != typeof(DateTime) &&
                    propInfo.PropertyType != typeof(DateTimeOffset) &&
                    propInfo.PropertyType != typeof(Type) &&
                    propInfo.PropertyType != typeof(Decimal) &&
                    propInfo.PropertyType != typeof(byte[]) &&
                    !propInfo.PropertyType.IsEnum)
                    continue;

                // Map property
                PropertyMap propMap = null;
                classMap.TryGetModelProperty(propInfo.Name, out propMap);
                PropertyInfo domainProperty = null;
                Object targetObject = retVal;

                // Set
                if (propMap == null)
                    domainProperty = typeof(TDomain).GetRuntimeProperty(propInfo.Name);
                else
                    domainProperty = typeof(TDomain).GetRuntimeProperty(propMap.DomainName);

                object domainValue = null;
                // Set value
                if (domainProperty == null || !domainProperty.CanWrite)
                    continue;
                //Debug.WriteLine ("Unmapped property ({0}).{1}", typeof(TModel).Name, propInfo.Name);
                else if (domainProperty.PropertyType == typeof(byte[]) && propInfo.PropertyType.StripNullable() == typeof(Guid))
                    domainProperty.SetValue(targetObject, ((Guid)propValue).ToByteArray());
                else if (
                    (domainProperty.PropertyType == typeof(DateTime) || domainProperty.PropertyType == typeof(DateTime?))
                    && (propInfo.PropertyType == typeof(DateTimeOffset) || propInfo.PropertyType == typeof(DateTimeOffset?)))
                {
                    domainProperty.SetValue(targetObject, ((DateTimeOffset)propValue).DateTime);
                }
                else if (domainProperty.PropertyType.IsAssignableFrom(propInfo.PropertyType))
                    domainProperty.SetValue(targetObject, propValue);
                else if (propInfo.PropertyType == typeof(Type) && domainProperty.PropertyType == typeof(String))
                    domainProperty.SetValue(targetObject, (propValue as Type).AssemblyQualifiedName);
                else if (MapUtil.TryConvert(propValue, domainProperty.PropertyType, out domainValue))
                    domainProperty.SetValue(targetObject, domainValue);
            }

            return retVal;
            */
        }

        /// <summary>
        /// Map the specified domain instance
        /// </summary>
        public TModel MapDomainInstance<TDomain, TModel>(TDomain domainInstance)
            where TModel : new()
            where TDomain : new()
        {
            if (domainInstance == null)
            {
                return default(TModel);
            }

            if (this.m_mappers.TryGetValue(typeof(TDomain), out IModelMapper modelMapper) && modelMapper is IModelMapper<TModel, TDomain> preferredMapper)
            {
                return preferredMapper.MapToSource(domainInstance);
            }
            else if (this.m_mappers.TryGetValue(typeof(TModel), out modelMapper) ||
                this.m_mappers.TryGetValue(domainInstance.GetType(), out modelMapper)
                )
            {
                if (modelMapper is IModelMapper<TModel, TDomain> smodelMapper)
                {
                    return smodelMapper.MapToSource(domainInstance);
                }
                else
                {
                    return (TModel)modelMapper.MapToSource(domainInstance);
                }
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.MAP_NOT_FOUND, typeof(TModel), typeof(TDomain)));
            }
        }

        /// <summary>
        /// Map domain instance
        /// </summary>
        public object MapDomainInstance(Type tDomain, Type tModel, object domainInstance)
        {
            if (this.m_mappers.TryGetValue(tDomain, out IModelMapper modelMapper) ||
                this.m_mappers.TryGetValue(tModel, out modelMapper))
            {
                return modelMapper.MapToSource(domainInstance);
            }
            else
            {
                return null;
            }
            /*
            ClassMap classMap = this.m_mapFile.GetModelClassMap(tModel, tDomain);

            if (domainInstance == null)
                return null;
            else
            {
                var cType = tModel;
                while (cType != null && classMap == null || !tDomain.IsAssignableFrom(Type.GetType(classMap.DomainClass)))
                {
                    cType = cType.BaseType;
                    if (cType != null)
                        classMap = this.m_mapFile.GetModelClassMap(cType);
                } // work up the tree
            }

            // Now the property maps
            object retVal = Activator.CreateInstance(tModel);

            // Key?
            if (classMap == null)
                return retVal;

            // Cache lookup
            var idEnt = retVal as IIdentifiedEntity;
            var vidEnt = retVal as IVersionedEntity;

            PropertyMap iKeyMap = null;
            if (idEnt != null)
                classMap.TryGetModelProperty("Key", out iKeyMap);
            if (iKeyMap != null)
            {
                object keyValue = tDomain.GetRuntimeProperty(iKeyMap.DomainName).GetValue(domainInstance);
                while (iKeyMap.Via != null)
                {
                    keyValue = keyValue.GetType().GetRuntimeProperty(iKeyMap.Via.DomainName).GetValue(keyValue);
                    iKeyMap = iKeyMap.Via;
                }
                if (keyValue is byte[])
                    keyValue = new Guid(keyValue as byte[]);

                // Set key vaue
                idEnt.Key = (Guid)keyValue;

                var cache = FireMappingToModel(this, (Guid)keyValue, retVal as IdentifiedData);
                if (cache != null && useCache)
                    return cache;
            }

            // Classifier value
            String classifierValue = null;
            String classPropertyName = String.Empty;
            if (!m_domainClassPropertyName.TryGetValue(tModel, out classPropertyName))
            {
                classPropertyName = tModel.GetCustomAttribute<ClassifierAttribute>()?.ClassifierProperty;
                lock (m_domainClassPropertyName)
                    if (!m_domainClassPropertyName.ContainsKey(tModel))
                        m_domainClassPropertyName.Add(tModel, classPropertyName);
            }

            if (classPropertyName != null)
            {
                // Key value
                classPropertyName = tModel.GetRuntimeProperty(classPropertyName)?.GetCustomAttribute<SerializationReferenceAttribute>()?.RedirectProperty ?? classPropertyName;

                if (classMap.TryGetModelProperty(classPropertyName ?? "____XXX", out iKeyMap))
                {
                    object keyValue = tDomain.GetRuntimeProperty(iKeyMap.DomainName).GetValue(domainInstance);
                    while (iKeyMap.Via != null)
                    {
                        keyValue = keyValue.GetType().GetRuntimeProperty(iKeyMap.Via.DomainName).GetValue(keyValue);
                        iKeyMap = iKeyMap.Via;
                    }
                    classifierValue = keyValue?.ToString();
                }
            }

            // Are we currently processing this?
            if (idEnt != null)
            {
                if (keyStack == null)
                    keyStack = new HashSet<Guid>();
                if (idEnt.Key.HasValue)
                {
                    if (keyStack.Contains(idEnt.Key.Value))
                        return null;
                    else
                        keyStack.Add(idEnt.Key.Value);
                }
            }

            // Properties
            // Properties
            PropertyInfo[] properties = null;
            Dictionary<String, PropertyInfo[]> propertyClassMap = null;
            if (!s_modelPropertyCache.TryGetValue(tModel, out propertyClassMap))
            {
                lock (s_modelPropertyCache)
                {
                    if (!s_modelPropertyCache.TryGetValue(tModel, out propertyClassMap))
                        propertyClassMap = new Dictionary<string, PropertyInfo[]>();

                    if (!s_modelPropertyCache.ContainsKey(tModel))
                    {
                        s_modelPropertyCache.Add(tModel, propertyClassMap);
                    }
                }
            }
            if (!propertyClassMap.TryGetValue(classifierValue ?? String.Empty, out properties))
            {
                lock (s_modelPropertyCache)
                {
                    properties = tModel.GetRuntimeProperties().Where(m => m != null &&
                    m.GetCustomAttribute<DataIgnoreAttribute>() == null &&
                    (primitives.Contains(m.PropertyType) || m.PropertyType.IsEnum ||
                    m.GetCustomAttributes<AutoLoadAttribute>().Any(o => o.ClassCode == classifierValue || o.ClassCode == null)) &&
                    m.CanWrite).ToArray();
                    if (!propertyClassMap.ContainsKey(classifierValue ?? String.Empty))
                        propertyClassMap.Add(classifierValue ?? String.Empty, properties);
                }
            }

            // Iterate the properties and map
            foreach (var modelPropertyInfo in properties)
            {
                // Map property
                PropertyMap propMap = null;
                classMap.TryGetModelProperty(modelPropertyInfo.Name, out propMap);

                if (propMap?.DontLoad == true)
                    continue;
                var propInfo = tDomain.GetRuntimeProperty(propMap?.DomainName ?? modelPropertyInfo.Name);
                if (propInfo == null)
                {
#if VERBOSE_DEBUG
                    Debug.WriteLine("Unmapped property ({0}[{1}]).{2}", typeof(TDomain).Name, idEnt.Key, modelPropertyInfo.Name);
#endif
                    continue;
                }

                var originalValue = propInfo.GetValue(domainInstance);
#if VERBOSE_DEBUG
                Debug.WriteLine("Value property ({0}[{1}]).{2} = {3}", typeof(TDomain).Name, idEnt.Key, modelPropertyInfo.Name, originalValue);
#endif

                // Property info
                try
                {
                    if (originalValue == null)
                        continue;
                }
                catch (Exception e) // HACK: For some reason, some LINQ providers will return NULL on EntityReferences with no value
                {
                    Debug.WriteLine(e.ToString());
                }

                // Traversal stuff
                PropertyInfo modelProperty = propMap == null ? modelPropertyInfo : tModel.GetRuntimeProperty(propMap.ModelName); ;
                object sourceObject = domainInstance;
                PropertyInfo sourceProperty = propInfo;

                // Go through the via elements in the object map. This code traces a path
                // through the domain class instantiating any necessary associative entity
                // Example when a model entity is really two or three tables in the DB..
                // This piece of code does whatever is necessary to traverse the data model,
                // kinda reminds me of a song:
                // 🎶 Ah for just one time, I would take the northwest passage
                // To find the hand of Franklin reaching for the Beaufort Sea.
                // Tracing one warm line, through a land so wide and savage
                // And make a northwest passage to the sea. 🎶
                if (propMap != null)
                {
                    var via = propMap.Via;
                    List<PropertyMap> viaWalk = new List<PropertyMap>();
                    while (via?.DontLoad == false)
                    {
                        viaWalk.Add(via);
                        via = via.Via;
                    }

                    sourceProperty = propInfo;
                    foreach (var p in viaWalk.Select(o => o))
                    {
                        if (!(sourceObject is IList))
                            sourceObject = sourceProperty.GetValue(sourceObject);
                        sourceProperty = this.ExtractDomainType(sourceProperty.PropertyType).GetRuntimeProperty(p.DomainName);
                    }
                }

                // validate property type
                if (propMap?.DontLoad == true)
                    continue;

#if VERBOSE_DEBUG
                Debug.WriteLine("Mapping property ({0}[{1}]).{2} = {3}", typeof(TDomain).Name, idEnt.Key, modelPropertyInfo.Name, originalValue);
#endif
                // Set value
                object pValue = null;

                //DebugWriteLine("Unmapped property ({0}).{1}", typeof(TDomain).Name, propInfo.Name);
                if (sourceProperty.PropertyType == typeof(byte[]) && modelProperty.PropertyType.StripNullable() == typeof(Guid)) // Guid to BA
                    modelProperty.SetValue(retVal, new Guid((byte[])sourceProperty.GetValue(sourceObject)));
                else if (modelProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    modelProperty.SetValue(retVal, sourceProperty.GetValue(sourceObject));
                else if (sourceProperty.PropertyType == typeof(String) && modelProperty.PropertyType == typeof(Type))
                    modelProperty.SetValue(retVal, Type.GetType(sourceProperty.GetValue(sourceObject) as String));
                else if (MapUtil.TryConvert(originalValue, modelProperty.PropertyType, out pValue))
                    modelProperty.SetValue(retVal, pValue);
                // Handles when a map is a list for example doing a VIA over a version relationship
                else if (originalValue is IList)
                {
                    var modelInstance = Activator.CreateInstance(modelProperty.PropertyType) as IList;
                    modelProperty.SetValue(retVal, modelInstance);
                    var instanceMapFunction = typeof(ModelMapper).GetGenericMethod("MapDomainInstance", new Type[] { sourceProperty.PropertyType.GenericTypeArguments[0], modelProperty.PropertyType.GenericTypeArguments[0] },
                        new Type[] { sourceProperty.PropertyType.GenericTypeArguments[0], typeof(bool), typeof(HashSet<Guid>) });
                    foreach (var itm in originalValue as IList)
                    {
                        // Traverse?
                        var instance = itm;
                        var via = propMap?.Via;
                        while (via != null)
                        {
                            instance = instance?.GetType().GetRuntimeProperty(via.DomainName)?.GetValue(instance);
                            if (instance is IList)
                            {
                                var parm = Expression.Parameter(instance.GetType());
                                Expression aggregateExpr = parm;
                                if (!String.IsNullOrEmpty(via.OrderBy))
                                    aggregateExpr = parm.Sort(via.OrderBy, via.SortOrder);
                                aggregateExpr = aggregateExpr.Aggregate(via.Aggregate);

                                // Get the generic method for LIST to be widdled down
                                instance = Expression.Lambda(aggregateExpr, parm).Compile().DynamicInvoke(instance);
                            }
                            via = via.Via;
                        }
                        modelInstance.Add(instanceMapFunction.Invoke(this, new object[] { instance, useCache, keyStack }));
                    }
                }
                // Flat map list 1..1
                else if (typeof(IList).IsAssignableFrom(modelProperty.PropertyType) &&
                    typeof(IList).IsAssignableFrom(sourceProperty.PropertyType))
                {
                    var modelInstance = Activator.CreateInstance(modelProperty.PropertyType) as IList;
                    modelProperty.SetValue(retVal, modelInstance);
                    var instanceMapFunction = typeof(ModelMapper).GetGenericMethod("MapDomainInstance", new Type[] { sourceProperty.PropertyType.GenericTypeArguments[0], modelProperty.PropertyType.GenericTypeArguments[0] },
                        new Type[] { sourceProperty.PropertyType.GenericTypeArguments[0], typeof(bool), typeof(HashSet<Guid>) });
                    var listValue = sourceProperty.GetValue(sourceObject);

                    // Is this list a versioned association??
                    if (tDomain.GetRuntimeProperty("VersionSequenceId") != null &&
                        sourceProperty.PropertyType.GenericTypeArguments[0].GetRuntimeProperty("EffectiveVersionSequenceId") != null) // Filter!!! Yay!
                    {
                        var parm = Expression.Parameter(listValue.GetType());
                        Expression aggregateExpr = null;
                        aggregateExpr = parm.IsActive(domainInstance);
                        listValue = Expression.Lambda(aggregateExpr, parm).Compile().DynamicInvoke(listValue);
                    }

                    foreach (var itm in listValue as IEnumerable)
                        modelInstance.Add(instanceMapFunction.Invoke(this, new object[] { itm, useCache, keyStack }));
                }
                else if (m_mapFile.GetModelClassMap(modelProperty.PropertyType) != null)
                {
                    // TODO: Clean this up
                    var instance = originalValue; //sourceProperty.GetValue(sourceObject);

                    var via = propMap?.Via;
                    while (via != null)
                    {
                        instance = instance?.GetType().GetRuntimeProperty(via.DomainName)?.GetValue(instance);
                        if (instance is IList)
                        {
                            var parm = Expression.Parameter(instance.GetType());
                            Expression aggregateExpr = parm;
                            if (!String.IsNullOrEmpty(via.OrderBy))
                                aggregateExpr = parm.Sort(via.OrderBy, via.SortOrder);
                            aggregateExpr = aggregateExpr.Aggregate(via.Aggregate);

                            // Get the generic method for LIST to be widdled down
                            instance = Expression.Lambda(aggregateExpr, parm).Compile().DynamicInvoke(instance);
                        }
                        via = via.Via;
                    }
                    if (instance != null)
                    {
                        var instanceMapFunction = typeof(ModelMapper).GetGenericMethod("MapDomainInstance", new Type[] { instance?.GetType(), modelProperty.PropertyType },
                           new Type[] { instance?.GetType(), typeof(bool), typeof(HashSet<Guid>) });
                        modelProperty.SetValue(retVal, instanceMapFunction.Invoke(this, new object[] { instance, useCache, keyStack }));
                    }
                }
            }

#if VERBOSE_DEBUG
            Debug.WriteLine("Leaving: {0}>{1}", typeof(TDomain).FullName, typeof(TModel).FullName);
#endif
            if (idEnt != null && useCache)
            {
                keyStack.Remove(idEnt.Key.Value);
                FireMappedToModel(this, vidEnt?.VersionKey ?? idEnt?.Key ?? Guid.Empty, retVal as IdentifiedData);
            }
            // (retVal as IdentifiedData).SetDelayLoad(true);

            return retVal;
            */
        }
    }
}