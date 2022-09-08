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
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// A reference to any identified data object
    /// </summary>
    /// <remarks>
    /// The identified data reference class is not persisted as an object per se, rather it
    /// serves as a link to a piece of data which already exists where the type may not
    /// be known or where the type is known but limited fields are required to reference the data
    /// </remarks>
    [XmlType("Reference", Namespace = "http://santedb.org/model"), JsonObject("Reference")]
    [XmlRoot("Reference", Namespace = "http://santedb.org/model")]
    public class IdentifiedDataReference : IdentifiedData
    {
        /// <summary>
        /// Get the type identifier
        /// </summary>
        [XmlIgnore, JsonProperty("$type")]
        public override string Type { get => "Reference"; set { } }

        /// <summary>
        /// Modified on
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public override DateTimeOffset ModifiedOn => DateTimeOffset.MinValue;
    }
}