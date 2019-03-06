/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: justi
 * Date: 2019-1-12
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a model class which is an assigning authority
    /// </summary>
    [Classifier(nameof(DomainName)), KeyLookup(nameof(DomainName))]
    [XmlType(nameof(AssigningAuthority), Namespace = "http://santedb.org/model"), JsonObject("AssigningAuthority")]
    [XmlRoot(nameof(AssigningAuthority), Namespace = "http://santedb.org/model")]
    public class AssigningAuthority : BaseEntityData
    {

        private bool m_minimal = false;

        /// <summary>
        /// Assigning authority
        /// </summary>
        public AssigningAuthority()
        {

        }

        /// <summary>
        /// Creates a new assigning authority 
        /// </summary>
        public AssigningAuthority(String domainName, String name, String oid)
        {
            this.DomainName = domainName;
            this.Name = name;
            this.Oid = oid;
        }

        // Assigning device id
        private Guid? m_assigningApplicationKey;

        // TODO: Change this to SecurityDevice
        private SecurityApplication m_assigningApplication;

        /// <summary>
        /// Gets or sets the name of the assigning authority
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the domain name of the assigning authority
        /// </summary>
        [XmlElement("domainName"), JsonProperty("domainName")]
        public String DomainName { get; set; }

        /// <summary>
        /// Gets or sets the description of the assigning authority
        /// </summary>
        [XmlElement("description"), JsonProperty("description")]
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the oid of the assigning authority
        /// </summary>
        [XmlElement("oid"), JsonProperty("oid")]
        public String Oid { get; set; }

        /// <summary>
        /// The URL of the assigning authority
        /// </summary>
        [XmlElement("url"), JsonProperty("url")]
        public String Url { get; set; }

        /// <summary>
        /// Represents scopes to which the authority is bound
        /// </summary>
        [AutoLoad, JsonProperty("scope"), XmlElement("scope")]
        public List<Guid> AuthorityScopeXml { get; set; }

        /// <summary>
        /// Assigning device identifier
        /// </summary>
        [XmlElement("assigningApplication"), JsonProperty("assigningApplication")]
        public Guid? AssigningApplicationKey
        {
            get { return this.m_assigningApplicationKey; }
            set
            {
                this.m_assigningApplicationKey = value;
                this.m_assigningApplication = null;
            }
        }

        /// <summary>
        /// Gets or sets the validation regex
        /// </summary>
        [XmlElement("validation"), JsonProperty("validation")]
        public String ValidationRegex { get; set; }

        /// <summary>
        /// True if the assigning authority values should be unique
        /// </summary>
        [XmlElement("isUnique"), JsonProperty("isUnique")]
        public bool IsUnique { get; set; }

        /// <summary>
        /// Should serialize IsUnique
        /// </summary>
        public bool ShouldSerializeIsUnique() => this.IsUnique;

        /// <summary>
        /// Gets or sets the assigning device
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(AssigningApplicationKey))]
        public SecurityApplication AssigningApplication
        {
            get
            {
                this.m_assigningApplication = base.DelayLoad(this.m_assigningApplicationKey, this.m_assigningApplication);
                return this.m_assigningApplication;
            }
            set
            {
                this.m_assigningApplication = value;
                this.m_assigningApplicationKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets concept sets to which this concept is a member
        /// </summary>
        [DataIgnore, XmlIgnore, JsonIgnore, SerializationReference(nameof(AuthorityScopeXml))]
        public List<Concept> AuthorityScope
        {
            get
            {
                return this.AuthorityScopeXml?.Select(o => EntitySource.Current.Get<Concept>(o)).ToList();
            }
            set
            {
                this.AuthorityScopeXml = value?.Where(o => o.Key.HasValue).Select(o => o.Key.Value).ToList();
            }
        }

        /// <summary>
        /// Represent the AA as a minimal info
        /// </summary>
        public AssigningAuthority ToMinimal()
        {
            return new AssigningAuthority()
            {
                Key = this.Key,
                DomainName = this.DomainName,
                Name = this.Name,
                Oid = this.Oid,
                m_minimal = true
            };
        }

        /// <summary>
        /// Force reloading of delay load properties
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.m_assigningApplication = null;
        }

        /// <summary>
        /// Gets the equals of this object
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as AssigningAuthority;
            if (other == null) return false;
            return base.SemanticEquals(obj) && other.DomainName == this.DomainName &&
                this.Oid == other.Oid &&
                this.Url == other.Url &&
                this.AssigningApplicationKey == other.AssigningApplicationKey;
        }


#pragma warning disable CS1591
        public bool ShouldSerializeAuthorityScopeXml() => !this.m_minimal;
        public bool ShouldSerializeUrl() => !this.m_minimal;
        public bool ShouldSerializeOid() => !this.m_minimal;
        public bool ShouldSerializeValidationRegex() => !this.m_minimal;
        public bool ShouldSerializeAssigningDeviceKey() => !this.m_minimal && this.AssigningApplicationKey.HasValue;
#pragma warning restore CS1591

        /// <summary>
        /// Represent this as a string
        /// </summary>
        public override string ToString()
        {
            return $"{this.DomainName},{this.Oid}";
        }
    }
}