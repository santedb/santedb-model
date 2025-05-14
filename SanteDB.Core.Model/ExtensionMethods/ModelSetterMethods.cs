/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using SanteDB.Core.i18n;
using SanteDB.Core.Model;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Map;
using SanteDB.Core.Model.Query;
using SanteDB.Core.Model.Serialization;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SanteDB
{
    /// <summary>
    /// Extension methods which can safely set properties from the HDSI expressions
    /// </summary>
    public static class ModelSetterMethods
    {

        // Model setter cache
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<String, Func<object, object>>> s_modelSetterCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Func<object, object>>>();
        // Non-metadata property cache
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> s_nonMetadataCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        // Property selector
        private static Regex s_hdsiPropertySelector = new Regex(@"(\w+)(?:\[([^]]+)\])?(?:\@(\w+))?\.?", RegexOptions.Compiled);

        /// <summary>
        /// Gets all non metadata properties (those marked with <see cref="SerializationMetadataAttribute"/>
        /// </summary>
        public static PropertyInfo[] GetNonMetadataProperties(this Type me)
        {
            if (!s_nonMetadataCache.TryGetValue(me, out var retVal))
            {
                retVal = me.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(o => o.GetCustomAttribute<SerializationMetadataAttribute>() == null).ToArray();
                s_nonMetadataCache.TryAdd(me, retVal);
            }
            return retVal;
        }

        /// <summary>
        /// Get the property value 
        /// </summary>
        /// <param name="root">The root object from which data should be selected</param>
        /// <param name="hdsiExpressionPath">The HDSI expression to retrieve</param>
        /// <returns>The property value</returns>
        /// <param name="replace">True if the value at <paramref name="hdsiExpressionPath"/> should be replaced with the <paramref name="valueToSet"/>, when false <paramref name="valueToSet"/> will be added to the object</param>
        /// <param name="valueToSet">The value to set the property to if no value at the <paramref name="hdsiExpressionPath"/> exists. Null if no value is to be set</param>
        /// <remarks>This method differs from the query expression parser in that it will actually modify 
        /// <paramref name="root"/> to set properties until it gets to the path expressed</remarks>
        public static object GetOrSetValueAtPath(this IdentifiedData root, string hdsiExpressionPath, object valueToSet = null, bool replace = true)
        {
            try
            {
                var matches = s_hdsiPropertySelector.Matches(hdsiExpressionPath);
                if (matches.Count == 0)
                {
                    throw new InvalidOperationException(); // todo: add message
                }

                // The code will iterate through the matches to get the source properties - the examples inline are for the
                // following HDSI expression:
                // relationship[Mother].target@Person.name[Given].component[Family].value
                Object focalObject = root;
                PropertyInfo sourceProperty = null;
                foreach (Match match in matches)
                {

                    // Get the source property , in our example iterations
                    // 1. relationship[Mother]
                    // 2. target@Person
                    // 3. name[Given]
                    // 4. component[Family]
                    // 5. value

                    if (!s_modelSetterCache.TryGetValue(focalObject.GetType(), out var setterDelegates))
                    {
                        setterDelegates = new ConcurrentDictionary<string, Func<object, object>>();
                        s_modelSetterCache.TryAdd(focalObject.GetType(), setterDelegates);
                    }
                    if (!setterDelegates.TryGetValue(match.Groups[0].Value, out var accessorDelegate))
                    {
                        var propertyAccessor = QueryExpressionParser.BuildPropertySelector(focalObject.GetType(), match.Groups[0].Value, true);
                        if (propertyAccessor.Body.NodeType == System.Linq.Expressions.ExpressionType.Coalesce && propertyAccessor.Body is BinaryExpression be) // Strip off the coalesce
                        {
                            propertyAccessor = Expression.Lambda(be.Left, propertyAccessor.Parameters[0]);
                        }

                        // Convert to a strong delegate
                        var parm = Expression.Parameter(typeof(Object));
                        var newLambda = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Invoke(propertyAccessor, Expression.Convert(parm, focalObject.GetType())), typeof(Object)), parm);
                        accessorDelegate = newLambda.Compile();
                        setterDelegates.TryAdd(match.Groups[0].Value, accessorDelegate);
                    }

                    object currentValue = null;
                    try
                    {
                        currentValue = accessorDelegate(focalObject);
                    }
                    catch
                    {

                    }

                    var originalValue = currentValue;
                    sourceProperty = focalObject.GetType().GetQueryProperty(match.Groups[1].Value);

                    if (valueToSet != null && Guid.TryParse(valueToSet.ToString(), out var uuid) && sourceProperty.GetCustomAttribute<SerializationReferenceAttribute>()?.RedirectProperty.EndsWith("Xml") == true) // HACK: Most of the time we want to set the redirect property
                    {
                        sourceProperty = focalObject.GetType().GetProperty($"{sourceProperty.Name}Xml");
                        valueToSet = uuid;
                    }
                    var sourcePropertyValue = sourceProperty.GetValue(focalObject);

                    // Is the property value not set so we want to create it if needed
                    if (currentValue == null)
                    {
                        // Get the source property
                        // 1. relationship
                        // 2. target
                        // 3. name
                        // 4. component
                        // 5. value

                        // Chained?
                        if (match.NextMatch().Success && sourceProperty.PropertyType.StripNullable() == typeof(Guid))
                        {
                            var redirect = sourceProperty.DeclaringType.GetProperties().FirstOrDefault(o => o.GetSerializationRedirectProperty() == sourceProperty);
                            if (redirect == null)
                            {
                                throw new InvalidOperationException(); // todo: add message
                            }
                            sourceProperty = redirect;
                        }

                        if (sourcePropertyValue == null && (sourceProperty.PropertyType.Implements(typeof(IIdentifiedResource)) ||
                            sourceProperty.PropertyType.Implements(typeof(IList)) && !sourceProperty.PropertyType.IsArray))
                        {

                            // Is there a cast?
                            // 2. target@Person => Create a new Person if one does not exist
                            var propertyType = sourceProperty.PropertyType;
                            if (!String.IsNullOrEmpty(match.Groups[3].Value))
                            {
                                propertyType = new ModelSerializationBinder().BindToType(typeof(ModelSerializationBinder).Assembly.FullName, match.Groups[3].Value);
                            }

                            sourcePropertyValue = Activator.CreateInstance(propertyType);
                            sourceProperty.SetValue(focalObject, sourcePropertyValue);
                        }

                        // If the new object is a list then we want to add
                        if (sourcePropertyValue is IList listObject)
                        {
                            sourcePropertyValue = Activator.CreateInstance(sourceProperty.PropertyType.StripGeneric());
                            if (Guid.Empty.Equals(sourcePropertyValue) && valueToSet is Guid) // HACK: A list of GUID indicates 
                            {
                                sourcePropertyValue = valueToSet;
                            }
                            listObject.Add(sourcePropertyValue);
                        }

                        currentValue = sourcePropertyValue;
                        // Is there a guard?
                        // 1. [Mother]
                        // 3. [OfficialRecord]
                        // 4. [Family]

                        if (currentValue is IdentifiedData && !String.IsNullOrEmpty(match.Groups[2].Value))
                        {
                            SetClassifier(currentValue, match.Groups[2].Value);
                        }

                    }
                    else if (!replace &&
                        match.NextMatch().Success &&
                        !match.NextMatch().NextMatch().Success) // We have a classifier property which is 
                    {
                        // HACK: Figure out a better way to do this - basically 
                        //        when we are near the terminal part of the path and the value is a collection 
                        //        we don't want to replace - we want to add the value - for example:
                        //          name[OfficialRecord].component[Given].value=Mary , replace=false
                        //          name[OfficialRecord].component[Given].value=Elizabeth , replace=false
                        //       the result is that name[OfficialRecord] should have 2 components rather than one
                        if (sourcePropertyValue is IList list)
                        {
                            currentValue = Activator.CreateInstance(sourceProperty.PropertyType.StripGeneric());
                            if (currentValue is IdentifiedData && !String.IsNullOrEmpty(match.Groups[2].Value))
                            {
                                SetClassifier(currentValue, match.Groups[2].Value);
                            }
                            list.Add(currentValue);
                        }
                    }

                    if ((currentValue == null || replace && !match.NextMatch().Success) && valueToSet != null)
                    {
                        currentValue = valueToSet;
                        if (MapUtil.TryConvert(currentValue, sourceProperty.PropertyType, out var result))
                        {
                            sourceProperty.SetValue(focalObject, result);
                        }
                        else if (currentValue is IdentifiedData)
                        {
                            (sourceProperty.GetSerializationModelProperty() ?? sourceProperty).SetValue(focalObject, currentValue);
                        }
                        else
                        {
                            (sourceProperty).SetValue(focalObject, currentValue);
                        }
                    }
                    focalObject = currentValue;
                }
                return focalObject;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.CANNOT_SET_VALUE_AT_PATH, hdsiExpressionPath, valueToSet?.GetType().Name), e);
            }
        }

        private static void SetClassifier(object sourceValue, String classifierValue)
        {
            var classifierProperty = sourceValue.GetType().GetSanteDBProperty<ClassifierAttribute>();
            if (Guid.TryParse(classifierValue, out Guid guidValue))
            {
                classifierProperty = classifierProperty.GetSerializationRedirectProperty();
                classifierProperty.SetValue(sourceValue, guidValue);
            }
            else
            {
                var currentValue = sourceValue;
                if (classifierProperty.PropertyType != typeof(String))
                {
                    sourceValue = Activator.CreateInstance(classifierProperty.PropertyType);
                    classifierProperty.SetValue(currentValue, sourceValue);
                    var simpleProperty = classifierProperty.PropertyType.GetSanteDBProperty<KeyLookupAttribute>();
                    simpleProperty.SetValue(sourceValue, classifierValue);
                }
                else
                {
                    classifierProperty.SetValue(currentValue, classifierValue);
                }
            }
        }
    }
}
