using SanteDB.Core.i18n;
using SanteDB.Core.Model;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Query;
using SanteDB.Core.Model.Serialization;
using System;
using System.Collections;
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
        // Property selector
        private static Regex s_hdsiPropertySelector = new Regex(@"(\w+)(?:\[([\w\|\-]+)\])?(?:\@(\w+))?\.?");

        /// <summary>
        /// Get the property value 
        /// </summary>
        /// <param name="root">The root object from which data should be selected</param>
        /// <param name="hdsiExpressionPath">The HDSI expression to retrieve</param>
        /// <returns>The property value</returns>
        /// <param name="valueToSet">The value to set the property to if no value at the <paramref name="hdsiExpressionPath"/> exists. Null if no value is to be set</param>
        /// <remarks>This method differs from the query expression parser in that it will actually modify 
        /// <paramref name="root"/> to set properties until it gets to the path expressed</remarks>
        public static object GetOrSetValueAtPath(this IdentifiedData root, string hdsiExpressionPath, object valueToSet = null)
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
                var propertyAccessor = QueryExpressionParser.BuildPropertySelector(focalObject.GetType(), match.Groups[0].Value, true);
                if (propertyAccessor.Body.NodeType == System.Linq.Expressions.ExpressionType.Coalesce && propertyAccessor.Body is BinaryExpression be) // Strip off the coalesce
                {
                    propertyAccessor = Expression.Lambda(be.Left, propertyAccessor.Parameters[0]);
                }

                var currentValue = propertyAccessor.Compile().DynamicInvoke(focalObject);

                // Is the property value not set so we want to create it if needed
                if (currentValue == null)
                {
                    // Get the source property
                    // 1. relationship
                    // 2. target
                    // 3. name
                    // 4. component
                    // 5. value
                    sourceProperty = focalObject.GetType().GetQueryProperty(match.Groups[1].Value);
                    var sourceValue = sourceProperty.GetValue(focalObject);

                    // Chained?
                    if (match.NextMatch() != null && sourceProperty.PropertyType.StripNullable() == typeof(Guid))
                    {
                        var redirect = sourceProperty.DeclaringType.GetProperties().FirstOrDefault(o => o.GetSerializationRedirectProperty() == sourceProperty);
                        if (redirect == null)
                        {
                            throw new InvalidOperationException(); // todo: add message
                        }
                        sourceProperty = redirect;
                    }

                    if (sourceValue == null && (sourceProperty.PropertyType.Implements(typeof(IIdentifiedResource)) ||
                        sourceProperty.PropertyType.Implements(typeof(IList))))
                    {

                        // Is there a cast?
                        // 2. target@Person => Create a new Person if one does not exist
                        var propertyType = sourceProperty.PropertyType;
                        if (!String.IsNullOrEmpty(match.Groups[3].Value))
                        {
                            propertyType = new ModelSerializationBinder().BindToType(typeof(ModelSerializationBinder).Assembly.FullName, match.Groups[3].Value);
                        }
                        sourceValue = Activator.CreateInstance(propertyType);
                        sourceProperty.SetValue(focalObject, sourceValue);
                    }

                    // If the new object is a list then we want to add
                    if (sourceValue is IList listObject)
                    {
                        sourceValue = Activator.CreateInstance(sourceProperty.PropertyType.StripGeneric());
                        listObject.Add(sourceValue);
                    }

                    currentValue = sourceValue;
                    // Is there a guard?
                    // 1. [Mother]
                    // 3. [OfficialRecord]
                    // 4. [Family]
                    if (sourceValue is IdentifiedData && !String.IsNullOrEmpty(match.Groups[2].Value))
                    {
                        var classifierValue = match.Groups[2].Value;
                        var classifierProperty = sourceValue.GetType().GetSanteDBProperty<ClassifierAttribute>();
                        if (Guid.TryParse(classifierValue, out Guid guidValue))
                        {
                            classifierProperty = classifierProperty.GetSerializationRedirectProperty();
                            classifierProperty.SetValue(sourceValue, guidValue);
                        }
                        else
                        {
                            sourceValue = Activator.CreateInstance(classifierProperty.PropertyType);
                            classifierProperty.SetValue(currentValue, sourceValue);
                            var simpleProperty = classifierProperty.PropertyType.GetSanteDBProperty<KeyLookupAttribute>();
                            simpleProperty.SetValue(sourceValue, classifierValue);
                        }

                    }
                }

                if (currentValue == null && valueToSet != null)
                {
                    currentValue = valueToSet;
                    sourceProperty.SetValue(focalObject, currentValue);
                }
                focalObject = currentValue;
            }
            return focalObject;
        }
    }
}
