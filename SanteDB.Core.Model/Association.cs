/*
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
using SanteDB.Core.Model.Interfaces;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Represents a base class for association between two objects
    /// </summary>
    /// <remarks></remarks>
    /// <typeparam name="TSourceType">The source type which indicates the type of object being associated from</typeparam>
    /// <remarks>
    /// <para>In SanteDB's data store, complex objects are associated with one another using this class. For example, when associating identifiers with an 
    /// <see cref="SanteDB.Core.Model.Acts.Act"/> with an <see cref="SanteDB.Core.Model.DataTypes.ActIdentifier"/> the ActIdentifier is an association</para>
    /// <para>This version of the assoication is used between non-versioned objects or for versioned objects where the version has no bearing on the association</para>
    /// </remarks>
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("Association")]
    public abstract class Association<TSourceType> : IdentifiedData, ISimpleAssociation where TSourceType : IdentifiedData, new()
    {

        // Target entity key
        private Guid? m_sourceEntityKey;
        // The target entity
        private TSourceType m_sourceEntity;

        /// <summary>
        /// Get the modification date
        /// </summary>
        public override DateTimeOffset ModifiedOn
        {
            get
            {
                if (this.m_sourceEntity != null)
                    return this.m_sourceEntity.ModifiedOn;
                return DateTime.Now;
            }
        }

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
                if (value != this.m_sourceEntity?.Key || value != this.m_sourceEntityKey)
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
        /// Source entity 
        /// </summary>
        object ISimpleAssociation.SourceEntity { get => this.SourceEntity; set => this.SourceEntity = (TSourceType)value; }


        /// <summary>
        /// Should serialize obsolete
        /// </summary>
        public virtual bool ShouldSerializeSourceEntityKey()
        {
            return this.m_sourceEntityKey.HasValue;
        }


        /// <summary>
        /// Force delay load properties to reload
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.m_sourceEntity = null;
        }

        /// <summary>
        /// Determines equality of this association
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Association<TSourceType>;
            if (other == null) return false;
            return base.SemanticEquals(obj);
        }
    }
}
