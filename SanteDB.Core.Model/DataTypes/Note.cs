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
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Generic note class
    /// </summary>
    [SimpleValue(nameof(Text))]
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("Note")]
    public abstract class Note<TBoundModel> : VersionedAssociation<TBoundModel> where TBoundModel : VersionedEntityData<TBoundModel>, new()
    {

        // Author id
        private Guid? m_authorKey;
        // Author entity

        private Entity m_author;

        /// <summary>
        /// Default ctor
        /// </summary>
        public Note()
        {

        }

        /// <summary>
        /// Creates a new instance of the entity note
        /// </summary>
        public Note(Guid authorKey, String text)
        {
            this.AuthorKey = authorKey;
            this.Text = text;
        }

        /// <summary>
        /// Gets or sets the note text
        /// </summary>
        [XmlElement("text"), JsonProperty("text")]
        public String Text { get; set; }

        /// <summary>
        /// Gets or sets the author key
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]

        [XmlElement("author"), JsonProperty("author")]
        public Guid? AuthorKey
        {
            get { return this.m_authorKey; }
            set
            {
                if (this.m_authorKey != value)
                {
                    this.m_authorKey = value;
                    this.m_author = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the author entity
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(AuthorKey))]
        public Entity Author
        {
            get
            {
                this.m_author = base.DelayLoad(this.m_authorKey, this.m_author);
                return this.m_author;
            }
            set
            {
                this.m_author = value;
                this.m_authorKey = value?.Key;
            }
        }

        /// <summary>
        /// Forces a refresh of the object
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.m_author = null;
        }

        /// <summary>
        /// Determine equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Note<TBoundModel>;
            if (other == null) return false;
            return base.SemanticEquals(obj) && this.AuthorKey == other.AuthorKey &&
                this.Text == other.Text;
        }

    }

    /// <summary>
    /// Represents a note attached to an entity
    /// </summary>
    [XmlType("EntityNote", Namespace = "http://santedb.org/model"), JsonObject("EntityNote")]
    public class EntityNote : Note<Entity>
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public EntityNote()
        {

        }

        /// <summary>
        /// Creates a new instance of the entity note
        /// </summary>
        public EntityNote(Guid authorKey, String text) : base(authorKey, text)
        {
        }

    }

    /// <summary>
    /// Represents a note attached to an entity
    /// </summary>
    [XmlType("ActNote", Namespace = "http://santedb.org/model"), JsonObject("ActNote")]
    public class ActNote : Note<Acts.Act>
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ActNote()
        {

        }

        /// <summary>
        /// Creates a new instance of the entity note
        /// </summary>
        public ActNote(Guid authorKey, String text) : base(authorKey, text)
        {
        }
    }

}
