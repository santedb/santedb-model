/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: justi
 * Date: 2019-1-12
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Represents versioned based data
    /// </summary>
    /// <remarks>
    /// <para>In SanteDB, certain objects are not actually updated or deleted, rather they are subsequent versions
    /// of the data appended to the previous. This class represents the base class of all objects which are versioned objects</para></remarks>

    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("VersionedEntityData")]
    public abstract class VersionedEntityData<THistoryModelType> : BaseEntityData, IVersionedEntity where THistoryModelType : VersionedEntityData<THistoryModelType>, new()
    {

        // Previous version id
        private Guid? m_previousVersionId;
        // Previous version

        private THistoryModelType m_previousVersion;

        /// <summary>
        /// Creates a new versioned base data class
        /// </summary>
        public VersionedEntityData()
        {
        }

        /// <summary>
        /// Previous version
        /// </summary>
        IVersionedEntity IVersionedEntity.PreviousVersion
        {
            get
            {
                return this.GetPreviousVersion();
            }
        }

        /// <summary>
        /// Override the ETag
        /// </summary>
        public override string Tag
        {
            get
            {
                return this.VersionKey?.ToString("N");

            }
        }

        /// <summary>
        /// Gets or sets the UUID of the previous version of this record
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("previousVersion"), JsonProperty("previousVersion")]
        public virtual Guid? PreviousVersionKey
        {
            get
            {
                return this.m_previousVersionId;
            }
            set
            {
                this.m_previousVersionId = value;
                this.m_previousVersion = default(THistoryModelType);
            }
        }

        /// <summary>
        /// Should serialize previous version?
        /// </summary>
        public bool ShouldSerializePreviousVersionKey()
        {
            return this.m_previousVersionId.HasValue;
        }

        /// <summary>
        /// Gets the previous version or loads it from the database if needed
        /// </summary>
        //[SerializationReference(nameof(PreviousVersionKey))]
        public virtual THistoryModelType GetPreviousVersion()
        {
            if (this.m_previousVersionId.HasValue && 
                this.m_previousVersion == null)
                this.m_previousVersion = EntitySource.Current.Get<THistoryModelType>(this.Key, this.m_previousVersionId.Value);
            return this.m_previousVersion;
        }

        /// <summary>
        /// Sets the previous version
        /// </summary>
        public virtual void SetPreviousVersion(THistoryModelType previousVersion)
        {
            this.m_previousVersion = previousVersion;
            this.m_previousVersionId = previousVersion?.VersionKey;
        }

        /// <summary>
        /// Gets or sets the UUID of the current version of this object
        /// </summary>
        [XmlElement("version"), JsonProperty("version")]
        public Guid? VersionKey { get; set; }

        /// <summary>
        /// The sequence number of the version (for ordering)
        /// </summary>
        [XmlElement("sequence"), JsonProperty("sequence")]
        public Int32? VersionSequence { get; set; }

        /// <summary>
        /// Represent the versioned data as a string
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} (K:{1}, V:{2})", this.GetType().Name, this.Key, this.VersionKey);
        }

        /// <summary>
        /// Force bound attributes to reload
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.m_previousVersion = default(THistoryModelType);
        }
    }

}
