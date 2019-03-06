/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: justi
 * Date: 2019-1-12
 */
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SanteDB.Core.Auditing
{
    /// <summary>
    /// Represents metadata keys
    /// </summary>
    [XmlType(nameof(AuditMetadataKey), Namespace = "http://santedb.org/audit")]
    public enum AuditMetadataKey
    {
        [XmlEnum("pid")]
        PID,
        [XmlEnum("process")]
        ProcessName,
        [XmlEnum("remoteHost")]
        RemoteHost,
        [XmlEnum("remoteEp")]
        RemoteEndpoint,
        [XmlEnum("localEp")]
        LocalEndpoint,
        [XmlEnum("submission_time")]
        SubmissionTime,
        [XmlEnum("format")]
        OriginalFormat,
        [XmlEnum("status")]
        SubmissionStatus,
        [XmlEnum("priority")]
        Priority,
        [XmlEnum("classification")]
        Classification,
        [XmlEnum("sessionId")]
        SessionId,
        [XmlEnum("enterpriseSiteId")]
        EnterpriseSiteID,
        [XmlEnum("auditSourceId")]
        AuditSourceID,
        [XmlEnum("auditSourceType")]
        AuditSourceType
    }

    /// <summary>
    /// Represents audit metadata such as submission time, submission sequence, etc.
    /// </summary>
    [XmlType(nameof(AuditMetadata), Namespace = "http://santedb.org/audit")]
    [JsonObject(nameof(AuditMetadata))]
    public class AuditMetadata 
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public AuditMetadata()
        {

        }

        /// <summary>
        /// Create new audit metadata with specified key and value
        /// </summary>
        public AuditMetadata(AuditMetadataKey key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the metadata key
        /// </summary>
        [XmlAttribute("key"), JsonProperty("key")]
        public AuditMetadataKey Key { get; set; }

        /// <summary>
        /// Gets or sets the process name
        /// </summary>
        [XmlAttribute("value"), JsonProperty("value")]
        public string Value { get; set; }

    }
}