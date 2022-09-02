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
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Roles
{
    /// <summary>
    /// Represents an entity which is a patient
    /// </summary>

    [XmlType("Patient", Namespace = "http://santedb.org/model"), JsonObject("Patient")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Patient")]
    [ClassConceptKey(EntityClassKeyStrings.Patient)]
    public class Patient : Person
    {
        /// <summary>
        /// Represents a patient
        /// </summary>
        public Patient() 
        {
            base.DeterminerConceptKey = DeterminerKeys.Specific;
            base.m_classConceptKey = EntityClassKeys.Patient;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.Patient;

        /// <summary>
        /// Gets or sets the date the patient was deceased
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTime? DeceasedDate { get; set; }

        /// <summary>
        /// Deceased date XML
        /// </summary>
        [XmlElement("deceasedDate"), JsonProperty("deceasedDate"), SerializationMetadata]
        public String DeceasedDateXml
        {
            get
            {
                return this.DeceasedDate?.ToString("yyyy-MM-dd");
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    // Try to parse ISO date
                    if (DateTime.TryParseExact(value, new String[] { "o", "yyyy-MM-dd", "yyyy-MM", "yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                        this.DeceasedDate = dt;
                    else
                        throw new FormatException($"Cannot parse {value} as a date");
                }
                else
                    this.DeceasedDate = null;
            }
        }

        /// <summary>
        /// Gets or sets the precision of the date of deceased
        /// </summary>
        [XmlElement("deceasedDatePrecision"), JsonProperty("deceasedDatePrecision")]
        public DatePrecision? DeceasedDatePrecision { get; set; }

        /// <summary>
        /// Gets or sets the multiple birth order of the patient
        /// </summary>
        [XmlElement("multipleBirthOrder"), JsonProperty("multipleBirthOrder")]
        public int? MultipleBirthOrder { get; set; }

        /// <summary>
        /// Gets or sets the VIP code
        /// </summary>
        [XmlElement("vipStatus"), JsonProperty("vipStatus")]
        public Guid? VipStatusKey { get; set; }

        /// <summary>
        /// Gets or sets the key of the marital status concept
        /// </summary>
        [XmlElement("maritalStatus"), JsonProperty("maritalStatus")]
        public Guid? MaritalStatusKey { get; set; }

        /// <summary>
        /// Gets or sets the key of the education level
        /// </summary>
        [XmlElement("educationLevel"), JsonProperty("educationLevel")]
        public Guid? EducationLevelKey { get; set; }

        /// <summary>
        /// Gets or sets the living arrangement
        /// </summary>
        [XmlElement("livingArrangement"), JsonProperty("livingArrangement")]
        public Guid? LivingArrangementKey { get; set; }

        /// <summary>
        /// Gets or sets the religious affiliation
        /// </summary>
        [XmlElement("religion"), JsonProperty("religion")]
        public Guid? ReligiousAffiliationKey { get; set; }

        /// <summary>
        /// Gets or sets the religious affiliation
        /// </summary>
        [XmlElement("nationality"), JsonProperty("nationality"), EditorBrowsable(EditorBrowsableState.Advanced)]
        public Guid? NationalityKey
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the marital status code
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(MaritalStatusKey))]
        public Concept MaritalStatus { get; set; }

        /// <summary>
        /// Gets the nationality of the patient
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(NationalityKey))]
        public Concept Nationality
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the VIP status code
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(VipStatusKey))]
        public Concept VipStatus { get; set; }

        /// <summary>
        /// Gets or sets the education level of the person
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(EducationLevelKey))]
        public Concept EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets the living arrangements
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(LivingArrangementKey))]
        public Concept LivingArrangement { get; set; }

        /// <summary>
        /// Gets or sets the religious affiliation
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(ReligiousAffiliationKey))]
        public Concept ReligiousAffiliation { get; set; }

        /// <summary>
        /// Gets or sets the ethnicity codes
        /// </summary>
        [XmlElement("ethnicity"), JsonProperty("ethnicity")]
        public Guid? EthnicGroupKey { get; set; }

        /// <summary>
        /// Gets the ethic group concepts
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(EthnicGroupKey))]
        public Concept EthnicGroup
        {
            get; set;
        }

        /// <summary>
        /// Should serialize deceased date?
        /// </summary>
        public bool ShouldSerializeDeceasedDateXml()
        {
            return this.DeceasedDate.HasValue;
        }

        /// <summary>
        /// Should serialize deceasd date
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDeceasedDatePrecision() => this.DeceasedDatePrecision.HasValue;

        /// <summary>
        /// Should serialize deceased date?
        /// </summary>
        public bool ShouldSerializeMultipleBirthOrder()
        {
            return this.MultipleBirthOrder.HasValue;
        }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Patient;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.DeceasedDate == other.DeceasedDate &&
                this.DeceasedDatePrecision == other.DeceasedDatePrecision;
        }

    }
}