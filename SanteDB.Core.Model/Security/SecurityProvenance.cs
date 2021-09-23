/*
 * Copyright (C) 2021 - 2021, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-5
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Represents security provenance information
    /// </summary>
    [XmlType(nameof(SecurityProvenance), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(SecurityProvenance), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(SecurityProvenance))]
    public class SecurityProvenance : IdentifiedData
    {

        // User
        private SecurityUser m_user;
        private SecurityApplication m_application;
        private SecurityDevice m_device;

        /// <summary>
        /// Gets the time that the provenance was modified / created
        /// </summary>
        public override DateTimeOffset ModifiedOn => this.CreationTime;

        /// <summary>
        /// Gets or sets the time at which the data was created
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CreationTimeXml))]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time in XML format
        /// </summary>
        [XmlElement("creationTime"), JsonProperty("creationTime"), DataIgnore()]
        public String CreationTimeXml
        {
            get
            {
                if (this.CreationTime == default(DateTimeOffset))
                    return null;
                else
                    return this.CreationTime.ToString("o", CultureInfo.InvariantCulture);
            }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.CreationTime = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else this.CreationTime = default(DateTimeOffset);
            }
        }

        /// <summary>
        /// Gets or sets the application key
        /// </summary>
        [XmlElement("application"), JsonProperty("application")]
        public Guid? ApplicationKey { get; set; }

        /// <summary>
        /// Gets or sets the user key
        /// </summary>
        [XmlElement("user"), JsonProperty("user")]
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Gets or sets the device key 
        /// </summary>
        [XmlElement("device"), JsonProperty("device")]
        public Guid? DeviceKey { get; set; }

        /// <summary>
        /// Gets or sets the session
        /// </summary>
        [XmlElement("session"), JsonProperty("session")]
        public Guid? SessionKey { get; set; }

        /// <summary>
        /// Gets or sets the external security object reference
        /// </summary>
        [XmlElement("extern"), JsonProperty("extern")]
        public Guid? ExternalSecurityObjectRefKey { get; set; }

        /// <summary>
        /// Gets the type of object that the external key references
        /// </summary>
        [XmlElement("externClass"), JsonProperty("externClass")]
        public String ExternalSecurityObjectRefType { get; set; }

        /// <summary>
        /// Gets the security user for the provenance if applicable
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(DeviceKey))]
        public SecurityDevice Device
        {
            get
            {
                this.m_device = base.DelayLoad(this.DeviceKey, this.m_device);
                return this.m_device;
            }
            set
            {
                this.m_device = value;
                this.DeviceKey = value?.Key;
            }
        }


        /// <summary>
        /// Gets the security user for the provenance if applicable
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(UserKey))]
        public SecurityUser User
        {
            get
            {
                this.m_user = base.DelayLoad(this.UserKey, this.m_user);
                return this.m_user;
            }
            set
            {
                this.m_user = value;
                this.UserKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets the security application for the provenance if applicable
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(ApplicationKey))]
        public SecurityApplication Application
        {
            get
            {
                this.m_application = base.DelayLoad(this.ApplicationKey, this.m_application);
                return this.m_application;
            }
            set
            {
                this.m_application = value;
                this.ApplicationKey = value?.Key;
            }
        }

    }
}
