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
 * Date: 2022-5-30
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
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
    /// <para>In the SanteDB model, certain objects (like Concepts, Entities, and Acts) aren't ever updated or deleted. Rather, the
    /// updating or deletion of an object will result in a new version</para>
    /// <para>The <see cref="P:ObsoletionTime"/> property is used to indicate the <b>version</b> of the object is obsolete, rather
    /// than the object itself. This means that a series of these <see cref="VersionedEntityData{THistoryModelType}"/> compose a single logical
    /// instance of the object.</para>
    /// <para>The previous versions (representations of this object) can be retrieved using the <see cref="P:PreviousVersion"/> property</para>
    /// </remarks>
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("VersionedEntityData")]
    public abstract class VersionedEntityData<THistoryModelType> : BaseEntityData, IVersionedData where THistoryModelType : VersionedEntityData<THistoryModelType>, new()
    {
        // Previous version
        private THistoryModelType m_previousVersion;

        /// <summary>
        /// Creates a new versioned base data class
        /// </summary>
        public VersionedEntityData()
        {
        }

        /// <summary>
        /// Override the ETag
        /// </summary>
        public override string Tag => $"{this.Type}.{this.VersionKey?.ToString()}";

        /// <summary>
        /// Gets or sets the UUID of the previous version of this record
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("previousVersion"), JsonProperty("previousVersion")]
        public virtual Guid? PreviousVersionKey { get; set; }

        /// <summary>
        /// True if the object is the head version
        /// </summary>
        [XmlIgnore, JsonIgnore, QueryParameter("head")]
        public bool IsHeadVersion { get; set; }

        /// <summary>
        /// Should serialize previous version?
        /// </summary>
        public bool ShouldSerializePreviousVersionKey() => this.PreviousVersionKey.HasValue;

        /// <summary>
        /// Gets the previous version or loads it from the database if needed
        /// </summary>
        //[SerializationReference(nameof(PreviousVersionKey))]
        public virtual THistoryModelType GetPreviousVersion()
        {
            if (this.PreviousVersionKey.HasValue &&
                this.m_previousVersion == null)
            {
                this.m_previousVersion = EntitySource.Current.Get<THistoryModelType>(this.Key, this.PreviousVersionKey.Value);
            }

            return this.m_previousVersion;
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
        public Int64? VersionSequence { get; set; }

        /// <summary>
        /// Represent the versioned data as a string
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} (K:{1}, V:{2})", this.GetType().Name, this.Key, this.VersionKey);
        }
    }
}