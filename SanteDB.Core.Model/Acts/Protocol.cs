﻿/*
 * Based on OpenIZ, Copyright (C) 2015 - 2019 Mohawk College of Applied Arts and Technology
 * Copyright (C) 2019 - 2020, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2019-11-27
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents the model of a clinical protocol
    /// </summary>
    /// <remarks>
    /// <para>The protocol type is used to store and retrieve the particular definition of a clinical protocol. In 
    /// SanteDB, a clinical protocol represents a series of steps that *should* be taken by a clinician when caring for 
    /// a patient.</para>
    /// <para>
    /// A series of proposed steps generated by these protocol definitions are used to represent a care plan (<see cref="CarePlan"/>).
    /// </para>
    /// </remarks>
    [XmlType(nameof(Protocol), Namespace = "http://santedb.org/model"), JsonObject(nameof(Protocol))]
    [KeyLookup(nameof(Name))]
    public class Protocol : BaseEntityData
    {

        /// <summary>
        /// Gets or sets the name of the protocol
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the handler for this protocol (which can load the definition
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Type HandlerClass
        {
            get
            {
                return System.Type.GetType(this.HandlerClassName);
            }
            set
            {
                this.HandlerClassName = value.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// Gets or sets the handler class AQN
        /// </summary>
        [XmlElement("handlerClass"), JsonProperty("handlerClass")]
        public String HandlerClassName { get; set; }

        /// <summary>
        /// Contains instructions which the handler class can understand
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public byte[] Definition { get; set; }

        /// <summary>
        /// Gets or sets the OID
        /// </summary>
        [XmlElement("oid"), JsonProperty("oid")]
        public String Oid { get; set; }

        /// <summary>
        /// Semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Protocol;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.Name == other.Name;
        }
    }
}