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
using SanteDB.Core.Model.Entities;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents an act whereby a substance is administered to the patient
    /// </summary>
    /// <remarks>
    /// <para>The substance administration act is used whenever a clinician administers, plans to administer or should administer to a patient, a substance. 
    /// The substance that is administered is open but should be represented as either a Consumable (something that was consumed in the act of administration 
    /// like a manufactured material (<see cref="ManufacturedMaterial"/>) or a product (if proposing or planning)).</para>
    /// <para>
    /// The type of administration (immunization, drug therapy, treatment, etc.) is classified by the substance administration's type concept (<see cref="Act.TypeConceptKey"/>). In some cases
    /// the dose quantity or dose measure are not required (when giving just "a dose") however it is recommended that implementations accurately track
    /// how much of the substance was administered.
    /// </para>
    /// </remarks>
    [XmlType("SubstanceAdministration", Namespace = "http://santedb.org/model"), JsonObject("SubstanceAdministration")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SubstanceAdministration")]
    [ClassConceptKey(ActClassKeyStrings.SubstanceAdministration)]
    public class SubstanceAdministration : Act
    {
        
        /// <summary>
        /// Substance administration ctor
        /// </summary>
        public SubstanceAdministration()
        {
            base.ClassConceptKey = ActClassKeys.SubstanceAdministration;
        }

        /// <summary>
        /// Gets or sets the class concept key
        /// </summary>
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        public override Guid? ClassConceptKey { get => ActClassKeys.SubstanceAdministration; set => base.ClassConceptKey = ActClassKeys.SubstanceAdministration; }

        /// <summary>
        /// Gets or sets the key for route
        /// </summary>
        [XmlElement("route"), JsonProperty("route")]
        public Guid? RouteKey { get; set; }

        /// <summary>
        /// Gets or sets the key for dosing unit
        /// </summary>
        [XmlElement("doseUnit"), JsonProperty("doseUnit")]
        public Guid? DoseUnitKey { get; set; }

        /// <summary>
        /// Gets or sets a concept which indicates the route of administration (eg: Oral, Injection, etc.)
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(RouteKey))]
        public Concept Route { get; set; }

        /// <summary>
        /// Gets or sets a concept which indicates the unit of measure for the dose (eg: 5 mL, 10 mL, 1 drop, etc.)
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(DoseUnitKey))]
        public Concept DoseUnit { get; set; }

        /// <summary>
        /// Gets or sets the amount of substance administered
        /// </summary>
        [XmlElement("doseQuantity"), JsonProperty("doseQuantity")]
        public Decimal DoseQuantity { get; set; }

        /// <summary>
        /// The sequence of the dose (i.e. OPV 0 = 0 , OPV 1 = 1, etc.)
        /// </summary>
        [XmlElement("doseSequence"), JsonProperty("doseSequence")]
        public int SequenceId { get; set; }


        /// <summary>
        /// Gets or sets the site
        /// </summary>
        [XmlElement("site"), JsonProperty("site")]
        public Guid? SiteKey { get; set; }

        /// <summary>
        /// Gets or sets a concept which indicates the site of administration
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(SiteKey))]
        public Concept Site { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as SubstanceAdministration;
            if (other == null) return false;
            return base.SemanticEquals(obj) && other.SiteKey == this.SiteKey &&
                other.RouteKey == this.RouteKey &&
                other.DoseUnitKey == this.DoseUnitKey &&
                other.DoseQuantity == this.DoseQuantity &&
                other.SequenceId == this.SequenceId;
        }

        /// <summary>
        /// Should serialize site key
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSiteKey() => this.SiteKey.HasValue;
    }
}
