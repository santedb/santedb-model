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
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Represents a security device
    /// </summary>
    
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityDevice")]
    [XmlType("SecurityDevice",  Namespace = "http://santedb.org/model"), JsonObject("SecurityDevice")]
    public class SecurityDevice : SecurityEntity
    {

        /// <summary>
        /// Gets or sets the device secret
        /// </summary>
        [XmlElement("deviceSecret"), JsonProperty("deviceSecret")]
        public String DeviceSecret { get; set; }

        /// <summary>
        /// Gets or sets the name of the security device/user/role/devie
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }



    }
}
