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
using SanteDB.Core.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Used to link two or more acts together
    /// </summary>
    /// <remarks>
    /// <para>
    /// An act relationship is used to link a source act with a target act (<see cref="TargetActKey"/>) in a particular type of
    /// relationship (<see cref="RelationshipTypeKey"/>). This structure is often used to link together sub-components of an
    /// encounter with the encounter, but can also be used to link together chronic care episodes.
    /// </para>
    /// </remarks>
    [Classifier(nameof(RelationshipType)), NonCached]
    [XmlType("ActRelationship", Namespace = "http://santedb.org/model"), JsonObject("ActRelationship")]
    public class ActRelationship : VersionedAssociation<Act>, ITargetedAssociation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActRelationship"/> class.
        /// </summary>
        public ActRelationship()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActRelationship"/> class.
        /// </summary>
        /// <param name="relationshipType">Type of the relationship.</param>
        /// <param name="target">The target.</param>
        public ActRelationship(Guid? relationshipType, Act target)
        {
            this.RelationshipTypeKey = relationshipType;
            this.TargetAct = target;
            this.TargetActKey = target.Key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActRelationship"/> class.
        /// </summary>
        /// <param name="relationshipType">Type of the relationship.</param>
        /// <param name="targetKey">The target key.</param>
        public ActRelationship(Guid? relationshipType, Guid? targetKey)
        {
            this.RelationshipTypeKey = relationshipType;
            this.TargetActKey = targetKey;
        }

        /// <summary>
        /// Gets or sets the an additional (sub-type) of the relationship
        /// </summary>
        /// <remarks>
        /// <para>The context classification allows consumers of this data to understand the context in which the relationship. For example,
        /// a Patient->NextOfKin[null]->Person may have a context of related person to indidcate the NOK is to be used as a
        /// formal relationship, whereas Patient->NextOfKin[EmergencyContact]->Person may indicate that NOK record is only for
        /// use as a contact and no other relationship can be inferred from the entry.</para>
        /// </remarks>

        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ClassificationKey))]
        public Concept Classification { get; set; }

        /// <summary>
        /// Association type key
        /// </summary>
        [XmlElement("classification"), JsonProperty("classification")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Binding(typeof(RelationshipClassKeys))]
        public Guid? ClassificationKey { get; set; }

        /// <summary>
        /// The target of the association
        /// </summary>
        [XmlElement("target"), JsonProperty("target")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Guid? TargetActKey { get; set; }

        /// <summary>
        /// Target act reference
        /// </summary>
        [SerializationReference(nameof(TargetActKey))]
        [XmlIgnore, JsonIgnore]
        public Act TargetAct { get; set; }

        /// <summary>
        /// Association type key
        /// </summary>
        [XmlElement("relationshipType"), JsonProperty("relationshipType")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Binding(typeof(ActRelationshipTypeKeys))]
        public Guid? RelationshipTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the association type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(RelationshipTypeKey))]
        public Concept RelationshipType { get; set; }

        /// <summary>
        /// Empty?
        /// </summary>
        public override bool IsEmpty() => this.RelationshipType == null && this.RelationshipTypeKey == null ||
                this.TargetAct == null && this.TargetActKey == null;

        /// <summary>
        /// Determine semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as ActRelationship;
            if (other == null) return false;
            return base.SemanticEquals(obj) && this.TargetActKey == other.TargetActKey &&
                this.RelationshipTypeKey == other.RelationshipTypeKey &&
                this.SourceEntityKey == other.SourceEntityKey;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}) {1} ", this.RelationshipType?.ToString() ?? this.RelationshipTypeKey?.ToString(), this.TargetActKey);
        }

        /// <summary>
        /// Gets or sets the targeted entity
        /// </summary>
        [JsonIgnore, XmlIgnore]
        Guid? ITargetedAssociation.TargetEntityKey
        {
            get => this.TargetActKey;
            set => this.TargetActKey = value;
        }

        /// <summary>
        /// Gets or sets the targeted entity
        /// </summary>
        [JsonIgnore, XmlIgnore]
        object ITargetedAssociation.TargetEntity
        {
            get => this.TargetAct;
            set => this.TargetAct = (Act)value;
        }

        /// <summary>
        /// Association type
        /// </summary>
        Guid? ITargetedAssociation.AssociationTypeKey { get => this.RelationshipTypeKey; set => this.RelationshipTypeKey = value; }
    }
}