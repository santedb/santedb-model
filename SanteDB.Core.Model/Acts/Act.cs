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
 * Date: 2021-8-27
 */
using Newtonsoft.Json;
using SanteDB.Core.i18n;
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
    ///     <item><term><see cref="SubstanceAdministration"/></term><description>The administration of a substance to a patient</description> </item>
    ///     <item><term><see cref="Observation"/></term><description>The observing of a value for the patient</description></item>
    ///     <item><term><see cref="PatientEncounter"/></term><description>An encounter or visit that occurs where the patient receives one or more services</description></item>
    ///     <item><term><see cref="Act"/></term><description>Any other action such as supply request, or problem recordation</description></item>
    /// </list>
    /// <para>
    /// The property which classifies what specific type of action an act represents is its <see cref="ClassConceptKey"/>, which indicates whether
    /// the act is an observation, substance administration, etc. Class concept keys can be found in the <see cref="ActClassKeys"/> constants declaration.
    /// </para>
    /// <para>
    /// Furthermore, the <see cref="Act"/> structure is used to represent events, proposals, requests, goals, etc. That is to say, the Act structure
    /// can represent the request to do an act, the intent to perform an act, or the actual act being performed itself. This classification of mode
    /// happens based on the <see cref="MoodConceptKey"/> mood concept. Mood concept keys can be found on the <see cref="ActMoodKeys"/> structure.
    /// </para>
    /// <para>
    /// Acts may also be further classified by their <see cref="TypeConceptKey"/>. The <see cref="TypeConceptKey"/> is an implementation specific value
    /// which is used by implementers to determine whether a particular act (for example, a <see cref="Observation"/>) was an observation of weight,
    /// of height, etc.
    /// </para>
    /// </remarks>
    [XmlType(Namespace = "http://santedb.org/model", TypeName = "Act")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Act")]
    [JsonObject("Act")]
    [Classifier(nameof(ClassConcept))]
    public class Act : VersionedEntityData<Act>, ITaggable, ISecurable, IExtendable, IHasClassConcept, IHasState, IGeoTagged, IHasTemplate, IHasIdentifiers, IHasRelationships
    {

        /// <summary>
        /// Internal class key
        /// </summary>
        protected Guid? m_classConceptKey;

        /// <summary>
        /// Constructor for ACT
        /// </summary>
        public Act()
        {
        }

        /// <summary>
        /// Identifies whether the act represented in this instance actually occurred
        /// </summary>
        /// <remarks>
        /// <para>Whenever an implementation requires the representation of an act which <strong>DID NOT</strong> occur,
        /// the <see cref="IsNegated"/> property is set to TRUE. This indicator has the following semantic meanings based on <see cref="MoodConceptKey"/>
        /// </para>
        /// <list type="table">
        ///     <item>
        ///         <term>EventOccurence</term>
        ///         <description>The act DID NOT occur</description>
        ///     </item>
        ///     <item>
        ///         <term>Propose</term>
        ///         <description>The act SHOULD NOT occur</description>
        ///     </item>
        ///     <item>
        ///         <term>Intent</term>
        ///         <description>The act WILL NOT occur</description>
        ///     </item>
        ///     <item>
        ///         <term>Goal</term>
        ///         <description>The goal is for the act TO NOT OCCUR</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="MoodConceptKey"/>
        [XmlElement("isNegated"), JsonProperty("isNegated")]
        public Boolean IsNegated { get; set; }

        /// <summary>
        /// The instant when the act occurred, or will occur
        /// </summary>
        /// <remarks>
        /// <para>The act time property is used to determine when the act being represented occurred. Based on the <see cref="MoodConceptKey"/> specified,
        /// the semantic meaning of the property differs slightly:</para>
        /// <list type="table">
        ///     <item>
        ///        <term>EventOccurence</term>
        ///        <description>The time that the act did occur (or if negation is false, the time the act did not occur)</description>
        ///     </item>
        ///     <item>
        ///        <term>Propose</term>
        ///        <description>The time that the act should occur</description>
        ///     </item>
        ///     <item>
        ///        <term>Goal</term>
        ///        <description>The due date/time for the goal statement</description>
        ///     </item>
        ///     <item>
        ///        <term>Intent</term>
        ///        <description>The time the indicated act is intended to be performed/occur</description>
        ///     </item>
        ///     <item>
        ///        <term>Request</term>
        ///        <description>The time that the specified request was made</description>
        ///     </item>
        /// </list>
        /// <para>If the desire is to represent the act as a time bounds (start and stop time) then use of the <see cref="StartTime"/> and <see cref="StopTime"/> should be
        /// used.</para>
        /// </remarks>
        /// <seealso cref="StartTime"/>
        /// <seealso cref="StopTime"/>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? ActTime { get; set; }

        /// <summary>
        /// The template on which the act is based
        /// </summary>
        /// <remarks>
        /// <para>
        /// Templates are used to classify the specific rules and input forms used to create the act. It further
        /// classifies the type of act in a manner which allows a consumer to render the data or to validate the data.
        /// </para>
        /// </remarks>
        /// <seealso cref="Template"/>
        [XmlElement("template"), JsonProperty("template")]
        public Guid? TemplateKey { get; set; }

        /// <summary>
        /// Delay load property for the template
        /// </summary>
        /// <remarks>
        /// <para>This property is used for easy access to the structured template information for the template
        /// on which the act is based. This property is loaded based on the UUID specified in <see cref="TemplateKey"/></para>
        /// <para>Templates are used to define:</para>
        /// <list type="bullet">
        ///     <item>The overall structure/seed data to be included in the instance</item>
        ///     <item>The implementation specific constraints on the instance (example: act.immunization vs. act.immunization.booster)</item>
        ///     <item>User interface forms and views to be rendered </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="TemplateKey" />
        /// <example lang="C#">
        /// <![CDATA[
        ///     var act = persistenceService.Find(o => o.ClassConceptKey == ClassConceptKeys.SubstanceAdministration).FirstOrDefault();
        ///     Console.WriteLine("This substance administration is based on template {0}", act.LoadProperty(o => o.Template).Name);
        /// ]]>
        /// </example>
        [SerializationReference(nameof(TemplateKey)), XmlIgnore, JsonIgnore]
        public TemplateDefinition Template { get; set; }

        /// <summary>
        /// The moment in time that this act occurred in ISO format
        /// </summary>
        /// <seealso cref="ActTime"/>
        /// <exception cref="FormatException">When the format of the provided string does not conform to ISO date format</exception>
        [SerializationMetadata, XmlElement("actTime"), JsonProperty("actTime")]
        public String ActTimeXml
        {
            get { return this.ActTime?.ToString("o", CultureInfo.InvariantCulture); }
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
                    this.ActTime = null;
            }
        }

        /// <summary>
        /// The date/time when the act started to occur
        /// </summary>
        /// <remarks><para>When the act is a long running action, or if the act is ongoing, then the <see cref="StartTime"/>
        /// may be set to indicate the beginning of the act.</para>
        /// <para>When the <see cref="MoodConceptKey"/> is set to <c>Propose</c> then this value represents the minimum safe
        /// start time of the activity represented.</para>
        /// <para>This property is typically used in conjunction with the <see cref="StopTime"/> property, which indicates the
        /// time and date when the activity stopped occurring (additionally, when <see cref="MoodConceptKey"/> is set, then
        /// the maximum safe time of the activity represented).</para>
        /// </remarks>
        /// <seealso cref="StopTime"/>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// The time when the act should or did start ocurring in ISO format
        /// </summary>
        /// <seealso cref="StartTime"/>
        /// <exception cref="FormatException">When the format of the provided string does not conform to ISO date format</exception>
        [SerializationMetadata, XmlElement("startTime"), JsonProperty("startTime")]
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
        /// The time and date when the act did or should stop occurring
        /// </summary>
        /// <remarks>
        /// <para>This property is used in conjunction with the <see cref="StartTime"/> to represent the
        /// date and time when the activity described in this act stopped, or should stop occurring. When the <see cref="MoodConceptKey"/>
        /// is Propose then this property represents the maximum safe time for the act to occur.</para>
        /// </remarks>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? StopTime { get; set; }

        /// <summary>
        /// The time when the act should or did stop ocurring in ISO format
        /// </summary>
        /// <see cref="StopTime"/>
        /// <exception cref="FormatException">When the provided value does not conform to ISO formatted date</exception>

        [SerializationMetadata, XmlElement("stopTime"), JsonProperty("stopTime")]
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
        /// The classification key of the activity
        /// </summary>
        /// <remarks>
        /// <para>The classification concept is used to dictate the overall structure and properties of the activity. The act loader
        /// in SanteDB will load the appropriate class instance of the act using this information. For example, calling <c>Get()</c> on
        /// an act which is a <see cref="SubstanceAdministration"/> will result in a SubstanceAdministration instance being loaded.</para>
        /// <para>Class concepts in SanteDB dictate the major classification of the object, for implementation specific or extended
        /// classifications (or sub-types) it is recommended that implementers use the <see cref="TypeConceptKey"/> property. </para>
        /// </remarks>
        /// <seealso cref="ClassConcept"/>
        /// <seealso cref="ActClassKeys"/>
        /// <seealso href="https://help.santesuite.org/santedb/architecture/data-and-information-architecture/conceptual-data-model/acts/class-concepts"/>
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        [Binding(typeof(ActClassKeys))]
        public Guid? ClassConceptKey 
        {
            get => this.m_classConceptKey;
            set
            {
                if(!this.ValidateClassKey(value))
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
        /// The mood (or mode) of the Act instance
        /// </summary>
        /// <see cref="ActMoodKeys"/>
        /// <remarks>
        /// <para>In SanteDB, a mood of an act describes the mode of that act. The mood of the act classifies whether the act did occur, is intended to occur, is requested to occur or proposed. Mood codes may include:</para>
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
        /// <seealso cref="MoodConcept"/>
        /// <seealso href="https://help.santesuite.org/santedb/architecture/data-and-information-architecture/conceptual-data-model/acts/mood-concepts"/>
        [XmlElement("moodConcept"), JsonProperty("moodConcept")]
        [Binding(typeof(ActMoodKeys))]
        public virtual Guid? MoodConceptKey { get; set; }

        /// <summary>
        /// Identifies a codified reason as to why this act did (or did not, or should or should not) occur.
        /// </summary>
        /// <see cref="ActReasonKeys"/>
        /// <see cref="NullReasonKeys"/>
        /// <remarks>
        /// <para>The reason concept is used to provide contextual information about why the act exists in its current state. For example: patient request,
        /// saftey concern, required by law, etc.</para>
        /// <para>Additionally, the reason code in SanteDB also allows for an indication of why an act did not occur (i.e. when the negation indicator is
        /// set) and can be used to indicate an override of a proposal by the user (i.e. patient safety or religious exception), or some other
        /// extenuating factor (i.e. value negative inifinity)</para>
        /// </remarks>
        /// <seealso cref="ReasonConcept"/>
        ///
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("reasonConcept"), JsonProperty("reasonConcept")]
        [Binding(typeof(ActReasonKeys))]
        public Guid? ReasonConceptKey { get; set; }

        /// <summary>
        /// The concept which describes the current status of the act
        /// </summary>
        /// <see cref="StatusKeys"/>
        /// <remarks>
        /// <para>The status concepts for an act allow for a basic state machine to be represented in SanteDB. The state machine for acts in SanteDB comprise of the
        /// following codes (defined in <see cref="StatusKeys"/>):</para>
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
        /// <seealso cref="StatusKeys" />
        /// <seealso cref="StatusConcept"/>
        /// <seealso href="https://help.santesuite.org/santedb/architecture/data-and-information-architecture/conceptual-data-model/acts/state-machine"/>
        [XmlElement("statusConcept"), JsonProperty("statusConcept")]
        [Binding(typeof(StatusKeys))]
        public Guid? StatusConceptKey { get; set; }

        /// <summary>
        /// Gets or sets the key of the concept which further classifies the type of act occurring
        /// </summary>
        /// <see cref="TypeConcept"/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("typeConcept"), JsonProperty("typeConcept")]
        public Guid? TypeConceptKey { get; set; }

        /// <summary>
        /// Gets the delay-loaded value of the <see cref="ClassConceptKey"/>
        /// </summary>
        /// <remarks>
        /// <para>The loading of this delay-loaded property is based on the configuration of the SanteDB host environment (i.e.
        /// whether deep-loading of lazy-loading of properties is enabled). It is good practice to load any delay-load properties
        /// using the <c>LoadProperty</c> method, such as:
        /// <code>act.LoadProperty(o=>o.ClassConcept)</code></para>
        /// </remarks>
        /// <seealso cref="ActClassKeys"/>
        /// <seealso cref="ClassConceptKey"/>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ClassConceptKey))]
        public Concept ClassConcept { get; set; }

        /// <summary>
        /// Gets the delay-loaded value of the <see cref="MoodConceptKey"/> property
        /// </summary>
        /// <remarks>
        /// <para>The loading of this delay-loaded property is based on the configuration of the SanteDB host environment (i.e.
        /// whether deep-loading of lazy-loading of properties is enabled). It is good practice to load any delay-load properties
        /// using the <c>LoadProperty</c> method, such as:
        /// <code>act.LoadProperty(o=>o.MoodConcept)</code></para>
        /// </remarks>
        /// <see cref="ActMoodKeys"/>
        /// <seealso cref="MoodConceptKey"/>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(MoodConceptKey))]
        public Concept MoodConcept { get; set; }

        /// <summary>
        /// Delay loads the concept from <see cref="ReasonConceptKey"/>
        /// </summary>
        /// <remarks>
        /// <para>The loading of this delay-loaded property is based on the configuration of the SanteDB host environment (i.e.
        /// whether deep-loading of lazy-loading of properties is enabled). It is good practice to load any delay-load properties
        /// using the <c>LoadProperty</c> method, such as:
        /// <code>act.LoadProperty(o=>o.ReasonConcept)</code></para>
        /// </remarks>
        /// <seealso cref="ActReasonKeys"/>
        /// <seealso cref="ReasonConceptKey"/>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ReasonConceptKey))]
        public Concept ReasonConcept { get; set; }

        /// <summary>
        /// Delay loads the concept represented in <see cref="StatusConceptKey"/>
        /// </summary>
        /// <remarks>
        /// <para>The loading of this delay-loaded property is based on the configuration of the SanteDB host environment (i.e.
        /// whether deep-loading of lazy-loading of properties is enabled). It is good practice to load any delay-load properties
        /// using the <c>LoadProperty</c> method, such as:
        /// <code>act.LoadProperty(o=>o.StatusConcept)</code></para>
        /// </remarks>
        /// <seealso cref="StatusConceptKey"/>
        ///
        [SerializationReference(nameof(StatusConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept StatusConcept { get; set; }

        /// <summary>
        /// Delay loads the concept represented in <see cref="TypeConceptKey"/>
        /// </summary>
        /// <remarks>
        /// <para>The loading of this delay-loaded property is based on the configuration of the SanteDB host environment (i.e.
        /// whether deep-loading of lazy-loading of properties is enabled). It is good practice to load any delay-load properties
        /// using the <c>LoadProperty</c> method, such as:
        /// <code>act.LoadProperty(o=>o.TypeConcept)</code></para>
        /// </remarks>
        /// <see cref="TypeConceptKey"/>
        [SerializationReference(nameof(TypeConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept TypeConcept { get; set; }

        /// <summary>
        /// Identifiers by which this act is known
        /// </summary>
        /// <remarks>
        /// <para>
        /// The identifiers property is used to collect identifiers issued by other systems which also maintian copies/links to
        /// it. The identifiers property, for example:
        /// </para>
        /// <list type="bullet">
        ///     <item>Accession numbers for the object on a PACS or RIS</item>
        ///     <item>Legal/Accounting tracking numbers</item>
        ///     <item>Original submission identification from third party systems</item>
        /// </list>
        /// <para>
        /// Identifiers are stored as a combinatin of an identity domain (the authority under which the identifier is issued) and
        /// the identifier value.
        /// </para>
        /// </remarks>
        /// <seealso cref="ActIdentifier"/>
        [XmlElement("identifier"), JsonProperty("identifier")]
        public List<ActIdentifier> Identifiers { get; set; }

        /// <summary>
        /// Gets a list of all associated acts for this act
        /// </summary>
        /// <remarks>
        /// The relationships of an act are used to relate one or more acts together either
        /// directly as in an encounter with component acts, or between care episodes for chronic
        /// care.
        /// </remarks>
        [XmlElement("relationship"), JsonProperty("relationship")]
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
        [XmlElement("extension"), JsonProperty("extension")]
        public List<ActExtension> Extensions { get; set; }

        /// <summary>
        /// Gets a list of all notes associated with the act
        /// </summary>
        /// <remarks>Allows one or more notes to be taken about an act by a clinician</remarks>
        [XmlElement("note"), JsonProperty("note")]
        public List<ActNote> Notes { get; set; }

        /// <summary>
        /// Gets a list of all tags associated with the act
        /// </summary>
        /// <remarks>
        /// A tag is a simple piece of data which is appended to an act which allows developers to
        /// extend the underlying application in ways not imagined by the original SanteDB team. Tags differ
        /// from extensions in that they can only carry simple values (strings) and they are not versioned.
        /// </remarks>
        [XmlElement("tag"), JsonProperty("tag")]
        public List<ActTag> Tags { get; set; }

        /// <summary>
        /// Identifies protocols attached to the act
        /// </summary>
        /// <remarks>
        /// The protocols element is used to track which clinical protocols where linked with
        /// the act.
        /// </remarks>
        [XmlElement("protocol"), JsonProperty("protocol")]
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
        /// <see cref="ActParticipationKeys"/>
        [XmlElement("participation"), JsonProperty("participation")]
        public List<ActParticipation> Participations { get; set; }

        /// <summary>
        /// True if reason concept key should be serialized
        /// </summary>
        public bool ShouldSerializeReasonConceptKey() => this.ReasonConceptKey.HasValue && this.ReasonConceptKey.GetValueOrDefault() != Guid.Empty;

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
        public bool ShouldSerializePolicies() => !this.Policies.IsNullOrEmpty();

        /// <summary>
        /// Gets the tags
        /// </summary>
        [XmlIgnore, JsonIgnore]
        IEnumerable<ITag> ITaggable.Tags { get { return this.LoadCollection<EntityTag>(nameof(Act.Tags)).OfType<ITag>(); } }

        /// <summary>
        /// Get all extensions on the act
        /// </summary>
        [XmlIgnore, JsonIgnore]
        IEnumerable<IModelExtension> IExtendable.Extensions { get { return this.LoadCollection<EntityExtension>(nameof(Act.Extensions)).OfType<IModelExtension>(); } }

        /// <summary>
        /// Gets or sets the geo-tag
        /// </summary>
        [XmlElement("geo"), JsonProperty("geo")]
        public GeoTag GeoTag { get; set; }

        /// <summary>
        /// Gets the geo tag key
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Guid? GeoTagKey { get; set; }

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
        /// Add a relationship to this relationship container
        /// </summary>
        /// <param name="association">The association to be added</param>
        void IHasRelationships.AddRelationship(ITargetedAssociation association)
        {
            if (association is ActRelationship ar)
            {
                this.LoadProperty(o => o.Relationships).Add(ar);
            }
            else
            {
                throw new InvalidOperationException($"Expected ActRelationship but got {association.GetType()}");
            }
        }

        /// <summary>
        /// Remove a relationship from this relationship container
        /// </summary>
        /// <param name="association">The association to be removed</param>
        void IHasRelationships.RemoveRelationship(ITargetedAssociation association)
        {
            if (association is ActRelationship ar)
            {
                this.LoadProperty(o => o.Relationships).Remove(ar);
            }
            else
            {
                throw new InvalidOperationException($"Expected ActRelationship but got {association.GetType()}");
            }
        }

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
        /// Add a tag to this act
        /// </summary>
        public ITag AddTag(String tagKey, String tagValue)
        {
            var tag = this.LoadProperty(o => o.Tags)?.FirstOrDefault(o => o.TagKey == tagKey);
            this.Tags = this.Tags ?? new List<ActTag>();
            if (tag == null)
            {
                tag = new ActTag(tagKey, tagValue);
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
        public string GetTag(string tagKey) => tagKey.StartsWith("$") ? this.Tags?.FirstOrDefault(o => o.TagKey == tagKey)?.Value : this.LoadProperty(o=>o.Tags).FirstOrDefault(t=>t.TagKey == tagKey)?.Value;

        /// <summary>
        /// Remove <paramref name="tagKey"/> from the tag collection
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
        /// Validate the class key
        /// </summary>
        protected virtual bool ValidateClassKey(Guid? classKey) => true;
    }
}