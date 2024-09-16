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
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// An entity which is a place where healthcare services are delivered
    /// </summary>

    [XmlType("Place", Namespace = "http://santedb.org/model"), JsonObject("Place")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Place")]
    [ClassConceptKey(EntityClassKeyStrings.Place)]
    [ClassConceptKey(EntityClassKeyStrings.ServiceDeliveryLocation)]
    [ClassConceptKey(EntityClassKeyStrings.PrecinctOrBorough)]
    [ClassConceptKey(EntityClassKeyStrings.CityOrTown)]
    [ClassConceptKey(EntityClassKeyStrings.Country)]
    [ClassConceptKey(EntityClassKeyStrings.CountyOrParish)]
    [ClassConceptKey(EntityClassKeyStrings.ZoneOrTerritory)]
    [ClassConceptKey(EntityClassKeyStrings.StateOrProvince)]
    [ResourceSensitivity(ResourceSensitivityClassification.Metadata)]
    public class Place : Entity, IGeoTagged
    {
        /// <summary>
        /// Place ctor
        /// </summary>
        public Place()
        {
            base.m_classConceptKey = EntityClassKeys.Place;
            base.DeterminerConceptKey = DeterminerKeys.Specific;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.Place ||
                    classKey == EntityClassKeys.ServiceDeliveryLocation ||
                    classKey == EntityClassKeys.StateOrProvince ||
                    classKey == EntityClassKeys.CityOrTown ||
                    classKey == EntityClassKeys.PrecinctOrBorough ||
                    classKey == EntityClassKeys.Country ||
                    classKey == EntityClassKeys.CountyOrParish ||
                    classKey == EntityClassKeys.ZoneOrTerritory;

        /// <summary>
        /// True if location is mobile
        /// </summary>
        [XmlElement("isMobile"), JsonProperty("isMobile")]
        public Boolean IsMobile { get; set; }

        /// <summary>
        /// Should serialize mobile
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeIsMobile() => this.IsMobile;

        /// <summary>
        /// Should serialize latitude
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLat() => this.GeoTag?.Lat != 0;

        /// <summary>
        /// Should serialize longitude
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLng() => this.GeoTag?.Lng != 0;

        /// <summary>
        /// Gets the services
        /// </summary>
        [XmlElement("service"), JsonProperty("service")]
        public List<PlaceService> Services { get; set; }

        /// <summary>
        /// Determine semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Place;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.IsMobile == other.IsMobile &&
                this.GeoTag?.Lat == other.GeoTag?.Lat &&
                this.GeoTag?.Lng == other.GeoTag?.Lng &&
                this.Services?.SemanticEquals(other.Services) != false;
        }
    }
}