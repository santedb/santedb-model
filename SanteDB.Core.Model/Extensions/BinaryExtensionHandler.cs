﻿/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: Justin Fyfe
 * Date: 2019-8-8
 */
using SanteDB.Core.Interfaces;

namespace SanteDB.Core.Extensions
{
    /// <summary>
    /// Binary extension handler
    /// </summary>
    public class BinaryExtensionHandler : IExtensionHandler
    {
        /// <summary>
        /// Gets the name of the handler
        /// </summary>
        public string Name
        {
            get
            {
                return "Binary";
            }
        }

        /// <summary>
        /// Deserialize data
        /// </summary> 
        public object DeSerialize(byte[] extensionData)
        {
            return extensionData;
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public T DeSerialize<T>(byte[] extensionData)
        {
            return (T)this.DeSerialize(extensionData);
        }

        /// <summary>
        /// Get display 
        /// </summary>
        public string GetDisplay(object data)
        {
            return $"Binary Data : {(data as byte[]).Length} Bytes";
        }

        /// <summary>
        /// Serialize
        /// </summary>
        public byte[] Serialize(object data)
        {
            return data as byte[];
        }
    }
}
