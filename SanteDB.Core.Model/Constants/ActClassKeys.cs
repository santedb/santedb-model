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
using System;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Strings for ActClassKeys
    /// </summary>
    public static class ActClassKeyStrings
    {
        /// <summary>
		/// The act represents generic account management such as adjudications, financial adjustments, stock counting, etc.
		/// </summary>
		public const String AccountManagement = "ca44a469-81d7-4484-9189-ca1d55afecbc";

        /// <summary>
        /// The act represents a generic act which has no special classification
        /// </summary>
        public const String Act = "d874424e-c692-4fd8-b94e-642e1cbf83e9";

        /// <summary>
        /// The act represents a simple battery of procedures/administrations/tests/etc.
        /// </summary>
        public const String Battery = "676de278-64aa-44f2-9b69-60d61fc1f5f5";

        /// <summary>
        /// The act represents some provision of care such as the seeking out services.
        /// </summary>
        public const String CareProvision = "1071d24e-6fe9-480f-8a20-b1825ae4d707";

        /// <summary>
        /// The act represents a problem or condition which the patient is suffering from.
        /// </summary>
        public const String Condition = "1987c53c-7ab8-4461-9ebc-0d428744a8c0";

        /// <summary>
        /// The control act event key is used to describe an infrastructural act which has no clinical meaning but can be used to wrap technical details.
        /// </summary>
        public const String ControlAct = "b35488ce-b7cd-4dd4-b4de-5f83dc55af9f";

        /// <summary>
        /// The act represents an encounter such as the patient presenting for care and receiving services during a visit.
        /// </summary>
        public const String Encounter = "54b52119-1709-4098-8911-5df6d6c84140";

        /// <summary>
        /// The act represents an attempt to provide additional clinical information.
        /// </summary>
        public const String Inform = "192f1768-d39e-409d-87be-5afd0ee0d1fe";

        /// <summary>
        /// The act represents an observation that is made about a patient such as a vital sign, an allergy, cause of death, etc..
        /// </summary>
        public const String Observation = "28d022c6-8a8b-47c4-9e6a-2bc67308739e";

        /// <summary>
        /// The act represents a procedure (something done to a patient).
        /// </summary>
        public const String Procedure = "8cc5ef0d-3911-4d99-937f-6cfdc2a27d55";
        /// <summary>
        /// The act represents a registration event such as the registration of a patient.
        /// </summary>
        public const String Registration = "6be8d358-f591-4a3a-9a57-1889b0147c7e";

        /// <summary>
        /// The act represents that a substance (medication, or otherwise) was, should, or will be administered to the patient.
        /// </summary>
        public const String SubstanceAdministration = "932a3c7e-ad77-450a-8a1f-030fc2855450";

        /// <summary>
        /// The act represents a supply of some material or financial instrument between entities.
        /// </summary>
        public const String Supply = "a064984f-9847-4480-8bea-dddf64b3c77c";

        /// <summary>
        /// The physical transporting of materials or people from one place to another.
        /// </summary>
        public const String Transport = "61677f76-dc05-466d-91de-47efc8e7a3e6";

        /// <summary>
        /// Represents a contract
        /// </summary>
        public const String Contract = "5b947fcb-055a-4e7b-9571-3c2502d72ba6";

        /// <summary>
        /// Represents a financial contract
        /// </summary>
        public const String FinancialContract = "ee5fabb7-f97c-417c-ad81-ac9d05b81c50";

        /// <summary>
        /// Represents an account for tracking financial obligations
        /// </summary>
        public const String Account = "efd2d8dd-45d4-4746-bf1a-38e95a358c05";

        /// <summary>
        /// Represents the financial transaction
        /// </summary>
        public const string FinancialTransaction = "6a37a91e-cc27-4e64-9b49-618a1f255904";

        /// <summary>
        /// Represents an individual invoice elemnt
        /// </summary>
        public const string InvoiceElement = "6755ee8b-48e6-4fd8-802f-59b89efa8966";

        /// <summary>
        /// List
        /// </summary>
        public const string List = "b0323489-9a09-411a-bb55-ff283830ea1a";

        /// <summary>
        /// Represents a document 
        /// </summary>
        public const string Document = "7a6944f7-8937-4e98-ae0d-452b1d8124f4";

        /// <summary>
        /// Represents a section within a document
        /// </summary>
        public const string DocumentSection = "e86ea735-f243-41f4-bf33-aa78eb2e8466";

        /// <summary>
        /// Represents a plan of care
        /// </summary>
        public const string CarePlan = "042232b9-a694-42e0-9708-f387393a6c80";

    }

    /// <summary>
    /// Represents a series of class keys for use on acts.
    /// </summary>
    public static class ActClassKeys
    {
        /// <summary>
        /// Document class key
        /// </summary>
        public static readonly Guid Document = Guid.Parse(ActClassKeyStrings.Document);

        /// <summary>
        /// Document section class key
        /// </summary>
        public static readonly Guid DocumentSection = Guid.Parse(ActClassKeyStrings.DocumentSection);

        /// <summary>
        /// Care plan class key
        /// </summary>
        public static readonly Guid CarePlan = Guid.Parse(ActClassKeyStrings.CarePlan);

        /// <summary>
        /// The act represents generic account management such as adjudications, financial adjustments, stock counting, etc.
        /// </summary>
        public static readonly Guid AccountManagement = Guid.Parse(ActClassKeyStrings.AccountManagement);

        /// <summary>
        /// The act represents a generic act which has no special classification
        /// </summary>
        public static readonly Guid Act = Guid.Parse(ActClassKeyStrings.Act);

        /// <summary>
        /// The act represents a simple battery of procedures/administrations/tests/etc.
        /// </summary>
        public static readonly Guid Battery = Guid.Parse(ActClassKeyStrings.Battery);

        /// <summary>
        /// The act represents some provision of care such as the seeking out services.
        /// </summary>
        [Obsolete("Use of this class code represents a potential privacy risk")]
        public static readonly Guid CareProvision = Guid.Parse(ActClassKeyStrings.CareProvision);

        /// <summary>
        /// The act represents a problem or condition which the patient is suffering from.
        /// </summary>
        [Obsolete("Use of this class code represents a potential privacy risk - please use Observation with Type Concept ObservationTypeKeys.Condition")]
        public static readonly Guid Condition = Guid.Parse(ActClassKeyStrings.Condition);

        /// <summary>
        /// The control act event key is used to describe an infrastructural act which has no clinical meaning but can be used to wrap technical details.
        /// </summary>
        public static readonly Guid ControlAct = Guid.Parse(ActClassKeyStrings.ControlAct);

        /// <summary>
        /// The act represents an encounter such as the patient presenting for care and receiving services during a visit.
        /// </summary>
        public static readonly Guid Encounter = Guid.Parse(ActClassKeyStrings.Encounter);

        /// <summary>
        /// The act represents an attempt to provide additional clinical information.
        /// </summary>
        public static readonly Guid Inform = Guid.Parse(ActClassKeyStrings.Inform);

        /// <summary>
        /// The act represents an observation that is made about a patient such as a vital sign, an allergy, cause of death, etc..
        /// </summary>
        public static readonly Guid Observation = Guid.Parse(ActClassKeyStrings.Observation);

        /// <summary>
        /// The act represents a procedure (something done to a patient).
        /// </summary>
        public static readonly Guid Procedure = Guid.Parse(ActClassKeyStrings.Procedure);

        /// <summary>
        /// The act represents a registration event such as the registration of a patient.
        /// </summary>
        public static readonly Guid Registration = Guid.Parse(ActClassKeyStrings.Registration);

        /// <summary>
        /// The act represents that a substance (medication, or otherwise) was, should, or will be administered to the patient.
        /// </summary>
        public static readonly Guid SubstanceAdministration = Guid.Parse(ActClassKeyStrings.SubstanceAdministration);

        /// <summary>
        /// The act represents a supply of some material or financial instrument between entities.
        /// </summary>
        public static readonly Guid Supply = Guid.Parse(ActClassKeyStrings.Supply);

        /// <summary>
        /// The physical transporting of materials or people from one place to another.
        /// </summary>
        public static readonly Guid Transport = Guid.Parse(ActClassKeyStrings.Transport);

        /// <summary>
        /// Represents a contract
        /// </summary>
        public static readonly Guid Contract = Guid.Parse(ActClassKeyStrings.Contract);

        /// <summary>
        /// Represents a financial contract
        /// </summary>
        public static readonly Guid FinancialContract = Guid.Parse(ActClassKeyStrings.FinancialContract);

        /// <summary>
        /// Represents an account for tracking financial obligations
        /// </summary>
        public static readonly Guid Account = Guid.Parse(ActClassKeyStrings.Account);

        /// <summary>
        /// Represents a single financial transaction
        /// </summary>
        public static readonly Guid FinancialTransaction = Guid.Parse(ActClassKeyStrings.FinancialTransaction);

        /// <summary>
        /// Represents a single invoice element
        /// </summary>
        public static readonly Guid InvoiceElement = Guid.Parse(ActClassKeyStrings.InvoiceElement);

        /// <summary>
        /// List
        /// </summary>
        public static readonly Guid List = Guid.Parse(ActClassKeyStrings.List);

    }
}