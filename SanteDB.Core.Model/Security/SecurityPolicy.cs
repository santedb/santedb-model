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
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Policy grant type
    /// </summary>
    public enum PolicyGrantType
    {
        /// <summary>
        /// Represents a policy grant type of deny.
        /// </summary>
        Deny = 0,

        /// <summary>
        /// Represnts a policy grant type of elevate.
        /// </summary>
        Elevate = 1,

        /// <summary>
        /// Represents a policy grant type of grant.
        /// </summary>
        Grant = 2
    }

    /// <summary>
    /// Represents a simply security policy
    /// </summary>
    [XmlType("SecurityPolicy", Namespace = "http://santedb.org/model"), JsonObject("SecurityPolicy")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityPolicy")]
    [KeyLookup(nameof(Oid)), SimpleValue(nameof(Oid))]
    public class SecurityPolicy : BaseEntityData
    {
        /// <summary>
        /// Create new security policy
        /// </summary>
        public SecurityPolicy()
        {
        }

        /// <summary>
        /// Create a new security policy
        /// </summary>
        public SecurityPolicy(String name, String oid, bool isPublic, bool canOverride)
        {
            this.Name = name;
            this.Oid = oid;
            this.IsPublic = IsPublic;
            this.CanOverride = canOverride;
        }

        /// <summary>
        /// Gets or sets the handler which may handle this policy
        /// </summary>
        [XmlElement("handler"), JsonProperty("handler")]
        public String Handler { get; set; }

        /// <summary>
        /// Gets or sets the name of the policy
        /// </summary>
        [XmlElement("name"), JsonProperty("name"), NoCase]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the universal ID
        /// </summary>
        [XmlElement("oid"), JsonProperty("oid")]
        public String Oid { get; set; }

        /// <summary>
        /// Whether the policy is public
        /// </summary>
        [XmlElement("isPublic"), JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Whether the policy can be elevated over
        /// </summary>
        [XmlElement("canOverride"), JsonProperty("canOverride")]
        public bool CanOverride { get; set; }

        /// <summary>
        /// Get the name of the object as a display string
        /// </summary>
        public override string ToDisplay() => $"{this.Name} [{this.Oid}]";
    }

    /// <summary>
    /// Represents a security policy instance
    /// </summary>
    [XmlType(nameof(SecurityPolicyInstance), Namespace = "http://santedb.org/model"), JsonObject("SecurityPolicyInstance")]
    public class SecurityPolicyInstance : Association<SecurityEntity>
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public SecurityPolicyInstance()
        {
        }

        /// <summary>
        /// Creates a new policy instance with the specified policy and grant
        /// </summary>
        public SecurityPolicyInstance(SecurityPolicy policy, PolicyGrantType grantType)
        {
            this.Policy = policy;
            this.PolicyKey = policy.Key;
            this.GrantType = grantType;
        }

        /// <summary>
        /// Gets or sets the policy key
        /// </summary>
        [XmlElement("policy"), JsonProperty("policy")]
        public Guid? PolicyKey { get; set; }

        /// <summary>
        /// The policy
        /// </summary>
        [JsonIgnore, XmlIgnore, SerializationReference(nameof(PolicyKey))]
        public SecurityPolicy Policy { get; set; }

        /// <summary>
        /// Gets or sets whether the policy is a Deny
        /// </summary>
        [XmlElement("grant"), JsonProperty("grant")]
        public PolicyGrantType GrantType { get; set; }
    }
}