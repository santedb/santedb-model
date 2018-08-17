using SanteDB.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Extensions
{
    /// <summary>
    /// Represents a concept extension handler
    /// </summary>
    public class UuidExtensionHandler : IExtensionHandler
    {
        /// <summary>
        /// Get the name of the extension handler
        /// </summary>
        public string Name => "Concept";

        /// <summary>
        /// Deserialize
        /// </summary>
        public object DeSerialize(byte[] extensionData)
        {
            return new Guid(extensionData);
        }

        /// <summary>
        /// Get the display value
        /// </summary>
        public string GetDisplay(object data)
        {
            return data.ToString();
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public byte[] Serialize(object data)
        {
            return ((Guid)data).ToByteArray();
        }
    }
}
