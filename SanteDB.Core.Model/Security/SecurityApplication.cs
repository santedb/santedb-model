/*
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
 * User: justin
 * Date: 2018-6-22
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Represents a security application
    /// </summary>

    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityApplication")]
    [XmlType("SecurityApplication", Namespace = "http://santedb.org/model"), JsonObject("SecurityApplication")]
    public class SecurityApplication : SecurityEntity
    {
        /// <summary>
        /// Gets or sets the application secret used for authenticating the application
        /// </summary>
        [XmlElement("applicationSecret"), JsonProperty("applicationSecret")]
        public String ApplicationSecret { get; set; }

        /// <summary>
        /// Gets or sets the name of the security device/user/role/device.
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }


        /// <summary>
        /// Gets or sets the lockout time as XML date
        /// </summary>
        [XmlElement("lockout"), JsonProperty("lockout"), DataIgnore]
        public String LockoutXml
        {
            get => this.Lockout?.ToString("o", CultureInfo.InvariantCulture);
            set => this.Lockout = value == null ? null : (DateTimeOffset?)DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the lockout
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(LockoutXml))]
        public DateTimeOffset? Lockout { get; set; }

        /// <summary>
        /// Gets or sets the number of invalid authentication attempts
        /// </summary>
        [XmlElement("invalidAuth"), JsonProperty("invalidAuth")]
        public int? InvalidAuthAttempts { get; set; }

        /// <summary>
        /// Gets the last authenticated time
        /// </summary>
        [XmlElement("lastAuthentication"), JsonProperty("lastAuthentication"), DataIgnore]
        public String LastAuthenticationXml
        {
            get => this.LastAuthentication?.ToString("o", CultureInfo.InvariantCulture);
            set => this.LastAuthentication = value == null ? null : (DateTimeOffset?)DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the last authentication time as a DTO
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(LastAuthenticationXml))]
        public DateTimeOffset? LastAuthentication { get; set; }
    }
}
