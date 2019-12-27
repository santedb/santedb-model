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
 * User: Justin Fyfe
 * Date: 2019-8-8
 */
using Newtonsoft.Json;
using SanteDB.Core.Model;

/*
* Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
* Date: 7-5-2012
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SanteDB.Core.Auditing
{
    /// <summary>
    /// Represents an event identifier type.
    /// </summary>
    [XmlType(nameof(EventIdentifierType), Namespace = "http://santedb.org/audit"), Flags]
    public enum EventIdentifierType
    {
        /// <summary>
        /// Represents a provisioning event.
        /// </summary>
        [XmlEnum("provision")]
        ProvisioningEvent = 0x01,

        /// <summary>
        /// Represents a medication event.
        /// </summary>
        [XmlEnum("medication")]
        MedicationEvent = 0x02,

        /// <summary>
        /// Represents a resource assignment.
        /// </summary>
        [XmlEnum("resource")]
        ResourceAssignment = 0x04,

        /// <summary>
        /// Represents a care episode.
        /// </summary>
        [XmlEnum("careep")]
        CareEpisode = 0x08,

        /// <summary>
        /// Represents a care protocol.
        /// </summary>
        [XmlEnum("careprotocol")]
        CareProtocol = 0x10,

        /// <summary>
        /// Represents a procedure record.
        /// </summary>
        [XmlEnum("procedure")]
        ProcedureRecord = 0x20,

        /// <summary>
        /// Represents a query.
        /// </summary>
        [XmlEnum("query")]
        Query = 0x40,

        /// <summary>
        /// Represents a patient record.
        /// </summary>
        [XmlEnum("patient")]
        PatientRecord = 0x80,

        /// <summary>
        /// Represents an order record.
        /// </summary>
        [XmlEnum("order")]
        OrderRecord = 0x100,

        /// <summary>
        /// Represents a network entry.
        /// </summary>
        [XmlEnum("network")]
        NetworkEntry = 0x200,

        /// <summary>
        /// Represents an import.
        /// </summary>
        [XmlEnum("import")]
        Import = 0x400,

        /// <summary>
        /// Represents an export.
        /// </summary>
        [XmlEnum("export")]
        Export = 0x800,

        /// <summary>
        /// Represents application activity.
        /// </summary>
        [XmlEnum("application")]
        ApplicationActivity = 0x1000,

        /// <summary>
        /// Represents a security alert.
        /// </summary>
        [XmlEnum("security")]
        SecurityAlert = 0x2000,

        /// <summary>
        /// Represents user authentication.
        /// </summary>
        [XmlEnum("auth")]
        UserAuthentication = 0x4000,

        /// <summary>
        /// Represents that an emergency override started.
        /// </summary>
        [XmlEnum("btg")]
        EmergencyOverrideStarted = 0x8000,

        /// <summary>
        /// Represents the use of a restricted function.
        /// </summary>
        [XmlEnum("restrictedFn")]
        UseOfRestrictedFunction = 0x10000,

        /// <summary>
        /// Represents a login.
        /// </summary>
        [XmlEnum("login")]
        Login = 0x20000,

        /// <summary>
        /// Represents a logout.
        /// </summary>
        [XmlEnum("logout")]
        Logout = 0x40000
    }

    /// <summary>
    /// Specific information related to an audit
    /// </summary>
    [XmlType(nameof(AuditData), Namespace = "http://santedb.org/audit")]
    [XmlRoot("Audit", Namespace = "http://marc-hi.ca/audit")]
    [JsonObject(nameof(AuditData))]
    public class AuditData : IdentifiedData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditData"/> class.
        /// </summary>
        public AuditData()
        {
            this.Timestamp = DateTime.Now;
            this.Actors = new List<AuditActorData>();
            this.AuditableObjects = new List<AuditableObject>();
            this.Metadata = new List<AuditMetadata>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditData"/> class.
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="actionCode">The action code.</param>
        /// <param name="outcome">The outcome.</param>
        /// <param name="eventIdentifier">The event identifier.</param>
        /// <param name="eventTypeCode">The event type code.</param>
        public AuditData(DateTime timeStamp, ActionType actionCode, OutcomeIndicator outcome,
            EventIdentifierType eventIdentifier, AuditCode eventTypeCode)
            : this()
        {
            this.Timestamp = timeStamp;
            this.ActionCode = actionCode;
            this.Outcome = outcome;
            this.EventIdentifier = eventIdentifier;
            this.EventTypeCode = eventTypeCode;
            this.Actors = new List<AuditActorData>();
            this.AuditableObjects = new List<AuditableObject>();
        }

        /// <summary>
        /// Identifies the action code
        /// </summary>
        [XmlElement("action"), JsonProperty("action")]
        public ActionType ActionCode { get; set; }

        /// <summary>
        /// Represents the actors within the audit event
        /// </summary>
        [XmlElement("actor"), JsonProperty("actor")]
        public List<AuditActorData> Actors { get; set; }

        /// <summary>
        /// Represents other objects of interest
        /// </summary>
        [XmlElement("object"), JsonProperty("object")]
        public List<AuditableObject> AuditableObjects { get; set; }

        /// <summary>
        /// Identifies the event
        /// </summary>
        [XmlElement("event"), JsonProperty("event")]
        public EventIdentifierType EventIdentifier { get; set; }

        /// <summary>
        /// Identifies the type of event
        /// </summary>
        [XmlElement("type"), JsonProperty("type")]
        public AuditCode EventTypeCode { get; set; }

        /// <summary>
        /// Identifies the outcome of the event
        /// </summary>
        [XmlElement("outcome"), JsonProperty("outcome")]
        public OutcomeIndicator Outcome { get; set; }

        /// <summary>
        /// Event timestamp
        /// </summary>
        [XmlElement("timestamp"), JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Metadata about the audit 
        /// </summary>
        [XmlElement("meta"), JsonProperty("meta")]
        public List<AuditMetadata> Metadata { get; set; }

        /// <summary>
        /// Represents the modified on
        /// </summary>
        public override DateTimeOffset ModifiedOn => this.Timestamp;

        /// <summary>
        /// Add metadata to the audit
        /// </summary>
        public void AddMetadata(AuditMetadataKey key, String value)
        {
            this.Metadata.Add(new AuditMetadata(key, value));
        }

        /// <summary>
        /// Represent as a display string
        /// </summary>
        public override string ToDisplay()
        {
            using (var sw = new StringWriter())
            {
                new XmlSerializer(typeof(AuditData)).Serialize(sw, this);
                return sw.ToString();
            }
        }
    }
}