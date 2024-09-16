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
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a container.
    /// </summary>
    /// <seealso cref="SanteDB.Core.Model.Entities.ManufacturedMaterial" />
    [XmlType(nameof(Container), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(Container), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(Container))]
    [ClassConceptKey(EntityClassKeyStrings.Container)]
    [ResourceSensitivity(ResourceSensitivityClassification.Metadata)]
    public class Container : Material
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        public Container()
        {
            this.m_classConceptKey = EntityClassKeys.Container;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.Container;


        /// <summary>
        /// Gets or sets the lot number of the manufactured material
        /// </summary>
        [XmlElement("lotNumber"), JsonProperty("lotNumber")]
        public String LotNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public Container(Guid key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the capacity quantity. The functional capacity of the container.
        /// </summary>
        /// <value>The capacity quantity.</value>
        [XmlElement("capacityQuantity"), JsonProperty("capacityQuantity")]
        public decimal? CapacityQuantity { get; set; }

        /// <summary>
        /// Gets or sets the diameter quantity. The outside diameter of the container.
        /// </summary>
        /// <value>The diameter quantity.</value>
        [XmlElement("diameterQuantity"), JsonProperty("diameterQuantity")]
        public decimal? DiameterQuantity { get; set; }

        /// <summary>
        /// Gets or sets the height quantity. The height of the container.
        /// </summary>
        /// <value>The height quantity.</value>
        [XmlElement("heightQuantity"), JsonProperty("heightQuantity")]
        public decimal? HeightQuantity { get; set; }

        /// <summary>
        /// Determines if two containers are semantically equal.
        /// </summary>
        /// <param name="obj">The container to compare against.</param>
        /// <returns>Returns true if the two containers are equal, otherwise false.</returns>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Container;

            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(other) &&
                    this.CapacityQuantity == other.CapacityQuantity &&
                    this.DiameterQuantity == other.DiameterQuantity &&
                    this.HeightQuantity == other.HeightQuantity;
        }
    }
}