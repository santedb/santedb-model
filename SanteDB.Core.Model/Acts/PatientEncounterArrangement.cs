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
using SanteDB.Core.Model.DataTypes;
using System;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Links a <see cref="PatientEncounter"/> with one or more codes which indicate special arrangements or 
    /// considerations made for a patient stay
    /// </summary>
    [XmlType("PatientEncounterArrangement", Namespace = "http://santedb.org/model"), JsonObject("PatientEncounterArrangement")]
    public class PatientEncounterArrangement : VersionedAssociation<Act>
    {

        /// <summary>
        /// Default ctor for serialization
        /// </summary>
        public PatientEncounterArrangement()
        {

        }

        /// <summary>
        /// Creates a new special consideration entry with the specified arrangement type key
        /// </summary>
        /// <param name="arrangementTypeKey">The arrangement type that should be set</param>
        public PatientEncounterArrangement(Guid arrangementTypeKey)
        {
            this.ArrangementTypeKey = arrangementTypeKey;
        }


        /// <summary>
        /// Gets or sets the arrangement made
        /// </summary>
        [XmlElement("type"), JsonProperty("type")]
        public Guid? ArrangementTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the type of arrangement
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ArrangementTypeKey))]
        public Concept ArrangementType { get; set; }

        /// <summary>
        /// Gets or sets the time that the arrangement is in effect
        /// </summary>
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
                    {
                        this.StartTime = val;
                    }
                    else
                    {
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                    }
                }
                else
                {
                    this.StartTime = default(DateTimeOffset);
                }
            }
        }

        /// <summary>
        /// Gets or sets the time that the arrangement is no longer in effect
        /// </summary>
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
                    {
                        this.StopTime = val;
                    }
                    else
                    {
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                    }
                }
                else
                {
                    this.StopTime = default(DateTimeOffset);
                }
            }
        }
    }
}