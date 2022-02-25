/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-27
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a basic information class which classifies the use of an identifier
    /// </summary>

    [XmlType(nameof(IdentifierType), Namespace = "http://santedb.org/model"), JsonObject("IdentifierType")]
    [XmlRoot(nameof(IdentifierType), Namespace = "http://santedb.org/model")]
    public class IdentifierType : BaseEntityData
    {

        /// <summary>
        /// Gets or sets the id of the scope concept
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("scopeConcept"), JsonProperty("scopeConcept")]
        public Guid? ScopeConceptKey { get; set; }

        /// <summary>
        /// Gets or sets the concept which identifies the type
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("typeConcept"), JsonProperty("typeConcept")]
        public Guid? TypeConceptKey { get; set; }

        /// <summary>
        /// Type concept
        /// </summary>
        [SerializationReference(nameof(TypeConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept TypeConcept { get; set; }

        /// <summary>
        /// Gets the scope of the identifier
        /// </summary>
        [SerializationReference(nameof(ScopeConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept ScopeConcept { get; set; }

    }
}