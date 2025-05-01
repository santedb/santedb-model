/*
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
    /// Represents an Account
    /// </summary>
    /// <remarks>An account represents a simple structure for tracking balance and one or more invoice entries
    /// against a record target (the entity for which the account is owned).</remarks>
    [XmlType("Account", Namespace = "http://santedb.org/model"), JsonObject("Account")]
    [XmlRoot("Account", Namespace = "http://santedb.org/model")]
    [ClassConceptKey(ActClassKeyStrings.Account)]
    [ResourceSensitivity(ResourceSensitivityClassification.Administrative)]
    public class Account : Act
    {

        /// <summary>
        /// Initializes the account
        /// </summary>
        public Account()
        {
            this.m_classConceptKey = ActClassKeys.Account;
        }

        /// <summary>
        /// Gets or sets the balance of this account
        /// </summary>
        [XmlElement("balance"), JsonProperty("balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the currency concept UUID for this account
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
        /// Gets or sets the minimum balance of this account
        /// </summary>
        [XmlElement("minBalance"), JsonProperty("minBalance")]
        public Decimal? MinBalance { get; set; }

        /// <summary>
        /// Gets or sets the maximum balance of this account
        /// </summary>
        [XmlElement("maxBalance"), JsonProperty("maxBalance")]
        public Decimal MaxBalance { get; set; }
    }
}