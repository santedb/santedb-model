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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SanteDB
{
    /// <summary>
    /// Extension methods which can safely set properties from the HDSI expressions
    /// </summary>
    public static class ModelSetterMethods
    {

        // Model setter cache
        private static ConcurrentDictionary<Type, ConcurrentDictionary<String, Delegate>> s_modelSetterCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Delegate>>();

        // Property selector
        private static Regex s_hdsiPropertySelector = new Regex(@"(\w+)(?:\[([\w\|\-]+)\])?(?:\@(\w+))?\.?", RegexOptions.Compiled);

        /// <summary>
        /// Get the property value 
        /// </summary>
        /// <param name="root">The root object from which data should be selected</param>
        /// <param name="hdsiExpressionPath">The HDSI expression to retrieve</param>
        /// <returns>The property value</returns>
        /// <param name="valueToSet">The value to set the property to if no value at the <paramref name="hdsiExpressionPath"/> exists. Null if no value is to be set</param>
        /// <remarks>This method differs from the query expression parser in that it will actually modify 
        /// <paramref name="root"/> to set properties until it gets to the path expressed</remarks>
        public static object GetOrSetValueAtPath(this IdentifiedData root, string hdsiExpressionPath, object valueToSet = null, bool replace = true)
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
                    setterDelegates = new ConcurrentDictionary<string, Delegate>();
                    s_modelSetterCache.TryAdd(focalObject.GetType(), setterDelegates);
                }
                if (!setterDelegates.TryGetValue(match.Groups[0].Value, out var accessorDelegate))
                {
                    var propertyAccessor = QueryExpressionParser.BuildPropertySelector(focalObject.GetType(), match.Groups[0].Value, true);
                    if (propertyAccessor.Body.NodeType == System.Linq.Expressions.ExpressionType.Coalesce && propertyAccessor.Body is BinaryExpression be) // Strip off the coalesce
                    {
                        propertyAccessor = Expression.Lambda(be.Left, propertyAccessor.Parameters[0]);
                    }

                    accessorDelegate = propertyAccessor.Compile();
                    setterDelegates.TryAdd(match.Groups[0].Value, accessorDelegate);
                }

                var currentValue = accessorDelegate.DynamicInvoke(focalObject);
                var originalValue = currentValue;
                sourceProperty = focalObject.GetType().GetQueryProperty(match.Groups[1].Value);
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
                        sourceProperty.PropertyType.Implements(typeof(IList))))
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
                    else
                    {
                        sourceProperty.SetValue(focalObject, currentValue);
                    }
                }
                focalObject = currentValue;
            }
            return focalObject;
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
                sourceValue = Activator.CreateInstance(classifierProperty.PropertyType);
                classifierProperty.SetValue(currentValue, sourceValue);
                var simpleProperty = classifierProperty.PropertyType.GetSanteDBProperty<KeyLookupAttribute>();
                simpleProperty.SetValue(sourceValue, classifierValue);
            }
        }
    }
}
