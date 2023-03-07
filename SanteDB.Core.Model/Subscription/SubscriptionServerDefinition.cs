﻿/*
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
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Subscription
{
    /// <summary>
    /// Represents a server subscription definition
    /// </summary>
    [XmlType(nameof(SubscriptionServerDefinition), Namespace = "http://santedb.org/subscription")]
    public class SubscriptionServerDefinition
    {

        /// <summary>
        /// Gets or sets the invariant name
        /// </summary>
        [JsonIgnore, XmlAttribute("invariant")]
        public string InvariantName { get; set; }

        /// <summary>
        /// Gets or sets the SQL definition
        /// </summary>
        [XmlText, JsonIgnore]
        public string Definition { get; set; }

    }
}