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
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents an entity which is a person
    /// </summary>

    [XmlType("Person", Namespace = "http://santedb.org/model"), JsonObject("Person")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Person")]
    [ClassConceptKey(EntityClassKeyStrings.Person)]
    [ResourceSensitivity(ResourceSensitivityClassification.PersonalHealthInformation)]
    public class Person : Entity
    {

        /// <summary>
        /// Person constructor
        /// </summary>
        public Person()
        {
            base.DeterminerConceptKey = DeterminerKeys.Specific;
            base.m_classConceptKey = EntityClassKeys.Person;

        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.Person;

        /// <summary>
        /// Gets or sets the person's date of birth
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the precision ofthe date of birth
        /// </summary>
        [XmlElement("dateOfBirthPrecision"), JsonProperty("dateOfBirthPrecision")]
        public DatePrecision? DateOfBirthPrecision { get; set; }

        /// <summary>
        /// Gets or sets the gender concept key
        /// </summary>
        [XmlElement("genderConcept"), JsonProperty("genderConcept")]
        public Guid? GenderConceptKey { get; set; }

        /// <summary>
        /// Gets or sets the gender concept
        /// </summary>
        [SerializationReference(nameof(GenderConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept GenderConcept { get; set; }

        /// <summary>
        /// Gets the date of birth as XML
        /// </summary>
        [XmlElement("dateOfBirth"), JsonProperty("dateOfBirth"), SerializationMetadata]
        public String DateOfBirthXml
        {
            get
            {
                return this.DateOfBirth?.ToString("yyyy-MM-dd");
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    // Try to parse ISO date
                    if (DateTime.TryParseExact(value, new String[] { "o", "yyyy-MM-dd", "yyyy-MM", "yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                    {
                        this.DateOfBirth = dt;
                    }
                    else if(DateTime.TryParse(value, CultureInfo.InvariantCulture , DateTimeStyles.AssumeLocal, out dt))
                    {
                        this.DateOfBirth = dt.Date;
                    }
                    else
                    {
                        throw new FormatException($"Cannot parse {value} as a date");
                    }
                }
                else
                {
                    this.DateOfBirth = null;
                }
            }
        }

        /// <summary>
        /// Gets the person's languages of communication
        /// </summary>
        [XmlElement("language"), JsonProperty("language")]
        public List<PersonLanguageCommunication> LanguageCommunication { get; set; }

        /// <summary>
        /// Gets or sets the religious affiliation
        /// </summary>
        [XmlElement("occupation"), JsonProperty("occupation")]
        public Guid? OccupationKey { get; set; }

        /// <summary>
        /// Gets or sets the marital status code
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(OccupationKey))]
        public Concept Occupation { get; set; }

        /// <summary>
        /// Should serialize date of birth precision
        /// </summary>
        public bool ShouldSerializeDateOfBirthPrecision()
        {
            return this.DateOfBirthPrecision.HasValue;
        }


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
                    {
                        this.DeceasedDate = dt;
                    }
                    else
                    {
                        throw new FormatException($"Cannot parse {value} as a date");
                    }
                }
                else
                {
                    this.DeceasedDate = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the precision of the date of deceased
        /// </summary>
        [XmlElement("deceasedDatePrecision"), JsonProperty("deceasedDatePrecision")]
        public DatePrecision? DeceasedDatePrecision { get; set; }

        /// <summary>
        /// Should serialize deceased date
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDeceasedDatePrecision() => this.DeceasedDatePrecision.HasValue;



        /// <summary>
        /// Gets or sets the VIP code
        /// </summary>
        [XmlElement("vipStatus"), JsonProperty("vipStatus")]
        public Guid? VipStatusKey { get; set; }

        /// <summary>
        /// Gets or sets the VIP status code
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(VipStatusKey))]
        public Concept VipStatus { get; set; }

        /// <summary>
        /// Gets or sets the religious affiliation
        /// </summary>
        [XmlElement("nationality"), JsonProperty("nationality")]
        public Guid? NationalityKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the nationality of the patient
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(NationalityKey))]
        public Concept Nationality
        {
            get;
            set;
        }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Person;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.DateOfBirth == other.DateOfBirth &&
                this.GenderConceptKey == other.GenderConceptKey &&
                this.DateOfBirthPrecision == other.DateOfBirthPrecision &&
                this.NationalityKey == other.NationalityKey &&
                this.LanguageCommunication?.SemanticEquals(other.LanguageCommunication) != false;
        }
    }
}