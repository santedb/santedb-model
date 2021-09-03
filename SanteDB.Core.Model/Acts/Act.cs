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
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents the base class for an act (something which is done or actioned on)
    /// </summary>
    /// <remarks>
    /// <para>
    /// An Act, in the context of the reference information model (RIM) represents something that is done to a patient. More precisely, an Act
    /// is anything that occurs involving entities in which the entity's state is changed or is documented.
    /// </para>
    /// <para>
    /// Examples of Acts Include:
    /// </para>
    /// <list type="bullet">
    ///     <item><see cref="SubstanceAdministration"/> - The administration of a substance to a patient</item>
    ///     <item><see cref="Observation"/> - The observing of a value for the patient</item>
    ///     <item><see cref="PatientEncounter"/> - An encounter or visit that occurs where the patient receives one or more services</item>
    ///     <item><see cref="Act"/> - Any other action such as supply request, or problem recordation</item>
    /// </list>
    /// <para>
    /// The property which classifies what specific type of action an act represents is its <see cref="ClassConceptKey"/>, which dictates
    /// what type an act is. Class concept keys can be found in here <see cref="ActClassKeys"/>.
    /// </para>
    /// <para>
    /// This structure is used to represent events, proposals, and requests. That is to say, the Act structure can represent the request to 
    /// do an act, the intent to perform an act, or the actual act being performed itself. This classification of mode happens based on the 
    /// <see cref="MoodConceptKey"/> mood concept. Mood concept keys can be found on the <see cref="ActMoodKeys"/> structure.
    /// </para>
    /// </remarks>
    [XmlType(Namespace = "http://santedb.org/model", TypeName = "Act")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Act")]
    [JsonObject("Act")]
    [Classifier(nameof(ClassConcept))]
    public class Act : VersionedEntityData<Act>, ITaggable, ISecurable, IExtendable, IHasClassConcept, IHasState, IGeoTagged, IHasTemplate, IHasIdentifiers, IHasRelationships
    {

        private Guid? m_classConceptKey;
        private Guid? m_typeConceptKey;
        private Guid? m_statusConceptKey;
        private Guid? m_moodConceptKey;
        private Guid? m_reasonConceptKey;
        private Guid? m_templateKey;

        private Concept m_classConcept;
        private Concept m_typeConcept;
        private Concept m_statusConcept;
        private Concept m_moodConcept;
        private Concept m_reasonConcept;
        private TemplateDefinition m_template;

        /// <summary>
        /// Constructor for ACT
        /// </summary>
        public Act()
        {
            this.Relationships = new List<ActRelationship>();
            this.Identifiers = new List<ActIdentifier>();
            this.Extensions = new List<ActExtension>();
            this.Notes = new List<ActNote>();
            this.Participations = new List<ActParticipation>();
            this.Tags = new List<ActTag>();
            this.Protocols = new List<ActProtocol>();
            this.Policies = new List<SecurityPolicyInstance>();

        }
        /// <summary>
        /// Gets or sets an indicator which identifies whether the act actually occurred, or
        /// specifically did not occur
        /// </summary>
        /// <remarks>
        /// The isNegated flag is important when the SanteDB system needs to keep track that an event
        /// specifically DID NOT OCCUR, or SHOULD NOT OCCUR. Typically this is paired with a reason concept (<see cref="ReasonConceptKey"/>)
        /// which describes why  the act did not or should not occur.
        /// </remarks>
        [XmlElement("isNegated"), JsonProperty("isNegated")]
        public Boolean IsNegated { get; set; }

        /// <summary>
        /// Gets or sets the instant in time when the act occurred (if applicable)
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset ActTime { get; set; }

        /// <summary>
        /// Gets the template UUID upon which this act is based
        /// </summary>
        /// <remarks>
        /// Templates are used to classify the specific rules and input forms used to create the act. It further
        /// classifies the type of act in a manner which allows a consumer to render the data or to validate the data.
        /// </remarks>
        [XmlElement("template"), JsonProperty("template")]
        public Guid? TemplateKey
        {
            get
            {
                return this.m_templateKey;
            }
            set
            {
                this.m_templateKey = value;
                if (value.HasValue && value != this.m_template?.Key)
                    this.m_template = null;
            }
        }

        /// <summary>
        /// Gets or sets the template definition
        /// </summary>
        [AutoLoad, SerializationReference(nameof(TemplateKey)), XmlIgnore, JsonIgnore]
        public TemplateDefinition Template
        {
            get
            {
                this.m_template = base.DelayLoad(this.m_templateKey, this.m_template);
                return this.m_template;
            }
            set
            {
                this.m_template = value;
                this.m_templateKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the moment in time that this act occurred in ISO format
        /// </summary>
        [DataIgnore, XmlElement("actTime"), JsonProperty("actTime")]
        public String ActTimeXml
        {
            get { return this.ActTime.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.ActTime = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else
                    this.ActTime = default(DateTimeOffset);
            }
        }

        /// <summary>
        /// Gets or sets the time when the act should or did start ocurring
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time when the act should or did start ocurring in ISO format
        /// </summary>
        [DataIgnore, XmlElement("startTime"), JsonProperty("startTime")]
        public String StartTimeXml
        {
            get { return this.StartTime?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.StartTime = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else
                    this.StartTime = default(DateTimeOffset);
            }
        }

        /// <summary>
        /// Gets or sets the time when the act did or should stop occurring
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? StopTime { get; set; }


        /// <summary>
        /// Gets or sets the time when the act should or did stop ocurring in ISO format
        /// </summary>
        [DataIgnore, XmlElement("stopTime"), JsonProperty("stopTime")]
        public String StopTimeXml
        {
            get { return this.StopTime?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.StopTime = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else
                    this.StopTime = default(DateTimeOffset);
            }
        }

        /// <summary>
        /// Gets or sets the key of the concept which classifies the act.
        /// </summary>
        /// <see cref="ClassConcept"/>
        /// <see cref="ActClassKeys"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        [Binding(typeof(ActClassKeys))]
        public virtual Guid? ClassConceptKey
        {
            get { return this.m_classConceptKey; }
            set
            {
                if (this.m_classConceptKey != value)
                {
                    this.m_classConceptKey = value;
                    this.m_classConcept = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the key of the concept which specifies the mood of the act.
        /// </summary>
        /// <see cref="ActMoodKeys"/>
        /// <remarks>
        /// <para>In SanteDB, a mood of an act describes the mode of that act. The mood of the act clasifies whether the act did occur, is intended to occur, is requested to occur or proposed. Mood codes may include:</para>
        /// <list type="bullet">
        /// <item>
        ///     <term>Event Occurence</term>
        ///     <description>The ACT did or did not occur (i.e. the patient presented and the data represents the outcome of that encounter)</description>
        /// </item>
        /// <item>
        ///     <term>Proposed</term>
        ///     <description>The ACT exists because a computerized process proposed that the act occur</description>
        /// </item>
        /// <item>
        ///     <term>Request</term>
        ///     <description>The ACT represents a request by a human to perform the action provided</description>
        /// </item>
        /// <item>
        ///     <term>Intent</term>
        ///     <description>The ACT has not yet occurred, however a human has indicated their intent to perform the act at a future date (i.e. appointment)</description>
        /// </item>
        /// <item>
        ///     <term>Goal</term>
        ///     <description>The ACT represents a goal rather than something that has, or will occur. A goal act is used to store goals in care plans</description>
        /// </item>
        /// </list>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("moodConcept"), JsonProperty("moodConcept")]
        [Binding(typeof(ActMoodKeys))]
        public virtual Guid? MoodConceptKey
        {
            get { return this.m_moodConceptKey; }
            set
            {
                if (this.m_moodConceptKey != value)
                {
                    this.m_moodConceptKey = value;
                    this.m_moodConcept = null;
                }
            }
        }


        /// <summary>
        /// Gets or sets the key of the concept which defines the reason why the act is or didn't occur
        /// </summary>
        /// <see cref="ActReasonKeys"/>
        /// <see cref="NullReasonKeys"/>
        /// <remarks>
        /// <para>The reason concept on an act indicates why something should, or did/did not occur. For example, when used in conjunction with IsNegated, this concept typically indicates WHY the action did not occur
        /// (safety concern, etc.)</para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("reasonConcept"), JsonProperty("reasonConcept")]
        [Binding(typeof(ActReasonKeys))]
        public Guid? ReasonConceptKey
        {
            get { return this.m_reasonConceptKey; }
            set
            {
                if (this.m_reasonConceptKey != value)
                {
                    this.m_reasonConceptKey = value;
                    this.m_reasonConcept = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the key of the concept which describes the current status of the act
        /// </summary>
        /// <see cref="StatusKeys"/>
        /// <remarks>
        /// <para>The status concepts for an act allow for a basic state machine to be represented in SanteDB. Common states for an act are:</para>
        /// <list type="bullet">
        ///     <item>
        ///         <term>New</term>
        ///         <description>The ACT has just newly been created (through an automated process, care plan, etc.) and has not been reviewed by a user or other process</description>
        ///     </item>
        ///     <item>
        ///         <term>Active</term>
        ///         <description>The ACT is currently in progress. This means that the ACT is still being actioned upon</description>
        ///     </item>
        ///     <item>
        ///         <term>Complete</term>
        ///         <description>The ACT or action described by the act has been completed</description>
        ///     </item>
        ///     <item>
        ///         <term>Obsolete</term>
        ///         <description>The ACT did happen, or did accurately describe an event, however the information in that ACT is no longer valid</description>
        ///     </item>
        ///     <item>
        ///         <term>Nullified</term>
        ///         <description>The ACT did not happen, and was created in error</description>
        ///     </item>
        /// </list>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("statusConcept"), JsonProperty("statusConcept")]
        [Binding(typeof(StatusKeys))]
        public Guid? StatusConceptKey
        {
            get { return this.m_statusConceptKey; }
            set
            {
                if (this.m_statusConceptKey != value)
                {
                    this.m_statusConceptKey = value;
                    this.m_statusConcept = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the key of the conccept which further classifies the type of act occurring
        /// </summary>
        /// <see cref="TypeConcept"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("typeConcept"), JsonProperty("typeConcept")]
        public Guid? TypeConceptKey
        {
            get { return this.m_typeConceptKey; }
            set
            {
                if (this.m_typeConceptKey != value)
                {
                    this.m_typeConceptKey = value;
                    this.m_typeConcept = null;
                }
            }
        }


        /// <summary>
        /// Gets or sets the concept which classifies the type of act
        /// </summary>
        /// <remarks>
        /// The class concept is used to classify the overall type of the act. This code will specify whether the 
        /// act is a substance administration, financial transaction, observation, etc.
        /// </remarks>
        /// <see cref="ActClassKeys"/>
        [XmlIgnore, JsonIgnore]
        [AutoLoad, SerializationReference(nameof(ClassConceptKey))]
        public Concept ClassConcept
        {
            get
            {
                this.m_classConcept = base.DelayLoad(this.m_classConceptKey, this.m_classConcept);
                return this.m_classConcept;
            }
            set
            {
                this.m_classConcept = value;
                this.m_classConceptKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the concept which specifies the mood of the act
        /// </summary>
        /// <remarks>
        /// Here the mood of the act is used to describe the mode of the act or specifically to classify 
        /// whether the act did occur (event ocurrence), should occur (propose), will occur (intent), or being requested to occur (request).
        /// </remarks>
        /// <see cref="ActMoodKeys"/>
        [XmlIgnore, JsonIgnore]
        [AutoLoad, SerializationReference(nameof(MoodConceptKey))]
        public Concept MoodConcept
        {
            get
            {
                this.m_moodConcept = base.DelayLoad(this.m_moodConceptKey, this.m_moodConcept);
                return this.m_moodConcept;
            }
            set
            {
                this.m_moodConcept = value;
                this.m_moodConceptKey = value?.Key;
            }
        }


        /// <summary>
        /// Gets or sets the concept which indicates the reason of the act
        /// </summary>
        /// <remarks>
        /// This concept is used to dictate why the act did occur (or if the negation indicator or mood concept indicate, why it didn't or shouldn't occur). Examples
        /// of reason codes may be "patient was too old", "out of stock", "patient has allergy", etc.
        /// </remarks>
        [XmlIgnore, JsonIgnore]
        [AutoLoad, SerializationReference(nameof(ReasonConceptKey))]
        public Concept ReasonConcept
        {
            get
            {
                this.m_reasonConcept = base.DelayLoad(this.m_reasonConceptKey, this.m_reasonConcept);
                return this.m_reasonConcept;
            }
            set
            {
                this.m_reasonConcept = value;
                this.m_reasonConceptKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the current status concept of the act
        /// </summary>
        /// <remarks>
        /// The status of the act will dictate which part of the lifecycle an act is 
        /// currently operating in.
        /// <list type="bullet">
        ///     <item>New - The Act is brand new and has yet to start ocurring</item>
        ///     <item>Active - The Act is still occurring</item>
        ///     <item>Completed - The Act has completed or is in the past</item>
        ///     <item>Obsolete - The Act did occur, however it is no longer accurate</item>
        ///     <item>Nullified - The Act never occurred, this record was created in error</item>
        /// </list>
        /// </remarks>
        [AutoLoad, SerializationReference(nameof(StatusConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept StatusConcept
        {
            get
            {
                this.m_statusConcept = base.DelayLoad(this.m_statusConceptKey, this.m_statusConcept);
                return this.m_statusConcept;
            }
            set
            {
                this.m_statusConcept = value;
                if (value == null)
                    this.m_statusConceptKey = Guid.Empty;
                else
                    this.m_statusConceptKey = value.Key;
            }
        }

        /// <summary>
        /// Type concept identifier
        /// </summary>
        [AutoLoad, SerializationReference(nameof(TypeConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept TypeConcept
        {
            get
            {
                this.m_typeConcept = base.DelayLoad(this.m_typeConceptKey, this.m_typeConcept);
                return this.m_typeConcept;
            }
            set
            {
                this.m_typeConceptKey = value?.Key;
                this.m_typeConcept = value;
            }
        }

        /// <summary>
        /// Gets or sets the identifiers by which this act is known as in other systems
        /// </summary>
        /// <remarks>
        /// The identifiers field is used to assign alternate identifiers to the act itself. These identifiers can 
        /// be used internally for tracking the act, or can be used to correlate an act in a way that an external system
        /// will know it.
        /// </remarks>
        [AutoLoad, XmlElement("identifier"), JsonProperty("identifier")]
        public List<ActIdentifier> Identifiers { get; set; }

        /// <summary>
        /// Gets a list of all associated acts for this act
        /// </summary>
        /// <remarks>
        /// The relationships of an act are used to relate one or more acts together either 
        /// directly as in an encounter with component acts, or between care episodes for chronic
        /// care.
        /// </remarks>
        [AutoLoad, XmlElement("relationship"), JsonProperty("relationship")]
        public List<ActRelationship> Relationships { get; set; }

        /// <summary>
        /// Gets or sets the security policy instances associated with the act
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
        /// Gets a list of all extensions associated with the act
        /// </summary>
        /// <remarks>
        /// An extension allows developers to store additional data about a particular act in a manner
        /// which the original SanteDB authors did not intend. This can be things such as equipment 
        /// used to record an observation, etc. 
        /// <para>
        /// The key difference beetween an extension and a tag is that extensions are versioned whereas tags
        /// are not
        /// </para>
        /// </remarks>
        [AutoLoad, XmlElement("extension"), JsonProperty("extension")]
        public List<ActExtension> Extensions { get; set; }

        /// <summary>
        /// Gets a list of all notes associated with the act
        /// </summary>
        /// <remarks>Allows one or more notes to be taken about an act by a clinician</remarks>
        [AutoLoad, XmlElement("note"), JsonProperty("note")]
        public List<ActNote> Notes { get; set; }

        /// <summary>
        /// Gets a list of all tags associated with the act
        /// </summary>
        /// <remarks>
        /// A tag is a simple piece of data which is appended to an act which allows developers to 
        /// extend the underlying application in ways not imagined by the original SanteDB team. Tags differ
        /// from extensions in that they can only carry simple values (strings) and they are not versioned. 
        /// </remarks>
        [AutoLoad, XmlElement("tag"), JsonProperty("tag")]
        public List<ActTag> Tags { get; set; }

        /// <summary>
        /// Identifies protocols attached to the act
        /// </summary>
        /// <remarks>
        /// The protocols element is used to track which clinical protocols where linked with 
        /// the act.
        /// </remarks>
        [AutoLoad, XmlElement("protocol"), JsonProperty("protocol")]
        public List<ActProtocol> Protocols
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entities and participations they play in the act
        /// </summary>
        /// <remarks>
        /// In an Act, one or more entities participate in the carrying out of the act. This property is used to 
        /// track that information. Examples of participations include:
        /// <list type="bullet">
        ///     <item>Consumable - An entity that was consumed in the process of carrying out the act</item>
        ///     <item>Product - A product that was administered or used to perform the act</item>
        ///     <item>Author - The person who recorded the information related to the act</item>
        ///     <item>Performer - The person(s) who performed the act</item>
        ///     <item>Location - Where the act took place</item>
        /// </list>
        /// </remarks>
        /// <see cref="ActParticipationKey"/>
        [XmlElement("participation"), JsonProperty("participation")]
        [AutoLoad]
        public List<ActParticipation> Participations { get; set; }

        /// <summary>
        /// True if reason concept key should be serialized
        /// </summary>
        public bool ShouldSerializeReasonConceptKey()
        {
            return this.ReasonConceptKey.HasValue && this.ReasonConceptKey.GetValueOrDefault() != Guid.Empty;
        }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        /// <see cref="IdentifiedData.SemanticEquals(object)"/>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Act;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.ActTime == other.ActTime &&
                this.ClassConceptKey == other.ClassConceptKey &&
                this.Extensions?.SemanticEquals(other.Extensions) == true &&
                this.Identifiers?.SemanticEquals(other.Identifiers) == true &&
                this.IsNegated == other.IsNegated &&
                this.MoodConceptKey == other.MoodConceptKey &&
                this.Notes?.SemanticEquals(other.Notes) == true &&
                this.Participations?.SemanticEquals(other.Participations) == true &&
                this.Policies?.SemanticEquals(other.Policies) == true &&
                this.Protocols?.SemanticEquals(other.Protocols) == true &&
                this.ReasonConceptKey == other.ReasonConceptKey &&
                this.Relationships?.SemanticEquals(other.Relationships) == true &&
                this.StartTime == other.StartTime &&
                this.StatusConceptKey == other.StatusConceptKey &&
                this.StopTime == other.StopTime &&
                this.Tags?.SemanticEquals(other.Tags) == true &&
                this.Template?.SemanticEquals(other.Template) == true &&
                this.TypeConceptKey == other.TypeConceptKey;
        }


        /// <summary>
        /// Should serialize relationships?
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRelationships() => this.Relationships?.Count > 0;
        /// <summary>
        /// Should serialize identifiers
        /// </summary>
        public bool ShouldSerializeIdentifiers() => this.Identifiers?.Count > 0;
        /// <summary>
        /// Should serialize extensions?
        /// </summary>
        public bool ShouldSerializeExtensions() => this.Extensions?.Count > 0;
        /// <summary>
        /// Should serialize notes
        /// </summary>
        public bool ShouldSerializeNotes() => this.Notes?.Count > 0;
        /// <summary>
        /// Should serialize participations
        /// </summary>
        public bool ShouldSerializeParticipations() => this.Participations?.Count > 0;
        /// <summary>
        /// Should serialize tags
        /// </summary>
        public bool ShouldSerializeTags() => this.Tags?.Count > 0;
        /// <summary>
        /// Should serialize protocols
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeProtocols() => this.Protocols?.Count > 0;

        /// <summary>
        /// Should serialize template key
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeTemplateKey() => this.TemplateKey.GetValueOrDefault() != Guid.Empty;

        /// <summary>
        /// Should serialize policies
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePolicies() => this.Policies.Count > 0;

        /// <summary>
        /// Gets the tags
        /// </summary>
        [XmlIgnore, JsonIgnore]
        IEnumerable<ITag> ITaggable.Tags { get { return this.LoadCollection<EntityTag>(nameof(Act.Tags)).OfType<ITag>(); } }

        [XmlIgnore, JsonIgnore]
        IEnumerable<IModelExtension> IExtendable.Extensions { get { return this.LoadCollection<EntityExtension>(nameof(Act.Extensions)).OfType<IModelExtension>(); } }

        /// <summary>
        /// Gets or sets the geo-tag
        /// </summary>
        [XmlElement("geo"), JsonProperty("geo")]
        public GeoTag GeoTag { get; set; }

        /// <summary>
        /// Has identifiers
        /// </summary>
        [JsonIgnore, XmlIgnore]
        IEnumerable<IExternalIdentifier> IHasIdentifiers.Identifiers => this.LoadCollection(o => o.Identifiers);

        /// <summary>
        /// Relationships
        /// </summary>
        [JsonIgnore, XmlIgnore]
        IEnumerable<ITargetedAssociation> IHasRelationships.Relationships => this.Relationships;

        /// <summary>
        /// Copies the entity
        /// </summary>
        /// <returns></returns>
        public IdentifiedData Copy()
        {
            var retVal = base.Clone() as Act;
            retVal.Relationships = new List<ActRelationship>(this.Relationships.ToArray());
            retVal.Identifiers = new List<ActIdentifier>(this.Identifiers.ToArray());
            retVal.Notes = new List<ActNote>(this.Notes.ToArray());
            retVal.Participations = new List<ActParticipation>(this.Participations.ToArray());
            retVal.Tags = new List<ActTag>(this.Tags.ToArray());
            retVal.Extensions = new List<ActExtension>(this.Extensions.ToArray());
            return retVal;
        }

        /// <summary>
        /// Add a policy to this act
        /// </summary>
        public void AddPolicy(string policyId)
        {
            var pol = EntitySource.Current.Provider.Query<SecurityPolicy>(o => o.Oid == policyId).SingleOrDefault();
            if (pol == null)
                throw new KeyNotFoundException($"Policy {policyId} not found");
            this.Policies.Add(new SecurityPolicyInstance(pol, PolicyGrantType.Grant));
        }

        /// <summary>
        /// Determines if the object has policy
        /// </summary>
        public bool HasPolicy(string policyId)
        {
            return this.LoadCollection<SecurityPolicyInstance>(nameof(Policies)).Any(o => o.LoadProperty<SecurityPolicy>(nameof(SecurityPolicyInstance.Policy)).Oid == policyId);
        }

        /// <summary>
        /// Add a tag to this act
        /// </summary>
        public ITag AddTag(String tagKey, String tagValue)
        {
            var tag = new ActTag(tagKey, tagValue);
            this.Tags.Add(tag);
            return tag;
        }

        /// <summary>
        /// Remove the specified extension
        /// </summary>
        /// <param name="extensionType">The type of extension to remove</param>
        public void RemoveExtension(Guid extensionType)
        {
            this.Extensions.RemoveAll(o => o.ExtensionTypeKey == extensionType);
        }

        /// <summary>
        /// Add the specified extension type to the collection
        /// </summary>
        /// <param name="extensionType">The extension type to be added</param>
        /// <param name="handlerType">The handler</param>
        /// <param name="value">The value</param>
        public void AddExtension(Guid extensionType, Type handlerType, object value)
        {
            this.Extensions.Add(new ActExtension(extensionType, handlerType, value));
        }

        /// <summary>
        /// Get the specified tag
        /// </summary>
        public string GetTag(string tagKey) => this.Tags.FirstOrDefault(o => o.TagKey == tagKey)?.Value;

        /// <summary>
        /// Remove <paramref name="tagKey"/> from the tag collection
        /// </summary>
        public void RemoveTag(string tagKey) => this.Tags.RemoveAll(o => o.TagKey == tagKey);
    }
}
