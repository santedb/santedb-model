/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-5-19
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
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
    [ClassConceptKey(ActClassKeyStrings.FinancialContract)]
    public class FinancialContract : Act
    {

        /// <summary>
        /// Creates the financial contract
        /// </summary>
        public FinancialContract()
        {
            base.m_classConceptKey = ActClassKeys.FinancialContract;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == ActClassKeys.FinancialContract;

        /// <summary>
        /// Gets or sets the payment terms
        /// </summary>
        [JsonProperty("paymentTerms"), XmlElement("paymentTerms")]
        public Guid? PaymentTermsKey { get; set; }

        /// <summary>
        /// Gets or sets the payment terms
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(PaymentTermsKey))]
        public Concept PaymentTerms { get; set; }

    }
}
