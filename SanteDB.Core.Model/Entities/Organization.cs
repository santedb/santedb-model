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
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Organization entity
    /// </summary>

    [XmlType("Organization", Namespace = "http://santedb.org/model"), JsonObject("Organization")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Organization")]
    [ClassConceptKey(EntityClassKeyStrings.Organization)]
    public class Organization : Entity
    {
        

        // Industry Concept
        /// <summary>
        /// Organization ctor
        /// </summary>
        public Organization()
        {
            this.DeterminerConceptKey = DeterminerKeys.Specific;
            this.ClassConceptKey = EntityClassKeys.Organization;
        }

        /// <summary>
        /// Gets or sets the class concept key
        /// </summary>
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        public override Guid? ClassConceptKey { get => EntityClassKeys.Organization; set => base.ClassConceptKey = EntityClassKeys.Organization; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Organization"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public Organization(Guid key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the industry concept key
        /// </summary>

        /// <summary>
        /// Gets or sets the industry in which the organization operates
        /// </summary>
        /// <remarks>
        /// The industry concept is used to classify the industrial sector to which an organization belongs. For example,
        /// an organization may be of type NGO, but the industry in which that organization operates is Healthcare
        /// </remarks>
        /// <see cref="IndustryConceptKey"/>
        [SerializationReference(nameof(IndustryConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept IndustryConcept { get; set; }

        /// <summary>
        /// Gets or sets the concept key which classifies the industry in which the organization operates
        /// </summary>
        /// <remarks>
        /// The industry concept is used to classify the industrial sector to which an organization belongs. For example,
        /// an organization may be of type NGO, but the industry in which that organization operates is Healthcare
        /// </remarks>
        /// <see cref="IndustryConcept"/>
        [XmlElement("industryConcept"), JsonProperty("industryConcept")]
        public Guid? IndustryConceptKey { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Organization;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.IndustryConceptKey == other.IndustryConceptKey;
        }
    }
}