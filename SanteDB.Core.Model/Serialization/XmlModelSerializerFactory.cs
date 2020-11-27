/*
 * Copyright (C) 2019 - 2020, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2020-2-26
 */
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Collection;
using System;
using System.Collections.Generic;
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
	    private readonly Dictionary<Type, string> m_serializerKeys = new Dictionary<Type, string>();

	    // Cache of created serializers
	    private readonly Dictionary<string, XmlSerializer> m_serializers  = new Dictionary<string, XmlSerializer>();

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
                lock (m_lock) {
                    if (!this.m_serializers.ContainsKey(key)) // Ensure that hasn't been generated since lock was acquired 
                    {
                        if(type.GetCustomAttribute<ResourceCollectionAttribute>() != null && extraTypes.Length == 0)
                        {
	                        extraTypes = typeof(XmlModelSerializerFactory)
		                        .GetTypeInfo()
		                        .Assembly
		                        .ExportedTypes
                                .Where(t => typeof(IdentifiedData).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()) && !t.GetTypeInfo().IsGenericTypeDefinition && !t.GetTypeInfo().IsAbstract)
                                .Union(ModelSerializationBinder.GetRegisteredTypes())
		                        .ToArray();
                        }

                        serializer = new XmlSerializer(type, extraTypes);

                        this.m_serializers.Add(key, serializer);

                        if (this.m_serializerKeys.TryGetValue(type, out var existingKey) &&
                            existingKey.Length < key.Length) // This one has more data than the existing
                        {
	                        this.m_serializerKeys[type] = key;
                        }
                        else
                        {
	                        this.m_serializerKeys.Add(type, key); // Link the key 
                        }
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
            return this.m_serializers.Values.FirstOrDefault(o => o.CanDeserialize(bodyReader));
        }
    }
}
