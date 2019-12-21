/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: Justin Fyfe
 * Date: 2019-8-8
 */
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Subscription
{
    /// <summary>
    /// Represents client side definition
    /// </summary>
    [XmlType(nameof(SubscriptionClientDefinition), Namespace = "http://santedb.org/subscription")]
    [JsonObject(nameof(SubscriptionClientDefinition))]
    public class SubscriptionClientDefinition
    {

        /// <summary>
        /// Gets or sets the resource type reference
        /// </summary>
        [XmlAttribute("resource"), JsonProperty("resource")]
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the name of the subscription
        /// </summary>
        [XmlAttribute("name"), JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the mode of the subscription
        /// </summary>
        [XmlAttribute("mode"), JsonProperty("mode")]
        public SubscriptionModeType Mode { get; set; }

        /// <summary>
        /// Gets or sets the trigger
        /// </summary>
        [XmlAttribute("trigger"), JsonProperty("trigger")]
        public SubscriptionTriggerType Trigger { get; set; }

        /// <summary>
        /// Gets or sets the ignore modified on (prevents If-Modified-Since from being used)
        /// </summary>
        [XmlAttribute("ignoreModifiedOn"), JsonProperty("ignoreModifiedOn")]
        public bool IgnoreModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the guards which indicate when this subscription can be activated
        /// </summary>
        [XmlArray("guards"), XmlArrayItem("when"), JsonProperty("guards")]
        public List<string> Guards { get; set; }

        /// <summary>
        /// Gets or sets the filters
        /// </summary>
        [XmlArray("filters"), XmlArrayItem("add"), JsonProperty("filters")]
        public List<string> Filters { get; set; }

    }
}