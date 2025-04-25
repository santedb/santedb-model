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
    /// Represents built in assigning authorities
    /// </summary>
    public static class IdentityDomainKeys
    {

        /// <summary>
        /// ISO 3166 Country Code Identifier Authority
        /// </summary>
        public static readonly Guid Iso3166CountryCode = Guid.Parse("ff6e7402-8545-48e1-8a70-ebf06f3ee4b8");

        /// <summary>
        /// GS1 Global Location Number
        /// </summary>
        public static readonly Guid Gs1GlobalLocationNumber = Guid.Parse("0f4a86d6-d8e0-423f-9a53-54739f2da409");

        /// <summary>
        /// GS1 GTIN
        /// </summary>
        public static readonly Guid Gs1GlobalTradeIdentificationNumber = Guid.Parse("ce5990db-2e2a-467d-a376-2a7b53481e84");
    }
}
