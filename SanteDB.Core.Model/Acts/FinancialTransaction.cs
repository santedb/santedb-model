using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents an act whereby financial devices are exchanged between accounts
    /// </summary>
    [XmlType("FinancialTransaction", Namespace = "http://santedb.org/model"), JsonObject("FinancialTransaction")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "FinancialTransaction")]
    public class FinancialTransaction : Act
    {

        // Backing fields
        private Guid? m_amountCurrencyKey;
        private Concept m_amountCurrency;

        /// <summary>
        /// Creates the financial transaction
        /// </summary>
        public FinancialTransaction()
        {
            base.ClassConceptKey = ActClassKeys.FinancialTransaction;
        }

        /// <summary>
        /// Gets or sets the amount of the financial transaction
        /// </summary>
        [XmlElement("amount"), JsonProperty("amount")]
        public Decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency key
        /// </summary>
        [XmlElement("currency"), JsonProperty("currency"), EditorBrowsable(EditorBrowsableState.Never)]
        public Guid? CurrencyKey {
            get
            {
                return this.m_amountCurrencyKey;
            }
            set
            {
                this.m_amountCurrencyKey = value;
                this.m_amountCurrency = null;
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
                this.m_amountCurrency = base.DelayLoad(this.m_amountCurrencyKey, this.m_amountCurrency);
                return this.m_amountCurrency;
            }
            set
            {
                this.m_amountCurrency = value;
                this.m_amountCurrencyKey = value?.Key;
            }
        }

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
