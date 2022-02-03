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
 * Date: 2021-8-27
 */
using SanteDB.Core.Interfaces;
using System;

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
        public string Name => "Uuid";

        /// <summary>
        /// Deserialize
        /// </summary>
        public object DeSerialize(byte[] extensionData)
        {
            return new Guid(extensionData);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public T DeSerialize<T>(byte[] extensionData)
        {
            return (T)this.DeSerialize(extensionData);
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
