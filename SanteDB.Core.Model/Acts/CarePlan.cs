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
    public class CarePlan : Act
    {
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
        /// Create care plan with acts
        /// </summary>
        public CarePlan(Patient p, IEnumerable<Act> acts) : this()
        {
            this.Relationships.AddRange(acts.Select(o => new ActRelationship(ActRelationshipTypeKeys.HasComponent, o)));
            this.Participations.Add(new ActParticipation(ActParticipationKeys.RecordTarget, p));
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