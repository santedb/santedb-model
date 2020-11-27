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
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a material which is manufactured by a particular organization or company.
    /// </summary>
    /// <remarks>
    /// A manufactured material represents an instance of a material which was or can be manufactured by 
    /// an organization and tracked as stock.
    /// </remarks>

    [XmlType("ManufacturedMaterial", Namespace = "http://santedb.org/model"), JsonObject("ManufacturedMaterial")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "ManufacturedMaterial")]
    [ClassConceptKey(EntityClassKeyStrings.ManufacturedMaterial)]
    public class ManufacturedMaterial : Material
    {
        /// <summary>
        /// Creates a new manufactured material
        /// </summary>
        public ManufacturedMaterial()
        {
            base.DeterminerConceptKey = DeterminerKeys.Specific;
            base.ClassConceptKey = EntityClassKeys.ManufacturedMaterial;
        }

        /// <summary>
        /// Gets or sets the lot number of the manufactured material
        /// </summary>
        [XmlElement("lotNumber"), JsonProperty("lotNumber")]
        public String LotNumber { get; set; }

        /// <summary>
        /// Semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as ManufacturedMaterial;
            if (other == null) return false;
            return base.SemanticEquals(obj) && this.LotNumber == other.LotNumber;
        }
    }
}