/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{

    /// <summary>
    /// Represents set of concepts
    /// </summary>
    [XmlType("ConceptSet", Namespace = "http://santedb.org/model")]
    [XmlRoot("ConceptSet", Namespace = "http://santedb.org/model")]
    [JsonObject("ConceptSet")]
    [Classifier(nameof(Mnemonic)), KeyLookup(nameof(Mnemonic))]
    public class ConceptSet : NonVersionedEntityData
    {

        /// <summary>
        /// Concept set
        /// </summary>
        public ConceptSet()
        {

        }

        /// <summary>
        /// Gets or sets the name of the concept set
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the mnemonic for the concept set (used for convenient lookup)
        /// </summary>
        [XmlElement("mnemonic"), JsonProperty("mnemonic")]
        public String Mnemonic { get; set; }

        /// <summary>
        /// Gets or sets the oid of the concept set
        /// </summary>
        [XmlElement("oid"), JsonProperty("oid")]
        public String Oid { get; set; }

        /// <summary>
        /// Gets or sets the url of the concept set
        /// </summary>
        [XmlElement("url"), JsonProperty("url")]
        public String Url { get; set; }

        /// <summary>
        /// Concepts as identifiers for XML purposes only
        /// </summary>
        [XmlElement("concept"), JsonProperty("concept")]
        //[Bundle(nameof(Concepts))]
        public List<Guid> ConceptsXml { get; set; }

        /// <summary>
        /// If the concept set is composed of other concept sets
        /// </summary>
        [XmlElement("compose"), JsonProperty("compose")]
        public List<ConceptSetComposition> Composition { get; set; }

        /// <summary>
        /// Gets the concepts in the set
        /// </summary>
        [SerializationMetadata, XmlIgnore, JsonIgnore]
        public IEnumerable<Concept> Concepts
        {
            get
            {
                return this.ConceptsXml?.Select(o => EntitySource.Current.Get<Concept>(o) ?? new Concept() { Key = o });
            }
            set
            {
                this.ConceptsXml = value?.Where(o => o.Key.HasValue).Select(o => o.Key.Value).ToList();
            }
        }


        /// <inheritdoc/>
        public override ICanDeepCopy DeepCopy() => this.CloneDeep();
    }
}
