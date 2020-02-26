using SanteDB.Core.Model.Collection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        // Cache of created serializers
        private Dictionary<string, XmlSerializer> m_serializers  = new Dictionary<string, XmlSerializer>();

        // Get serializer keys for a registered type
        private Dictionary<Type, String> m_serializerKeys = new Dictionary<Type, string>();

        // Current instance
        private static XmlModelSerializerFactory m_current;
        // Lock
        private static object m_lock = new object();

        /// <summary>
        /// Private ctor
        /// </summary>
        private XmlModelSerializerFactory()
        {

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
            if (extraTypes.Length > 0 || !this.m_serializerKeys.TryGetValue(type, out String key))
            {
                key = type.AssemblyQualifiedName;
                if (extraTypes != null)
                    key += String.Join(";", extraTypes.Select(o => o.AssemblyQualifiedName));
            }

            // Exists?
            if (!this.m_serializers.TryGetValue(key, out XmlSerializer serializer))
            {
                lock (m_lock) {
                    if (!this.m_serializers.ContainsKey(key)) // Ensure that hasn't been generated since lock was acquired 
                    {
                        if(typeof(Bundle).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) && extraTypes.Length == 0)
                             extraTypes = typeof(XmlModelSerializerFactory)
                                    .GetTypeInfo()
                                    .Assembly
                                    .ExportedTypes
                                    .Union(ModelSerializationBinder.GetRegisteredTypes())
                                    .Where(t => typeof(IdentifiedData).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()) && !t.GetTypeInfo().IsGenericTypeDefinition && !t.GetTypeInfo().IsAbstract)
                                    .ToArray();
                        serializer = new XmlSerializer(type, extraTypes);

                        this.m_serializers.Add(key, serializer);

                        if (this.m_serializerKeys.TryGetValue(type, out string existingKey) &&
                            existingKey.Length < key.Length) // This one has more data than the existing
                            this.m_serializerKeys[type] = key;
                        else
                            this.m_serializerKeys.Add(type, key); // Link the key 
                    }
                }
            }
            return serializer;

        }

        /// <summary>
        /// Gets the current instance of the serializer factory
        /// </summary>
        public static XmlModelSerializerFactory Current
        {
            get
            {
                if (m_current == null)
                    lock (m_lock)
                        if (m_current == null)
                            m_current = new XmlModelSerializerFactory();
                return m_current;
            }
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
