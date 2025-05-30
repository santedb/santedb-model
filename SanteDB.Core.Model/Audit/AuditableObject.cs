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

/*
* Copyright 2012-2013 Mohawk College of Applied Arts and Technology
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
* Date: 23-8-2012
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Audit
{
    /// <summary>
    /// Identifies an object that adds context to the audit
    /// </summary>
	/// <remarks>
	/// <para>In the SanteDB audit structure, an auditable object represets an object that was actioned on (read, disclosed, updated, etc.) 
	/// or represents an audit that provides context to the audit event (query performed, name of transaction, etc.)</para>
	/// </remarks>
    [XmlType(nameof(AuditableObject), Namespace = "http://santedb.org/audit")]
    [JsonObject(nameof(AuditableObject))]
    public class AuditableObject
    {
        /// <summary>
        /// New object data
        /// </summary>
        public AuditableObject()
        {
            this.ObjectData = new List<ObjectDataExtension>();

        }

        /// <summary>
        /// Custom id type code
        /// </summary>
        [XmlElement("customCode"), JsonProperty("customCode")]
        public AuditCode CustomIdTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the id type code
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public AuditableObjectIdType? IDTypeCode { get; set; }

        /// <summary>
        /// Identifies the type of identifier supplied
        /// </summary>
        [XmlElement("idType"), JsonProperty("idType")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public AuditableObjectIdType IDTypeCodeXml
        { get { return this.IDTypeCode.GetValueOrDefault(); } set { this.IDTypeCode = value; } }

        /// <summary>
        /// Gets whether ID type code is specified
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool IDTypeCodeXmlSpecified
        { get { return this.IDTypeCode.HasValue; } }

        /// <summary>
        /// Lifecycle type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public AuditableObjectLifecycle? LifecycleType { get; set; }

        /// <summary>
        /// Identifies where in the lifecycle of the object this object is currently within
        /// </summary>
        [XmlElement("lifecycle"), JsonProperty("lifecycle")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public AuditableObjectLifecycle LifecycleTypeXml
        { get { return this.LifecycleType.GetValueOrDefault(); } set { this.LifecycleType = value; } }

        /// <summary>
        /// Gets whether ID type code is specified
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool LifecycleTypeXmlSpecified
        { get { return this.LifecycleType.HasValue; } }

        /// <summary>
        /// Data associated with the object
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public string NameData { get; set; }

        /// <summary>
        /// Additional object data
        /// </summary>
        [XmlArray("data"), XmlArrayItem("d"), JsonProperty("data")]
        public List<ObjectDataExtension> ObjectData { get; set; }

        /// <summary>
        /// Identifies the object in the event
        /// </summary>
        [XmlElement("id"), JsonProperty("id")]
        public string ObjectId { get; set; }

        /// <summary>
        /// Data associated with the object
        /// </summary>
        [XmlElement("queryData"), JsonProperty("queryData")]
        public string QueryData { get; set; }

        /// <summary>
        /// Identifies the role type of the object
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public AuditableObjectRole? Role { get; set; }

        /// <summary>
        /// Identifies the role type of the object
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("role"), JsonProperty("role")]
        public AuditableObjectRole RoleXml
        { get { return this.Role.GetValueOrDefault(); } set { this.Role = value; } }

        /// <summary>
        /// Gets whether ID type code is specified
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool RoleXmlSpecified
        { get { return this.Role.HasValue; } }

        /// <summary>
        /// Identifies the type of object being expressed
        /// </summary>
        [XmlElement("type"), JsonProperty("type")]
        public AuditableObjectType Type { get; set; }
    }

    /// <summary>
    /// Represents object data extension
    /// </summary>
    /// <remarks>This allows auditors to affix any additional data to an audit which the default structure does not support</remarks>
    [XmlType(nameof(ObjectDataExtension), Namespace = "http://santedb.org/audit")]
    public class ObjectDataExtension
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ObjectDataExtension()
        {
        }

        /// <summary>
        /// Object data extension
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ObjectDataExtension(String key, byte[] value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
		/// Object data extension
		/// </summary>
		public ObjectDataExtension(String key, string value) : this(key, null == value ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(value))
        {
        }

        /// <summary>
        /// Key of the extension
        /// </summary>
        [XmlAttribute("key"), JsonProperty("key")]
        public String Key { get; set; }

        /// <summary>
        /// Value of the extension
        /// </summary>
        [XmlAttribute("value"), JsonProperty("value")]
        public Byte[] Value { get; set; }
    }
}