/*
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
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Reliability of the identifier
    /// </summary>
    [XmlType("IdentifierReliability", Namespace = "http://santedb.org/model")]
    public enum IdentifierReliability
    {
        /// <summary>
        /// Unspecified
        /// </summary>
        [XmlEnum("u")]
        Unspecified = 0,

        /// <summary>
        /// Authoritative
        /// </summary>
        [XmlEnum("a")]
        Authoritative = 1,

        /// <summary>
        /// Informative
        /// </summary>
        [XmlEnum("i")]
        Informative = 2
    }

    /// <summary>
    /// Entity identifiers
    /// </summary>
    [XmlType("EntityIdentifier", Namespace = "http://santedb.org/model"), JsonObject("EntityIdentifier")]
    public class EntityIdentifier : IdentifierBase<Entity>
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public EntityIdentifier()
        {
        }

        /// <summary>
        /// Creates a new entity identifier with specified authority
        /// </summary>
        public EntityIdentifier(Guid authorityId, String value)
        {
            this.IdentityDomainKey = authorityId;
            this.Value = value;
        }

        /// <summary>
        /// Creates a new entity identifier
        /// </summary>
        public EntityIdentifier(IdentityDomain authority, String value)
        {
            this.IdentityDomain = authority;
            this.IdentityDomainKey = authority?.Key;
            this.Value = value;
        }

        /// <summary>
        /// Represent as a display
        /// </summary>
        public override string ToDisplay() => $"{this.Value}^^^{this.IdentityDomain?.DomainName ?? this.IdentityDomainKey.ToString()}";

        /// <inheritdoc/>
        public override ICanDeepCopy DeepCopy() => this.CloneDeep();
    }

    /// <summary>
    /// Act identifier
    /// </summary>

    [XmlType(Namespace = "http://santedb.org/model", TypeName = "ActIdentifier"), JsonObject("ActIdentifier")]
    public class ActIdentifier : IdentifierBase<Act>
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ActIdentifier()
        {
        }

        /// <summary>
        /// Creates a new entity identifier with specified authority
        /// </summary>
        public ActIdentifier(Guid authorityId, String value)
        {
            this.IdentityDomainKey = authorityId;
            this.Value = value;
        }

        /// <summary>
        /// Creates a new entity identifier
        /// </summary>
        public ActIdentifier(IdentityDomain authority, String value)
        {
            this.IdentityDomain = authority;
            this.Value = value;
        }

        /// <inheritdoc/>
        public override ICanDeepCopy DeepCopy() => this.CloneDeep();
    }

    /// <summary>
    /// Represents an external assigned identifier
    /// </summary>
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("IdentifierBase")]
    [Classifier(nameof(IdentityDomain), nameof(IdentityDomainKey))]
    public abstract class IdentifierBase<TBoundModel> : VersionedAssociation<TBoundModel>, IExternalIdentifier, IHasExternalKey where TBoundModel : VersionedEntityData<TBoundModel>, new()
    {
        /// <summary>
        /// Gets or sets the value of the identifier
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public String Value { get; set; }

        /// <summary>
        /// Gets or sets the external key for the object
        /// </summary>
        /// <remarks>Sometimes, when communicating with an external communications another system needs to 
        /// refer to this by a particular key</remarks>
        [XmlElement("externId"), JsonProperty("externId")]
        public string ExternalKey { get; set; }

        /// <summary>
        /// Serialization property for issued date
        /// </summary>
        [XmlElement("issued"), JsonProperty("issued"), SerializationMetadata, EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        public String IssueDateXml
        {
            get
            {
                return this.IssueDate?.ToString("yyyy-MM-dd");
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    // Try to parse ISO date
                    if (DateTime.TryParseExact(value, new String[] { "o", "yyyy-MM-dd", "yyyy-MM", "yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                    {
                        this.IssueDate = dt;
                    }
                    else
                    {
                        throw new FormatException($"Cannot parse {value} as a date");
                    }
                }
                else
                {
                    this.IssueDate = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the date of issue
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public DateTimeOffset? IssueDate { get; set; }

        /// <summary>
        /// Serialization field for expiry date
        /// </summary>
        [XmlElement("expires"), JsonProperty("expires"), SerializationMetadata, EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        public String ExpiryDateXml
        {
            get
            {
                return this.ExpiryDate?.ToString("yyyy-MM-dd");
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    // Try to parse ISO date
                    if (DateTime.TryParseExact(value, new String[] { "o", "yyyy-MM-dd", "yyyy-MM", "yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                    {
                        this.ExpiryDate = dt;
                    }
                    else
                    {
                        throw new FormatException($"Cannot parse {value} as a date");
                    }
                }
                else
                {
                    this.ExpiryDate = null;
                }
            }
        }

        /// <inheritdoc/>
        public bool ShouldSerializeExpiryDateXml() => this.ExpiryDate.HasValue;

        /// <summary>
        /// Gets or sets the expiration date of the identifier
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets the check-digit if it is stored separate from the identifier
        /// </summary>
        [XmlElement("checkDigit"), JsonProperty("checkDigit")]
        public String CheckDigit { get; set; }

        /// <summary>
        /// Gets or sets the assinging authority id
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("domain"), JsonProperty("domain")]
        public Guid? IdentityDomainKey { get; set; }


        /// <summary>
        /// Authority - used for backwards compatibility only
        /// </summary>
        [XmlElement("authority"), JsonIgnore, SerializationMetadataAttribute]
        public IdentityDomain AuthorityCompatibilityDoNotUse
        {
            get => this.IdentityDomain;
            set => this.IdentityDomain = value;
        }

        /// <summary>
        /// True if the authority attribute should be serialized
        /// </summary>
        public bool ShouldSerializeAuthorityCompatibilityDoNotUse() => false;

        /// <summary>
        /// Gets or sets the type identifier
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("type"), JsonProperty("type")]
        public Guid? IdentifierTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the identifier type
        /// </summary>
        [SerializationReference(nameof(IdentifierTypeKey))]
        [XmlIgnore, JsonIgnore]
        public Concept IdentifierType { get; set; }

        /// <summary>
        /// Represents the authority information
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(IdentityDomainKey))]
        public IdentityDomain IdentityDomain { get; set; }

        /// <summary>
        /// Gets or sets the reliability of the identifier
        /// </summary>
        [XmlElement("reliability"), JsonProperty("reliability")]
        public IdentifierReliability Reliability { get; set; }

        /// <summary>
        /// Represents the authority information
        /// </summary>
        IdentityDomain IExternalIdentifier.IdentityDomain
        {
            get => this.LoadProperty(o => o.IdentityDomain);
            set => this.IdentityDomain = value;
        }


        /// <summary>
        /// True if the identifier is empty
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return String.IsNullOrEmpty(this.Value);
        }

        /// <summary>
        /// Returns true if the objects are equal
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as IdentifierBase<TBoundModel>;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) && this.Value == other.Value && this.IdentityDomainKey == other.IdentityDomainKey;
        }

        /// <summary>
        /// Represents this identifier as a string
        /// </summary>
        public override string ToString()
        {
            return $"{this.Value} [{this.IdentityDomain}]";
        }
    }
}