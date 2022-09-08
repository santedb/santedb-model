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
 * Date: 2022-9-2
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Security;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Indicates the reliability of an application to assign identifiers in a particular identity domain
    /// </summary>
    [XmlType(nameof(AssigningAuthority), Namespace = "http://santedb.org/model"), JsonObject(nameof(AssigningAuthority))]
    [XmlRoot(nameof(AssigningAuthority), Namespace = "http://santedb.org/model")]
    public class AssigningAuthority : BaseEntityData, ISimpleAssociation
    {

        /// <summary>
        /// Gets or sets the application which can assign identity
        /// </summary>
        [XmlElement("assigningApplication"), JsonProperty("assigningApplication")]
        public Guid? AssigningApplicationKey { get; set; }

        /// <summary>
        /// Gets or sets the assigning device
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(AssigningApplicationKey))]
        public SecurityApplication AssigningApplication { get; set; }

        /// <summary>
        /// Gets or sets the reliability of identifiers assigned by this application
        /// </summary>
        [XmlElement("reliability"), JsonProperty("reliability")]
        public IdentifierReliability Reliability { get; set; }


        /// <summary>
        /// Gets the source entity key
        /// </summary>
        [XmlElement("source"), JsonProperty("source")]
        public Guid? SourceEntityKey
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the source entity
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(SourceEntityKey))]
        public IdentityDomain SourceEntity { get; set; }

        /// <summary>
        /// Source type of authority
        /// </summary>
        public Type SourceType => typeof(IdentityDomain);

        /// <summary>
        /// Source entity
        /// </summary>
        object ISimpleAssociation.SourceEntity { get => null; set { } }
    }
}