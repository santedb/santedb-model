﻿/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
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
 * Date: 2017-9-1
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Concept relationship type
    /// </summary>
    [Classifier(nameof(Mnemonic)), KeyLookup(nameof(Mnemonic))]
    [XmlRoot("ConceptRelationshipType", Namespace = "http://santedb.org/model")]
    [XmlType("ConceptRelationshipType",  Namespace = "http://santedb.org/model"), JsonObject("ConceptRelationshipType")]
    public class ConceptRelationshipType : NonVersionedEntityData
    {

        /// <summary>
        /// Gets or sets the name of the relationship
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// The invariant of the relationship type
        /// </summary>
        [XmlElement("mnemonic"), JsonProperty("mnemonic")]
        public String Mnemonic { get; set; }

    }
}