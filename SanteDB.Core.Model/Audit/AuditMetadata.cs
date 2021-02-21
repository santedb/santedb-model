/*
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
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System.Xml.Serialization;

namespace SanteDB.Core.Auditing
{
    /// <summary>
    /// Represents metadata keys
    /// </summary>
    [XmlType(nameof(AuditMetadataKey), Namespace = "http://santedb.org/audit")]
    public enum AuditMetadataKey
    {
        /// <summary>
        /// The metadata represents a patient identifier
        /// </summary>
        [XmlEnum("pid")]
        PID,
        /// <summary>
        /// The object is a process name
        /// </summary>
        [XmlEnum("process")]
        ProcessName,
        /// <summary>
        /// Identifies the remote host
        /// </summary>
        [XmlEnum("remoteHost")]
        RemoteHost,
        /// <summary>
        /// Identifies the remote endpoint
        /// </summary>
        [XmlEnum("remoteEp")]
        RemoteEndpoint,
        /// <summary>
        /// Identifies the local endpoint
        /// </summary>
        [XmlEnum("localEp")]
        LocalEndpoint,
        /// <summary>
        /// Identifies the time a batch was submitted
        /// </summary>
        [XmlEnum("submission_time")]
        SubmissionTime,
        /// <summary>
        /// Identifies if an object was in original format
        /// </summary>
        [XmlEnum("format")]
        OriginalFormat,
        /// <summary>
        /// Identifies the status of the object
        /// </summary>
        [XmlEnum("status")]
        SubmissionStatus,
        /// <summary>
        /// Identifies the priority of the object
        /// </summary>
        [XmlEnum("priority")]
        Priority,
        /// <summary>
        /// Identifies the object classification
        /// </summary>
        [XmlEnum("classification")]
        Classification,
        /// <summary>
        /// Identifies the object as a session identifier
        /// </summary>
        [XmlEnum("sessionId")]
        SessionId,
        /// <summary>
        /// Identifies the object as an enterprise site identifier
        /// </summary>
        [XmlEnum("enterpriseSiteId")]
        EnterpriseSiteID,
        /// <summary>
        /// Identifies the object's metadata as the source
        /// </summary>
        [XmlEnum("auditSourceId")]
        AuditSourceID,
        /// <summary>
        /// Identifies the source type
        /// </summary>
        [XmlEnum("auditSourceType")]
        AuditSourceType,
        /// <summary>
        /// Allows the correlation of data between audits
        /// </summary>
        [XmlEnum("correlationToken")]
        CorrelationToken
    }

    /// <summary>
    /// Represents audit metadata such as submission time, submission sequence, etc.
    /// </summary>
    [XmlType(nameof(AuditMetadata), Namespace = "http://santedb.org/audit")]
    [JsonObject(nameof(AuditMetadata))]
    [Classifier(nameof(Key))]
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
        [XmlAttribute("key"), JsonProperty("key"), QueryParameter("key")]
        public AuditMetadataKey Key { get; set; }

        /// <summary>
        /// Gets or sets the process name
        /// </summary>
        [XmlAttribute("value"), JsonProperty("value"), QueryParameter("value")]
        public string Value { get; set; }

    }
}