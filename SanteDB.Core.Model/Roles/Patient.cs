﻿/*
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
using System;
using System.Collections.Generic;
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

        // Marital statuses
        private Guid? m_maritalStatusKey;
        private Concept m_maritalStatus;
        private Guid? m_educationLevelKey;
        private Concept m_educationLevel;
        private Guid? m_livingArrangementKey;
        private Concept m_livingArrangement;
        private Guid? m_religiousAffiliationKey;
        private Concept m_religiousAffiliation;
        private Guid? m_ethnicGroupKey;
        private Concept m_ethnicGroup;
        private Guid? m_vipStatusKey;
        private Concept m_vipStatus;

        /// <summary>
        /// Represents a patient
        /// </summary>
        public Patient()
        {
            base.DeterminerConceptKey = DeterminerKeys.Specific;
            base.ClassConceptKey = EntityClassKeys.Patient;
        }

        /// <summary>
        /// Gets or sets the date the patient was deceased
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTime? DeceasedDate { get; set; }

        /// <summary>
        /// Deceased date XML
        /// </summary>
        [XmlElement("deceasedDate"), JsonProperty("deceasedDate"), DataIgnore]
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
        [XmlElement("vipStatus"), JsonProperty("vipStatus"), EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? VipStatusKey
        {
            get => this.m_vipStatusKey;
            set
            {
                this.m_vipStatusKey = value;
                this.m_vipStatus = null;
            }
        }


        /// <summary>
        /// Gets or sets the key of the marital status concept
        /// </summary>
        [XmlElement("maritalStatus"), JsonProperty("maritalStatus"), EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? MaritalStatusKey
        {
            get => this.m_maritalStatusKey;
            set
            {
                this.m_maritalStatusKey = value;
                this.m_maritalStatus = null;
            }
        }

        /// <summary>
        /// Gets or sets the key of the education level
        /// </summary>
        [XmlElement("educationLevel"), JsonProperty("educationLevel"), EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? EducationLevelKey
        {
            get => this.m_educationLevelKey;
            set
            {
                this.m_educationLevelKey = value;
                this.m_educationLevel = null;
            }
        }

        /// <summary>
        /// Gets or sets the living arrangement
        /// </summary>
        [XmlElement("livingArrangement"), JsonProperty("livingArrangement"), EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? LivingArrangementKey
        {
            get => this.m_livingArrangementKey;
            set
            {
                this.m_livingArrangementKey = value;
                this.m_livingArrangement = null;
            }
        }

        /// <summary>
        /// Gets or sets the religious affiliation
        /// </summary>
        [XmlElement("religion"), JsonProperty("religion"), EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? ReligiousAffiliationKey
        {
            get => this.m_religiousAffiliationKey;
            set
            {
                this.m_religiousAffiliationKey = value;
                this.m_religiousAffiliation = null;
            }
        }

        /// <summary>
        /// Gets or sets the marital status code
        /// </summary>
        [AutoLoad, XmlIgnore, JsonIgnore, SerializationReference(nameof(MaritalStatusKey))]
        public Concept MaritalStatus
        {
            get
            {
                this.m_maritalStatus = base.DelayLoad(this.m_maritalStatusKey, this.m_maritalStatus);
                return this.m_maritalStatus;
            }
            set
            {
                this.m_maritalStatus = value;
                this.m_maritalStatusKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the VIP status code
        /// </summary>
        [AutoLoad, XmlIgnore, JsonIgnore, SerializationReference(nameof(VipStatusKey))]
        public Concept VipStatus
        {
            get
            {
                this.m_vipStatus = base.DelayLoad(this.m_vipStatusKey, this.m_vipStatus);
                return this.m_vipStatus;
            }
            set
            {
                this.m_vipStatus = value;
                this.m_vipStatusKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the education level of the person
        /// </summary>
        [AutoLoad, XmlIgnore, JsonIgnore, SerializationReference(nameof(EducationLevelKey))]
        public Concept EducationLevel
        {
            get
            {
                this.m_educationLevel = base.DelayLoad(this.m_educationLevelKey, this.m_educationLevel);
                return this.m_educationLevel;
            }
            set
            {
                this.m_educationLevel = value;
                this.m_educationLevelKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the living arrangements
        /// </summary>
        [AutoLoad, XmlIgnore, JsonIgnore, SerializationReference(nameof(LivingArrangementKey))]
        public Concept LivingArrangement
        {
            get
            {
                this.m_livingArrangement = base.DelayLoad(this.m_livingArrangementKey, this.m_livingArrangement);
                return this.m_livingArrangement;
            }
            set
            {
                this.m_livingArrangement = value;
                this.m_livingArrangementKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the religious affiliation
        /// </summary>
        [AutoLoad, XmlIgnore, JsonIgnore, SerializationReference(nameof(ReligiousAffiliationKey))]
        public Concept ReligiousAffiliation
        {
            get
            {
                this.m_religiousAffiliation = base.DelayLoad(this.m_religiousAffiliationKey, this.m_religiousAffiliation);
                return this.m_religiousAffiliation;
            }
            set
            {
                this.m_religiousAffiliation = value;
                this.m_religiousAffiliationKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the ethnicity codes
        /// </summary>
        [XmlElement("ethnicity"), JsonProperty("ethnicity")]
        public Guid? EthnicGroupCodeKey
        {
            get => this.m_ethnicGroupKey;
            set
            {
                this.m_ethnicGroupKey = value;
                this.m_ethnicGroup = null;
            }
        }

        /// <summary>
        /// Gets the ethic group concepts
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(EthnicGroupCodeKey))]
        public Concept EthnicGroup
        {
            get
            {
                this.m_ethnicGroup = base.DelayLoad(this.m_ethnicGroupKey, this.m_ethnicGroup);
                return this.m_ethnicGroup;
            }
            set
            {
                this.m_ethnicGroup = value;
                this.m_ethnicGroupKey = value?.Key;
            }
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
