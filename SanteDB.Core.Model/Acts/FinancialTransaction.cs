﻿/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents an act whereby financial devices are exchanged between accounts
    /// </summary>
    [XmlType("FinancialTransaction", Namespace = "http://santedb.org/model"), JsonObject("FinancialTransaction")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "FinancialTransaction")]
    [ClassConceptKey(ActClassKeyStrings.FinancialTransaction)]
    [ResourceSensitivity(ResourceSensitivityClassification.Administrative)]
    public class FinancialTransaction : Act
    {

        /// <summary>
        /// Creates the financial transaction
        /// </summary>
        public FinancialTransaction()
        {
            base.m_classConceptKey = ActClassKeys.FinancialTransaction;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == ActClassKeys.FinancialTransaction;

        /// <summary>
        /// Gets or sets the amount of the financial transaction
        /// </summary>
        [XmlElement("amount"), JsonProperty("amount")]
        public Decimal? Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency key
        /// </summary>
        [XmlElement("currency"), JsonProperty("currency"), EditorBrowsable(EditorBrowsableState.Never)]
        [Binding(typeof(CurrencyKeys))]
        public Guid? CurrencyKey { get; set; }

        /// <summary>
        /// Gets or sets the currency
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CurrencyKey))]
        public Concept Currency { get; set; }

        /// <summary>
        /// Gets or sets the crediting exchange rate
        /// </summary>
        [XmlElement("creditExchange"), JsonProperty("creditExchange")]
        public float CreditExchangeRate { get; set; }

        /// <summary>
        /// Gets or sets the debit exchange rate
        /// </summary>
        [XmlElement("debitExchange"), JsonProperty("debitExchange")]
        public float DebitExchangeRate { get; set; }
    }
}