/*
 * Copyright (C) 2021 - 2021, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-5
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a material which describes a type of material (i.e. scapel, antigen, etc.) and serves as a base class for manufactured materials
    /// </summary>
    /// <remarks>In SanteDB, a Material represents the base class for kinds and instances of materials which may or may not be manufactured by a manufacturer</remarks>
    [XmlType("Material", Namespace = "http://santedb.org/model"), JsonObject("Material")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Material")]
    [ClassConceptKey(EntityClassKeyStrings.Material)]
    public class Material : Entity
    {
        // Form concept
        private Concept m_formConcept;

        // Form concept key
        private Guid? m_formConceptKey;

        // Quantity concept
        private Concept m_quantityConcept;

        // Quantity concept key
        private Guid? m_quantityConceptKey;

        /// <summary>
        /// Material ctor
        /// </summary>
        public Material()
        {
            this.ClassConceptKey = EntityClassKeys.Material;
            this.DeterminerConceptKey = DeterminerKeys.Described;
        }

        /// <summary>
        /// Gets or sets the expiry date of the material
        /// </summary>
        [XmlElement("expiryDate"), JsonProperty("expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the concept which dictates the form of the material (solid, liquid, capsule, injection, etc.)
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(FormConceptKey))]
        public Concept FormConcept
        {
            get
            {
                this.m_formConcept = base.DelayLoad(this.m_formConceptKey, this.m_formConcept);
                return this.m_formConcept;
            }
            set
            {
                this.m_formConcept = value;
                this.m_formConceptKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the form concept's key
        /// </summary>
        [XmlElement("formConcept"), JsonProperty("formConcept")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? FormConceptKey
        {
            get { return this.m_formConceptKey; }
            set
            {
                if (this.m_formConceptKey != value)
                {
                    this.m_formConceptKey = value;
                    this.m_formConcept = null;
                }
            }
        }

        /// <summary>
        /// True if the material is simply administrative
        /// </summary>
        [XmlElement("isAdministrative"), JsonProperty("isAdministrative")]
        public Boolean IsAdministrative { get; set; }

        /// <summary>
        /// The base quantity of the object in the units. This differs from quantity on the relationship
        /// which is a /per ...
        /// </summary>
        [XmlElement("quantity"), JsonProperty("quantity")]
        public Decimal? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the concept which dictates the unit of measure for a single instance of this entity
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(QuantityConceptKey))]
        public Concept QuantityConcept
        {
            get
            {
                this.m_quantityConcept = base.DelayLoad(this.m_quantityConceptKey, this.m_quantityConcept);
                return this.m_quantityConcept;
            }
            set
            {
                this.m_quantityConcept = value;
                this.m_quantityConceptKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the quantity concept ref
        /// </summary>
        [XmlElement("quantityConcept"), JsonProperty("quantityConcept")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? QuantityConceptKey
        {
            get { return this.m_quantityConceptKey; }
            set
            {
                if (this.m_quantityConceptKey != value)
                {
                    this.m_quantityConceptKey = value;
                    this.m_quantityConcept = null;
                }
            }
        }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Material;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.Quantity == other.Quantity &&
                this.QuantityConceptKey == other.QuantityConceptKey &&
                this.FormConceptKey == other.FormConceptKey &&
                this.ExpiryDate == other.ExpiryDate &&
                this.IsAdministrative == other.IsAdministrative;
        }
    }
}