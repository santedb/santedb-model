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
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Batch operation type
    /// </summary>
    [XmlType(nameof(BatchOperationType), Namespace = "http://santedb.org/model")]
    public enum BatchOperationType
    {
        /// <summary>
        /// Automatically decide 
        /// </summary>
        Auto = 0,
        /// <summary>
        /// Insert the object only
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Insert the object or update it
        /// </summary>
        InsertOrUpdate = 2,
        /// <summary>
        /// Update the object only
        /// </summary>
        Update = 3,
        /// <summary>
        /// Delete the object
        /// </summary>
        Delete = 4,
        /// <summary>
        /// Ignore this object - it is for reference only
        /// </summary>
        Ignore = 5
    }
}
