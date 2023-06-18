/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-5-19
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a reference term relationship between a concept and reference term
    /// </summary>
    [Classifier(nameof(ReferenceTerm))]
    [XmlType("ConceptReferenceTerm", Namespace = "http://santedb.org/model"), JsonObject("ConceptReferenceTerm")]
    public class ConceptReferenceTerm : VersionedAssociation<Concept>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptReferenceTerm"/> class.
        /// </summary>
        public ConceptReferenceTerm()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptReferenceTerm"/> class.
        /// </summary>
        /// <param name="referenceTermKey">The reference term identifier.</param>
        /// <param name="relationshipTypeKey">The relationship type identifier.</param>
        public ConceptReferenceTerm(Guid? referenceTermKey, Guid? relationshipTypeKey)
        {
            this.RelationshipTypeKey = relationshipTypeKey;
            this.ReferenceTermKey = referenceTermKey;
        }

        /// <summary>
        /// Gets or sets the reference term identifier
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("term"), JsonProperty("term")]
        public Guid? ReferenceTermKey { get; set; }

        /// <summary>
        /// Gets or set the reference term
        /// </summary>
        [SerializationReference(nameof(ReferenceTermKey))]
        [XmlIgnore, JsonIgnore]
        public ReferenceTerm ReferenceTerm { get; set; }

        /// <summary>
        /// Gets or sets the relationship type identifier
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("relationshipType"), JsonProperty("relationshipType")]
        public Guid? RelationshipTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the relationship type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(RelationshipTypeKey))]
        public ConceptRelationshipType RelationshipType { get; set; }

    }
}