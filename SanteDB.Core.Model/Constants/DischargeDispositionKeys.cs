/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2022-5-30
 */
using System;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Dispositions where a patient is discharged from an encounter/visit
    /// </summary>
    public static class DischargeDispositionKeys
    {

        /// <summary>
        /// The patient has been admitted as an inpatient 
        /// </summary>
        public static Guid AdmitToHospital = Guid.Parse("5A6D3DB4-CB61-4608-A061-89592F539276");
        /// <summary>
        /// The patient has been discharged home
        /// </summary>
        public static Guid Home = Guid.Parse("499F18C5-56C4-496E-9E23-E5CF862CFAB8");
        /// <summary>
        /// The patient has been transfered to a hospital
        /// </summary>
        public static Guid TransferToHospital = Guid.Parse("01F28777-FC6C-40BD-A50B-E0C98D15892D");
        /// <summary>
        /// The patient has been transfered to an intermediate care facility
        /// </summary>
        public static Guid TransferToICF = Guid.Parse("7347937E-8D0E-43D7-92C0-B0A410173079");
        /// <summary>
        /// The patient has been transfered to another type of service
        /// </summary>
        public static Guid TransferToOther = Guid.Parse("1A117EAD-3882-4692-9ECC-E660ADBC6D8C");
        /// <summary>
        /// The patient died
        /// </summary>
        public static Guid Died = Guid.Parse("6DF3720B-857F-4BA2-826F-B7F1D3C3ADBB");
        /// <summary>
        /// The patient opted out of care (left against medical advice)
        /// </summary>
        public static Guid OptedOutOfCare = Guid.Parse("EE99C69D-BFC4-4363-ABEC-28D8CBFE9976");
        /// <summary>
        /// The patient was transfered to rehabilitation services
        /// </summary>
        public static Guid TransferRehab = Guid.Parse("fba28777-fc6c-40bd-a50b-e0c98d15892d");
        /// <summary>
        /// The patient was transfered to long-term-care facility
        /// </summary>
        public static Guid TransferLtc = Guid.Parse("fef28777-fc6c-40bd-a50b-e0c98d15892d");
    }
}
