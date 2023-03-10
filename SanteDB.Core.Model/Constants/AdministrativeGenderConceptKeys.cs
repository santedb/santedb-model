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
    /// Administrative gender concept reference keys
    /// </summary>
    public static class AdministrativeGenderConceptKeys
    {
        /// <summary>
        /// Gender for administrative reference is Male
        /// </summary>
        public static readonly Guid Male = Guid.Parse("f4e3a6bb-612e-46b2-9f77-ff844d971198");

        /// <summary>
        /// Gender for administrative reference is Female
        /// </summary>
        public static readonly Guid Female = Guid.Parse("094941e9-a3db-48b5-862c-bc289bd7f86c");

        /// <summary>
        /// Gender for administrative purposes is not differentiated
        /// </summary>
        public static readonly Guid Undifferentiated = Guid.Parse("ae94a782-1485-4241-9bca-5b09db2156bf");


    }
}
