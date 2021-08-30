﻿using System;
using System.Collections.Generic;
using System.Text;

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
