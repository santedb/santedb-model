/*
 * Based on OpenIZ, Copyright (C) 2015 - 2019 Mohawk College of Applied Arts and Technology
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
 * Date: 2019-11-27
 */
using SanteDB.Core.Interfaces;
using SanteDB.Core.Model;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Extensions
{
    /// <summary>
    /// Reference types
    /// </summary>
    public enum ReferenceType : byte
    {
        /// <summary>
        /// The reference is to an entity
        /// </summary>
        Entity = 0x0,
        /// <summary>
        /// The reference is to an act
        /// </summary>
        Act = 0x1,
        /// <summary>
        /// The reference is to a concept
        /// </summary>
        Concept = 0x2

    }
    /// <summary>
    /// Represents an extension handler which stores references to another type
    /// </summary>
    public class ReferenceExtensionHandler : IExtensionHandler
    {
        /// <summary>
        /// Gets the name of the extension handler
        /// </summary>
        public string Name => "Reference";

        /// <summary>
        /// De-serialize the object
        /// </summary>
        public object DeSerialize(byte[] extensionData)
        {
            Guid uuid = Guid.Empty;
            ReferenceType refType = (ReferenceType)extensionData[0];
            if (extensionData.Length == 0x11) // Data in pure binary format - LEGACY FROM OPENIZ
                uuid = new Guid(extensionData.Skip(1).ToArray());
            else if (extensionData[1] == (byte)'^') // Data is from text format 
            {
                var extData = Encoding.UTF8.GetString(extensionData, 0, extensionData.Length);
                var data = extData.Split('^');
                if (data[0][0] >= (byte)'0')
                    refType = (ReferenceType)byte.Parse(data[0]);
                uuid = Guid.Parse(data[1]);
            }
            else
                throw new ArgumentOutOfRangeException(nameof(extensionData), $"Argument not in appropriate format. Expecting 11 byte binary reference or string format ({BitConverter.ToString(extensionData)}");
            switch (refType)
            {
                case ReferenceType.Act:
                    return Model.EntityLoader.EntitySource.Current.Get<Act>(uuid);
                case ReferenceType.Entity:
                    return Model.EntityLoader.EntitySource.Current.Get<Entity>(uuid);
                case ReferenceType.Concept:
                    return Model.EntityLoader.EntitySource.Current.Get<Concept>(uuid);
                default:
                    throw new InvalidOperationException($"Control type {extensionData[0]} is unknown");
            }

        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public T DeSerialize<T>(byte[] extensionData)
        {
            return (T)this.DeSerialize(extensionData);
        }

        /// <summary>
        /// Get the display data
        /// </summary>
        public string GetDisplay(object data)
        {
            return data?.ToString();
        }

        /// <summary>
        /// Serialize the object
        /// </summary>
        public byte[] Serialize(object data)
        {
            StringBuilder retVal = new StringBuilder();
            if (data is Entity)
                retVal.Append(ReferenceType.Entity);
            else if (data is Act)
                retVal.Append(ReferenceType.Act);
            else if (data is Concept)
                retVal.Append(ReferenceType.Concept);
            else
                throw new ArgumentOutOfRangeException("Only Entity, Act, or Concept can be stored in this extension type");

            retVal.AppendFormat("^{0}", (data as IdentifiedData).Key);
            return Encoding.UTF8.GetBytes(retVal.ToString());
        }
    }
}
