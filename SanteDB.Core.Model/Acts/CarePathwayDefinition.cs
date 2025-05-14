/*
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
 * Date: 2024-12-12
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents a single care pathway
    /// </summary>
    [XmlType(nameof(CarePathwayDefinition), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(CarePathwayDefinition), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(CarePathwayDefinition))]
    public class CarePathwayDefinition : NonVersionedEntityData
    {
        /// <summary>
        /// Gets or sets the mnemonic for the care pathway
        /// </summary>
        [XmlElement("mnemonic"), JsonProperty("mnemonic")]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Gets or sets the name of the care pathway
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the care pathway
        /// </summary>
        [XmlElement("description"), JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the eligibility criteria (when someone can be enroled to create a care plan)
        /// </summary>
        [XmlElement("eligibility"), JsonProperty("eligibility")]
        public string EligibilityCriteria { get; set; }

        /// <summary>
        /// Gets or sets the enrolment mode
        /// </summary>
        [XmlElement("enrollment"), JsonProperty("enrollment")]
        public CarePathwayEnrollmentMode EnrollmentMode { get; set; }

        /// <summary>
        /// Gets or sets the type of encounter 
        /// </summary>
        [XmlElement("encounterTemplate"), JsonProperty("encounterTemplate")]
        public Guid? TemplateKey { get; set; }

        /// <summary>
        /// Identifies the template definition (delay load for <see cref="TemplateKey"/>)
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(TemplateKey))]
        public TemplateDefinition Template { get; set; }
    }
}