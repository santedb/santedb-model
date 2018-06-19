﻿/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
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
 * Date: 2017-9-1
 */
using SanteDB.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Serialize the data into byte array
        /// </summary>
        public byte[] Serialize(object data)
        {
            return BitConverter.GetBytes((bool)data);
        }
    }
}
