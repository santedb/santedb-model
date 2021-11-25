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
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a relationship between two concepts
    /// </summary>
    [Classifier(nameof(RelationshipType))]
    [XmlType("ConceptRelationship", Namespace = "http://santedb.org/model"), JsonObject("ConceptRelationship")]
    public class ConceptRelationship : VersionedAssociation<Concept>
    {

        /// <summary>
        /// Serialization default constructor
        /// </summary>
        public ConceptRelationship()
        {

        }

        /// <summary>
        /// Creates a new concept relationship between the holder concept and <paramref name="targetConceptKey"/>
        /// </summary>
        /// <param name="relationshipType">The type of relationship the source has with the <paramref name="targetConceptKey"/></param>
        /// <param name="targetConceptKey">The type of relationship which exists between the source and target</param>
        public ConceptRelationship(Guid relationshipType, Guid targetConceptKey)
        {
            this.TargetConceptKey = targetConceptKey;
            this.RelationshipTypeKey = relationshipType;
        }

        /// <summary>
        /// Gets or sets the target concept identifier
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("targetConcept"), JsonProperty("targetConcept")]
        public Guid? TargetConceptKey { get; set; }

        /// <summary>
        /// Gets or sets the target concept
        /// </summary>
        [SerializationReference(nameof(TargetConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept TargetConcept { get; set; }

        /// <summary>
        /// Relationship type
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Binding(typeof(ConceptRelationshipTypeKeys))]
        [XmlElement("relationshipType"), JsonProperty("relationshipType")]
        public Guid? RelationshipTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the relationship type
        /// </summary>
        [SerializationReference(nameof(RelationshipTypeKey))]
        [XmlIgnore, JsonIgnore]
        public ConceptRelationshipType RelationshipType { get; set; }

    }
}