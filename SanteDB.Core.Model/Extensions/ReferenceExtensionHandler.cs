using SanteDB.Core.Interfaces;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Extensions
{
    /// <summary>
    /// Indicates the type of object the reference is point at
    /// </summary>
    public enum ReferenceExtensionType : byte
    {
        /// <summary>
        /// The reference is an act
        /// </summary>
        Act = 1,
        /// <summary>
        /// The reference is an entity
        /// </summary>
        Entity = 2,
        /// <summary>
        /// The reference is a concept
        /// </summary>
        Concept = 3
    };

    /// <summary>
    /// Represents a reference exension handler which compactly references other objects 
    /// </summary>
    /// <remarks>This is really just a binary reference handler where the first byte of the data stream
    /// indicates the type of the reference and the following 16 bytes reference the key</remarks>
    public class ReferenceExtensionHandler : IExtensionHandler
    {
        
        /// <summary>
        /// Gets the name of this extension
        /// </summary>
        public string Name => "Reference";

        /// <summary>
        /// De-serialize the reference
        /// </summary>
        /// <param name="extensionData"></param>
        /// <returns></returns>
        public object DeSerialize(byte[] extensionData)
        {
            Guid key = new Guid(extensionData.Skip(1).ToArray());
            switch(extensionData[0])
            {
                case (byte)ReferenceExtensionType.Act:
                    return EntityLoader.EntitySource.Current.Get<Act>(key);
                case (byte)ReferenceExtensionType.Entity:
                    return EntityLoader.EntitySource.Current.Get<Entity>(key);
                case (byte)ReferenceExtensionType.Concept:
                    return EntityLoader.EntitySource.Current.Get<Concept>(key);
                default:
                    throw new ArgumentException($"Reference type byte {extensionData[0]} is invalid");
            }
        }

        /// <summary>
        /// Get the display 
        /// </summary>
        public string GetDisplay(object data)
        {
            return data?.ToString();
        }

        /// <summary>
        /// Serialize the reference
        /// </summary>
        public byte[] Serialize(object data)
        {
            byte[] retVal = new byte[17];
            if (data is Act)
                retVal[0] = (byte)ReferenceExtensionType.Act;
            else if (data is Entity)
                retVal[0] = (byte)ReferenceExtensionType.Entity;
            else if (data is Concept)
                retVal[0] = (byte)ReferenceExtensionType.Concept;
            else
                throw new ArgumentOutOfRangeException("Only types Act, Entity, or Concept can be referenced in extensions");

            // Copy uuid
            Array.Copy((data as IdentifiedData).Key.Value.ToByteArray(), 0, retVal, 1, 16);

            return retVal;
        }
    }
}
