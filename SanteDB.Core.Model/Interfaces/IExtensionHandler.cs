﻿/*
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2021-2-9
 */
using System;

namespace SanteDB.Core.Interfaces
{
    /// <summary>
    /// Extension handler contract
    /// </summary>
    public interface IExtensionHandler
    {

        /// <summary>
        /// Gets the name of the handler
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Represents the data as a .net value
        /// </summary>
        object DeSerialize(byte[] extensionData);

        /// <summary>
        /// Serializes the data 
        /// </summary>
        byte[] Serialize(object data);

        /// <summary>
        /// Gets the display value
        /// </summary>
        String GetDisplay(object data);

        /// <summary>
        /// De-serialize as a strongly type object
        /// </summary>
        T DeSerialize<T>(byte[] extensionValueXml);
    }
}
