/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: justi
 * Date: 2019-1-12
 */
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a simple geographic tagging attribute
    /// </summary>
    [XmlType(nameof(GeoTag), Namespace = "http://santedb.org/model"), JsonObject(nameof(GeoTag))]
    public class GeoTag : IdentifiedData
    {

        /// <summary>
        /// Gets the default modified on
        /// </summary>
        public override DateTimeOffset ModifiedOn => default(DateTimeOffset);

        /// <summary>
        /// Creates a new geo-tag
        /// </summary>
        public GeoTag()
        {

        }

        /// <summary>
        /// Creates a new geo tag
        /// </summary>
        public GeoTag(double lat, double lng, bool? precise)
        {
            this.Lat = lat;
            this.Lng = lng;
            this.Precise = precise;
        }

        /// <summary>
        /// Gets the latitude
        /// </summary>
        [XmlElement("lat"), JsonProperty("lat")]
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the object
        /// </summary>
        [XmlElement("lng"), JsonProperty("lng")]
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the accuracy of the geo-tag
        /// </summary>
        [XmlElement("precise"), JsonProperty("precise")]
        public bool? Precise { get; set; }

    }
}
