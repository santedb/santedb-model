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
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a template definition
    /// </summary>
    /// <remarks>A template definition specifies the registerd templates for entities and acts in SanteDB like 
    /// act templates, or entity templates</remarks>
    [KeyLookup(nameof(Mnemonic))]
    [XmlRoot(nameof(TemplateDefinition), Namespace = "http://santedb.org/model")]
    [XmlType(nameof(TemplateDefinition), Namespace = "http://santedb.org/model"), JsonObject(nameof(TemplateDefinition))]
    public class TemplateDefinition : NonVersionedEntityData
    {

        /// <summary>
        /// Gets or sets the mnemonic
        /// </summary>
        [JsonProperty("mnemonic"), XmlElement("mnemonic")]
        public String Mnemonic { get; set; }

        /// <summary>
        /// Gets or set the name 
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the oid of the concept set
        /// </summary>
        [XmlElement("oid"), JsonProperty("oid")]
        public String Oid { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [XmlElement("description"), JsonProperty("description")]
        public String Description { get; set; }

        /// <summary>
        /// Determines the semantic equality of <paramref name="obj"/> and this object
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>True if the objects have the same meaning</returns>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as TemplateDefinition;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) && other.Mnemonic == this.Mnemonic;
        }
    }
}
