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
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents the base class for tags
    /// </summary>
    [Classifier(nameof(TagKey)), SimpleValue(nameof(Value))]
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("Tag")]
    public abstract class Tag<TSourceType> : BaseEntityData, ITag, ISimpleAssociation where TSourceType : IdentifiedData, new()
    {
        /// <summary>
        /// Default ctor for serialization
        /// </summary>
        public Tag()
        {
        }

        /// <summary>
        /// Creates a new tag
        /// </summary>
        public Tag(string key, string value)
        {
            this.TagKey = key;
            this.Value = value;
        }

        /// <summary>
        /// Get the source type
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Type SourceType => typeof(TSourceType);

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

        /// <summary>
        /// Gets or sets the source entity's key (where the relationship is FROM)
        /// </summary>
        [XmlElement("source"), JsonProperty("source")]
        public virtual Guid? SourceEntityKey { get; set; }

        /// <summary>
        /// The entity that this relationship targets
        /// </summary>
        [SerializationReference(nameof(SourceEntityKey))]
        [XmlIgnore, JsonIgnore, SerializationMetadata]
        public TSourceType SourceEntity { get; set; }

        /// <summary>
        /// Gets the source entity
        /// </summary>
        object ISimpleAssociation.SourceEntity { get => this.SourceEntity; set => this.SourceEntity = (TSourceType)value; }

        /// <summary>
        /// Semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Tag<TSourceType>;
            if (other == null)
            {
                return false;
            }

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

        /// <summary>
        /// Any tag that starts with . is not to be persisted
        /// </summary>
        public override bool IsEmpty()
        {
            return this.TagKey?.StartsWith(".") == true;
        }
    }


    /// <summary>
    /// Represents a tag associated with an concept
    /// </summary>

    [XmlType("ConceptTag", Namespace = "http://santedb.org/model"), JsonObject("ConceptTag")]
    public class ConceptTag : Tag<Entity>
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ConceptTag()
        {
        }

        /// <summary>
        /// Construtor setting key and tag
        /// </summary>
        public ConceptTag(String key, String value)
        {
            this.TagKey = key;
            this.Value = value;
        }
    }


    /// <summary>
    /// Represents a tag associated with an entity
    /// </summary>

    [XmlType("EntityTag", Namespace = "http://santedb.org/model"), JsonObject("EntityTag")]
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

    [XmlType("ActTag", Namespace = "http://santedb.org/model"), JsonObject("ActTag")]
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