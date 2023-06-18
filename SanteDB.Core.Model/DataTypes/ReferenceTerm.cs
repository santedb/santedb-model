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
 * Date: 2023-5-19
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a basic reference term
    /// </summary>
    [Classifier(nameof(CodeSystem))]
    [XmlType("ReferenceTerm", Namespace = "http://santedb.org/model"), JsonObject("ReferenceTerm")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "ReferenceTerm")]
    public class ReferenceTerm : NonVersionedEntityData
    {

        /// <summary>
        /// Create reference term instance 
        /// </summary>
        public ReferenceTerm()
        {
        }

        /// <summary>
        /// Creates a new reference term with the specified <paramref name="mnemonic"/> in the idicated <paramref name="codeSystemKey"/>
        /// </summary>
        /// <param name="mnemonic">The mnemonic of the concept reference term</param>
        /// <param name="codeSystemKey">The code system in which the reference term belongs</param>
        public ReferenceTerm(String mnemonic, Guid codeSystemKey)
        {
            this.CodeSystemKey = codeSystemKey;
            this.Mnemonic = mnemonic;
        }

        /// <summary>
        /// Gets or sets the mnemonic for the reference term
        /// </summary>
        [XmlElement("mnemonic"), JsonProperty("mnemonic")]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Gets or sets the code system
        /// </summary>
        [SerializationReference(nameof(CodeSystemKey))]
        [XmlIgnore, JsonIgnore]
        public CodeSystem CodeSystem { get; set; }

        /// <summary>
        /// Gets or sets the code system identifier
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("codeSystem"), JsonProperty("codeSystem")]
        [Binding(typeof(CodeSystemKeys))]
        public Guid? CodeSystemKey { get; set; }

        /// <summary>
        /// Gets display names associated with the reference term
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public List<ReferenceTermName> DisplayNames { get; set; }

        /// <summary>
        /// Get display name for the reference term
        /// </summary>
        public string GetDisplayName(String language = null)
        {
            language = language ?? System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            return this.LoadCollection(o => o.DisplayNames).FirstOrDefault(o => language.Equals(o.Language, StringComparison.OrdinalIgnoreCase))?.Name;
        }
    }
}