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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Map.Builder
{
    /// <summary>
    /// Represents a model mapper which uses reflection in contexts where CodeDom cannot be created
    /// </summary>
    public class ReflectionModelMapper : IModelMapper
    {
        // The map configuration
        private ClassMap m_classMap;

        // The context in which this mapper operates
        private ModelMapper m_context;

        /// <summary>
        /// Reflection model mapper
        /// </summary>
        public ReflectionModelMapper(ClassMap map, ModelMapper context)
        {
            this.m_classMap = map;
            this.m_context = context;
        }

        /// <summary>
        /// Gets the source type
        /// </summary>
        public Type SourceType => this.m_classMap.ModelType;

        /// <summary>
        /// Gets the target type
        /// </summary>
        public Type TargetType => this.m_classMap.DomainType;

        /// <summary>
        /// Map to source
        /// </summary>
        public object MapToSource(object domainInstance)
        {
            if (domainInstance == null)
                return null;

            // Now the property maps
            var retVal = Activator.CreateInstance(this.m_classMap.ModelType);

            // Iterate the properties and map
            foreach (var sourceProperty in domainInstance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Map property
                PropertyInfo modelProperty = null;
                if (!m_classMap.TryGetModelProperty(sourceProperty.Name, out PropertyMap propMap))
                {
                    modelProperty = this.m_classMap.ModelType.GetProperty(sourceProperty.Name);
                }
                else
                {
                    modelProperty = this.m_classMap.ModelType.GetProperty(propMap.ModelName);
                }

                if (modelProperty == null || propMap?.DontLoad == true)
                {
                    continue;
                }

#if VERBOSE_DEBUG
                Debug.WriteLine("Mapping property ({0}[{1}]).{2} = {3}", typeof(TDomain).Name, idEnt.Key, modelPropertyInfo.Name, originalValue);
#endif
                // Set value
                var originalValue = sourceProperty.GetValue(domainInstance);

                this.Set(retVal, modelProperty, originalValue, sourceProperty);
            }

            return retVal;
        }

        /// <summary>
        /// Map <paramref name="modelInstance"/> to whatever target is configured
        /// </summary>
        public object MapToTarget(object modelInstance)
        {
            if (m_classMap == null || modelInstance == null)
                return null;

            var retVal = Activator.CreateInstance(this.m_classMap.DomainType);

            // Iterate through properties
            foreach (var propInfo in modelInstance.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var propValue = propInfo.GetValue(modelInstance);
                // Property info
                if (propValue == null)
                    continue;

                if (!propInfo.PropertyType.IsPrimitive && propInfo.PropertyType != typeof(Guid) &&
                    (!propInfo.PropertyType.IsGenericType || propInfo.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) &&
                    propInfo.PropertyType != typeof(String) &&
                    propInfo.PropertyType.StripNullable() != typeof(DateTime) &&
                    propInfo.PropertyType.StripNullable() != typeof(DateTimeOffset) &&
                    propInfo.PropertyType.StripNullable() != typeof(Type) &&
                    propInfo.PropertyType.StripNullable() != typeof(Decimal) &&
                    propInfo.PropertyType.StripNullable() != typeof(byte[]) &&
                    !propInfo.PropertyType.StripNullable().IsEnum)
                    continue;

                // Map property
                PropertyInfo domainProperty = null;
                if (m_classMap.TryGetModelProperty(propInfo.Name, out PropertyMap propMap))
                {
                    domainProperty = this.m_classMap.DomainType.GetRuntimeProperty(propMap.DomainName);
                }
                else
                {
                    domainProperty = this.m_classMap.DomainType.GetRuntimeProperty(propInfo.Name);
                }
                Object targetObject = retVal;

                this.Set(targetObject, domainProperty, propValue, propInfo);
            }

            return retVal;
        }

        /// <summary>
        /// Set the value via reflection
        /// </summary>
        private void Set(Object targetObject, PropertyInfo targetProperty, Object sourceObject, PropertyInfo sourceProperty)
        {
            if (targetProperty == null || !targetProperty.CanWrite || sourceObject == null)
                return;
            //Debug.WriteLine ("Unmapped property ({0}).{1}", typeof(TModel).Name, propInfo.Name);
            else if (targetProperty.PropertyType == typeof(byte[]) && sourceProperty.PropertyType.StripNullable() == typeof(Guid))
                targetProperty.SetValue(targetObject, ((Guid)sourceObject).ToByteArray());
            else if (
                (targetProperty.PropertyType == typeof(DateTime) || targetProperty.PropertyType == typeof(DateTime?))
                && (sourceProperty.PropertyType == typeof(DateTimeOffset) || sourceProperty.PropertyType == typeof(DateTimeOffset?)))
            {
                targetProperty.SetValue(targetObject, ((DateTimeOffset)sourceObject).DateTime);
            }
            else if (targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                targetProperty.SetValue(targetObject, sourceObject);
            else if (sourceProperty.PropertyType == typeof(Type) && targetProperty.PropertyType == typeof(String))
                targetProperty.SetValue(targetObject, (sourceObject as Type).AssemblyQualifiedName);
            else if (MapUtil.TryConvert(sourceObject, targetProperty.PropertyType, out object domainValue))
                targetProperty.SetValue(targetObject, domainValue);
            else if (targetProperty.PropertyType == typeof(String) && sourceProperty.PropertyType.StripNullable().IsEnum)
            {
                // Is XML Enum?
                var fields = sourceProperty.PropertyType.StripNullable().GetFields();
                object value = sourceObject.ToString();
                if (fields.Any(f => f.GetCustomAttribute<XmlEnumAttribute>() != null))
                {
                    var fn = Enum.GetName(sourceProperty.PropertyType.StripNullable(), sourceObject);
                    value = fields.First(o => o.Name == fn).GetCustomAttribute<XmlEnumAttribute>().Name;
                }
                targetProperty.SetValue(targetObject, value);
            }
            else if (targetProperty.PropertyType.StripNullable().IsEnum && sourceProperty.PropertyType == typeof(String))
            {
                // Is XML Enum?
                var fields = targetProperty.PropertyType.StripNullable().GetFields();
                var value = fields.FirstOrDefault(f => f.GetCustomAttribute<XmlEnumAttribute>()?.Name == sourceObject.ToString())?.GetValue(null) ??
                    Enum.Parse(targetProperty.PropertyType.StripNullable(), sourceObject.ToString());
                targetProperty.SetValue(targetObject, value);
            }
        }
    }
}