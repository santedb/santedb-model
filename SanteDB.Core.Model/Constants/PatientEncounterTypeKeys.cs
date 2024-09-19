/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 */
using System;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Types of patient encounters
    /// </summary>
    public static class PatientEncounterTypeKeys
    {
        /// <summary>
        /// Ambulatory care setting
        /// </summary>
        public static Guid Ambulatory = Guid.Parse("42765002-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Emergency encounter (in an ER)
        /// </summary>
        public static Guid Emergency = Guid.Parse("427650FC-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Field hospital encounter
        /// </summary>
        public static Guid Field = Guid.Parse("427652C8-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Home care encounter
        /// </summary>
        public static Guid Home = Guid.Parse("427653EA-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Inpatient encounter
        /// </summary>
        public static Guid Inpatient = Guid.Parse("427654B2-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Pre-admission encounter
        /// </summary>
        public static Guid PreAdmission = Guid.Parse("4276573C-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Virtual care encounter
        /// </summary>
        public static Guid Virtual = Guid.Parse("42765804-17BE-11EB-ADC1-0242AC120002");
    }
}
