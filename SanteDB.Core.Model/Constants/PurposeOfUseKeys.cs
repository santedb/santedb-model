﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Purpose of use keys
    /// </summary>
    public class PurposeOfUseKeys
    {
        /// <summary>
        /// Use is for coverage eligibility query or insurance related uses
        /// </summary>
        public static readonly Guid Coverage = Guid.Parse("6af98142-9169-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use is for emergency treatment
        /// </summary>
        public static readonly Guid EmergencyTreatment = Guid.Parse("6af982f0-9169-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use intended for marketing to subject
        /// </summary>
        public static readonly Guid Marketing = Guid.Parse("6af98494-9169-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use is for general operations (import, export, etc.)
        /// </summary>
        public static readonly Guid Operations = Guid.Parse("6af988fe-9169-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use is for payment processing
        /// </summary>
        public static readonly Guid Payment = Guid.Parse("8b18bd66-916a-11ea-bb37-0242ac130002");
        /// <summary>
        /// Research related uses
        /// </summary>
        public static readonly Guid Research = Guid.Parse("8b18c126-916a-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use is at the request of the patient
        /// </summary>
        public static readonly Guid PatientRequest = Guid.Parse("8b18c2b6-916a-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use is for public health
        /// </summary>
        public static readonly Guid PublicHealth = Guid.Parse("8b18c694-916a-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use is for routine treatment of a patient's condition
        /// </summary>
        public static readonly Guid Treatment = Guid.Parse("8b18c81a-916a-11ea-bb37-0242ac130002");
        /// <summary>
        /// Use is for security purposes (changing password, setting attributes, etc.)
        /// </summary>
        public static readonly Guid SecurityAdmin = Guid.Parse("8b18c8ce-916a-11ea-bb37-0242ac130002");

    }
}
