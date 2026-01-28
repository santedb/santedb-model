/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Roles
{
    /// <summary>
    /// Represents a provider role of a person
    /// </summary>

    [XmlType("Provider", Namespace = "http://santedb.org/model"), JsonObject("Provider")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Provider")]
    [ResourceSensitivity(ResourceSensitivityClassification.Metadata)]
    [ClassConceptKey(EntityClassKeyStrings.Provider)]
    public class Provider : Person
    {

        /// <summary>
        /// Creates a new provider
        /// </summary>
        public Provider()
        {
            this.DeterminerConceptKey = DeterminerKeys.Specific;
            this.m_classConceptKey = EntityClassKeys.Provider;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.Provider;

        /// <summary>
        /// Gets or sets the provider specialty key
        /// </summary>
        [XmlElement("providerSpecialty"), JsonProperty("providerSpecialty")]
        public Guid? SpecialtyKey { get; set; }

        /// <summary>
        /// Gets or sets the provider specialty
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(SpecialtyKey))]
        public Concept Specialty { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Provider;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.SpecialtyKey == other.SpecialtyKey;
        }

        /// <inheritdoc/>
        public override ICanDeepCopy DeepCopy() => this.CloneDeep();
    }
}