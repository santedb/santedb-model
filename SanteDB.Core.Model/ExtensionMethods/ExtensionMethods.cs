/*
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2021-2-9
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Map;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Reflection tools
    /// </summary>
    public static class ExtensionMethods
    {

        /// <summary>
        /// Indicates a properly was load/checked 
        /// </summary>
        private struct PropertyLoadCheck
        {

            /// <summary>
            /// Creates a new laod check struct
            /// </summary>
            public PropertyLoadCheck(String propertyName)
            {
                this.PropertyName = propertyName;
            }

            /// <summary>
            /// Gets or sets the name of the property 
            /// </summary>
            public String PropertyName { get; set; }

            /// <summary>
            /// Get the property hash code
            /// </summary>
            public override int GetHashCode() => this.PropertyName.GetHashCode();

        }
        // Property cache
        private static ConcurrentDictionary<String, PropertyInfo> s_propertyCache = new ConcurrentDictionary<string, PropertyInfo>();

        // Type property cache
        private static ConcurrentDictionary<Type, PropertyInfo[]> s_typePropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        // Parameterless ctor cache
        private static ConcurrentDictionary<Type, bool> s_parameterlessCtor = new ConcurrentDictionary<Type, bool>();

        // Enumerable types
        private static ConcurrentDictionary<Type, bool> m_enumerableTypes = new ConcurrentDictionary<Type, bool>();

        /// <summary>
        /// Convert a hex string to byte array
        /// </summary>
        public static byte[] ParseHexString(this String hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                                   .Where(x => x % 2 == 0)
                                   .Select(x => System.Convert.ToByte(hexString.Substring(x, 2), 16))
                                   .ToArray();
        }

        /// <summary>
        /// Parse base 64 encode
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static byte[] ParseBase64UrlEncode(this String base64String)
        {
            string incoming = base64String.Replace('_', '/').Replace('-', '+');
            switch (base64String.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            return System.Convert.FromBase64String(incoming);
        }

        /// <summary>
        /// Convert to base 64
        /// </summary>
        public static String Base64UrlEncode(this byte[] array)
        {
            return System.Convert.ToBase64String(array)
                    .TrimEnd(new Char[] { '=' }).Replace('+', '-').Replace('/', '_');

        }

        /// <summary>
        /// Convert a hex string to byte array
        /// </summary>
        public static String ToHexString(this byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }

        /// <summary>
        /// Get postal code
        /// </summary>
        public static String Value(this EntityAddress me, Guid addressType)
        {
            return me.LoadCollection<EntityAddressComponent>("Component").FirstOrDefault(o => o.ComponentTypeKey == addressType)?.Value;
        }

        /// <summary>
        /// Delay load property
        /// </summary>
        public static TReturn LoadProperty<TReturn>(this IIdentifiedEntity me, string propertyName)
        {
            return (TReturn)me.LoadProperty(propertyName);
        }

        /// <summary>
        /// Delay load property
        /// </summary>
        public static IEnumerable<TReturn> LoadCollection<TReturn>(this IIdentifiedEntity me, string propertyName)
        {
            return me.LoadProperty(propertyName) as IEnumerable<TReturn> ?? new List<TReturn>();
        }

        /// <summary>
        /// Load collection of <typeparamref name="TReturn"/> from <typeparamref name="TSource"/> 
        /// </summary>
        public static IEnumerable<TReturn> LoadCollection<TSource, TReturn>(this TSource me, Expression<Func<TSource, IEnumerable<TReturn>>> selector) 
            where TSource : IIdentifiedEntity
            where TReturn : IIdentifiedEntity
        {
            if (selector is LambdaExpression lambda)
            {
                var body = lambda.Body as MemberExpression;
                if (body != null)
                    return me.LoadCollection<TReturn>(body.Member.Name);
                else
                    throw new InvalidOperationException("Must be simple property selector");
            }
            else
                throw new InvalidOperationException("Unknown expression passed");
        }

        /// <summary>
        /// Load collection of <typeparamref name="TReturn"/> from <typeparamref name="TSource"/> 
        /// </summary>
        public static TReturn LoadProperty<TSource, TReturn>(this TSource me, Expression<Func<TSource, TReturn>> selector) 
            where TSource : IIdentifiedEntity
        {
            if (selector is LambdaExpression lambda)
            {
                var body = lambda.Body as MemberExpression;
                if (body != null)
                    return me.LoadProperty<TReturn>(body.Member.Name);
                else
                    throw new InvalidOperationException("Must be simple property selector");
            }
            else
                throw new InvalidOperationException("Unknown expression passed");
        }

        /// <summary>
        /// Delay load property
        /// </summary>
        public static object LoadProperty(this IIdentifiedEntity me, string propertyName)
        {
            if (me == null) return null;

            var propertyToLoad = me.GetType().GetRuntimeProperty(propertyName);
            if (propertyToLoad == null) return null;
            var currentValue = propertyToLoad.GetValue(me);
            var loadCheck = new PropertyLoadCheck(propertyName);

            if (me.GetAnnotations<PropertyLoadCheck>().Contains(loadCheck))
            {
                return currentValue;
            }
            else
            {
                me.AddAnnotation(loadCheck);
            }

            if (typeof(IList).IsAssignableFrom(propertyToLoad.PropertyType)) // Collection we load by key
            {
                if ((currentValue == null || (currentValue as IList)?.Count == 0) && me.Key.HasValue)
                {
                    var mi = typeof(IEntitySourceProvider).GetGenericMethod(nameof(IEntitySourceProvider.GetRelations), new Type[] { propertyToLoad.PropertyType.StripGeneric() }, new Type[] { typeof(Guid?) });
                    var loaded = Activator.CreateInstance(propertyToLoad.PropertyType, mi.Invoke(EntitySource.Current.Provider, new object[] { me.Key.Value }));
                    propertyToLoad.SetValue(me, loaded);
                    return loaded;
                }
                return currentValue;
            }
            else if (currentValue == null)
            {
                var keyValue = propertyToLoad.GetSerializationRedirectProperty()?.GetValue(me) as Guid?;
                if (keyValue.GetValueOrDefault() == default(Guid))
                    return currentValue;
                else
                {
                    var mi = typeof(IEntitySourceProvider).GetGenericMethod(nameof(IEntitySourceProvider.Get), new Type[] { propertyToLoad.PropertyType }, new Type[] { typeof(Guid?) });
                    var loaded = mi.Invoke(EntitySource.Current.Provider, new object[] { keyValue });
                    propertyToLoad.SetValue(me, loaded);
                    return loaded;
                }
            }
            else return currentValue;
            // Now we want to 

        }

        /// <summary>
        /// Gets the serialization runtime property
        /// </summary>
        public static PropertyInfo GetSerializationRedirectProperty(this PropertyInfo me)
        {
            var xsr = me.GetCustomAttribute<SerializationReferenceAttribute>();
            if (xsr == null) return null;
            return me.DeclaringType.GetRuntimeProperty(xsr.RedirectProperty);
        }

        /// <summary>
        /// Create aggregation functions
        /// </summary>
        public static Expression Aggregate(this Expression me, AggregationFunctionType aggregation)
        {
            var aggregateMethod = typeof(Enumerable).GetGenericMethod(aggregation.ToString(),
               new Type[] { me.Type.GenericTypeArguments[0] },
               new Type[] { me.Type });
            return Expression.Call(aggregateMethod as MethodInfo, me);
        }

        /// <summary>
        /// The purpose of this method is to convert object <paramref name="me"/> to <typeparamref name="TReturn"/>. Why?
        /// Because if you have an instance of Act that actually needs to be a SubstanceAdministration we can't just cast
        /// so we have to copy.
        /// </summary>
        public static TReturn Convert<TReturn>(this Object me) where TReturn : new()
        {
            if (me is TReturn) return (TReturn)me;
            else
            {
                var retVal = new TReturn();
                retVal.CopyObjectData(me);
                return retVal;
            }
        }

        /// <summary>
        /// Copy object data from one object instance to another
        /// </summary>
        public static TObject CopyObjectData<TObject>(this TObject toEntity, TObject fromEntity)
        {
            return toEntity.CopyObjectData(fromEntity, false);
        }

        /// <summary>
        /// Strips all version information from the specified object. 
        /// </summary>
        public static TObject StripAssociatedItemSources<TObject>(this TObject toEntity)
        {
            if (toEntity == null)
                throw new ArgumentNullException(nameof(toEntity));

            PropertyInfo[] properties = null;
            if (!s_typePropertyCache.TryGetValue(toEntity.GetType(), out properties))
            {
                properties = toEntity.GetType().GetRuntimeProperties().Where(destinationPi => destinationPi.GetCustomAttribute<DataIgnoreAttribute>() == null &&
                    destinationPi.CanWrite).ToArray();
                s_typePropertyCache.TryAdd(toEntity.GetType(), properties);
            }
            foreach (var destinationPi in properties)
            {
                if (typeof(ISimpleAssociation).IsAssignableFrom(destinationPi.PropertyType.StripGeneric())) // Versioned association
                {
                    var value = destinationPi.GetValue(toEntity);
                    if (value is IList list)
                        foreach (ISimpleAssociation itm in list)
                        {
                            (itm as IIdentifiedEntity).Key = null;
                            itm.SourceEntityKey = null;

                            if (itm is IVersionedAssociation va)
                                va.EffectiveVersionSequenceId = null;
                            itm.StripAssociatedItemSources();
                        }
                    else if (value is ISimpleAssociation assoc)
                    {
                        (assoc as IIdentifiedEntity).Key = null;
                        assoc.SourceEntityKey = null;
                        if (assoc is IVersionedAssociation va)
                            va.EffectiveVersionSequenceId = null;
                        assoc.StripAssociatedItemSources();
                    }
                }

            }

            return toEntity;
        }

        /// <summary>
        /// Update property data if required
        /// </summary>
        public static TObject CopyObjectData<TObject>(this TObject toEntity, TObject fromEntity, bool overwritePopulatedWithNull, bool ignoreTypeMismatch = false)
        {
            if (toEntity == null)
                throw new ArgumentNullException(nameof(toEntity));
            else if (fromEntity == null)
                throw new ArgumentNullException(nameof(fromEntity));
            else if (!ignoreTypeMismatch && !fromEntity.GetType().IsAssignableFrom(toEntity.GetType()))
                throw new ArgumentException($"Type mismatch {toEntity.GetType().FullName} != {fromEntity.GetType().FullName}", nameof(fromEntity));

            PropertyInfo[] properties = null;
            if (!s_typePropertyCache.TryGetValue(toEntity.GetType(), out properties))
            {
                properties = toEntity.GetType().GetRuntimeProperties().Where(destinationPi => destinationPi.GetCustomAttribute<DataIgnoreAttribute>() == null &&
                    destinationPi.CanWrite).ToArray();
                s_typePropertyCache.TryAdd(toEntity.GetType(), properties);
            }
            foreach (var destinationPi in properties)
            {
                var sourcePi = fromEntity.GetType().GetRuntimeProperty(destinationPi.Name);
                // Skip properties no in the source
                if (sourcePi == null)
                    continue;

                // Skip data ignore
                if (destinationPi.PropertyType.IsGenericType &&
                    destinationPi.PropertyType.GetGenericTypeDefinition().Namespace.StartsWith("System.Data.Linq") ||
                    destinationPi.PropertyType.Namespace.StartsWith("SanteDB.Persistence"))
                    continue;

                object newValue = sourcePi.GetValue(fromEntity),
                    oldValue = destinationPi.GetValue(toEntity);

                // HACK: New value wrap for nullables
                if (newValue is Guid? && newValue != null)
                    newValue = (newValue as Guid?).Value;

                // HACK: Empty lists are NULL
                if ((newValue as IList)?.Count == 0)
                    newValue = null;

                object defaultValue = null;
                var hasConstructor = false;
                if (newValue != null && !s_parameterlessCtor.TryGetValue(newValue.GetType(), out hasConstructor))
                {
                    /// HACK: Some .NET types don't like to be constructed
                    try
                    {
                        Activator.CreateInstance(newValue.GetType());
                        hasConstructor = true;
                    }
                    catch { hasConstructor = false; }
                    s_parameterlessCtor.TryAdd(newValue.GetType(), hasConstructor);
                }
                if (newValue != null && hasConstructor)
                    defaultValue = Activator.CreateInstance(newValue.GetType());

                if (newValue is IList && oldValue is IList)
                {
                    if (!Enumerable.SequenceEqual<Object>(((IList)newValue).OfType<Object>(), ((IList)oldValue).OfType<Object>()))
                        destinationPi.SetValue(toEntity, newValue);
                }
                else if (
                    newValue != null &&
                    !newValue.Equals(oldValue) == true &&
                    (destinationPi.PropertyType.StripNullable() != destinationPi.PropertyType || typeof(String) == destinationPi.PropertyType && !String.IsNullOrEmpty(newValue.ToString()) || !newValue.Equals(defaultValue) || !destinationPi.PropertyType.IsValueType))
                    destinationPi.SetValue(toEntity, newValue);
                else if (newValue == null && oldValue != null && overwritePopulatedWithNull)
                    destinationPi.SetValue(toEntity, newValue);

            }
            return toEntity;
        }

        /// <summary>
		/// Update property data with data from <paramref name="fromEntities"/> if the property is not semantically equal
		/// </summary>
		public static TObject SemanticCopy<TObject>(this TObject toEntity, params TObject[] fromEntities) where TObject : IdentifiedData
        {
            return SemanticCopyInternal(toEntity, false, null, fromEntities);
        }

        /// <summary>
        /// Update property data with data from <paramref name="fromEntities"/> if the property is not semantically equal
        /// </summary>
        public static TObject SemanticCopyNullFields<TObject>(this TObject toEntity, params TObject[] fromEntities) where TObject : IdentifiedData
        {
            return SemanticCopyInternal(toEntity, true, null, fromEntities);
        }

        /// <summary>
        /// Update property data with data from <paramref name="fromEntities"/> if and only if the source property is null
        /// in the original toEntity
        /// </summary>
        public static TObject SemanticCopyFields<TObject>(this TObject toEntity, TObject[] fromEntities, params String[] fieldNames) where TObject : IdentifiedData
        {
            return SemanticCopyInternal(toEntity, false, fieldNames, fromEntities);
        }

        /// <summary>
        /// Perform semantic copy
        /// </summary>
        private static TObject SemanticCopyInternal<TObject>(this TObject toEntity, bool onlyIfNull, String[] fieldNames, params TObject[] fromEntities) where TObject : IdentifiedData
        {
            if (toEntity == null)
                throw new ArgumentNullException(nameof(toEntity));
            else if (fromEntities == null)
                throw new ArgumentNullException(nameof(fromEntities));
            else if (fromEntities.Length == 0)
                return toEntity;
            else if (!fromEntities.Any(e => e.GetType().IsAssignableFrom(toEntity.GetType())))
                throw new ArgumentException($"Type mismatch {toEntity.GetType().FullName} != {fromEntities.GetType().FullName}", nameof(fromEntities));

            PropertyInfo[] properties = null;
            if (!s_typePropertyCache.TryGetValue(toEntity.GetType(), out properties))
            {
                properties = toEntity.GetType().GetRuntimeProperties().Where(destinationPi => destinationPi.GetCustomAttribute<DataIgnoreAttribute>() == null &&
                    destinationPi.CanWrite).ToArray();
                s_typePropertyCache.TryAdd(toEntity.GetType(), properties);
            }

            // Properties which are lists that have had data added from the fromEntities array
            Dictionary<PropertyInfo, List<IdentifiedData>> mergedListProperties = new Dictionary<PropertyInfo, List<IdentifiedData>>();
            String[] ignoreProperties = { "Key", "VersionKey", "VersionSequenceId", "CreationTime", "CreatedByKey", "CreatedBy", "UpdatedBy", "UpdatedByKey", "UpdatedTime", "ObsoletedBy", "ObsoletedByKey", "ObsoletionTime", "ReplacesVersionKey", "ReplacesVersion" };

            // Destination properties
            foreach (var destinationPi in properties.Where(p => !ignoreProperties.Contains(p.Name)))
            {
                object oldValue = destinationPi.GetValue(toEntity);
                if ((onlyIfNull && (oldValue == null || (oldValue as IList)?.Count == 0) || !onlyIfNull) &&
                    (fieldNames == null || fieldNames.Contains(destinationPi.Name)))
                    foreach (var fromEntity in fromEntities.OrderBy(k => k.ModifiedOn))
                    {

                        var sourcePi = fromEntity.GetType().GetRuntimeProperty(destinationPi.Name);
                        // Skip properties no in the source
                        if (sourcePi == null)
                            continue;

                        // Skip data ignore
                        if (destinationPi.PropertyType.IsGenericType &&
                            destinationPi.PropertyType.GetGenericTypeDefinition().Namespace.StartsWith("System.Data.Linq") ||
                            destinationPi.PropertyType.Namespace.StartsWith("SanteDB.Persistence"))
                            continue;

                        object newValue = sourcePi.GetValue(fromEntity);

                        // HACK: New value wrap for nullables
                        if (newValue is Guid? && newValue != null)
                            newValue = (newValue as Guid?).Value;

                        // HACK: Empty lists are NULL
                        if ((newValue as IList)?.Count == 0)
                            newValue = null;

                        if (newValue != null) // The new value has something 
                        {
                            // The destination is a list, so we should add if the item doesn't exist in the list
                            if (typeof(IList).IsAssignableFrom(destinationPi.PropertyType))
                            {
                                // No current value 
                                if (oldValue == null)
                                {
                                    oldValue = Activator.CreateInstance(destinationPi.PropertyType);
                                    destinationPi.SetValue(toEntity, oldValue);
                                }

                                // Cast lists
                                IList<IdentifiedData> oldList = (oldValue as IList).OfType<IdentifiedData>().ToList(),
                                    newList = (newValue as IList).OfType<IdentifiedData>().ToList();
                                IList modifyList = oldValue as IList;

                                // Initialize list if needed
                                List<IdentifiedData> addedObjects = null;
                                if (!mergedListProperties.TryGetValue(destinationPi, out addedObjects))
                                {
                                    addedObjects = new List<IdentifiedData>();
                                    mergedListProperties.Add(destinationPi, addedObjects);
                                }

                                // Copy / update items
                                foreach (var itm in newList)
                                {
                                    // Get an existing item that matches this
                                    var existing = oldList.FirstOrDefault(o => o.SemanticEquals(itm));
                                    // Couldn't find something that "means the same" as the current
                                    if (existing == null)
                                    {
                                        var citm = itm.Clone();
                                        citm.Key = null;
                                        modifyList.Add(citm);
                                    }
                                    addedObjects.Add(itm);
                                }

                            }
                            else if (newValue is IdentifiedData &&
                                !(newValue as IdentifiedData).SemanticEquals(oldValue))
                                destinationPi.SetValue(toEntity, newValue);
                            else if (!newValue.Equals(oldValue) &&
                                (destinationPi.PropertyType.StripNullable() != destinationPi.PropertyType || !newValue.Equals(Activator.CreateInstance(newValue.GetType())) || !destinationPi.PropertyType.IsValueType))
                                destinationPi.SetValue(toEntity, newValue);
                        }
                    }
            }

            return toEntity;
        }

        /// <summary>
        /// Get generic method
        /// </summary>
        public static MethodBase GetGenericMethod(this Type type, string name, Type[] typeArgs, Type[] argTypes)
        {
            int typeArity = typeArgs.Length;
            var methods = type.GetRuntimeMethods()
                .Where(m => m.Name == name)
                .Where(m => m.GetGenericArguments().Length == typeArity)
                .Where(m => m.GetParameters().Length == argTypes.Length)
                .Select(m => m.MakeGenericMethod(typeArgs)).ToList();

            methods = methods.Where(m => Enumerable.Range(0, argTypes.Length).All(i => m.GetParameters()[i].IsOut || m.GetParameters()[i].ParameterType.IsAssignableFrom(argTypes[i]))).ToList();

            return methods.FirstOrDefault();
            //return Type.DefaultBinder.SelectMethod(flags, methods.ToArray(), argTypes, null);
        }

        /// <summary>
        /// True if enumerable
        /// </summary>
        public static bool IsEnumerable(this Type me)
        {
            if (!m_enumerableTypes.TryGetValue(me, out bool retVal))
            {
                retVal = typeof(IEnumerable).IsAssignableFrom(me);
                m_enumerableTypes.TryAdd(me, retVal);
            }
            return retVal;

        }

        /// <summary>
        /// Get the serialization name
        /// </summary>
        public static string GetSerializationName(this PropertyInfo me)
        {
            var xmlName = me.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault()?.ElementName ?? me.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
            if (xmlName == null)
            {
                var refName = me.GetCustomAttribute<SerializationReferenceAttribute>()?.RedirectProperty;
                if (refName == null)
                    return null;
                xmlName = me.DeclaringType.GetRuntimeProperty(refName)?.GetCustomAttribute<XmlElementAttribute>()?.ElementName;
            }
            else if (xmlName == String.Empty)
                xmlName = me.Name;
            return xmlName;
        }

        /// <summary>
        /// Determine if <paramref name="me"/> implements all of <paramref name="interfaces"/>
        /// </summary>
        public static bool Implements(this Type me, params Type[] interfaces)
        {
            return interfaces.All(o => o.IsAssignableFrom(me));
        }

        /// <summary>
        /// Get the serialization name
        /// </summary>
        public static string GetQueryName(this PropertyInfo me)
        {
            return me.GetCustomAttributes<QueryParameterAttribute>().FirstOrDefault()?.ParameterName
                ?? me.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault()?.ElementName
                ?? me.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
        }
        /// <summary>
        /// Get the serialization type name
        /// </summary>
        public static String GetSerializationName(this Type type)
        {
            return type.GetCustomAttribute<XmlRootAttribute>()?.ElementName ?? type.GetCustomAttribute<JsonObjectAttribute>()?.Id ?? type.Name;
        }

        /// <summary>
        /// Get a property based on XML property and/or serialization redirect and/or query parameter
        /// </summary>
        public static PropertyInfo GetQueryProperty(this Type type, string propertyName, bool followReferences = false)
        {
            PropertyInfo retVal = null;
            var key = String.Format("{0}.{1}[{2}]", type.FullName, propertyName, followReferences);
            if (!s_propertyCache.TryGetValue(key, out retVal))
            {
                retVal = type.GetRuntimeProperties().FirstOrDefault(o => o.GetCustomAttributes<XmlElementAttribute>()?.FirstOrDefault()?.ElementName == propertyName || o.GetCustomAttribute<QueryParameterAttribute>()?.ParameterName == propertyName);
                if (retVal == null)
                    throw new MissingMemberException($"{type.FullName}.{propertyName}");
                if (followReferences) retVal = type.GetRuntimeProperties().FirstOrDefault(o => o.GetCustomAttribute<SerializationReferenceAttribute>()?.RedirectProperty == retVal.Name) ?? retVal;

                if (retVal.Name.EndsWith("Xml"))
                    retVal = type.GetRuntimeProperty(retVal.Name.Substring(0, retVal.Name.Length - 3));

                s_propertyCache.TryAdd(key, retVal);
            }
            return retVal;
        }

        /// <summary>
        /// Compute a basic hash string
        /// </summary>
        public static String HashCode(this byte[] me)
        {
            long hash = 1009;
            foreach (var b in me)
                hash = ((hash << 5) + hash) ^ b;
            return BitConverter.ToString(BitConverter.GetBytes(hash)).Replace("-", "");
        }

        /// <summary>
        /// Determines whether the specified property name has property.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the specified property name has property; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">source - Value cannot be null</exception>
        /// <exception cref="System.ArgumentException">Value cannot be null or empty - propertyName</exception>
        public static bool HasProperty(this ExpandoObject source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Value cannot be null");
            }

            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(propertyName));
            }

            return ((IDictionary<string, object>)source).ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets the full name of the user entity.
        /// </summary>
        /// <param name="entity">The user entity.</param>
        /// <param name="nameUseKey">The name use key.</param>
        /// <returns>Returns the full name of the user entity.</returns>
        /// <exception cref="System.ArgumentNullException">If the entity is null.</exception>
        public static string GetFullName(this UserEntity entity, Guid nameUseKey)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Value cannot be null");
            }

            var given = entity.Names.Where(n => n.NameUseKey == nameUseKey).SelectMany(n => n.Component).Where(c => c.ComponentTypeKey == NameComponentKeys.Given).Select(c => c.Value).ToList();
            var family = entity.Names.Where(n => n.NameUseKey == nameUseKey).SelectMany(n => n.Component).Where(c => c.ComponentTypeKey == NameComponentKeys.Family).Select(c => c.Value).ToList();

            return string.Join(" ", given) + " " + string.Join(" ", family);
        }

        /// <summary>
        /// Create a version filter
        /// </summary>
        /// <param name="me">Me.</param>
        /// <param name="domainInstance">The domain instance.</param>
        /// <returns>Expression.</returns>
        public static Expression IsActive(this Expression me, Object domainInstance)
        {
            // Extract boundary properties
            var effectiveVersionMethod = me.Type.GenericTypeArguments[0].GetRuntimeProperty("EffectiveVersionSequenceId");
            var obsoleteVersionMethod = me.Type.GenericTypeArguments[0].GetRuntimeProperty("ObsoleteVersionSequenceId");
            if (effectiveVersionMethod == null || obsoleteVersionMethod == null)
                return me;

            // Create predicate type and find WHERE method
            Type predicateType = typeof(Func<,>).MakeGenericType(me.Type.GenericTypeArguments[0], typeof(bool));
            var whereMethod = typeof(Enumerable).GetGenericMethod("Where",
                new Type[] { me.Type.GenericTypeArguments[0] },
                new Type[] { me.Type, predicateType });

            // Create Where Expression
            var guardParameter = Expression.Parameter(me.Type.GenericTypeArguments[0], "x");
            var currentSequenceId = domainInstance?.GetType().GetRuntimeProperty("VersionSequenceId").GetValue(domainInstance);
            var bodyExpression = Expression.MakeBinary(ExpressionType.AndAlso,
                Expression.MakeBinary(ExpressionType.LessThanOrEqual, Expression.MakeMemberAccess(guardParameter, effectiveVersionMethod), Expression.Constant(currentSequenceId)),
                Expression.MakeBinary(ExpressionType.OrElse,
                    Expression.MakeBinary(ExpressionType.Equal, Expression.MakeMemberAccess(guardParameter, obsoleteVersionMethod), Expression.Constant(null)),
                    Expression.MakeBinary(ExpressionType.GreaterThan, Expression.MakeMemberAccess(
                        Expression.MakeMemberAccess(guardParameter, obsoleteVersionMethod),
                        typeof(Nullable<Decimal>).GetRuntimeProperty("Value")), Expression.Constant(currentSequenceId))
                )
            );

            // Build strongly typed lambda
            var builderMethod = typeof(Expression).GetGenericMethod(nameof(Expression.Lambda), new Type[] { predicateType }, new Type[] { typeof(Expression), typeof(ParameterExpression[]) });
            var sortLambda = builderMethod.Invoke(null, new object[] { bodyExpression, new ParameterExpression[] { guardParameter } }) as Expression;
            return Expression.Call(whereMethod as MethodInfo, me, sortLambda);
        }

        /// <summary>
        /// Create a version filter
        /// </summary>
        /// <param name="me">Me.</param>
        /// <returns>Expression.</returns>
        public static Expression IsActive(this Expression me)
        {
            // Extract boundary properties
            var obsoleteVersionMethod = me.Type.GenericTypeArguments[0].GetRuntimeProperty("ObsoleteVersionSequenceId");
            if (obsoleteVersionMethod == null)
                return me;

            // Create predicate type and find WHERE method
            Type predicateType = typeof(Func<,>).MakeGenericType(me.Type.GenericTypeArguments[0], typeof(bool));
            var whereMethod = typeof(Enumerable).GetGenericMethod("Where",
                new Type[] { me.Type.GenericTypeArguments[0] },
                new Type[] { me.Type, predicateType });

            // Create Where Expression
            var guardParameter = Expression.Parameter(me.Type.GenericTypeArguments[0], "x");
            var bodyExpression = Expression.MakeBinary(ExpressionType.Equal, Expression.MakeMemberAccess(guardParameter, obsoleteVersionMethod), Expression.Constant(null));

            // Build strongly typed lambda
            var builderMethod = typeof(Expression).GetGenericMethod(nameof(Expression.Lambda), new Type[] { predicateType }, new Type[] { typeof(Expression), typeof(ParameterExpression[]) });
            var sortLambda = builderMethod.Invoke(null, new object[] { bodyExpression, new ParameterExpression[] { guardParameter } }) as Expression;
            return Expression.Call(whereMethod as MethodInfo, me, sortLambda);
        }

        /// <summary>
        /// Gets the latest version of the versioned entity data instance from a given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Returns the latest version only of the versioned entity data.</returns>
        public static IEnumerable<T> LatestVersionOnly<T>(this IEnumerable<T> source) where T : IVersionedEntity
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Value cannot be null");
            }

            var latestVersions = new List<T>();

            var keys = source.Select(e => e.Key.Value).Distinct();

            foreach (var key in keys)
            {
                var maxVersionSequence = source.Select(e => source.Where(a => a.Key == key).Max<T>(a => a.VersionSequence)).FirstOrDefault();

                var latestVersion = source.FirstOrDefault(a => a.Key == key && a.VersionSequence == maxVersionSequence);

                if (latestVersion != null)
                {
                    latestVersions.Add(latestVersion);
                }
            }

            return latestVersions;
        }

        /// <summary>
        /// Determine semantic equality of each item in me and other
        /// </summary>
        public static bool SemanticEquals<TEntity>(this IEnumerable<TEntity> me, IEnumerable<TEntity> other) where TEntity : IdentifiedData
        {
            if (other == null) return false;
            bool equals = me.Count() == other.Count();
            foreach (var itm in me)
                equals &= other.Any(o => o.SemanticEquals(itm));
            foreach (var itm in other)
                equals &= me.Any(o => o.SemanticEquals(itm));

            return equals;
        }

        /// <summary>
        /// Create sort expression.
        /// </summary>
        /// <param name="me">Me.</param>
        /// <param name="orderByProperty">The order by property.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>Expression.</returns>
        public static Expression Sort(this Expression me, String orderByProperty, SortOrderType sortOrder)
        {
            // Get sort property
            var sortProperty = me.Type.GenericTypeArguments[0].GetRuntimeProperty(orderByProperty);
            Type predicateType = typeof(Func<,>).MakeGenericType(me.Type.GenericTypeArguments[0], sortProperty.PropertyType);
            var sortMethod = typeof(Enumerable).GetGenericMethod(sortOrder.ToString(),
                new Type[] { me.Type.GenericTypeArguments[0], sortProperty.PropertyType },
                new Type[] { me.Type, predicateType });

            // Get builder methods
            var sortParameter = Expression.Parameter(me.Type.GenericTypeArguments[0], "sort");
            var builderMethod = typeof(Expression).GetGenericMethod(nameof(Expression.Lambda), new Type[] { predicateType }, new Type[] { typeof(Expression), typeof(ParameterExpression[]) });
            var sortLambda = builderMethod.Invoke(null, new object[] { Expression.MakeMemberAccess(sortParameter, sortProperty), new ParameterExpression[] { sortParameter } }) as Expression;
            return Expression.Call(sortMethod as MethodInfo, me, sortLambda);
        }

        /// <summary>
        /// Strips list
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>Returns the type.</returns>
        public static Type StripGeneric(this Type t)
        {
            return t.IsGenericType ? t.GenericTypeArguments[0] : t;
        }

        /// <summary>
        /// Strips any nullable typing.
        /// </summary>
        /// <param name="t">The typ.</param>
        /// <returns>Returns the type.</returns>
        public static Type StripNullable(this Type t)
        {
            if (t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(Nullable<>))
                return t.GenericTypeArguments[0];
            return t;
        }

        /// <summary>
        /// Return the age 
        /// </summary>
        /// <remarks>This exists for the extended query filter only</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TimeSpan Age(this DateTime me, DateTime atDateTime)
        {
            return me.Subtract(atDateTime);
        }
    }
}