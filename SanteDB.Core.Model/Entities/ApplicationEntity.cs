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
using SanteDB.Core.Model.Security;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents the clinical data object for Applications
    /// </summary>
    /// <remarks>
    /// In SanteDB's data model, all security objects (objects which may alter the state of data) are separated
    /// into a security object (in this example <see cref="SecurityApplication"/>) and a clinical object (<see cref="ApplicationEntity"/>).
    /// This allows SanteDB to store provenance data (authoriship, origin, etc) without conflating the original
    /// device, application or user with the security user, device, or application.
    /// </remarks>

    [XmlType("ApplicationEntity", Namespace = "http://santedb.org/model"), JsonObject("ApplicationEntity")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "ApplicationEntity")]
    [ClassConceptKey(EntityClassKeyStrings.NonLivingSubject)]
    public class ApplicationEntity : Entity
    {
        /// <summary>
        /// Creates application entity
        /// </summary>
        public ApplicationEntity()
        {
            this.m_classConceptKey = EntityClassKeys.NonLivingSubject;
            this.DeterminerConceptKey = DeterminerKeys.Specific;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.NonLivingSubject;

        /// <summary>
        /// Gets or sets the security application
        /// </summary>
        [SerializationReference(nameof(SecurityApplicationKey))]
        [XmlIgnore, JsonIgnore]
        public SecurityApplication SecurityApplication { get; set; }

        /// <summary>
        /// Gets or sets the security application
        /// </summary>
        [XmlElement("securityApplication"), JsonProperty("securityApplication")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Guid? SecurityApplicationKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the software
        /// </summary>
        [XmlElement("softwareName"), JsonProperty("softwareName")]
        public String SoftwareName { get; set; }

        /// <summary>
        /// Gets or sets the vendoer name of the software
        /// </summary>
        [XmlElement("vendorName"), JsonProperty("vendorName")]
        public String VendorName { get; set; }

        /// <summary>
        /// Gets or sets the version of the software
        /// </summary>
        [XmlElement("versionName"), JsonProperty("versionName")]
        public String VersionName { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as ApplicationEntity;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) && this.SecurityApplicationKey == other.SecurityApplicationKey &&
                this.SoftwareName == other.SoftwareName &&
                this.VersionName == other.VersionName &&
                this.VendorName == other.VendorName;
        }
    }
}