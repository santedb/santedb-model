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

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents an invoice element
    /// </summary>
    /// <remarks>An invoice element represents an act on a single financial transaction which records
    /// the financial impact of a particular service.</remarks>
    [XmlType("InvoiceElement", Namespace = "http://santedb.org/model"), JsonObject("InvoiceElement")]
    [XmlRoot("InvoiceElement", Namespace = "http://santedb.org/model")]
    [ClassConceptKey(ActClassKeyStrings.InvoiceElement)]
    public class InvoiceElement : Act
    {
        /// <summary>
        /// Creates the invoice element
        /// </summary>
        public InvoiceElement()
        {
            this.ClassConceptKey = ActClassKeys.InvoiceElement;
        }

        /// <summary>
        /// Gets or sets the class concept
        /// </summary>
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        public override Guid? ClassConceptKey { get => ActClassKeys.InvoiceElement; set => base.ClassConceptKey = ActClassKeys.InvoiceElement; }

        /// <summary>
        /// Gets or sets the modifier
        /// </summary>
        [XmlElement("modifier"), JsonProperty("modifier"), EditorBrowsable(EditorBrowsableState.Advanced)]
        public Guid? ModifierKey { get; set; }

        /// <summary>
        /// Gets or sets the modifier
        /// </summary>
        /// <remarks>The modifier allows further description of how/why the price of this invoice element was changed</remarks>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(ModifierKey))]
        public Concept Modifier { get; set; }

        /// <summary>
        /// Gets or sets the number of units included in the price
        /// </summary>
        [XmlElement("unitQty"), JsonProperty("unitQty")]
        public Decimal? UnitQuantity { get; set; }

        /// <summary>
        /// Gets or sets the price of each unit
        /// </summary>
        [XmlElement("unitPrice"), JsonProperty("unitPrice")]
        public Decimal? UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the net amount
        /// </summary>
        [XmlElement("netPrice"), JsonProperty("netPrice")]
        public Decimal? NetPrice { get; set; }

        /// <summary>
        /// Gets or sets the currency of the invoice line item
        /// </summary>
        [XmlElement("currency"), JsonProperty("currency"), EditorBrowsable(EditorBrowsableState.Advanced)]
        public Guid? CurrencyKey { get; set; }

        /// <summary>
        /// Gets or sets the currency of this transaction
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CurrencyKey))]
        public Concept Currency { get; set; }

        /// <summary>
        /// When provided, can specify the factor to allow for different amounts to be charged based on insurance provider negotiations
        /// </summary>
        [XmlElement("factor"), JsonProperty("factor")]
        public float? Factor { get; set; }
    }
}