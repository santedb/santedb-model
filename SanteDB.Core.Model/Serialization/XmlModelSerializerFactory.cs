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
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Serialization
{
    /// <summary>
    /// Serializer factory
    /// </summary>
    /// <remarks>Properly constructs a serializer based on all registered types</remarks>
    public class XmlModelSerializerFactory
    {
        // Current instance
        private static XmlModelSerializerFactory m_current;

        // Lock
        private static readonly object m_lock = new object();

        // Get serializer keys for a registered type
        private readonly ConcurrentDictionary<Type, string> m_serializerKeys = new ConcurrentDictionary<Type, string>();

        // Cache of created serializers
        private readonly ConcurrentDictionary<string, XmlSerializer> m_serializers = new ConcurrentDictionary<string, XmlSerializer>();

        static readonly Type s_AddDependentSerializersType = typeof(AddDependentSerializersAttribute);
        static readonly Type s_XmlRootType = typeof(XmlRootAttribute);
        static readonly Type s_XmlTypeType = typeof(XmlTypeAttribute);

        /// <summary>
        /// Private ctor
        /// </summary>
        private XmlModelSerializerFactory()
        {
        }

        /// <summary>
        /// Gets the current instance of the serializer factory
        /// </summary>
        public static XmlModelSerializerFactory Current
        {
            get
            {
                if (m_current == null)
                {
                    lock (m_lock)
                    {
                        if (m_current == null)
                        {
                            m_current = new XmlModelSerializerFactory();
                        }
                    }
                }

                return m_current;
            }
        }

        /// <summary>
        /// Creates a new Xml Serializer to grabs an existing one (although .NET already caches this constructor)
        /// </summary>
        /// <param name="type">The primary type of the serializer</param>
        /// <param name="extraTypes">The extra types to be included</param>
        /// <returns>The specified serializer</returns>
        public XmlSerializer CreateSerializer(Type type, params Type[] extraTypes)
        {
            if (null == type)
            {
                return null;
            }

            // Generate key
            if (extraTypes.Length > 0 || !this.m_serializerKeys.TryGetValue(type, out var key))
            {
                key = type.AssemblyQualifiedName;
                if (extraTypes != null)
                {
                    key += string.Join(";", extraTypes.Select(o => o.AssemblyQualifiedName));
                }
            }

            // Exists?
            if (!this.m_serializers.TryGetValue(key, out var serializer))
            {
                if (!this.m_serializers.ContainsKey(key)) // Ensure that hasn't been generated since lock was acquired
                {
                    if (type.HasCustomAttribute(s_AddDependentSerializersType) && extraTypes.Length == 0)
                    {
                        extraTypes = AppDomain.CurrentDomain.GetAllTypes()
                            .Union(ModelSerializationBinder.GetRegisteredTypes())
                            .Where(t => t.HasCustomAttribute(s_XmlRootType) && !t.IsEnum && !t.IsGenericTypeDefinition && !t.IsAbstract && !t.IsInterface)
                            .ToArray();
                    }
                    else if (extraTypes.Length == 0)
                    {
                        extraTypes = AppDomain.CurrentDomain.GetAllTypes()
                            .Where(t => t.HasCustomAttribute(s_XmlTypeType))
                            .Where(t => t.GetConstructor(Type.EmptyTypes) != null && !t.IsEnum && !t.IsGenericTypeDefinition && !t.IsAbstract && !t.IsInterface && (type.IsAssignableFrom(t) || type.GetProperties().Select(p => p.PropertyType.StripGeneric()).Any(p => !p.IsAbstract && !p.IsInterface && typeof(IdentifiedData).IsAssignableFrom(p) && p.IsAssignableFrom(t))))
                            .ToArray();
                    }

                    serializer = new XmlSerializer(type, extraTypes);
                    this.m_serializers.TryAdd(key, serializer);

                    if (this.m_serializerKeys.TryGetValue(type, out var existingKey) &&
                        existingKey.Length < key.Length) // This one has more data than the existing
                    {
                        this.m_serializerKeys[type] = key;
                    }
                    else if (existingKey == null)
                    {
                        this.m_serializerKeys.TryAdd(type, key); // Link the key
                    }
                }
            }
            return serializer;
        }

        /// <summary>
        /// Gets a serializer which can read the specified body
        /// </summary>
        public XmlSerializer GetSerializer(XmlReader bodyReader)
        {
            var xmlroottype = typeof(XmlRootAttribute);

            var retVal = this.m_serializers.Values.FirstOrDefault(o => o.CanDeserialize(bodyReader));
            if (retVal == null)
            {
                var type = new ModelSerializationBinder().BindToType(String.Empty, bodyReader.LocalName);
                if (type == null) // Couldn't find type so attempt to do a deep resolution
                {
                    var candidates = AppDomain.CurrentDomain.GetAllTypes().Where(x => x.HasCustomAttribute(xmlroottype))
                        .Select(o => new { X = o.GetCustomAttribute(xmlroottype) as XmlRootAttribute, T = o })
                        .Where(x => x.X.Namespace == bodyReader.NamespaceURI && x.X.ElementName == bodyReader.LocalName);
                    if (!candidates.Any() || candidates.Count() > 1)
                    {
                        throw new InvalidOperationException($"{bodyReader.NamespaceURI}#{bodyReader.LocalName} is not understood or is ambiguous - are you missing a plugin?");
                    }
                    type = candidates.Single().T;
                    ModelSerializationBinder.RegisterModelType(type);
                }
                return this.CreateSerializer(type);
            }
            return retVal;
        }
    }
}