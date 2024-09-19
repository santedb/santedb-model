﻿/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 */
using SanteDB.Core.Interfaces;
using System;

namespace SanteDB.Core.Extensions
{
    /// <summary>
    /// Boolean extension handler
    /// </summary>
    public class BooleanExtensionHandler : IExtensionHandler
    {
        /// <summary>
        /// Gets the name of the handler
        /// </summary>
        public string Name => "Boolean";

        /// <summary>
        /// Gets the boolean obect from a byte array
        /// </summary>
        public object DeSerialize(byte[] extensionData)
        {
            return extensionData != null && BitConverter.ToBoolean(extensionData, 0);
        }

        /// <summary>
        /// Get display name
        /// </summary>
        public string GetDisplay(object data)
        {
            return data.ToString();
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public T DeSerialize<T>(byte[] extensionData)
        {
            return (T)this.DeSerialize(extensionData);
        }

        /// <summary>
        /// Serialize the data into byte array
        /// </summary>
        public byte[] Serialize(object data)
        {
            return BitConverter.GetBytes((bool)data);
        }
    }
}
