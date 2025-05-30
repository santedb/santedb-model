﻿/*
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
 * Date: 2024-6-21
 */
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Attributes
{

    /// <summary>
    /// Classifications of resources
    /// </summary>
    [Flags]
    public enum ResourceSensitivityClassification
    {
        /// <summary>
        /// Resource is PHI
        /// </summary>
        [XmlEnum("phi")]
        PersonalHealthInformation = 0x1,
        /// <summary>
        /// Resource contains adminstrative data
        /// </summary>
        [XmlEnum("admin")]
        Administrative =0x2,
        /// <summary>
        /// Resource is metadata
        /// </summary>
        [XmlEnum("meta")]
        Metadata = 0x4
    }

    /// <summary>
    /// Tags a resource's class as either:
    ///     * Clinical
    ///     * Administrative 
    ///     * Metadata
    ///     
    /// This allows other generic processes to understand the resource's sensitivity and to take appropriate 
    /// auditing and/or privacy decisions (such as export, etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class ResourceSensitivityAttribute : Attribute
    {

        /// <summary>
        /// Create a new sensitivity attribute
        /// </summary>
        public ResourceSensitivityAttribute(ResourceSensitivityClassification classificationType)
        {
            this.Classification = classificationType;
        }

        /// <summary>
        /// Gets the classification of this reosurce
        /// </summary>
        public ResourceSensitivityClassification Classification { get; }
    }
}
