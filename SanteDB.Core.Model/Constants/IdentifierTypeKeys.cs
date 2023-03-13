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
 * Date: 2023-3-10
 */
using System;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Identifier type keys
    /// </summary>
    public static class IdentifierTypeKeys
    {

        /// <summary>
        /// Identifier is a drivers license
        /// </summary>
        public static readonly Guid DriversLicense = Guid.Parse("CB6E4F39-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a passwport number
        /// </summary>
        public static readonly Guid Passport = Guid.Parse("CB6E4F4A-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is an MRN
        /// </summary>
        public static readonly Guid MedicalRecordNumber = Guid.Parse("CB6E4F5B-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is an employer number
        /// </summary>
        public static readonly Guid Employer = Guid.Parse("CB6E4F4A-5A6C-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a TIN
        /// </summary>
        public static readonly Guid Tax = Guid.Parse("CB6E4F7D-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is an insurance number
        /// </summary>
        public static readonly Guid NationalInsurance = Guid.Parse("CB6E4F8E-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a primary identifier used to identify the patient
        /// </summary>
        public static readonly Guid PrimaryIdentifier = Guid.Parse("CB6E4F9F-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a medical license number issued by an authority of medical doctors, nuerses, etc.
        /// </summary>
        public static readonly Guid MedicalLicense = Guid.Parse("CB6E4FA0-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a jurisdictional health number
        /// </summary>
        public static readonly Guid JurisdictionalNumber = Guid.Parse("CB6E4FB1-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a bank identifier account number
        /// </summary>
        public static readonly Guid Bank = Guid.Parse("CB6E4FB2-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a temporary number
        /// </summary>
        public static readonly Guid Temp = Guid.Parse("CB6E4FC3-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is a person identifier
        /// </summary>
        public static readonly Guid PersonIdentifier = Guid.Parse("CB6E4FD4-5A78-408E-9B5E-A41018057FB1");

        /// <summary>
        /// Identifier is an external identifier
        /// </summary>
        public static readonly Guid ExternalIdentifier = Guid.Parse("CB6E4FE5-5A78-408E-9B5E-A41018057FB1");

    }
}
