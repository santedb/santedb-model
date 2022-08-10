/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-27
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a name for an entity
    /// </summary>
    /// <remarks>In SanteDB an entity name is a structured object which is made up of multiple
    /// components. This allows SanteDB to store complex names without having to copy multiple
    /// name components into a single field.</remarks>
    [Classifier(nameof(NameUse))]
    [XmlType("EntityName", Namespace = "http://santedb.org/model"), JsonObject("EntityName")]
    public class EntityName : VersionedAssociation<Entity>
    {
        
        // Name use concept
        /// <summary>
        /// Creates a new name
        /// </summary>
        public EntityName(Guid nameUse, String family, params String[] given)
        {
            this.NameUseKey = nameUse;
            this.Component = new List<EntityNameComponent>();

            if (!String.IsNullOrEmpty(family))
                this.Component.Add(new EntityNameComponent(NameComponentKeys.Family, family));
            foreach (var nm in given)
                if (!String.IsNullOrEmpty(nm))
                    this.Component.Add(new EntityNameComponent(NameComponentKeys.Given, nm));
        }

        /// <summary>
        /// Creates a new simple name
        /// </summary>
        /// <param name="nameUse"></param>
        /// <param name="name"></param>
        public EntityName(Guid nameUse, String name)
        {
            this.NameUseKey = nameUse;
            this.Component = new List<EntityNameComponent>()
            {
                new EntityNameComponent(name)
            };
        }

        /// <summary>
        /// Default ctor
        /// </summary>
        public EntityName()
        {
            
        }

        /// <summary>
        /// Gets or sets the individual component types
        /// </summary>
		[XmlElement("component"), JsonProperty("component")]
        public List<EntityNameComponent> Component { get; set; }

        /// <summary>
        /// Gets or sets the name use
        /// </summary>
        [SerializationReference(nameof(NameUseKey))]
        [XmlIgnore, JsonIgnore]
        public Concept NameUse { get; set; }

        /// <summary>
        /// Gets or sets the name use key
        /// </summary>
        [XmlElement("use"), JsonProperty("use")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Binding(typeof(NameUseKeys))]
        public Guid? NameUseKey { get; set; }

        /// <summary>
        /// True if empty
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return this.Component.IsNullOrEmpty() || this.Component?.All(c=>c.IsEmpty()) == true;
        }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as EntityName;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.NameUseKey == other.NameUseKey &&
                this.Component?.SemanticEquals(other.Component) == true;
        }

        /// <summary>
        /// Never need to serialize the entity source key
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSerializeSourceEntityKey()
        {
            return false;
        }

        /// <summary>
        /// Represent the name as a string
        /// </summary>
        public override string ToString()
        {
            if (this.LoadProperty(o=>o.Component).IsNullOrEmpty())
            {
                return "";
            }
            else if (this.Component.Count == 1)
            {
                return this.Component[0].Value;
            }
            else
            {
                return $"{this.Component.Find(o => o.ComponentTypeKey == NameComponentKeys.Given)?.Value} {this.Component.Find(o => o.ComponentTypeKey == NameComponentKeys.Family)?.Value}";
            }
        }

        /// <summary>
        /// Represent the entity name as a display string
        /// </summary>
        public override string ToDisplay()
        {
            return this.ToString();
        }
    }
}