/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 */
using Newtonsoft.Json;
using SanteDB.Core.i18n;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents a care plan
    /// </summary>
    /// <remarks>
    /// The care plan object is used to represent a collection of clinical protocols which the care planning
    /// engine proposes should be done as part of the patient's course of care.
    /// </remarks>
    [XmlType(nameof(CarePlan), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(CarePlan), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(CarePlan))]
    [XmlInclude(typeof(SubstanceAdministration))]
    [XmlInclude(typeof(QuantityObservation))]
    [XmlInclude(typeof(CodedObservation))]
    [XmlInclude(typeof(TextObservation))]
    [XmlInclude(typeof(PatientEncounter))]
    [ClassConceptKey(ActClassKeyStrings.CarePlan)]
    [ResourceSensitivity(ResourceSensitivityClassification.PersonalHealthInformation)]
    public class CarePlan : Act
    {

        private static readonly Guid?[] COPY_RELATIONSHIP_TYPES = new Guid?[]
        {
            ActRelationshipTypeKeys.Fulfills,
            ActRelationshipTypeKeys.Evaluates,
            ActRelationshipTypeKeys.IsCauseOf,
            ActRelationshipTypeKeys.Appends
        };

        /// <summary>
        /// Default ctor
        /// </summary>
        public CarePlan()
        {
            this.Key = Guid.NewGuid();
            this.CreationTime = DateTimeOffset.Now;
            this.MoodConceptKey = ActMoodKeys.Propose;
            base.m_classConceptKey = ActClassKeys.CarePlan;
        }

        /// <summary>
        /// Gets or sets the class concept key
        /// </summary>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == ActClassKeys.CarePlan;

        /// <summary>
        /// Gets or sets a human title for the care plan
        /// </summary>
        [XmlElement("title"), JsonProperty("title")]
        public String Title { get; set; }

        /// <summary>
        /// Identifies the care pathway identifier which this care plan is generated for
        /// </summary>
        /// <remarks>
        /// The program can be used to identify where the care plan "fits" in the broader patient care picture. For example, a care plan 
        /// generated as part of an HIV/TB care pathway may be kept separate from an ANC or pregnancy care plan
        /// </remarks>
        [XmlElement("pathway"), JsonProperty("pathway")]
        public Guid? CarePathwayKey { get; set; }

        /// <summary>
        /// Delay load property for <see cref="CarePathwayKey"/>
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CarePathwayKey))]
        public CarePathwayDefinition CarePathway { get; set; }

        /// <summary>
        /// Create care plan with acts
        /// </summary>
        public CarePlan(Patient p, IEnumerable<Act> acts) : this()
        {
            this.Relationships = new List<ActRelationship>(acts.Select(o => new ActRelationship(ActRelationshipTypeKeys.HasComponent, o)));
            this.Participations = new List<ActParticipation>() { new ActParticipation(ActParticipationKeys.RecordTarget, p) };
        }

        /// <summary>
        /// Create a care plan request
        /// </summary>
        public static CarePlan CreateCarePlanRequest(Patient p)
        {
            return new CarePlan(p, new Act[0]) { MoodConceptKey = ActMoodKeys.Request };
        }

    }
}