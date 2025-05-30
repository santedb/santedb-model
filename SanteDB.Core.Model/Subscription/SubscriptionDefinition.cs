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
using Newtonsoft.Json;
using SanteDB.Core.Model.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Subscription
{
    /// <summary>
    /// Class which is used to define a subscription type which clients can consume
    /// </summary>
    [XmlType(nameof(SubscriptionDefinition), Namespace = "http://santedb.org/subscription")]
    [XmlRoot(nameof(SubscriptionDefinition), Namespace = "http://santedb.org/subscription")]
    [JsonObject(nameof(SubscriptionDefinition))]
    public class SubscriptionDefinition : IdentifiedData
    {
        // XmlSerializer
        private static XmlSerializer m_xsz = XmlModelSerializerFactory.Current.CreateSerializer(typeof(SubscriptionDefinition));

        /// <summary>
        /// Gets or sets the uuid
        /// </summary>
        [XmlAttribute("uuid"), JsonProperty("uuid")]
        public Guid Uuid { get => base.Key.GetValueOrDefault(); set => base.Key = value; }

        /// <summary>
        /// Don't include uuid
        /// </summary>
        public bool ShouldSerializeUuid() => false;

        /// <summary>
        /// Default ctor
        /// </summary>
        public SubscriptionDefinition()
        {
            this.Key = Guid.NewGuid();
            this.ServerDefinitions = new List<SubscriptionServerDefinition>();
            this.ClientDefinitions = new List<SubscriptionClientDefinition>();
        }

        /// <summary>
        /// Gets the time that this was modified
        /// </summary>
        public override DateTimeOffset ModifiedOn => DateTimeOffset.Now;

        /// <summary>
        /// Gets the name of the subscription
        /// </summary>
        [XmlAttribute("name"), JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the resource type
        /// </summary>
        [XmlAttribute("resource"), JsonIgnore]
        public String Resource { get; set; }

        /// <summary>
        /// Gets the order of synchronization
        /// </summary>
        [XmlAttribute("order"), JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Gets the resource type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Type ResourceType => new ModelSerializationBinder().BindToType("SanteDB.Core.Model", this.Resource);

        /// <summary>
        /// Gets or sets the server definitions
        /// </summary>
        [XmlArray("server"), XmlArrayItem("definition"), JsonIgnore]
        public List<SubscriptionServerDefinition> ServerDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the client side definitions
        /// </summary>
        [XmlArray("client"), XmlArrayItem("definition"), JsonProperty("definitions")]
        public List<SubscriptionClientDefinition> ClientDefinitions { get; set; }

        /// <summary>
        /// Load the specified subscription definition  
        /// </summary>
        public static SubscriptionDefinition Load(MemoryStream ms)
        {
            return m_xsz.Deserialize(ms) as SubscriptionDefinition;
        }
    }
}
