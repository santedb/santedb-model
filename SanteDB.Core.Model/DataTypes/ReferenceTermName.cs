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
using SanteDB.Core.Model.Interfaces;
using System;
using System.Threading;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Display name of a code system or reference term
    /// </summary>
    [XmlType("ReferenceTermName", Namespace = "http://santedb.org/model"), JsonObject("ReferenceTermName")]
    [Classifier(nameof(Language)), SimpleValue(nameof(Name))]
    public class ReferenceTermName : BaseEntityData, ISimpleAssociation
    {
        /// <summary>
        /// Gets the source type of this relationship
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Type SourceType => typeof(ReferenceTerm);

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceTermName"/> class.
        /// </summary>
        public ReferenceTermName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceTermName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ReferenceTermName(string name)
        {
            this.Language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceTermName"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="name">The name.</param>
        public ReferenceTermName(string language, string name) : this(name)
        {
            this.Language = language;
        }

        /// <summary>
        /// Should serialize reference term key
        /// </summary>
        public bool ShouldSerializeSourceEntityKey()
        {
            return this.SourceEntityKey.HasValue;
        }

        /// <summary>
        /// Gets or sets the language code of the object
        /// </summary>
        /// <remarks> In XML, <see cref="Language"/> is represented as:
        /// <code lang="XML">&lt;language&gt;en&lt;/language&gt;</code>
        /// In JSON, <see cref="Language"/> is represented as:
        /// <code lang="JSON">"language": "en"</code>
        /// </remarks>
        [XmlElement("language"), JsonProperty("language")]
        public String Language { get; set; }

        /// <summary>
        /// Gets or sets the name of the reference term
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public String Name { get; set; }

        /// <summary>
        /// Gets the source entity key
        /// </summary>
        [XmlElement("source"), JsonProperty("source")]
        public Guid? SourceEntityKey
        {
            get; set;
        }

        /// <summary>
        /// Source entity
        /// </summary>
        object ISimpleAssociation.SourceEntity { get => null; set { } }
    }
}