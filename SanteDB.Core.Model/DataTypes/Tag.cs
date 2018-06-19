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
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents the base class for tags
    /// </summary>
    [Classifier(nameof(TagKey)), SimpleValue(nameof(Value))]
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("Tag")]
    public abstract class Tag<TSourceType> :  BaseEntityData, ITag, ISimpleAssociation where TSourceType : IdentifiedData, new()
    {

        /// <summary>
        /// Gets or sets the key of the tag
        /// </summary>
        [XmlElement("key"), JsonProperty("key")]
        public String TagKey { get; set; }

        /// <summary>
        /// Gets or sets the value of the tag
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public String Value { get; set; }

        // Target entity key
        private Guid? m_sourceEntityKey;
        // The target entity

        private TSourceType m_sourceEntity;

        /// <summary>
        /// Gets or sets the source entity's key (where the relationship is FROM)
        /// </summary>
        [XmlElement("source"), JsonProperty("source")]
        public virtual Guid? SourceEntityKey
        {
            get
            {
                return this.m_sourceEntityKey;
            }
            set
            {
                if (this.m_sourceEntityKey != value)
                {
                    this.m_sourceEntityKey = value;
                    this.m_sourceEntity = null;
                }
            }
        }

        /// <summary>
        /// The entity that this relationship targets
        /// </summary>
        [SerializationReference(nameof(SourceEntityKey))]
        [XmlIgnore, JsonIgnore, DataIgnore]
        public TSourceType SourceEntity
        {
            get
            {
                this.m_sourceEntity = this.DelayLoad(this.m_sourceEntityKey, this.m_sourceEntity);
                return this.m_sourceEntity;
            }
            set
            {
                this.m_sourceEntity = value;
                this.m_sourceEntityKey = value?.Key;
            }
        }

        /// <summary>
        /// Semantic equality 
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Tag<TSourceType>;
            if (other == null) return false;
            return 
                other.TagKey == this.TagKey &&
                other.Value == this.Value;
        }

        /// <summary>
        /// Represent tag as key/value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} [{1}={2}]", base.ToString(), this.TagKey, this.Value);
        }
    }

    /// <summary>
    /// Represents a tag associated with an entity
    /// </summary>
    
    [XmlType("EntityTag",  Namespace = "http://santedb.org/model"), JsonObject("EntityTag")]
    public class EntityTag : Tag<Entity>
    {

        /// <summary>
        /// Default ctor
        /// </summary>
        public EntityTag()
        {

        }

        /// <summary>
        /// Construtor setting key and tag
        /// </summary>
        public EntityTag(String key, String value)
        {
            this.TagKey = key;
            this.Value = value;
        }
    }


    /// <summary>
    /// Represents a tag on an act
    /// </summary>
    
    [XmlType("ActTag",  Namespace = "http://santedb.org/model"), JsonObject("ActTag")]
    public class ActTag : Tag<Act>
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ActTag()
        {

        }

        /// <summary>
        /// Construtor setting key and tag
        /// </summary>
        public ActTag(String key, String value)
        {
            this.TagKey = key;
            this.Value = value;
        }
    }

}
