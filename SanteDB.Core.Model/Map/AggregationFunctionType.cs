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
 * Date: 2023-6-21
 */
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Map
{
    /// <summary>
    /// Represents the aggregate function to use when mapping
    /// </summary>
    [XmlType(nameof(AggregationFunctionType), Namespace = "http://santedb.org/model/map")]
    public enum AggregationFunctionType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Use LastOrDefault function
        /// </summary>
        [XmlEnum("last")]
        LastOrDefault,
        /// <summary>
        /// Use FirstOrDefault function
        /// </summary>
        [XmlEnum("first")]
        FirstOrDefault,
        /// <summary>
        /// Use Single() function (throwing an error if more than on present)
        /// </summary>
        [XmlEnum("single")]
        SingleOrDefault,
        /// <summary>
        /// Use the count function
        /// </summary>
        [XmlEnum("count")]
        Count,
        /// <summary>
        /// Use the sum function
        /// </summary>
        [XmlEnum("sum")]
        Sum
    }
}