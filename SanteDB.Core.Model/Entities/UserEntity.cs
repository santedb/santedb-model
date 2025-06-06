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
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.Security;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a user entity
    /// </summary>
    /// <remarks>A UserEntity is used to represent a user which is not a provider nor a patient which
    /// may use the system - a person with whom a user is associated</remarks>
    [XmlType("UserEntity", Namespace = "http://santedb.org/model"), JsonObject("UserEntity")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "UserEntity")]
    [ClassConceptKey(EntityClassKeyStrings.UserEntity)]
    [ResourceSensitivity(ResourceSensitivityClassification.Administrative)]
    public class UserEntity : Person
    {
        /// <summary>
        /// Creates a new instance of user entity
        /// </summary>
        public UserEntity()
        {
            this.DeterminerConceptKey = DeterminerKeys.Specific;
            this.m_classConceptKey = EntityClassKeys.UserEntity;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.UserEntity;

        /// <summary>
        /// Gets or sets the security user key
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationMetadata]
        [SerializationReference(nameof(SecurityUserKey))]
        public SecurityUser SecurityUser { get; set; }

        /// <summary>
        /// Gets or sets the security user key
        /// </summary>
        [XmlElement("securityUser"), JsonProperty("securityUser")]
        public Guid? SecurityUserKey { get; set; }

        /// <summary>
        /// Determine semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as UserEntity;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.SecurityUserKey == other.SecurityUserKey;
        }
    }
}