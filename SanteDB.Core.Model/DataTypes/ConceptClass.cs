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
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Identifies a classification for a concept
    /// </summary>
    
    [XmlType("ConceptClass",  Namespace = "http://santedb.org/model"), JsonObject("ConceptClass")]
    [XmlRoot(nameof(ConceptClass), Namespace = "http://santedb.org/model")]

    [Classifier(nameof(Mnemonic)), KeyLookup(nameof(Mnemonic))]
    public class ConceptClass : NonVersionedEntityData
    {

        /// <summary>
        /// Gets or sets the name of the concept class
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the mnemonic
        /// </summary>
        [XmlElement("mnemonic"), JsonProperty("mnemonic")]
        public string Mnemonic { get; set; }


    }
}