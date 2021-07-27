/*
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2021-2-9
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents an association between two entities
    /// </summary>
    [Classifier(nameof(RelationshipType)), NonCached]
    [XmlRoot("EntityRelationship", Namespace = "http://santedb.org/model")]
    [XmlType("EntityRelationship", Namespace = "http://santedb.org/model"), JsonObject("EntityRelationship")]
    public class EntityRelationship : VersionedAssociation<Entity>, ITargetedAssociation
    {
        // The association type key
        private Guid? m_associationTypeKey;
        private Concept m_relationshipType;
        private Guid? m_classificationKey;
        private Concept m_classification;
        private Guid? m_roleTypeKey;
        private Concept m_roleType;

        private Entity m_targetEntity;

        // The entity key
        private Guid? m_targetEntityKey;

        // The target entity
        // The association type
        /// <summary>
        /// Default constructor for entity relationship
        /// </summary>
        public EntityRelationship()
        {
            this.Key = Guid.NewGuid();

        }

        /// <summary>
        /// Entity relationship between <see cref="Association{TSourceType}.SourceEntityKey"/> container and <paramref name="target"/> having relationship type <paramref name="relationshipType"/>
        /// </summary>
        /// <param name="relationshipType">The key of the concept representing the relationship between source and target</param>
        /// <param name="target">The entity which is the target of this relationship</param>
        public EntityRelationship(Guid? relationshipType, Entity target)
        {
            this.RelationshipTypeKey = relationshipType;
            this.TargetEntity = target;
            this.Strength = 1.0;
            this.Key = Guid.NewGuid();

        }

        /// <summary>
        /// Entity relationship between <see cref="Association{TSourceType}.SourceEntityKey"/> container and <paramref name="targetKey"/>
        /// </summary>
        public EntityRelationship(Guid? relationshipType, Guid? targetKey)
        {
            this.Key = Guid.NewGuid();
            this.RelationshipTypeKey = relationshipType;
            this.TargetEntityKey = targetKey;
            this.Strength = 1.0;
        }

        /// <summary>
        /// Entity relationship between <see cref="Association{TSourceType}.SourceEntityKey"/> container and <paramref name="targetKey"/>
        /// </summary>
        public EntityRelationship(Guid? relationshipType, Guid? sourceKey, Guid? targetKey, Guid? classificationKey) : this(relationshipType, targetKey)
        {
            this.SourceEntityKey = sourceKey;
            this.ClassificationKey = classificationKey;
        }

        /// <summary>
        /// The entity that this relationship targets
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(HolderKey)), DataIgnore]
        public Entity Holder
        {
            get
            {
                return this.SourceEntity;
            }
            set
            {
                this.SourceEntity = value;
            }
        }

        /// <summary>
        /// The entity that this relationship targets
        /// </summary>
        [JsonProperty("holder"), XmlElement("holder")]
        public Guid? HolderKey
        {
            get
            {
                return this.SourceEntityKey;
            }
            set
            {
                this.SourceEntityKey = value;
            }
        }

        /// <summary>
        /// The inversion indicator
        /// </summary>
        [XmlElement("inversionInd"), JsonProperty("inversionInd")]
        public bool InversionIndicator { get; set; }

        /// <summary>
        /// The strength (confidence) of the relationship between source and target
        /// </summary>
        [XmlElement("strength"), JsonProperty("strength")]
        public double? Strength { get; set; }

        /// <summary>
        /// Represents the quantity of target in source
        /// </summary>
        [XmlElement("quantity"), JsonProperty("quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the association type
        /// </summary>
        [AutoLoad]
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(RelationshipTypeKey))]
        public Concept RelationshipType
        {
            get
            {
                this.m_relationshipType = base.DelayLoad(this.m_associationTypeKey, this.m_relationshipType);
                return this.m_relationshipType;
            }
            set
            {
                this.m_relationshipType = value;
                this.m_associationTypeKey = value?.Key;
            }
        }

        /// <summary>
        /// Association type key
        /// </summary>
        [XmlElement("relationshipType"), JsonProperty("relationshipType")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Binding(typeof(EntityRelationshipTypeKeys))]
        public Guid? RelationshipTypeKey
        {
            get { return this.m_associationTypeKey; }
            set
            {
                if (this.m_associationTypeKey != value)
                {
                    this.m_associationTypeKey = value;
                    this.m_relationshipType = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the association type
        /// </summary>
        [AutoLoad]
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(RelationshipRoleKey))]
        public Concept RelationshipRole
        {
            get
            {
                this.m_roleType = base.DelayLoad(this.m_roleTypeKey, this.m_roleType);
                return this.m_roleType;
            }
            set
            {
                this.m_roleType = value;
                this.m_roleTypeKey = value?.Key;
            }
        }

        /// <summary>
        /// Association type key
        /// </summary>
        [XmlElement("relationshipRole"), JsonProperty("relationshipRole")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? RelationshipRoleKey
        {
            get { return this.m_roleTypeKey; }
            set
            {
                if (this.m_roleTypeKey != value)
                {
                    this.m_roleTypeKey = value;
                    this.m_roleType = null;
                }
            }
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
        [AutoLoad]
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ClassificationKey))]
        public Concept Classification
        {
            get
            {
                this.m_classification = base.DelayLoad(this.m_classificationKey, this.m_classification);
                return this.m_classification;
            }
            set
            {
                this.m_classification = value;
                this.m_classificationKey = value?.Key;
            }
        }

        /// <summary>
        /// Association type key
        /// </summary>
        [XmlElement("classification"), JsonProperty("classification")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Binding(typeof(RelationshipClassKeys))]
        public Guid? ClassificationKey
        {
            get { return this.m_classificationKey; }
            set
            {
                if (this.m_classificationKey != value)
                {
                    this.m_classificationKey = value;
                    this.m_classification = null;
                }
            }
        }

        /// <summary>
        /// Target entity reference
        /// </summary>
        [SerializationReference(nameof(TargetEntityKey))]
        [XmlIgnore, JsonIgnore]
        public Entity TargetEntity
        {
            get
            {
                this.m_targetEntity = base.DelayLoad(this.m_targetEntityKey, this.m_targetEntity);
                return this.m_targetEntity;
            }
            set
            {
                this.m_targetEntity = value;
                this.m_targetEntityKey = value?.Key;
            }
        }

        /// <summary>
        /// The target of the association
        /// </summary>
        [XmlElement("target"), JsonProperty("target")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? TargetEntityKey
        {
            get { return this.m_targetEntityKey; }
            set
            {
                if (this.m_targetEntityKey != value)
                {
                    this.m_targetEntityKey = value;
                    this.m_targetEntity = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the targeted entity
        /// </summary>
        [JsonIgnore, XmlIgnore]
        object ITargetedAssociation.TargetEntity
        {
            get => this.TargetEntity;
            set => this.TargetEntity = (Entity)value;
        }

        /// <summary>
        /// Is empty
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return this.RelationshipType == null && this.RelationshipTypeKey == null ||
                this.TargetEntity == null && this.TargetEntityKey == null;
        }

        /// <summary>
        /// Determine semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as EntityRelationship;
            if (other == null) return false;
            return this.Key == other.Key || base.SemanticEquals(obj) && this.TargetEntityKey == other.TargetEntityKey &&
                this.RelationshipTypeKey == other.RelationshipTypeKey &&
                this.Quantity == other.Quantity &&
                ((this.SourceEntity == null) ^ (other.SourceEntity == null) || this.SourceEntityKey == other.SourceEntityKey) &&
                this.ClassificationKey == other.ClassificationKey &&
                this.RelationshipRoleKey == other.RelationshipRoleKey
                ;
        }

        /// <summary>
        /// Should serialize inversion indicator?
        /// </summary>
        public bool ShouldSerializeInversionIndicator()
        {
            return this.InversionIndicator;
        }

        /// <summary>
        /// Should serialize quantity?
        /// </summary>
        public bool ShouldSerializeQuantity()
        {
            return this.Quantity.HasValue;
        }
        /// <summary>
        /// Shoudl serialize source entity?
        /// </summary>
        public override bool ShouldSerializeSourceEntityKey()
        {
            return false;
        }

        /// <summary>
        /// Represent as string
        /// </summary>
        public override string ToString()
        {
            return string.Format("{1} => [{0}] => {2} (QTY: {3})", this.RelationshipType?.ToString() ?? this.RelationshipTypeKey?.ToString(), this.SourceEntityKey, this.TargetEntityKey, this.Quantity);
        }

        /// <summary>
        /// Association type
        /// </summary>
        Guid? ITargetedAssociation.AssociationTypeKey { get => this.RelationshipTypeKey; set => this.RelationshipTypeKey = value; }

    }
}