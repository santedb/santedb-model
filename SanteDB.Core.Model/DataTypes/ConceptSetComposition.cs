/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{


    /// <summary>
    /// Concept set reference role
    /// </summary>
    [XmlType("ConceptSetCompositionOperation", Namespace = "http://santedb.org/model")]
    public enum ConceptSetCompositionOperation
    {
        /// <summary>
        /// Source Includes Target
        /// </summary>
        [XmlEnum("include")]
        Include = 1,
        /// <summary>
        /// Source Excludes Target
        /// </summary>
        [XmlEnum("exclude")]
        Exclude = 2
    }


    /// <summary>
    /// Represents a reference between two different concept sets 
    /// </summary>
    [Classifier(nameof(Operation))]
    [XmlType("ConceptSetComposition", Namespace = "http://santedb.org/model"), JsonObject("ConceptSetComposition")]
    public class ConceptSetComposition : Association<ConceptSet>
    {

        /// <summary>
        /// The composition operation 
        /// </summary>
        [XmlElement("instruction"), JsonProperty("instruction")]
        public ConceptSetCompositionOperation Operation { get; set; }

        /// <summary>
        /// Gets the target of the composition
        /// </summary>
        [XmlElement("target"), JsonProperty("target")]
        public Guid? TargetKey { get; set; }

        /// <summary>
        /// Gets or sets the target key 
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(TargetKey))]
        public ConceptSet Target { get; set; }

    }
}