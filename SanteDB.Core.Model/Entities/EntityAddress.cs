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
 * User: fyfej
 * Date: 2023-6-21
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// A structured address for an entity
    /// </summary>
    /// <remarks>
    /// Addresses in SanteDB are structured as a collection of components. This structure
    /// ensures that addresses a flexible when they are stored, searched and reproduced
    /// </remarks>
    [Classifier(nameof(AddressUse))]
    [XmlType("EntityAddress", Namespace = "http://santedb.org/model"), JsonObject("EntityAddress")]
    public class EntityAddress : VersionedAssociation<Entity>, IHasExternalKey
    {

        /// <summary>
        /// Create the address from components
        /// </summary>
        public EntityAddress(Guid useKey, String streetAddressLine, String city, String province, String country, String zipCode)
        {
            this.AddressUseKey = useKey;
            this.Component = new List<EntityAddressComponent>();
            if (!String.IsNullOrEmpty(streetAddressLine))
            {
                this.Component.Add(new EntityAddressComponent(AddressComponentKeys.StreetAddressLine, streetAddressLine));
            }

            if (!String.IsNullOrEmpty(city))
            {
                this.Component.Add(new EntityAddressComponent(AddressComponentKeys.City, city));
            }

            if (!String.IsNullOrEmpty(province))
            {
                this.Component.Add(new EntityAddressComponent(AddressComponentKeys.State, province));
            }

            if (!String.IsNullOrEmpty(country))
            {
                this.Component.Add(new EntityAddressComponent(AddressComponentKeys.Country, country));
            }

            if (!String.IsNullOrEmpty(zipCode))
            {
                this.Component.Add(new EntityAddressComponent(AddressComponentKeys.PostalCode, zipCode));
            }
        }

        /// <summary>
        /// Create the address from components
        /// </summary>
        public EntityAddress(Guid useKey, String streetAddressLine, String precinct, String city, String county, String province, String country, String zipCode) : this(useKey, streetAddressLine, city, province, country, zipCode)
        {
            if (!String.IsNullOrEmpty(precinct))
            {
                this.Component.Add(new EntityAddressComponent(AddressComponentKeys.Precinct, precinct));
            }

            if (!String.IsNullOrEmpty(county))
            {
                this.Component.Add(new EntityAddressComponent(AddressComponentKeys.County, county));
            }
        }

        /// <summary>
        /// Default CTOR
        /// </summary>
        public EntityAddress()
        {

        }

        /// <summary>
        /// Gets or sets the address use
        /// </summary>
        [SerializationReference(nameof(AddressUseKey))]
        [XmlIgnore, JsonIgnore]

        public Concept AddressUse { get; set; }


        /// <summary>
        /// Gets or sets the external key for the object
        /// </summary>
        /// <remarks>Sometimes, when communicating with an external communications another system needs to 
        /// refer to this by a particular key</remarks>
        [XmlElement("externId"), JsonProperty("externId")]
        public string ExternalKey { get; set; }

        /// <summary>
        /// Gets or sets the address use key
        /// </summary>
        [XmlElement("use"), JsonProperty("use")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Binding(typeof(AddressUseKeys))]
        public Guid? AddressUseKey { get; set; }

        /// <summary>
        /// Gets or sets the component types
        /// </summary>
        [XmlElement("component"), JsonProperty("component")]
        public List<EntityAddressComponent> Component { get; set; }

        /// <summary>
        /// Get components
        /// </summary>
        public string GetComponent(Guid key)
        {
            var comps = this.LoadProperty(o => o.Component);
            return comps.FirstOrDefault(o => o.ComponentTypeKey == key)?.Value;
        }


        /// <summary>
        /// True if empty
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return this.Component.IsNullOrEmpty() || this.Component?.All(c => c.IsEmpty()) == true;
        }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as EntityAddress;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.AddressUseKey == other.AddressUseKey &&
                this.Component?.SemanticEquals(other.Component) != false;
        }

        /// <summary>
        /// Never need to serialize the entity source key
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSerializeSourceEntityKey()
        {
            return false;
        }

        /// <summary>
        /// Represent as display
        /// </summary>
        public override string ToDisplay()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.StreetAddressLine)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.StreetAddressLine));
            }

            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.AddressLine)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.AddressLine));
            }

            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.Precinct)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.Precinct));
            }

            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.City)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.City));
            }

            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.County)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.County));
            }

            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.State)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.State));
            }

            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.Country)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.Country));
            }

            if (!string.IsNullOrEmpty(this.GetComponent(AddressComponentKeys.PostalCode)))
            {
                sb.AppendFormat("{0}, ", this.GetComponent(AddressComponentKeys.PostalCode));
            }

            if (sb.Length > 2)
            {
                sb.Remove(sb.Length - 2, 2);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Represent this address as a string
        /// </summary>
        public override string ToString()
        {
            return this.ToDisplay();
        }
    }
}