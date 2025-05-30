﻿/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-6-21
 */
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Audit
{
    /// <summary>
    /// Data related to actors that participate in the event
    /// </summary>
	/// <remarks><para>In SanteDB an actor represents an individual participant in the action which can include a user, 
	/// a system, a device, etc.</para></remarks>
    [XmlType(nameof(AuditActorData), Namespace = "http://santedb.org/audit")]
    [JsonObject(nameof(AuditActorData))]
    public class AuditActorData
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public AuditActorData() { ActorRoleCode = new List<AuditCode>(); }

        /// <summary>
        /// Identifies the role(s) that the actor has played
        /// </summary>
        [XmlElement("role"), JsonProperty("role")]
        public List<AuditCode> ActorRoleCode { get; set; }

        /// <summary>
        /// Alternative user identifier
        /// </summary>
        [XmlAttribute("altUid"), JsonProperty("altUid")]
        public string AlternativeUserId { get; set; }

        /// <summary>
        /// Identifies the network access point from which the user accessed the system
        /// </summary>
        [XmlElement("apId"), JsonProperty("apId")]
        public string NetworkAccessPointId { get; set; }

        /// <summary>
        /// Identifies the type of network access point
        /// </summary>
        [XmlElement("apType"), JsonProperty("apType")]
        public NetworkAccessPointType NetworkAccessPointType { get; set; }

        /// <summary>
        /// Network access point type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool NetworkAccessPointTypeSpecified { get { return (int)this.NetworkAccessPointType != 0; } }

        /// <summary>
        /// The unique identifier for the user in the system
        /// </summary>
        [XmlElement("uid"), JsonProperty("uid")]
        public string UserIdentifier { get; set; }

        /// <summary>
        /// True if the user is the primary requestor
        /// </summary>
        [XmlElement("isReq"), JsonProperty("isReq")]
        public bool UserIsRequestor { get; set; }

        /// <summary>
        /// The name of the user in the system
        /// </summary>
        [XmlElement("uname"), JsonProperty("uname")]
        public string UserName { get; set; }
    }
}