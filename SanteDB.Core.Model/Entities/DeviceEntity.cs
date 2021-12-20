/*
 * Copyright (C) 2021 - 2021, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-5
 */

using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Security;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a device entity
    /// </summary>

    [XmlType("DeviceEntity", Namespace = "http://santedb.org/model"), JsonObject("DeviceEntity")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "DeviceEntity")]
    [ClassConceptKey(EntityClassKeyStrings.Device)]
    public class DeviceEntity : Entity
    {
        /// <summary>
        /// Device entity ctor
        /// </summary>
        public DeviceEntity()
        {
            this.DeterminerConceptKey = DeterminerKeys.Specific;
        }

        /// <summary>
        /// Gets or sets the class concept key
        /// </summary>
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        public override Guid? ClassConceptKey { get => EntityClassKeys.Device; set => base.ClassConceptKey = EntityClassKeys.Device; }

        /// <summary>
        /// Gets or sets the manufacturer model name
        /// </summary>
        [XmlElement("manufacturerModelName"), JsonProperty("manufacturerModelName")]
        public String ManufacturerModelName { get; set; }

        /// <summary>
        /// Gets or sets the operating system name
        /// </summary>
        [XmlElement("operatingSystemName"), JsonProperty("operatingSystemName")]
        public String OperatingSystemName { get; set; }

        /// <summary>
        /// Gets or sets the security device
        /// </summary>
        [SerializationReference(nameof(SecurityDeviceKey))]
        [XmlIgnore, JsonIgnore]
        public SecurityDevice SecurityDevice { get; set; }

        /// <summary>
        /// Gets or sets the security device key
        /// </summary>
        [XmlElement("securityDevice"), JsonProperty("securityDevice")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Guid? SecurityDeviceKey { get; set; }

        /// <summary>
        /// Determine semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as DeviceEntity;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.SecurityDeviceKey == other.SecurityDeviceKey &&
                this.ManufacturerModelName == other.ManufacturerModelName &&
                this.OperatingSystemName == other.OperatingSystemName;
        }
    }
}