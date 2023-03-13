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
using SanteDB.Core.Model.Attributes;
using System;
using System.Threading;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a name (human name) that a concept may have
    /// </summary>
    [Classifier(nameof(Language)), SimpleValue(nameof(Name))]
    [XmlType("ConceptName", Namespace = "http://santedb.org/model"), JsonObject("ConceptName")]
    public class ConceptName : VersionedAssociation<Concept>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptName"/> class.
        /// </summary>
        public ConceptName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ConceptName(string name)
        {
            this.Name = name;
            this.Language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptName"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="name">The name.</param>
        public ConceptName(string language, string name) : this(name)
        {
            this.Language = language;
        }

        /// <summary>
        /// Gets or sets the language code of the object
        /// </summary>
        [XmlElement("language"), JsonProperty("language")]
        public String Language { get; set; }

        /// <summary>
        /// Gets or sets the name of the reference term
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public String Name { get; set; }

        /// <summary>
        /// Represent as a string
        /// </summary>
        public override string ToString()
        {
            return $"{this.Name} [{this.Language}]";
        }
    }
}