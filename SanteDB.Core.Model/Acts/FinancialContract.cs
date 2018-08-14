using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents a financial contract which is executed between two parties (examples: insurance)
    /// </summary>
    /// <remarks>A financial contract represents a contract between two parties whereby there is a financial 
    /// motive involved. This can be a contract between an employer and an employee, two or more clinics,
    /// or even an insurance policy</remarks>
    [XmlType("FinancialContract", Namespace = "http://santedb.org/model"), JsonObject("FinancialContract")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "FinancialContract")]
    public class FinancialContract : Act
    {

        // Payment terms key
        private Guid? m_paymentTermsKey;
        private Concept m_paymentTerms;
        
        /// <summary>
        /// Creates the financial contract
        /// </summary>
        public FinancialContract()
        {
            base.ClassConceptKey = ActClassKeys.FinancialContract;
        }

        /// <summary>
        /// Gets or sets the payment terms
        /// </summary>
        [JsonProperty("paymentTerms"), XmlElement("paymentTerms")]
        public Guid? PaymentTermsKey
        {
            get
            {
                return this.m_paymentTermsKey;
            }
            set
            {
                this.m_paymentTermsKey = value;
                this.m_paymentTerms = null;
            }
        }

        /// <summary>
        /// Gets or sets the payment terms
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(PaymentTermsKey))]
        public Concept PaymentTerms
        {
            get
            {
                this.m_paymentTerms = base.DelayLoad(this.m_paymentTermsKey, this.m_paymentTerms);
                return this.m_paymentTerms;
            }
            set
            {
                this.m_paymentTerms = value;
                this.m_paymentTermsKey = value?.Key;
            }
        }
    }
}
