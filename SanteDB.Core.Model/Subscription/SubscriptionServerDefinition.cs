/*
 * Based on OpenIZ, Copyright (C) 2015 - 2019 Mohawk College of Applied Arts and Technology
 * Copyright (C) 2019 - 2020, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2019-11-27
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