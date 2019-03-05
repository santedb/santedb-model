/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 *
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
 * User: JustinFyfe
 * Date: 2019-1-22
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
    /// <remarks>An account represents a simple structure for tracking balance and one or more invoice entries</remarks>
    [XmlType("Account", Namespace = "http://santedb.org/model"), JsonObject("Account")]
    [XmlRoot("Account", Namespace = "http://santedb.org/model")]
    public class Account : Act
    {

        // Backing fields
        private Guid? m_currencyKey;
        private Concept m_currency;

        /// <summary>
        /// Initializes the account
        /// </summary>
        public Account()
        {
            this.ClassConceptKey = ActClassKeys.Account;
        }

        /// <summary>
        /// Gets or sets the balance
        /// </summary>
        [XmlElement("balance"), JsonProperty("balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the currency
        /// </summary>
        [XmlElement("currency"), JsonProperty("currency"), EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? CurrencyKey
        {
            get => this.m_currencyKey;
            set
            {
                this.m_currencyKey = value;
                this.m_currency = null;
            }
        }

        /// <summary>
        /// Gets or sets the currency
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CurrencyKey))]
        public Concept Currency
        {
            get
            {
                this.m_currency = base.DelayLoad(this.m_currencyKey, this.m_currency);
                return this.m_currency;
            }
            set
            {
                this.m_currency = value;
                this.m_currencyKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the minimum balance
        /// </summary>
        [XmlElement("minBalance"), JsonProperty("minBalance")]
        public Decimal? MinBalance { get; set; }

        /// <summary>
        /// Gets or sets the maximum balance
        /// </summary>
        [XmlElement("maxBalance"), JsonProperty("maxBalance")]
        public Decimal MaxBalance { get; set; }
    }
}
