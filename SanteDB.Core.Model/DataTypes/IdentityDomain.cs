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
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
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
    /// TODO: Refactor for V3 -> Identity Domain
    [Classifier(nameof(DomainName)), KeyLookup(nameof(DomainName))]
    [XmlType(nameof(IdentityDomain), Namespace = "http://santedb.org/model"), JsonObject(nameof(IdentityDomain))]
    [XmlRoot(nameof(IdentityDomain), Namespace = "http://santedb.org/model")]
    public class IdentityDomain : NonVersionedEntityData
    {
        private bool m_minimal = false;

        // private validator
        private IIdentifierValidator m_validator;
        private ICheckDigitAlgorithm m_checkDigit;

        /// <summary>
        /// Assigning authority
        /// </summary>
        public IdentityDomain()
        {
        }

        /// <summary>
        /// Creates a new assigning authority
        /// </summary>
        public IdentityDomain(String domainName, String name, String oid)
        {
            this.DomainName = domainName;
            this.Name = name;
            this.Oid = oid;
        }

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
        [JsonProperty("scope"), XmlElement("scope")]
        public List<Guid> AuthorityScopeXml
        {
            get;
            set;
        }

        /// <summary>
        /// Assigning device identifier
        /// </summary>
        [XmlElement("assigningAuthority"), JsonProperty("assigningAuthority")]
        public List<AssigningAuthority> AssigningAuthority { get; set; }

        /// <summary>
        /// Gets or sets the policy
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(PolicyKey))]
        public SecurityPolicy Policy { get; set; }

        /// <summary>
        /// Gets or sets the policy key associated with this assigning authority for disclosure
        /// </summary>
        [XmlElement("policy"), JsonProperty("policy")]
        public Guid? PolicyKey { get; set; }

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
        /// Gets or sets the IIdentifierValidator instance to use for this solution
        /// </summary>
        [XmlElement("customValidator"), JsonProperty("customValidator")]
        public string CustomValidator { get; set; }

        /// <summary>
        /// Gets or sets the check digit algorithm 
        /// </summary>
        [XmlElement("checkDigitAlgorithm"), JsonProperty("checkDigitAlgorithm")]
        public string CheckDigitAlgorithm { get; set; }

        /// <summary>
        /// Get check digit algorithm
        /// </summary>
        public ICheckDigitAlgorithm GetCheckDigitAlgorithm()
        {
            if (this.m_validator == null && !String.IsNullOrEmpty(this.CheckDigitAlgorithm))
            {
                var t = System.Type.GetType(this.CustomValidator);
                if (t == null)
                {
                    throw new InvalidOperationException($"Validator {this.CheckDigitAlgorithm} is not valid");
                }

                this.m_checkDigit = Activator.CreateInstance(t) as ICheckDigitAlgorithm;
            }
            return this.m_checkDigit;
        }

        /// <summary>
        /// Gets the custom validator
        /// </summary>
        /// <returns>The validator</returns>
        public IIdentifierValidator GetCustomValidator()
        {
            if (this.m_validator == null && !String.IsNullOrEmpty(this.CustomValidator))
            {
                var t = System.Type.GetType(this.CustomValidator);
                if (t == null)
                {
                    throw new InvalidOperationException($"Validator {this.CustomValidator} is not valid");
                }

                this.m_validator = Activator.CreateInstance(t) as IIdentifierValidator;
            }
            return this.m_validator;
        }

        /// <summary>
        /// Should serialize IsUnique
        /// </summary>
        public bool ShouldSerializeIsUnique() => this.IsUnique;

        /// <summary>
        /// Gets concept sets to which this concept is a member
        /// </summary>
        [SerializationMetadata, XmlIgnore, JsonIgnore, SerializationReference(nameof(AuthorityScopeXml))]
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
        public IdentityDomain ToMinimal()
        {
            return new IdentityDomain()
            {
                Key = this.Key,
                DomainName = this.DomainName,
                Name = this.Name,
                Oid = this.Oid,
                m_minimal = true
            };
        }

        /// <summary>
        /// Returns whether <paramref name="obj"/> has the same meaning as this object
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as IdentityDomain;
            if (other == null)
            {
                return false;
            }

            return other.DomainName == this.DomainName ||
                this.Oid == other.Oid ||
                this.Url == other.Url ||
                this.AssigningAuthority?.SequenceEqual(other.AssigningAuthority) == true;
        }

#pragma warning disable CS1591
        public bool ShouldSerializeAuthorityScopeXml() => !this.m_minimal;
        public bool ShouldSerializeUrl() => !this.m_minimal;
        public bool ShouldSerializeOid() => !this.m_minimal;
        public bool ShouldSerializeValidationRegex() => !this.m_minimal;
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