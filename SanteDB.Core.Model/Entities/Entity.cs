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
 * Date: 2022-5-30
 */
using Newtonsoft.Json;
using SanteDB.Core.i18n;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents the base class of all entities (persons, places, things) in SanteDB
    /// </summary>
    /// <remarks>
    /// In SanteDB, an entity represents a physical object which can be acted upon or can participate in an act.
    /// </remarks>
    [XmlType("Entity", Namespace = "http://santedb.org/model"), JsonObject("Entity")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Entity")]
    [Classifier(nameof(ClassConcept))]
    public class Entity : VersionedEntityData<Entity>, ITaggable, IExtendable, IHasClassConcept, IHasTypeConcept, IHasState, IHasTemplate, IHasIdentifiers, IHasRelationships, IGeoTagged
    {

        /// <summary>
        /// Internal reference for class concept
        /// </summary>
        protected Guid? m_classConceptKey;

        /// <summary>
        /// Creates a new instance of the entity class
        /// </summary>
        public Entity()
        {
            this.DeterminerConceptKey = DeterminerKeys.Specific;
        }

        /// <summary>
        /// Gets a list of all addresses associated with the entity
        /// </summary>
        [XmlElement("address"), JsonProperty("address")]
        public List<EntityAddress> Addresses { get; set; }

        /// <summary>
        /// Class concept datal load property
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ClassConceptKey))]
        public Concept ClassConcept { get; set; }

        /// <summary>
        /// Class concept
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        [Binding(typeof(EntityClassKeys))]
        public Guid? ClassConceptKey
        {
            get => this.m_classConceptKey;
            set
            {
                if (value.HasValue && !this.ValidateClassKey(value))
                {
                    throw new InvalidOperationException(ErrorMessages.INVALID_CLASS_CODE);
                }
                else
                {
                    this.m_classConceptKey = value;
                }
            }
        }

        /// <summary>
        /// Creation act reference
        /// </summary>
        [SerializationReference(nameof(CreationActKey))]
        [XmlIgnore, JsonIgnore]
        public Act CreationAct { get; set; }

        /// <summary>
        /// Creation act reference
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("creationAct"), JsonProperty("creationAct")]
        public Guid? CreationActKey { get; set; }

        /// <summary>
        /// Determiner concept
        /// </summary>
        [SerializationReference(nameof(DeterminerConceptKey))]
        [XmlIgnore, JsonIgnore]
        public virtual Concept DeterminerConcept { get; set; }

        /// <summary>
        /// Determiner concept
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("determinerConcept"), JsonProperty("determinerConcept")]
        [Binding(typeof(DeterminerKeys))]
        public virtual Guid? DeterminerConceptKey { get; set; }

        /// <summary>
        /// Gets a list of all extensions associated with the entity
        /// </summary>
        [XmlElement("extension"), JsonProperty("extension")]
        public List<EntityExtension> Extensions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the identifiers associated with this entity
        /// </summary>
        [XmlElement("identifier"), JsonProperty("identifier")]
        public List<EntityIdentifier> Identifiers { get; set; }

        /// <summary>
        /// Gets a list of all names associated with the entity
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public List<EntityName> Names { get; set; }

        /// <summary>
        /// Gets a list of all notes associated with the entity
        /// </summary>
        [XmlElement("note"), JsonProperty("note")]
        public List<EntityNote> Notes { get; set; }

        /// <summary>
        /// Gets the acts in which this entity participates
        /// </summary>
        [XmlElement("participation"), JsonProperty("participation")]
        public List<ActParticipation> Participations { get; set; }

        /// <summary>
        /// Gets a list of all associated entities for this entity
        /// </summary>
        [XmlElement("relationship"), JsonProperty("relationship")]
        public List<EntityRelationship> Relationships
        {
            get;
            set;
        }

        /// <summary>
        /// Status concept id
        /// </summary>
        [SerializationReference(nameof(StatusConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept StatusConcept { get; set; }

        /// <summary>
        /// Status concept id
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("statusConcept"), JsonProperty("statusConcept")]
        [Binding(typeof(StatusKeys))]
        public Guid? StatusConceptKey { get; set; }

        /// <summary>
        /// Gets a list of all tags associated with the entity
        /// </summary>
        [XmlElement("tag"), JsonProperty("tag")]
        public List<EntityTag> Tags { get; set; }

        /// <summary>
        /// Gets a list of all telecommunications addresses associated with the entity
        /// </summary>
        [XmlElement("telecom"), JsonProperty("telecom")]
        public List<EntityTelecomAddress> Telecoms
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the template key
        /// </summary>
        [XmlElement("template"), JsonProperty("template")]
        public Guid? TemplateKey { get; set; }

        /// <summary>
        /// Gets or sets the template definition
        /// </summary>
        [SerializationReference(nameof(TemplateKey)), XmlIgnore, JsonIgnore]
        public TemplateDefinition Template { get; set; }

        /// <summary>
        /// Type concept identifier
        /// </summary>
        [SerializationReference(nameof(TypeConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept TypeConcept { get; set; }

        /// <summary>
        /// Type concept identifier
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("typeConcept"), JsonProperty("typeConcept")]
        public Guid? TypeConceptKey { get; set; }

        /// <summary>
        /// Gets or sets the security policy instances associated with the entity
        /// </summary>
        /// <remarks>
        /// This property allows authors to tag an act with a particular security policy. Here the
        /// security policies may be something akin to "Taboo information" or "Research Only". From there
        /// the SanteDB policy decision point will determine whether or not the particular piece of
        /// data should be exposed or masked based on user credentials.
        /// </remarks>
        [XmlElement("policy"), JsonProperty("policy")]
        public List<SecurityPolicyInstance> Policies { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Entity;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.Addresses?.SemanticEquals(other.Addresses) != false &&
                this.ClassConceptKey == other.ClassConceptKey &&
                this.CreationActKey == other.CreationActKey &&
                this.DeterminerConceptKey == other.DeterminerConceptKey &&
                this.Extensions?.SemanticEquals(other.Extensions) != false &&
                this.Identifiers?.SemanticEquals(other.Identifiers) != false &&
                this.Names?.SemanticEquals(other.Names) != false &&
                this.Notes?.SemanticEquals(other.Notes) != false &&
                this.Participations?.SemanticEquals(other.Participations) != false &&
                this.Relationships?.SemanticEquals(other.Relationships) != false &&
                this.StatusConceptKey == other.StatusConceptKey &&
                this.Tags?.SemanticEquals(other.Tags) != false &&
                this.Telecoms?.SemanticEquals(other.Telecoms) != false &&
                this.TemplateKey == other.TemplateKey &&
                this.TypeConceptKey == other.TypeConceptKey;
        }

        /// <summary>
        /// Should serialize creation act
        /// </summary>
        public bool ShouldSerializeCreationActKey() => this.CreationActKey.HasValue;

        /// <summary>
        /// Should serialize type concept
        /// </summary>
        public bool ShouldSerializeTypeConceptKey() => this.TypeConceptKey.HasValue;

        /// <summary>
        /// Should serialize identifiers
        /// </summary>
        public bool ShouldSerializeIdentifiers() => this.Identifiers?.Count > 0;

        /// <summary>
        /// Should serialize Names
        /// </summary>
        public bool ShouldSerializeNames() => this.Names?.Count > 0;

        /// <summary>
        /// Should serialize addresses
        /// </summary>
        public bool ShouldSerializeAddresses() => this.Addresses?.Count > 0;

        /// <summary>
        /// Should serialize participations
        /// </summary>
        public bool ShouldSerializeParticipations() => this.Participations?.Count > 0;

        /// <summary>
        /// Should serialize tags
        /// </summary>
        public bool ShouldSerializeTags() => this.Tags?.Count > 0;

        /// <summary>
        /// Shoudl serialize extensions
        /// </summary>
        public bool ShouldSerializeExtensions() => this.Extensions?.Count > 0;

        /// <summary>
        /// Should serialize notes
        /// </summary>
        public bool ShouldSerializeNotes() => this.Notes?.Count > 0;

        /// <summary>
        /// Should serialize telecoms
        /// </summary>
        public bool ShouldSerializeTelecoms() => this.Telecoms?.Count > 0;

        [XmlIgnore, JsonIgnore]
        IEnumerable<ITag> ITaggable.Tags { get { return this.LoadCollection(o => o.Tags).OfType<ITag>(); } }

        [XmlIgnore, JsonIgnore]
        IEnumerable<IModelExtension> IExtendable.Extensions { get { return this.LoadCollection(o => o.Extensions).OfType<IModelExtension>(); } }

        /// <summary>
        /// Has identifiers
        /// </summary>
        [JsonIgnore, XmlIgnore]
        IEnumerable<IExternalIdentifier> IHasIdentifiers.Identifiers => this.LoadCollection(o => o.Identifiers);

        /// <summary>
        /// Relationships
        /// </summary>
        [JsonIgnore, XmlIgnore]
        IEnumerable<ITargetedAssociation> IHasRelationships.Relationships => this.LoadCollection(o => o.Relationships);

        /// <summary>
        /// Add a relationship to this relationship container
        /// </summary>
        /// <param name="association">The association to be added</param>
        void IHasRelationships.AddRelationship(ITargetedAssociation association)
        {
            if (association is EntityRelationship er)
            {
                this.LoadProperty(o => o.Relationships).Add(er);
            }
            else
            {
                throw new InvalidOperationException($"Expected EntityRelationship but got {association.GetType()}");
            }
        }

        /// <summary>
        /// Remove a relationship from this relationship container
        /// </summary>
        /// <param name="association">The association to be removed</param>
        void IHasRelationships.RemoveRelationship(ITargetedAssociation association)
        {
            if (association is EntityRelationship er)
            {
                this.LoadProperty(o => o.Relationships).Remove(er);
            }
            else
            {
                throw new InvalidOperationException($"Expected EntityRelationship but got {association.GetType()}");
            }
        }


        /// <summary>
        /// Should serialize template key
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeTemplateKey() => this.TemplateKey.GetValueOrDefault() != Guid.Empty;

        /// <summary>
        /// Add a policy to this entity
        /// </summary>
        public void AddPolicy(string policyId)
        {
            var pol = EntitySource.Current.Provider.Query<SecurityPolicy>(o => o.Oid == policyId).SingleOrDefault();
            if (pol == null)
            {
                throw new KeyNotFoundException($"Policy {policyId} not found");
            }

            this.Policies.Add(new SecurityPolicyInstance(pol, PolicyGrantType.Grant));
        }

        /// <summary>
        /// Add a tag to this entity
        /// </summary>
        public ITag AddTag(String tagKey, String tagValue)
        {
            var tag = this.LoadProperty(o => o.Tags)?.FirstOrDefault(o => o.TagKey == tagKey);
            this.Tags = this.Tags ?? new List<EntityTag>();
            if (tag == null)
            {
                tag = new EntityTag(tagKey, tagValue);
                this.Tags.Add(tag);
            }
            else
            {
                tag.Value = tagValue;
            }
            return tag;
        }

        /// <summary>
        /// Remove the specified extension
        /// </summary>
        public void RemoveExtension(Guid extensionTypeKey)
        {
            this.Extensions.RemoveAll(o => o.ExtensionTypeKey == extensionTypeKey);
        }

        /// <summary>
        /// Add an extension to this entity
        /// </summary>
        public void AddExtension(Guid extensionType, Type handlerType, object value)
        {
            // Is there already an extension type? if so just replace
            this.Extensions.Add(new EntityExtension(extensionType, handlerType, value));
        }

        /// <summary>
        /// Render the display of this entity
        /// </summary>
        /// <returns></returns>
        public override string ToDisplay() => $"{this.Type} : {this.LoadCollection(o => o.Names)?.FirstOrDefault()?.ToDisplay()} (K:{this.Key})";

        /// <summary>
        /// Get the specified tag
        /// </summary>
        public string GetTag(string tagKey) => tagKey.StartsWith("$") ? this.Tags?.FirstOrDefault(o => o.TagKey == tagKey)?.Value : this.LoadCollection(o => o.Tags).FirstOrDefault(o => o.TagKey == tagKey)?.Value;

        /// <summary>
        /// Remove the specified <paramref name="tagKey"/> from this objects tags
        /// </summary>
        public void RemoveTag(string tagKey)
        {
            if (tagKey.StartsWith("$"))
            {
                this.Tags?.RemoveAll(o => o.TagKey == tagKey);
            }
            else
            {
                this.LoadProperty(o => o.Tags).RemoveAll(t => t.TagKey == tagKey);
            }
        }


        /// <summary>
        /// Remove tags matching <paramref name="predicate"/> from the tag collection
        /// </summary>
        public void RemoveAllTags(Predicate<ITag> predicate) => this.Tags?.RemoveAll(predicate);

        /// <summary>
        /// Try to fetch the tag
        /// </summary>
        public bool TryGetTag(string tagKey, out ITag tag)
        {
            tag = this.Tags?.FirstOrDefault(o => o.TagKey == tagKey);
            return tag != null;
        }


        /// <summary>
        /// Gets or sets the geo tag
        /// </summary>
        [XmlElement("geo"), JsonProperty("geo")]
        public GeoTag GeoTag { get; set; }

        /// <summary>
        /// Gets the geo tag key
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Guid? GeoTagKey { get; set; }

        /// <summary>
        /// Validate the class key
        /// </summary>
        /// <param name="classKey">The UUID of the class concept</param>
        /// <returns>True if the <paramref name="classKey"/> is valid for this type of object</returns>
        protected virtual bool ValidateClassKey(Guid? classKey) => true;
    }
}