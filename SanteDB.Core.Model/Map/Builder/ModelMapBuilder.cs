/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2022-9-7
 */
using SanteDB.Core.Exceptions;
using SanteDB.Core.i18n;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Map.Builder
{
    /// <summary>
    /// Model map builder
    /// </summary>
    /// <remarks>
    /// The old version of the model mapping class used reflection which was extremely inefficient for mapping
    /// large sets of data. As part of the nuado refactoring, the model mapping is now handled by programmatically
    /// generated model mapping classes which contain explicit instructions on how to map the data from the source
    /// model into the target model
    /// </remarks>
    public class ModelMapBuilder
    {
        private static readonly CodePrimitiveExpression s_true = new CodePrimitiveExpression(true);
        private static readonly CodePrimitiveExpression s_false = new CodePrimitiveExpression(false);
        private static readonly CodePrimitiveExpression s_null = new CodePrimitiveExpression(null);
        private static readonly CodeThisReferenceExpression s_this = new CodeThisReferenceExpression();

        private static readonly CodeFieldReferenceExpression s_mapperContext = new CodeFieldReferenceExpression(s_this, "m_mapper");
        private static readonly CodeVariableReferenceExpression s_retVal = new CodeVariableReferenceExpression("retVal");
        private static readonly CodeTypeReference s_tIdentifiedData = new CodeTypeReference(typeof(IdentifiedData));
        private static readonly CodeTypeReference s_tType = new CodeTypeReference(typeof(Type));

        /// <summary>
        /// Model map builder
        /// </summary>
        public ModelMapBuilder()
        {
        }

        /// <summary>
        /// Creates a String.Format() expression
        /// </summary>
        private CodeExpression CreateStringFormatExpression(string formatString, params CodeExpression[] parms)
        {
            var retVal = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(String)), "Format"), new CodePrimitiveExpression(formatString));
            foreach (var p in parms)
            {
                retVal.Parameters.Add(p);
            }

            return retVal;
        }

        /// <summary>
        /// Create a instance cast expression
        /// </summary>
        private CodeStatement CreateCastTryCatch(Type toType, CodeExpression targetObject, CodeExpression sourceObject, CodeStatement failExpression)
        {
            return new CodeTryCatchFinallyStatement(
                new CodeStatement[] {
                    new CodeAssignStatement(targetObject, new CodeCastExpression(new CodeTypeReference(toType), sourceObject)),
                },
                new CodeCatchClause[] {
                new CodeCatchClause("e", new CodeTypeReference(typeof(Exception)),
                    failExpression)
                });
        }

        /// <summary>
        /// Create a simaple method call expression
        /// </summary>
        private CodeExpression CreateSimpleMethodCallExpression(CodeExpression target, String methodName, params Object[] simpleParameters)
        {
            var retVal = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(target, methodName));
            foreach (var p in simpleParameters)
            {
                retVal.Parameters.Add(new CodePrimitiveExpression(p));
            }

            return retVal;
        }

        /// <summary>
        /// Create a simaple method call expression
        /// </summary>
        private CodeExpression CreateMethodCallExpression(CodeExpression target, String methodName, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(target, methodName), parameters);
        }

        /// <summary>
        /// Create ConvertExpression
        /// </summary>
        private CodeExpression CreateConvertExpression(String methodName, CodeExpression parameter)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Convert)), methodName, parameter);
        }

        /// <summary>
        /// Create an equals statement
        /// </summary>
        private CodeMethodInvokeExpression CreateValueEqualsStatement(CodeExpression left, CodeExpression right)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(left, "Equals"), right);
        }

        /// <summary>
        /// Create a throw exception
        /// </summary>
        /// <param name="exceptionType">The type of exception to be thrown</param>
        /// <param name="exceptionArguments">The arguments to the throw</param>
        /// <returns>The exception statement</returns>
        private CodeThrowExceptionStatement CreateThrowException(Type exceptionType, params CodeExpression[] exceptionArguments)
        {
            return new CodeThrowExceptionStatement(new CodeObjectCreateExpression(exceptionType, exceptionArguments));
        }

        /// <summary>
        /// Creates a call to GetType()
        /// </summary>
        private CodeExpression CreateGetTypeExpression(CodeExpression _object)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(_object, "GetType"));
        }

        /// <summary>
        /// Create a code namespace for the specified assembly
        /// </summary>
        public CodeNamespace CreateCodeNamespace(ModelMap map, String name)
        {
            CodeNamespace retVal = new CodeNamespace($"SanteDB.Core.Model.Map.{name}");

            foreach (var t in map.Class)
            {
                var ctdecl = this.CreateMapper(t, map.Class.Where(o => o != t && o.ModelType.IsAssignableFrom(t.ModelType) && o.DomainType != t.DomainType));
                if (ctdecl != null)
                {
                    retVal.Types.Add(ctdecl);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Creates a view model serializer
        /// </summary>
        public CodeTypeDeclaration CreateMapper(ClassMap map, IEnumerable<ClassMap> otherMappedTypes)
        {
            // Cannot process this type
            if (map.DomainType == null || map.ModelType == null)
            {
                return null;
            }

            // Generate the type definition
            string className = $"{map.ModelType.Name}To{map.DomainType.Name}ModelMapper";
            CodeTypeDeclaration retVal = new CodeTypeDeclaration(className);
            retVal.IsClass = true;
            retVal.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            retVal.BaseTypes.Add(typeof(IModelMapper));
            retVal.BaseTypes.Add(typeof(IModelMapper<,>).MakeGenericType(map.ModelType, map.DomainType));

            // Map up the field count
            retVal.Members.Add(new CodeMemberField(typeof(ModelMapper), "m_mapper") { Attributes = MemberAttributes.Private });
            retVal.Members.Add(this.CreateInitializorConstructor(className));

            // Add methods
            retVal.Members.Add(this.CreateTypeGetProperty("SourceType", map.ModelType));
            retVal.Members.Add(this.CreateTypeGetProperty("TargetType", map.DomainType));

            retVal.Members.Add(this.CreateObjectMapMethod("MapToSource", map.DomainType));
            retVal.Members.Add(this.CreateObjectMapMethod("MapToTarget", map.ModelType));

            retVal.Members.Add(this.CreateMapToTargetMethod(map));
            retVal.Members.Add(this.CreateMapToSourceMethod(map));

            // Add other maps to match the to/from
            foreach (var t in otherMappedTypes)
            {
                var interfaceDefinition = new CodeTypeReference(typeof(IModelMapper<,>).MakeGenericType(map.ModelType, t.DomainType));

                retVal.BaseTypes.Add(interfaceDefinition);
                var newMap = new ClassMap()
                {
                    Cast = t.Cast,
                    CollapseKey = t.CollapseKey,
                    DomainClass = t.DomainClass,
                    ModelClass = map.ModelClass,
                    Property = t.Property
                };
                retVal.Members.Add(this.CreateMapToTargetMethod(newMap, interfaceDefinition));
                retVal.Members.Add(this.CreateMapToSourceMethod(newMap, interfaceDefinition));
            }
            return retVal;
        }

        /// <summary>
        /// Create MapToSource Method based on the instructions in the <paramref name="map"/>
        /// </summary>
        /// <param name="map">The map to use</param>
        /// <param name="implementedInterface">The interface this type implements</param>
        /// <returns>The type member</returns>
        private CodeMemberMethod CreateMapToSourceMethod(ClassMap map, CodeTypeReference implementedInterface = null)
        {
            var retVal = new CodeMemberMethod()
            {
                PrivateImplementationType = implementedInterface,
                Name = "MapToSource",
                ReturnType = new CodeTypeReference(map.ModelType),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            retVal.Parameters.Add(new CodeParameterDeclarationExpression(map.DomainType, "instance"));
            var _instance = new CodeVariableReferenceExpression("instance");

            retVal.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(_instance, CodeBinaryOperatorType.IdentityEquality, s_null), new CodeMethodReturnStatement(new CodeDefaultValueExpression(new CodeTypeReference(map.ModelType)))));
            retVal.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(map.ModelType), "retVal", new CodeObjectCreateExpression(new CodeTypeReference(map.ModelType))));

            // Iterate through the inbound properties and copy them over
            foreach (var propInfo in map.DomainType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!propInfo.PropertyType.StripNullable().IsPrimitive
                    && propInfo.PropertyType.StripNullable() != typeof(Guid) &&
                   propInfo.PropertyType.StripGeneric() != typeof(String) &&
                   propInfo.PropertyType.StripGeneric() != typeof(DateTime) &&
                   propInfo.PropertyType.StripGeneric() != typeof(DateTimeOffset) &&
                   propInfo.PropertyType.StripGeneric() != typeof(Type) &&
                   propInfo.PropertyType.StripGeneric() != typeof(Decimal) &&
                   propInfo.PropertyType.StripGeneric() != typeof(byte[]) &&
                   !propInfo.PropertyType.IsEnum)
                {
                    continue;
                }

                // Auto map property
                var otherProp = map.ModelType.GetProperty(propInfo.Name);
                if (map.TryGetDomainProperty(propInfo.Name, out PropertyMap configMap))
                {
                    otherProp = map.ModelType.GetProperty(configMap.ModelName);
                }

                // Other property
                if (otherProp == null || !otherProp.CanWrite)
                {
                    otherProp = map.ModelType.GetProperty(propInfo.Name);
                    if (otherProp == null || !otherProp.CanWrite) // recheck
                    {
                        continue;
                    }
                }

                // Special cases
                try
                {
                    retVal.Statements.Add(this.CreateCheckedAssignment(propInfo.PropertyType, new CodePropertyReferenceExpression(_instance, propInfo.Name), otherProp.PropertyType, new CodePropertyReferenceExpression(s_retVal, otherProp.Name)));
                }
                catch (InvalidOperationException e)
                {
                    throw new ModelMapValidationException(new ValidationResultDetail[] { new ValidationResultDetail(ResultDetailType.Error, $"{map.ModelType.FullName}.{otherProp.Name} = {map.DomainType.FullName}.{propInfo.Name} - {e.Message}", e, "") });
                }
            }
            retVal.Statements.Add(new CodeMethodReturnStatement(s_retVal));
            return retVal;
        }

        /// <summary>
        /// Create the MapToTarget method
        /// </summary>
        /// <param name="map">The map to use</param>
        /// <param name="implementedType">The implemented type for explicit interface implementation</param>
        /// <returns>The target map method</returns>
        private CodeMemberMethod CreateMapToTargetMethod(ClassMap map, CodeTypeReference implementedType = null)
        {
            var retVal = new CodeMemberMethod()
            {
                PrivateImplementationType = implementedType,
                Name = "MapToTarget",
                ReturnType = new CodeTypeReference(map.DomainType),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            retVal.Parameters.Add(new CodeParameterDeclarationExpression(map.ModelType, "instance"));

            var _instance = new CodeVariableReferenceExpression("instance");

            retVal.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(_instance, CodeBinaryOperatorType.IdentityEquality, s_null), new CodeMethodReturnStatement(new CodeDefaultValueExpression(new CodeTypeReference(map.DomainType)))));
            retVal.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(map.DomainType), "retVal", new CodeObjectCreateExpression(new CodeTypeReference(map.DomainType))));

            // Iterate through the inbound properties and copy them over
            foreach (var propInfo in map.DomainType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!propInfo.PropertyType.StripNullable().IsPrimitive
                    && propInfo.PropertyType.StripNullable() != typeof(Guid) &&
                   propInfo.PropertyType.StripGeneric() != typeof(String) &&
                   propInfo.PropertyType.StripGeneric() != typeof(DateTime) &&
                   propInfo.PropertyType.StripGeneric() != typeof(DateTimeOffset) &&
                   propInfo.PropertyType.StripGeneric() != typeof(Type) &&
                   propInfo.PropertyType.StripGeneric() != typeof(Decimal) &&
                   propInfo.PropertyType.StripGeneric() != typeof(byte[]) &&
                   !propInfo.PropertyType.IsEnum)
                {
                    continue;
                }

                // Auto map property
                var modelProp = map.ModelType.GetProperty(propInfo.Name);
                if (map.TryGetDomainProperty(propInfo.Name, out PropertyMap configMap))
                {
                    modelProp = map.ModelType.GetProperty(configMap.ModelName);
                }

                // Other property
                if (modelProp == null || !modelProp.CanWrite)
                {
                    modelProp = map.ModelType.GetProperty(propInfo.Name);
                    if (modelProp == null || !modelProp.CanWrite)
                    {
                        continue;
                    }
                }
                else if (!propInfo.CanWrite)
                {
                    continue;
                }

                // Special cases
                try
                {
                    retVal.Statements.Add(this.CreateCheckedAssignment(modelProp.PropertyType, new CodePropertyReferenceExpression(_instance, modelProp.Name), propInfo.PropertyType, new CodePropertyReferenceExpression(s_retVal, propInfo.Name)));
                }
                catch (InvalidOperationException e)
                {
                    throw new ModelMapValidationException(new ValidationResultDetail[] { new ValidationResultDetail(ResultDetailType.Error, $"{map.DomainType.FullName}.{propInfo.Name} = {map.ModelType.FullName}.{modelProp.Name} - {e.Message}", e, "") });
                }
            }
            retVal.Statements.Add(new CodeMethodReturnStatement(s_retVal));
            return retVal;
        }

        /// <summary>
        /// Create a constructor which accepts a <see cref="ModelMapper"/>
        /// </summary>
        /// <param name="className">The name of the constructor class</param>
        private CodeConstructor CreateInitializorConstructor(string className)
        {
            var retVal = new CodeConstructor()
            {
                Name = className,
                Attributes = MemberAttributes.Public,
            };
            retVal.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ModelMapper), "mapper"));
            retVal.Statements.Add(new CodeAssignStatement(s_mapperContext, new CodeVariableReferenceExpression("mapper")));
            return retVal;
        }

        /// <summary>
        /// Create a checked assignment statement which ensures that the value from <paramref name="fromObject"/>
        /// </summary>
        private CodeStatement CreateCheckedAssignment(Type fromType, CodeExpression fromObject, Type toType, CodeExpression toObject)
        {
            // BYTE[] <-- GUID
            if (typeof(byte[]).Equals(toType) && typeof(Guid).Equals(fromType))
            {
                return new CodeAssignStatement(toObject, this.CreateMethodCallExpression(fromObject, nameof(Guid.ToByteArray)));
            }
            // GUID <-- BYTE[]
            else if (typeof(Guid).Equals(toType.StripNullable()) && typeof(byte[]).Equals(fromType))
            {
                return new CodeAssignStatement(toObject, new CodeObjectCreateExpression(typeof(Guid), fromObject));
            }
            else if (typeof(Type).Equals(toType) && typeof(String).Equals(fromType))
            {
                return new CodeAssignStatement(toObject, this.CreateMethodCallExpression(new CodeTypeReferenceExpression(typeof(Type)), "GetType", fromObject));
            }
            // ENUM <-- INT
            else if (toType.StripNullable().IsEnum && typeof(Int32).Equals(fromType))
            {
                return new CodeAssignStatement(toObject, new CodeCastExpression(new CodeTypeReference(toType), fromObject));
            }
            // DATETIME <-- DATETIMEOFFSET
            else if (typeof(DateTime).Equals(toType.StripNullable()) && typeof(DateTimeOffset).IsAssignableFrom(fromType))
            {
                return new CodeAssignStatement(toObject, new CodePropertyReferenceExpression(fromObject, nameof(DateTimeOffset.DateTime)));
            }
            // DATETIMEOFFST <== DATETIME
            else if (typeof(DateTimeOffset).Equals(toType.StripNullable()) && typeof(DateTime).IsAssignableFrom(fromType))
            {
                return new CodeAssignStatement(toObject, new CodeCastExpression(typeof(DateTimeOffset), fromObject));
            }
            // INT <== ENUM
            else if (typeof(Int32).Equals(toType.StripNullable()) && fromType.IsEnum)
            {
                return new CodeAssignStatement(toObject, new CodeCastExpression(typeof(Int32), fromObject));
            }
            // ENUM <== STRING
            else if (toType.StripNullable().IsEnum && typeof(String).Equals(fromType))
            {
                if (toType.StripNullable().GetFields().Any(f => f.GetCustomAttribute<XmlEnumAttribute>() != null))
                {
                    CodeConditionStatement retVal = null;
                    foreach (var f in toType.StripNullable().GetFields())
                    {
                        var enumV = f.GetCustomAttribute<XmlEnumAttribute>();
                        if (enumV == null)
                        {
                            continue;
                        }

                        var logicExpression = new CodeConditionStatement(this.CreateValueEqualsStatement(new CodePrimitiveExpression(enumV.Name), fromObject), new CodeAssignStatement(toObject, new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(toType.StripGeneric()), f.Name)));
                        if (retVal == null)
                        {
                            retVal = logicExpression;
                        }
                        else
                        {
                            logicExpression.FalseStatements.Add(retVal);
                            retVal = logicExpression;
                        }
                    }

                    return retVal;
                }
                else
                {
                    return new CodeAssignStatement(toObject, new CodeCastExpression(toType.StripNullable(), this.CreateMethodCallExpression(new CodeTypeReferenceExpression(typeof(Enum)), "Parse", new CodeTypeOfExpression(fromType.StripNullable()), fromObject)));
                }
            }
            // STRING <== ENUM
            else if (typeof(String).Equals(toType) && fromType.IsEnum)
            {
                // Use the XML enum?
                if (fromType.GetFields().Any(f => f.GetCustomAttribute<XmlEnumAttribute>() != null))
                {
                    CodeConditionStatement retVal = null;
                    foreach (var f in fromType.GetFields())
                    {
                        var enumV = f.GetCustomAttribute<XmlEnumAttribute>();
                        if (enumV == null)
                        {
                            continue;
                        }

                        // Convert with an if
                        var logicExpression = new CodeConditionStatement(new CodeBinaryOperatorExpression(fromObject, CodeBinaryOperatorType.ValueEquality, new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(fromType), f.Name)),
                            new CodeAssignStatement(toObject, new CodePrimitiveExpression(enumV.Name)));
                        if (retVal == null)
                        {
                            retVal = logicExpression;
                        }
                        else
                        {
                            logicExpression.FalseStatements.Add(retVal);
                            retVal = logicExpression;
                        }
                    }
                    return retVal;
                }
                else
                {
                    // Just TOSTRING()
                    return new CodeAssignStatement(toObject, this.CreateToStringExpression(fromObject));
                }
            }
            // STRING <== TYPE
            else if (typeof(String).Equals(toType) && fromType.Equals(typeof(Type)))
            {
                return new CodeAssignStatement(toObject, new CodePropertyReferenceExpression(fromObject, nameof(Type.AssemblyQualifiedName)));
            }
            // TYPE? <== TYPE
            else if (Nullable.GetUnderlyingType(fromType) != null) // generic
            {
                return new CodeConditionStatement(
                    new CodePropertyReferenceExpression(fromObject, "HasValue"),
                    this.CreateCheckedAssignment(fromType.StripNullable(), new CodePropertyReferenceExpression(fromObject, "Value"), toType, toObject)
                    );
            }
            // TYPE == TYPE
            else if (fromType.Equals(toType) || toType.IsAssignableFrom(fromType))
            {
                return new CodeAssignStatement(toObject, fromObject);
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.MAP_INCOMPATIBLE_TYPE, fromType, toType));
            }
        }

        /// <summary>
        /// Creates a non-generic form of the MapToX method, casting to appropriate <paramref name="parameterType"/>
        /// </summary>
        /// <param name="methodName">The name of the method to generate</param>
        /// <param name="parameterType">The type of parameter to cast</param>
        /// <returns>The code type member</returns>
        private CodeTypeMember CreateObjectMapMethod(string methodName, Type parameterType)
        {
            var retVal = new CodeMemberMethod()
            {
                Name = methodName,
                ReturnType = new CodeTypeReference(typeof(Object)),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            retVal.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Object), "o"));
            var _o = new CodeVariableReferenceExpression("o");

            retVal.Statements.Add(this.CreateArgumentNullCheckStatement(_o));
            retVal.Statements.Add(new CodeVariableDeclarationStatement(parameterType, "instance"));
            var _instance = new CodeVariableReferenceExpression("instance");

            // Cast the object to the correct type
            retVal.Statements.Add(this.CreateCastTryCatch(parameterType, _instance, _o, this.CreateThrowException(typeof(ArgumentException), new CodePrimitiveExpression("instance"), this.CreateStringFormatExpression(ErrorMessages.MAP_INVALID_TYPE, this.CreateGetTypeExpression(_o)), new CodeVariableReferenceExpression("e"))));

            // Call and return
            retVal.Statements.Add(new CodeMethodReturnStatement(this.CreateMethodCallExpression(s_this, methodName, _instance)));

            return retVal;
        }

        /// <summary>
        /// Create null check for argument <paramref name="argumentToCheck"/>
        /// </summary>
        /// <param name="argumentToCheck">The argument to check</param>
        /// <returns>The conditional statement</returns>
        /// <remarks>Checks that <paramref name="argumentToCheck"/> is null, and if so, then throws an argumentnullexception</remarks>
        private CodeConditionStatement CreateArgumentNullCheckStatement(CodeVariableReferenceExpression argumentToCheck)
        {
            return new CodeConditionStatement(new CodeBinaryOperatorExpression(argumentToCheck, CodeBinaryOperatorType.IdentityEquality, s_null), this.CreateThrowException(typeof(ArgumentNullException), new CodePrimitiveExpression(argumentToCheck.VariableName)));
        }

        /// <summary>
        /// Create a ToString call on <paramref name="sourceObject"/>
        /// </summary>
        /// <param name="sourceObject">The object to call ToString on</param>
        /// <returns>The ToString expression</returns>
        private CodeMethodInvokeExpression CreateToStringExpression(CodeExpression sourceObject)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(sourceObject, "ToString"));
        }

        /// <summary>
        /// Create one of the type properties on <see cref="IModelMapper{TSource, TTarget}"/>
        /// </summary>
        /// <param name="propertyName">The name of the property in the interface</param>
        /// <param name="type">The type to return from the property</param>
        /// <returns>The type member to be attached to the source property</returns>
        private CodeTypeMember CreateTypeGetProperty(String propertyName, Type type)
        {
            var retVal = new CodeMemberProperty()
            {
                Name = propertyName,
                Type = s_tType,
                HasGet = true,
                HasSet = false,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            retVal.GetStatements.Add(new CodeMethodReturnStatement(new CodeTypeOfExpression(type)));
            return retVal;
        }
    }
}