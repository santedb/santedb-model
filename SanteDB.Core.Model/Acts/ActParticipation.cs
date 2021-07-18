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
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Associates an entity which participates in an act
    /// </summary>
    /// <remarks>
    /// <para>
    /// An act participation instance is used to link an <see cref="Entity"/> entity instance to an <see cref="Act"/> act instance. It is said that the
    /// player (<see cref="PlayerEntityKey"/>) participates in the act (<see cref="ActKey"/>) in a particular role (<see cref="ParticipationRoleKey"/>).
    /// </para>
    /// <para>
    /// Act participations can also be quantified. For example, if 100 doses of a particlar material (<see cref="ManufacturedMaterial"/>) were consumed
    /// as part of an act, then the quantity would be 100.
    /// </para>
    /// </remarks>
    [Classifier(nameof(ParticipationRole)), NonCached]
    [XmlType(Namespace = "http://santedb.org/model", TypeName = "ActParticipation"), JsonObject(nameof(ActParticipation))]
    public class ActParticipation : VersionedAssociation<Act> , ITargetedAssociation
    {

       
        /// <summary>
        /// Default constructor for act participation
        /// </summary>
        public ActParticipation()
        {
        }

        /// <summary>
        /// Act participation relationship between <paramref name="roleType" /> and <paramref name="player" />
        /// </summary>
        /// <param name="roleType">Type of the role.</param>
        /// <param name="player">The player.</param>
        public ActParticipation(Guid? roleType, Entity player)
        {
            this.ParticipationRoleKey = roleType;
            this.PlayerEntity = player;
            this.PlayerEntityKey = player.Key;
        }

        /// <summary>
        /// Entity relationship between <paramref name="roleType" /> and <paramref name="playerKey" />
        /// </summary>
        /// <param name="roleType">Type of the role.</param>
        /// <param name="playerKey">The player key.</param>
        public ActParticipation(Guid? roleType, Guid? playerKey)
        {
            this.ParticipationRoleKey = roleType;
            this.PlayerEntityKey = playerKey;
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
        [Binding(typeof(RelationshipClassKeys))]
        public Guid? ClassificationKey { get; set; }

        /// <summary>
        /// Gets or sets the target entity reference
        /// </summary>
        [XmlElement("player"), JsonProperty("player")]
        public Guid? PlayerEntityKey { get; set; }

        /// <summary>
        /// Gets or sets the participation role key
        /// </summary>
        [Binding(typeof(ActParticipationKey))]
        [XmlElement("participationRole"), JsonProperty("participationRole")]
        public Guid? ParticipationRoleKey { get; set; }

        /// <summary>
        /// Gets or sets the entity which participated in the act
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(PlayerEntityKey))]
        public Entity PlayerEntity { get; set; }

        /// <summary>
        /// Gets or sets the role that the entity played in participating in the act
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ParticipationRoleKey))]
        public Concept ParticipationRole { get; set; }

        /// <summary>
        /// The entity that this relationship targets
        /// </summary>
        [JsonProperty("act"), XmlElement("act")]
        public Guid? ActKey
        {
            get => this.SourceEntityKey;
            set => this.SourceEntityKey = value;
        }

        /// <summary>
        /// The entity that this relationship targets
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(ActKey)), SerializationMetadata]
        public Act Act
        {
            get => this.SourceEntity;
            set => this.SourceEntity = value;
        }

        /// <summary>
        /// Gets or sets the quantity of player in the act
        /// </summary>
        [XmlElement("quantity"), JsonProperty("quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// Determine if this is empty
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return !this.ParticipationRoleKey.HasValue && this.ParticipationRole == null ||
                this.PlayerEntity == null && !this.PlayerEntityKey.HasValue;
        }

        /// <summary>
        /// Determine equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as ActParticipation;
            if (other == null) return false;
            return base.SemanticEquals(obj) && other.ActKey == this.ActKey &&
                other.PlayerEntityKey == this.PlayerEntityKey &&
                other.ParticipationRoleKey == this.ParticipationRoleKey;
        }

        /// <summary>
        /// Don't serialize source entity
        /// </summary>
        public override bool ShouldSerializeSourceEntityKey() => false;

        /// <summary>
        /// Should serialize quantity
        /// </summary>
        public bool ShouldSerializeQuantity() => this.Quantity.HasValue;

        /// <summary>
        /// Should serialize act key
        /// </summary>
        public bool ShouldSerializeActKey() => ActKey.HasValue;

        /// <summary>
        /// Represent as string
        /// </summary>
        public override string ToString()
        {
            return string.Format("({0}) {1} = {2}", this.ParticipationRole?.ToString() ?? this.ParticipationRoleKey?.ToString(), this.PlayerEntityKey, this.Quantity);
        }

        /// <summary>
        /// Gets or sets the targeted entity
        /// </summary>
        [JsonIgnore, XmlIgnore]
        Guid? ITargetedAssociation.TargetEntityKey
        {
            get => this.PlayerEntityKey;
            set => this.PlayerEntityKey = value;
        }

        /// <summary>
        /// Gets or sets the targeted entity
        /// </summary>
        [JsonIgnore, XmlIgnore]
        object ITargetedAssociation.TargetEntity
        {
            get => this.PlayerEntity;
            set => this.PlayerEntity = (Entity)value;
        }

        /// <summary>
        /// Association type
        /// </summary>
        Guid? ITargetedAssociation.AssociationTypeKey { get => this.ParticipationRoleKey; set => this.ParticipationRoleKey = value; }
    }
}
