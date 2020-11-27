/*
 * Copyright (C) 2019 - 2020, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2019-11-27
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Security;
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
    public class Person : Entity
    {

        /// <summary>
        /// Person constructor
        /// </summary>
        public Person()
        {
            base.DeterminerConceptKey = DeterminerKeys.Specific;
            base.ClassConceptKey = EntityClassKeys.Person;
            this.LanguageCommunication = new List<PersonLanguageCommunication>();

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
                    // Try to parse ISO date
                    if (DateTime.TryParseExact(value, new String[] { "o", "yyyy-MM-dd", "yyyy-MM", "yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                        this.DateOfBirth = dt;
                    else
                        throw new FormatException($"Cannot parse {value} as a date");
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