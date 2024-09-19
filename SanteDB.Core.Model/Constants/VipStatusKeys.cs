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
    /// Status concept keys for very imporatnt person indicators
    /// </summary>
    public static class VipStatusKeys
    {
        /// <summary>
        /// Person is a board member
        /// </summary>
        public static readonly Guid BoardMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130002");
        /// <summary>
        /// Person is a family member of a physician
        /// </summary>
        public static readonly Guid PhysicianFamilyMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130003");
        /// <summary>
        /// Person is a staff physician
        /// </summary>
        public static readonly Guid StaffPhysician = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130004");
        /// <summary>
        /// Person is a financial donor
        /// </summary>
        public static readonly Guid FinancialDonor = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130005");
        /// <summary>
        /// Person is a foreign dignitary
        /// </summary>
        public static readonly Guid ForeignDignitary = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130006");
        /// <summary>
        /// Person is a government dignitary
        /// </summary>
        public static readonly Guid GovernmentDignitary = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130007");
        /// <summary>
        /// Person is a family member of a staff person
        /// </summary>
        public static readonly Guid StaffFamilyMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130008");
        /// <summary>
        /// Person is a staff member
        /// </summary>
        public static readonly Guid StaffMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130009");
        /// <summary>
        /// Person is a very important person (celebrity, public figure, etc.) of an uclassified nature.
        /// </summary>
        public static readonly Guid VeryImportantPerson = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC13000A");
    }
}
