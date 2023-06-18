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
using System;

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
