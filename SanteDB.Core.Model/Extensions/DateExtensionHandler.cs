﻿/*
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
using System;
using System.Text;

namespace SanteDB.Core.Extensions
{
    /// <summary>
    /// Date extension handler
    /// </summary>
    public class DateExtensionHandler : IExtensionHandler
    {
        /// <summary>
        /// Gets the name of the handler
        /// </summary>
        public string Name
        {
            get
            {
                return "Date";
            }
        }

        /// <summary>
        /// Serialize to bytes
        /// </summary>
        public object DeSerialize(byte[] extensionData)
        {
            if (extensionData.Length == 8)
            {
                Int64 tickData = BitConverter.ToInt64(extensionData, 0);
                return new DateTime(tickData, DateTimeKind.Utc);
            }
            else
            {
                string sdata = Encoding.UTF8.GetString(extensionData, 0, extensionData.Length);
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(sdata, out dt))
                    return dt;
                throw new InvalidOperationException("Cannot parse data");
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
        /// Get the display value
        /// </summary>
        public string GetDisplay(object data)
        {
            return ((DateTime)data).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Serialize
        /// </summary>
        public byte[] Serialize(object data)
        {
            DateTime dt = (DateTime)data;
            if (dt.Kind == DateTimeKind.Local)
                dt = dt.ToUniversalTime(); // adjust to UTC ticks
            return BitConverter.GetBytes(dt.Ticks);
        }
    }
}
