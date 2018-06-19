﻿/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
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
 * Date: 2017-9-1
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Collection;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Roles;
using SanteDB.Core.Model.Security;
using System;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Patch
{

    /// <summary>
    /// Represents a single patch operation
    /// </summary>
    [XmlType(nameof(PatchOperation), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(PatchOperation), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(PatchOperation))]
    [XmlInclude(typeof(String))]
    [XmlInclude(typeof(DateTimeOffset))]
    [XmlInclude(typeof(Guid))]
    [XmlInclude(typeof(Int32))]
    [XmlInclude(typeof(byte[]))]
    [XmlInclude(typeof(Concept))]
    [XmlInclude(typeof(EntityIdentifier))]
    [XmlInclude(typeof(ActIdentifier))]
    [XmlInclude(typeof(ActTag))]
    [XmlInclude(typeof(EntityTag))]
    [XmlInclude(typeof(EntityExtension))]
    [XmlInclude(typeof(ActExtension))]
    [XmlInclude(typeof(EntityName))]
    [XmlInclude(typeof(EntityAddress))]
    [XmlInclude(typeof(EntityTelecomAddress))]
    [XmlInclude(typeof(DateTime))]
    [XmlInclude(typeof(EntityRelationship))]
    [XmlInclude(typeof(ActParticipation))]
    [XmlInclude(typeof(ActRelationship))]
    [XmlInclude(typeof(PersonLanguageCommunication))]
    [XmlInclude(typeof(TemplateDefinition))]
    [XmlInclude(typeof(ActProtocol))]
    public class PatchOperation
    {

        /// <summary>
        /// Patch operation default ctor
        /// </summary>
        public PatchOperation()
        {

        }

        /// <summary>
        /// Patch operation
        /// </summary>
        public PatchOperation(PatchOperationType operation, String path, Object value)
        {
            this.OperationType = operation;
            this.Path = path;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the operation type
        /// </summary>
        [XmlAttribute("op"), JsonProperty("op")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PatchOperationType OperationType { get; set; }

        /// <summary>
        /// Gets or sets the path
        /// </summary>
        [XmlAttribute("path"), JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// Get or sets the value
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public Object Value { get; set; }

        /// <summary>
        /// To string representation
        /// </summary>
        public override string ToString()
        {
            switch (this.OperationType)
            {
                case PatchOperationType.Add:
                    return $"++++ {this.Path}:{this.Value}";
                case PatchOperationType.Remove:
                    return $"---- {this.Path} : {this.Value}";
                case PatchOperationType.Replace:
                    return $"==== {this.Path} : {this.Value}";
                case PatchOperationType.Test:
                    return $"???? {this.Path} : {this.Value}";
                default:
                    return base.ToString();
            }
        }
    }
}