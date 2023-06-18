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
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents an encounter a patient has with the health system
    /// </summary>
    ///<remarks>
    ///<para>An encounter is a special type of act which represents an episode of care which a patient experiences with the health system.
    ///An encounter is used to document things like hospital visits, inpatient care encounters, or any longer running series of actions which
    ///are linked by the admit -&gt; discharge workflow.</para>
    /// </remarks>
    [XmlType("PatientEncounter", Namespace = "http://santedb.org/model"), JsonObject("PatientEncounter")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "PatientEncounter")]
    [ClassConceptKey(ActClassKeyStrings.Encounter)]
    public class PatientEncounter : Act
    {
        /// <summary>
        /// Patient encounter ctor
        /// </summary>
        public PatientEncounter()
        {
            base.m_classConceptKey = ActClassKeys.Encounter;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == ActClassKeys.Encounter;

        /// <summary>
        /// Gets or sets the key of discharge disposition
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("dischargeDisposition"), JsonProperty("dischargeDisposition")]
        public Guid? DischargeDispositionKey { get; set; }

        /// <summary>
        /// Gets or sets the admission source type
        /// </summary>
        [XmlElement("admissionSource"), JsonProperty("admissionSource")]
        public Guid? AdmissionSourceTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the discharge disposition (how the patient left the encounter
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(DischargeDispositionKey))]
        public Concept DischargeDisposition { get; set; }

        /// <summary>
        /// Gets or sets the code indicating the type of place which was responsible for care prior to admission
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(AdmissionSourceTypeKey))]
        public Concept AdmissionSourceType { get; set; }

        /// <summary>
        /// A list of special arrangements which are to be made to the patient during the encounter
        /// </summary>
        /// <remarks>These are special arrangements which should be made for this encounter. These can be 
        /// special considerations (VIP, private room, etc.) or other codes indicating care for the patient (seeing eye dog, attendant, interpreter, etc.)</remarks>
        [XmlElement("specialArrangement"), JsonProperty("specialArrangement")]
        public List<PatientEncounterArrangement> SpecialArrangements { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as PatientEncounter;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) && other.DischargeDispositionKey == this.DischargeDispositionKey;
        }
    }
}