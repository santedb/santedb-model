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
using Newtonsoft.Json;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Serialization;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Patch
{
    /// <summary>
    /// Represents a target of a patch
    /// </summary>
    [XmlType(nameof(PatchTarget), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(PatchTarget))]
    public class PatchTarget
    {

        // Binder
        private static ModelSerializationBinder s_binder = new ModelSerializationBinder();

        /// <summary>
        /// Default ctor
        /// </summary>
        public PatchTarget()
        {

        }

        /// <summary>
        /// Construct a new patch target
        /// </summary>
        public PatchTarget(IdentifiedData existing)
        {
            this.Type = existing.GetType();
            this.Key = existing.Key;
            this.VersionKey = (existing as IVersionedData)?.VersionKey;
            this.Tag = existing.Tag;
        }

        /// <summary>
        /// Identifies the target type
        /// </summary>
        [XmlAttribute("type"), JsonProperty("type")]
        public string TypeXml
        {
            get
            {
                s_binder.BindToName(this.Type, out _, out var type);
                return type;
            }
            set
            {
                this.Type = s_binder.BindToType(typeof(IdentifiedData).Assembly.FullName, value);
            }
        }

        /// <summary>
        /// Represents the type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        [XmlElement("id"), JsonProperty("id")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        [XmlElement("version"), JsonProperty("version")]
        public Guid? VersionKey { get; set; }

        /// <summary>
        /// Gets or sets the tag of the item
        /// </summary>
        [XmlElement("tag"), JsonProperty("etag")]
        public string Tag { get; set; }
    }
}