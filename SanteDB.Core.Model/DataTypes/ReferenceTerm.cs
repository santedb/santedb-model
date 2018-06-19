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
using System.Linq;


using System;
using System.Collections.Generic;
using System.ComponentModel;
using SanteDB.Core.Model.Attributes;
using System.Xml.Serialization;
using SanteDB.Core.Model.EntityLoader;
using Newtonsoft.Json;
using SanteDB.Core.Model.Constants;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a basic reference term
    /// </summary>
    [Classifier(nameof(CodeSystem))]
    [XmlType("ReferenceTerm",  Namespace = "http://santedb.org/model"), JsonObject("ReferenceTerm")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "ReferenceTerm")]
    public class ReferenceTerm : NonVersionedEntityData
    {

        // Backing field for code system identifier
        private Guid? m_codeSystemId;
        // Code system
        
        private CodeSystem m_codeSystem;
        // Display names
        
        private List<ReferenceTermName> m_displayNames;

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
        public CodeSystem CodeSystem {
            get
            {
                this.m_codeSystem = base.DelayLoad(this.m_codeSystemId, this.m_codeSystem);
                return this.m_codeSystem;
            }
            set
            {
                this.m_codeSystem = value;
                this.m_codeSystemId = value?.Key;
            }
        }
        
        /// <summary>
        /// Gets or sets the code system identifier
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("codeSystem"), JsonProperty("codeSystem")]
        [Binding(typeof(CodeSystemKeys))]
        public Guid?  CodeSystemKey {
            get { return this.m_codeSystemId; }
            set
            {
                this.m_codeSystemId = value;
                this.m_codeSystem = null;
            }
        }

        /// <summary>
        /// Gets display names associated with the reference term
        /// </summary>
        
        [XmlElement("name"), JsonProperty("name")]
        public List<ReferenceTermName> DisplayNames {
            get
            {
                if(this.m_displayNames == null && this.IsDelayLoadEnabled)
                    this.m_displayNames = EntitySource.Current.Provider.Query<ReferenceTermName>(o => o.SourceEntityKey == this.Key && o.ObsoletionTime == null).ToList();
                return this.m_displayNames;
            }
            set
            {
                this.m_displayNames = value;
            }
        }

        /// <summary>
        /// Force reloading of delay load properties
        /// </summary>
        public override void Refresh()
        {
            this.m_codeSystem = null;
            this.m_displayNames = null;
        }

    }
}