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
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.DataTypes;
using System;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Describes a class that is an external identifier
    /// </summary>
    public interface IExternalIdentifier
    {
        /// <summary>
        /// Gets the assigning authority
        /// </summary>
        [QueryParameter("domain")]
        IdentityDomain IdentityDomain { get; }

        /// <summary>
        /// Get the authority key
        /// </summary>
        Guid? IdentityDomainKey { get; }

        /// <summary>
        /// Gets the value of the identity
        /// </summary>
        [QueryParameter("value")]
        String Value { get; }

        /// <summary>
        /// Check digit
        /// </summary>
        String CheckDigit { get; }

        /// <summary>
        /// Gets or sets the reliability
        /// </summary>
        [QueryParameter("reliability")]
        IdentifierReliability Reliability { get; set; }
    }
}