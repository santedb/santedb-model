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
 * Date: 2023-6-21
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
    /// Act participations can also be quantified. For example, if 100 doses of a particular material (<see cref="ManufacturedMaterial"/>) were consumed
    /// as part of an act, then the quantity would be 100.
    /// </para>
    /// </remarks>
    [Classifier(nameof(ParticipationRole)), NonCached]
    [XmlType(Namespace = "http://santedb.org/model", TypeName = "ActParticipation"), JsonObject(nameof(ActParticipation))]
    public class ActParticipation : VersionedAssociation<Act>, ITargetedVersionedExtension, IHasExternalKey
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
        /// Gets or sets the external key for the object
        /// </summary>
        /// <remarks>Sometimes, when communicating with an external communications another system needs to 
        /// refer to this by a particular key</remarks>
        [XmlElement("externId"), JsonProperty("externId")]
        public string ExternalKey { get; set; }

        /// <summary>
        /// Identifies the classification of the participation.
        /// </summary>
        /// <remarks><para>Classifications are used to further specify the meaning and class of the participation between
        /// the holder <see cref="Act"/> and the player <see cref="Entity"/>. For example, classifiers may indicate that the
        /// participation is private (should not be disclosed), contained (cannot exist without the holder), etc.</para></remarks>
        /// <seealso cref="RelationshipClassKeys"/>
        [XmlElement("classification"), JsonProperty("classification")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Binding(typeof(RelationshipClassKeys))]
        public Guid? ClassificationKey { get; set; }

        /// <summary>
        /// Identifies the entity which played the <see cref="ParticipationRoleKey"/>
        /// </summary>
        /// <remarks>
        /// <para>The player represents the <see cref="Entity"/> which plays the <see cref="ParticipationRoleKey"/> in the act. The player 
        /// is related to the act in this (and only this) manner. It should not be assumed that a player, for example, playing the role of <see cref="ActParticipationKeys.Admitter"/> is 
        /// also the <see cref="ActParticipationKeys.Authororiginator"/> of the Act. Rather these participations would be represented as separate instances of <see cref="ActParticipation"/> 
        /// on the <see cref="Act"/></para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("player"), JsonProperty("player")]
        public Guid? PlayerEntityKey { get; set; }

        /// <summary>
        /// Identifies the role which the <see cref="PlayerEntityKey"/> performs in the <see cref="ActKey"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// The participation role indicates the type of role which the <see cref="PlayerEntityKey"/> plays in the containing Act. Roles of
        /// a player entity can vary from <see cref="ActParticipationKeys.Admitter"/> (the Entity which admitted the patient), to <see cref="ActParticipationKeys.RecordTarget"/>
        /// (the entity about which the act exists), or even <see cref="ActParticipationKeys.Product"/> (the Entity representing the product which was used
        /// or administered in the act).
        /// </para>
        /// <para>Participation roles are validated based on the <see cref="Entity.ClassConceptKey"/> for the player and the <see cref="Act.ClassConceptKey"/>. </para>
        /// </remarks>
        /// <seealso cref="ActParticipationKeys"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Binding(typeof(ActParticipationKeys))]
        [XmlElement("participationRole"), JsonProperty("participationRole")]
        public Guid? ParticipationRoleKey { get; set; }

        /// <summary>
        /// The delay-load property for <see cref="PlayerEntityKey"/>
        /// </summary>
        /// <example>
        /// <code language="cs">
        /// <![CDATA[
        /// void DoSomething(ActParticipation foo) {
        ///     // This property is used with .LoadProperty to provide convenient loading of data
        ///     var playerType = foo.LoadProperty(o=>o.PlayerEntity).TypeConceptKey;
        /// }
        /// ]]>
        /// </code>
        /// </example>
        /// <seealso cref="PlayerEntityKey"/>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(PlayerEntityKey))]
        public Entity PlayerEntity { get; set; }

        /// <summary>
        /// Delay load point for <see cref="ParticipationRoleKey"/>
        /// </summary>
        /// <seealso cref="ParticipationRoleKey"/>
        /// <example>
        /// <code language="cs">
        /// <![CDATA[
        /// void DoSomething(ActParticipation foo) {
        ///     // This property is used to load nested data
        ///     var roleMnemonic = foo.LoadProperty(o=>o.ParticipationRole).Mnemonic;
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ParticipationRoleKey))]
        public Concept ParticipationRole { get; set; }

        /// <summary>
        /// Identifies the <see cref="Act"/> to which the participation belongs
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="Act"/> is the holder of this type of relationship. An <see cref="ActParticipation"/> is a relationship
        /// between <see cref="Act"/> and <see cref="Entity"/>.</para>
        /// </remarks>
        [JsonProperty("act"), XmlElement("act")]
        public Guid? ActKey
        {
            get => this.SourceEntityKey;
            set => this.SourceEntityKey = value;
        }

        /// <summary>
        /// Delay load property for <see cref="ActKey"/>
        /// </summary>
        /// <seealso cref="ActKey"/>
        /// <example>
        /// <code language="cs">
        /// <![CDATA[
        /// void DoSomething(ActParticipation foo) {
        ///     // This property is used to delay-load nested data
        ///     var typeOfConcept = foo.LoadProperty(o=>o.Act).TypeConceptKey;
        /// }
        /// ]]></code></example>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(ActKey)), SerializationMetadata]
        public Act Act
        {
            get => this.SourceEntity;
            set => this.SourceEntity = value;
        }

        /// <summary>
        /// Identifies the number of <see cref="PlayerEntityKey"/> which participates in <see cref="ActKey"/>
        /// </summary>
        /// <remarks>The quantity property is used to express the number of entities which are participating in the 
        /// act. Some examples where quantity may be used:
        /// <list type="bullet">
        /// <item><term>30 Syringes were shipped</term><description>A <see cref="Act"/> with class <see cref="ActClassKeys.Supply"/> has a participation of type <see cref="ActParticipationKeys.Consumable"/> to a <see cref="ManufacturedMaterial"/> with quantity of 30</description></item>
        /// <item><term>1 dose of BCG administered</term><description>A <see cref="SubstanceAdministration"/> has a participation of type <see cref="ActParticipationKeys.Consumable"/> to a <see cref="ManufacturedMaterial"/> with quantity of 1</description></item>
        /// </list></remarks>
        [XmlElement("quantity"), JsonProperty("quantity")]
        public int? Quantity { get; set; }

        /// <inheritdoc/>
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
            if (other == null)
            {
                return false;
            }

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
        [XmlIgnore, JsonIgnore]
        Guid? ITargetedAssociation.AssociationTypeKey { get => this.ParticipationRoleKey; set => this.ParticipationRoleKey = value; }

        /// <inheritdoc/>
        public override ICanDeepCopy DeepCopy() => this.CloneDeep();
    }
}