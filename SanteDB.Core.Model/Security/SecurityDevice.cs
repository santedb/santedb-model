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
 * Date: 2023-3-10
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Represents a security device
    /// </summary>

    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityDevice")]
    [XmlType("SecurityDevice", Namespace = "http://santedb.org/model"), JsonObject("SecurityDevice")]
    public class SecurityDevice : SecurityEntity
    {

        /// <summary>
        /// Gets or sets the device secret
        /// </summary>
        [XmlElement("deviceSecret"), JsonProperty("deviceSecret")]
        public String DeviceSecret { get; set; }

        /// <summary>
        /// Gets the public key of the security device
        /// </summary>
        [XmlElement("publicKey"), JsonProperty("publicKey")]
        public String PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the security device/user/role/device
        /// </summary>
        [XmlElement("name"), JsonProperty("name"), NoCase]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the lockout time as XML date
        /// </summary>
        [XmlElement("lockout"), JsonProperty("lockout"), SerializationMetadata]
        public String LockoutXml
        {
            get => this.Lockout?.ToString("o", CultureInfo.InvariantCulture);
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                    {
                        this.Lockout = val;
                    }
                    else
                    {
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                    }
                }
                else
                {
                    this.Lockout = default(DateTimeOffset);
                }
            }
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
        [XmlElement("lastAuthenticationTime"), JsonProperty("lastAuthenticationTime"), SerializationMetadata]
        public String LastAuthenticationXml
        {
            get => this.LastAuthentication?.ToString("o", CultureInfo.InvariantCulture);
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                    {
                        this.LastAuthentication = val;
                    }
                    else
                    {
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                    }
                }
                else
                {
                    this.LastAuthentication = default(DateTimeOffset);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last authentication time as a DTO
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(LastAuthenticationXml))]
        public DateTimeOffset? LastAuthentication { get; set; }

        /// <summary>
        /// Get the name of the object as a display string
        /// </summary>
        public override string ToDisplay() => $"{this.Name} [{this.Key}]";
    }
}
