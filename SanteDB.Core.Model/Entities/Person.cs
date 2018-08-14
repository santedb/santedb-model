/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
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
 * Date: 2017-9-1
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
	/// <summary>
	/// Represents an entity which is a person
	/// </summary>

	[XmlType("Person", Namespace = "http://santedb.org/model"), JsonObject("Person")]
	[XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Person")]
	public class Person : Entity
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

        // Complex relationships
        private List<Guid> m_raceCodeKeys;
        private List<Guid> m_disabilityKeys;
        private List<Guid> m_ethnicGroupKeys;
        private List<Guid> m_citizenshipKeys;

		/// <summary>
		/// Person constructor
		/// </summary>
		public Person()
		{
			base.DeterminerConceptKey = DeterminerKeys.Specific;
			base.ClassConceptKey = EntityClassKeys.Person;
			this.LanguageCommunication = new List<PersonLanguageCommunication>();
            this.RaceCodeKeys = new List<Guid>();
            this.EthnicGroupCodeKeys = new List<Guid>();
            this.DisabilityCodeKeys = new List<Guid>();
            this.CitizenshipKeys = new List<Guid>();
		}

		/// <summary>
		/// Gets the security user account associated with this person if applicable
		/// </summary>
		[XmlIgnore, JsonIgnore, DataIgnore]
		public virtual SecurityUser AsSecurityUser { get { return null; } }

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
		/// Gets the date of birth as XML
		/// </summary>
		[XmlElement("dateOfBirth"), JsonProperty("dateOfBirth"), DataIgnore]
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
                    if(value.Length > 10)
                        this.DateOfBirth = DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture);
                    else
                        this.DateOfBirth = DateTime.ParseExact(value.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                }
                else
                    this.DateOfBirth = null;
			}
		}

		/// <summary>
		/// Gets the person's languages of communication
		/// </summary>
		[AutoLoad, XmlElement("language"), JsonProperty("language")]
		public List<PersonLanguageCommunication> LanguageCommunication { get; set; }

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
                this.m_educationLevelKey= value?.Key;
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
        /// Gets or sets the race codes
        /// </summary>
        [XmlElement("race"), JsonProperty("race")]
        public List<Guid> RaceCodeKeys
        {
            get => this.m_raceCodeKeys;
            set => this.m_raceCodeKeys = value;
        }

        /// <summary>
        /// Gets or sets the ethnicity codes
        /// </summary>
        [XmlElement("ethnicity"), JsonProperty("ethnicity")]
        public List<Guid> EthnicGroupCodeKeys
        {
            get => this.m_ethnicGroupKeys;
            set => this.m_ethnicGroupKeys = value;
        }

        /// <summary>
        /// Gets or sets the disability codes
        /// </summary>
        [XmlElement("disability"), JsonProperty("disability")]
        public List<Guid> DisabilityCodeKeys
        {
            get => this.m_disabilityKeys;
            set => this.m_disabilityKeys = value;
        }


        /// <summary>
        /// Gets or sets the disability codes
        /// </summary>
        [XmlElement("citizenship"), JsonProperty("citizenship")]
        public List<Guid> CitizenshipKeys
        {
            get => this.m_citizenshipKeys;
            set => this.m_citizenshipKeys = value;
        }

        /// <summary>
        /// Gets the race concepts
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public IEnumerable<Concept> RaceCodes
        {
            get => this.m_raceCodeKeys.Select(r => base.DelayLoad<Concept>(r, null));
        }

        /// <summary>
        /// Gets the ethic group concepts
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public IEnumerable<Concept> EthnicGroupCodes
        {
            get => this.m_ethnicGroupKeys.Select(r => base.DelayLoad<Concept>(r, null));
        }

        /// <summary>
        /// Gets the disability concepts
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public IEnumerable<Concept> DisabiltyCodes
        {
            get => this.m_disabilityKeys.Select(r => base.DelayLoad<Concept>(r, null));
        }

        /// <summary>
        /// Gets the citizenships of the person
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public IEnumerable<Concept> Citizenship
        {
            get => this.m_citizenshipKeys.Select(r => base.DelayLoad<Concept>(r, null));
        }

        /// <summary>
        /// Should serialize date of birth precision
        /// </summary>
        public bool ShouldSerializeDateOfBirthPrecision()
        {
            return this.DateOfBirthPrecision.HasValue;
        }

		/// <summary>
		/// Semantic equality function
		/// </summary>
		public override bool SemanticEquals(object obj)
		{
			var other = obj as Person;
			if (other == null) return false;
			return base.SemanticEquals(obj) &&
				this.DateOfBirth == other.DateOfBirth &&
				this.DateOfBirthPrecision == other.DateOfBirthPrecision &&
				this.LanguageCommunication?.SemanticEquals(other.LanguageCommunication) == true;
		}
	}
}