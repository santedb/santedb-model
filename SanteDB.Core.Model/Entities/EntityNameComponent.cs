/*
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
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a name component which is bound to a name
    /// </summary>
    [XmlType(Namespace = "http://santedb.org/model", TypeName = "EntityNameComponent")]
    [JsonObject("EntityNameComponent")]
    public class EntityNameComponent : GenericComponentValues<EntityName>
    {
       
        /// <summary>
        /// Entity name component
        /// </summary>
        public EntityNameComponent()
        {
        }

        /// <summary>
        /// Creates the entity name component with the specified value
        /// </summary>
        /// <param name="value"></param>
        public EntityNameComponent(String value) : base(value)
        {
        }

        /// <summary>
        /// Creates the entity name component with the specified value and part type classifier
        /// </summary>
        /// <param name="partTypeKey"></param>
        /// <param name="value"></param>
        public EntityNameComponent(Guid partTypeKey, String value) : base(partTypeKey, value)
        {
        }

        /// <summary>
        /// Gets or sets the component type key
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("type"), JsonProperty("type")]
        [Binding(typeof(NameComponentKeys))]
        public override Guid? ComponentTypeKey
        {
            get
            {
                return base.ComponentTypeKey;
            }
            set
            {
                base.ComponentTypeKey = value;
            }
        }

        /// <summary>
        /// Value of the name
        /// </summary>
        public override string ToString()
        {
            return this.Value;
        }
    }
}