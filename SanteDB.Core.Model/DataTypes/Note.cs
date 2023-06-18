/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-5-19
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Entities;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Generic note class
    /// </summary>
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("Note")]
    public abstract class Note<TBoundModel> : VersionedAssociation<TBoundModel> where TBoundModel : VersionedEntityData<TBoundModel>, new()
    {

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
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("author"), JsonProperty("author")]
        public Guid? AuthorKey { get; set; }

        /// <summary>
        /// Gets or sets the author entity
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(AuthorKey))]
        public Entity Author { get; set; }

        /// <summary>
        /// Determine equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Note<TBoundModel>;
            if (other == null)
            {
                return false;
            }

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