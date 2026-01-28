/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
    /// Telecommunications address use keys
    /// </summary>
    public static class TelecomAddressTypeKeys
    {

        /// <summary>
        /// pager
        /// </summary>
        public static readonly Guid Pager = Guid.Parse("788000B4-E37A-4055-A2AA-C650089CE3B1");

        /// <summary>
        /// Telephone (can receive voice calls)
        /// </summary>
        public static readonly Guid Telephone = Guid.Parse("c1c0a4e9-4238-4044-b89b-9c9798995b99");

        /// <summary>
        /// Cellular phone (can receive MMS and SMS)
        /// </summary>
        public static readonly Guid CellularPhone = Guid.Parse("c1c0a4e9-4238-4044-b89b-9c9798995b90");

        /// <summary>
        /// Modem (can be dialed into)
        /// </summary>
        public static readonly Guid Modem = Guid.Parse("c1c0a4e9-4238-4044-b89b-9c9798995b91");

        /// <summary>
        /// Fax machine (can receive fax data)
        /// </summary>
        public static readonly Guid FaxMachine = Guid.Parse("c1c0a4e9-4238-4044-b89b-9c9798995b92");

        /// <summary>
        /// Internet address (can receive SMTP e-mail)
        /// </summary>
        public static readonly Guid Internet = Guid.Parse("c1c0a4e9-4238-4044-b89b-9c9798995b93");


    }
}