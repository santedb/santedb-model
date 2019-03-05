﻿/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 *
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
 * User: JustinFyfe
 * Date: 2019-1-22
 */
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Map
{
    /// <summary>
    /// Represents sort order
    /// </summary>
    [XmlType(nameof(SortOrderType), Namespace = "http://santedb.org/model/map")]
    public enum SortOrderType
    {
        /// <summary>
        /// Order by ascending.
        /// </summary>
        [XmlEnum("asc")]
        OrderBy,

        /// <summary>
        /// Order by descending.
        /// </summary>
        [XmlEnum("desc")]
        OrderByDescending
    }
}