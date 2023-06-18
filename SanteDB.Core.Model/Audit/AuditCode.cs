/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-5-19
 */
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Audit
{
    /// <summary>
    /// Represents an audit code which has a code system and a value
    /// </summary>
    [XmlType(nameof(AuditCode), Namespace = "http://santedb.org/audit")]
    [JsonObject(nameof(AuditCode))]
    public class AuditCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditCode"/> class.
        /// </summary>
        public AuditCode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditCode"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="codeSystem">The code system.</param>
        public AuditCode(string code, string codeSystem)
        {
            this.Code = code;
            this.CodeSystem = codeSystem;
        }

        /// <summary>
        /// Gets or sets the code of the code value.
        /// </summary>
        [XmlElement("code"), JsonProperty("code")]
        public String Code { get; set; }

        /// <summary>
        /// Gets or sets the system in which the code value is drawn.
        /// </summary>
        [XmlElement("system"), JsonProperty("system")]
        public String CodeSystem { get; set; }

        /// <summary>
        /// Gets or sets the human readable name of the code system.
        /// </summary>
        [XmlElement("systemName"), JsonProperty("systemName")]
        public string CodeSystemName { get; set; }

        /// <summary>
        /// Gets or sets the version of the code system.
        /// </summary>
        [XmlElement("systemVersion"), JsonProperty("systemVersion")]
        public string CodeSystemVersion { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [XmlElement("display"), JsonProperty("display")]
        public String DisplayName { get; set; }
    }
}