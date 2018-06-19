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
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SanteDB.Core.Model.Roles
{
    /// <summary>
    /// Represents a provider role of a person
    /// </summary>
    
    [XmlType("Provider",  Namespace = "http://santedb.org/model"), JsonObject("Provider")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Provider")]
    public class Provider : Person
    {

        // Specialty key
        private Guid? m_providerSpecialtyKey;
        // Specialty value
        
        private Concept m_providerSpeciality;

        /// <summary>
        /// Creates a new provider
        /// </summary>
        public Provider()
        {
            this.DeterminerConceptKey = DeterminerKeys.Specific;
            this.ClassConceptKey = EntityClassKeys.Provider;
        }

        /// <summary>
        /// Gets or sets the provider specialty key
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        
        [XmlElement("providerSpecialty"), JsonProperty("providerSpecialty")]
        public Guid? ProviderSpecialtyKey
        {
            get
            {
                return this.m_providerSpecialtyKey;
            }
            set
            {
                if (this.m_providerSpecialtyKey != value)
                {
                    this.m_providerSpecialtyKey = value;
                    this.m_providerSpeciality = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the provider specialty
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ProviderSpecialtyKey))]
        public Concept ProviderSpecialty
        {
            get
            {
                this.m_providerSpeciality = base.DelayLoad(this.m_providerSpecialtyKey, this.m_providerSpeciality);
                return this.m_providerSpeciality;
            }
            set
            {
                this.m_providerSpeciality = value;
                this.m_providerSpecialtyKey = value?.Key;
            }
        }

        /// <summary>
        /// Force a refresh of the delay load properties
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.m_providerSpeciality = null;
        }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Provider;
            if (other == null)
                return false;
            return base.SemanticEquals(obj) &&
                this.ProviderSpecialtyKey == other.ProviderSpecialtyKey;
        }
    }
}
