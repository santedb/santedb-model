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
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents information related to the clinical protocol to which an act is a member of
    /// </summary>
    /// <remarks>
    /// The <see cref="ActProtocol"/> class is used to link an act instance (<see cref="Act"/>) with the clinical 
    /// protocol (<see cref="Protocol"/>) to which the act belongs.
    /// </remarks>
    [XmlType(nameof(ActProtocol), Namespace = "http://santedb.org/model"), JsonObject(nameof(ActProtocol))]
    public class ActProtocol : Association<Act>
    {

        // The protocol
        private Protocol m_protocol;

        /// <summary>
        /// Gets or sets the protocol  to which this act belongs
        /// </summary>
        [XmlElement("protocol"), JsonProperty("protocol")]
        public Guid? ProtocolKey { get; set; }

        /// <summary>
        /// Gets or sets the protocol data related to the protocol
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReferenceAttribute(nameof(ProtocolKey)), SerializationMetadata]
        public Protocol Protocol
        {
            get => this.m_protocol;
            set => this.m_protocol = value;
        }

        /// <summary>
        /// Gets the version of the protocol that was used to generate the 
        /// </summary>
        [XmlElement("version"), JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Represents the sequence of the act in the protocol
        /// </summary>
        [XmlElement("sequence"), JsonProperty("sequence")]
        public int Sequence { get; set; }

        /// <summary>
        /// Represents any state data related to the act / protocol link
        /// </summary>
        [XmlElement("state"), JsonProperty("state")]
        public byte[] StateData { get; set; }

        /// <summary>
        /// Determines equality of this association
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as ActProtocol;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) && other.ProtocolKey == this.ProtocolKey;
        }

        /// <summary>
        /// Shoud serialize source
        /// </summary>
        public override bool ShouldSerializeSourceEntityKey() => false;
    }
}