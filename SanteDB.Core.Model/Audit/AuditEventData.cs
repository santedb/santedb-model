/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2022-5-30
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Audit
{

    /// <summary>
    /// Represents a single audit event 
    /// </summary>
    /// <remarks><para>This class contains the information for a single audit event that occurs within the SanteDB system or 
    /// to be dispatched to an upstream audit system.</para></remarks>
    [XmlType(nameof(AuditEventData), Namespace = "http://santedb.org/audit")]
    [XmlRoot("Audit", Namespace = "http://santedb.org/audit")]
    [JsonObject(nameof(AuditEventData))]
    public class AuditEventData : IdentifiedData
    {

        // Serializer ref
        private static XmlSerializer s_serializer = XmlModelSerializerFactory.Current.CreateSerializer(typeof(AuditEventData));

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditEventData"/> class.
        /// </summary>
        public AuditEventData()
        {
            this.Timestamp = DateTime.Now;
            this.Metadata = new List<AuditMetadata>();
            this.AuditableObjects = new List<AuditableObject>();
            this.Actors = new List<AuditActorData>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditEventData"/> class.
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="actionCode">The action code.</param>
        /// <param name="outcome">The outcome.</param>
        /// <param name="eventIdentifier">The event identifier.</param>
        /// <param name="eventTypeCode">The event type code.</param>
        public AuditEventData(DateTimeOffset timeStamp, ActionType actionCode, OutcomeIndicator outcome,
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
        /// Gets or sets the action performed code
        /// </summary>
        /// <remarks>This property indicates the type of action that was performed</remarks>
        [XmlElement("action"), JsonProperty("action")]
        public ActionType ActionCode { get; set; }

        /// <summary>
        /// Gets or sets the actors involved in the event
        /// </summary>
        /// <remarks>This property stores the actors (people, users, devices, etc.) which were 
        /// responsible, or involved in the audit action.</remarks>
        [XmlElement("actor"), JsonProperty("actor")]
        public List<AuditActorData> Actors { get; set; }

        /// <summary>
        /// Gets or sets the objects which were actioned on
        /// </summary>
        /// <remarks>This property contains the objects (patients, acts, persons, places, etc.)
        /// which were actioned or part of this audit event. Audit objects may also include metadata such as 
        /// queries and names of transactions.</remarks>
        [XmlElement("object"), JsonProperty("object")]
        public List<AuditableObject> AuditableObjects { get; set; }

        /// <summary>
        /// Gets or sets the classification of the event
        /// </summary>
        /// <remarks>The event identifier provides a classification of th event which occurred, such 
        /// as a security event, application event, etc.</remarks>
        [XmlElement("event"), JsonProperty("event")]
        public EventIdentifierType EventIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the custom type of event
        /// </summary>
        /// <remarks>This property is used to represent the a custom 
        /// type of event which ocucrred.</remarks>
        [XmlElement("type"), JsonProperty("type")]
        public AuditCode EventTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the value indicating the outcome of the event
        /// </summary>
        /// <remarks>This property is used to indicate whether the event which the audit 
        /// represents was successful, a failure, etc.</remarks>
        [XmlElement("outcome"), JsonProperty("outcome")]
        public OutcomeIndicator Outcome { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the audit
        /// </summary>
        /// <remarks>This is the time that the event occurred</remarks>
        [XmlElement("timestamp"), JsonProperty("timestamp")]
        public string TimestampXml
        {
            get => XmlConvert.ToString(this.Timestamp);
            set
            {
                if (!DateTimeOffset.TryParse(value, out var parsed))
                {
                    parsed = XmlConvert.ToDateTimeOffset(value);
                }
                this.Timestamp = parsed;
            }
        }

        /// <summary>
        /// Gets or sets the timestamp for the object
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets metadata about the audit 
        /// </summary>
        /// <remarks>The metadata object is used to store information about the event such as the process name,
        /// process ID, process user, etc.</remarks>
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
                s_serializer.Serialize(sw, this);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Represent as a string
        /// </summary>
        public override string ToString() => $"AUDIT [OUTCOME={this.Outcome}, EVENT={this.EventIdentifier}]";
    }
}