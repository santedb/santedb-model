using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Serialization
{
    /// <summary>
    /// Represents an XML resolver that uses embedded data
    /// </summary>
    public class EmbeddedResourceXmlResolver : XmlResolver
    {
        private readonly Assembly m_assembly;
        private readonly string m_rootNamespace;

        /// <summary>
        /// Createa a new XML resolver
        /// </summary>
        /// <param name="baseAssembly">The base assembly</param>
        /// <param name="manifestResourceStreamRoot">The resource stream root</param>
        public EmbeddedResourceXmlResolver(Assembly baseAssembly, String manifestResourceStreamRoot)
        {
            this.m_assembly = baseAssembly;
            this.m_rootNamespace = manifestResourceStreamRoot;
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if(absoluteUri == null)
            {
                throw new ArgumentNullException(nameof(absoluteUri));
            }
            else if(absoluteUri.Scheme != "file")
            {
                throw new ArgumentOutOfRangeException(nameof(absoluteUri));
            }

            var streamName = $"{this.m_rootNamespace}.{absoluteUri.Segments.Last()}";
            var stream = this.m_assembly.GetManifestResourceStream(streamName);
            if(stream == null)
            {
                throw new FileNotFoundException($"{streamName}");
            }
            return stream;
        }
    }
}
