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
using SanteDB.Core.Model.Interfaces;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents an entity which is alive, but not a person
    /// </summary>
    /// <remarks>
    /// <para>In SanteDB a non-person living subject is used to represent entities such as parasites, viruses,
    /// bacteria, plants, etc. The primary use of this class is to capture information in relation
    /// to infections, protections or components of other entities.</para>
    /// </remarks>
    [XmlType(nameof(NonPersonLivingSubject), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(NonPersonLivingSubject), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(NonPersonLivingSubject))]
    [ClassConceptKey(EntityClassKeyStrings.LivingSubject)]
    [ClassConceptKey(EntityClassKeyStrings.Food)]
    [ClassConceptKey(EntityClassKeyStrings.Animal)]
    [ResourceSensitivity(ResourceSensitivityClassification.PersonalHealthInformation)]
    public class NonPersonLivingSubject : Entity
    {

        /// <summary>
        /// Living subject
        /// </summary>
        public NonPersonLivingSubject()
        {
            this.m_classConceptKey = EntityClassKeys.LivingSubject;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.LivingSubject || classKey == EntityClassKeys.Animal || classKey == EntityClassKeys.Food;


        /// <summary>
        /// Gets the description of the strain
        /// </summary>
        [XmlElement("strain"), JsonProperty("strain")]
        public Guid? StrainKey
        {
            get; set;
        }

        /// <summary>
        /// Strain
        /// </summary>
        [SerializationReference(nameof(StrainKey)), XmlIgnore, JsonIgnore]
        public Concept Strain
        {
            get; set;
        }

        /// <inheritdoc/>
        public override ICanDeepCopy DeepCopy() => this.CloneDeep();
    }
}