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
using SanteDB.Core.Model.Interfaces;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Represents a relational class which is bound on a version boundary
    /// </summary>
    /// <remarks>
    /// <para>This association is used to link two complex objects to one another when the version 
    /// of the source object at time of assoication carries meaning.</para>
    /// </remarks>

    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("VersionedAssociation")]
    public abstract class VersionedAssociation<TSourceType> : Association<TSourceType>, IVersionedAssociation where TSourceType : VersionedEntityData<TSourceType>, new()
    {

        // The identifier of the version where this data is effective
        private Int32? m_effectiveVersionSequenceId;
        // The identifier of the version where this data is no longer effective
        private Int32? m_obsoleteVersionSequenceId;
        // The version where this data is effective

        /// <summary>
        /// Gets or sets the version sequence of the source object when this assoication became active
        /// </summary>
        [XmlElement("effectiveVersionSequence"), JsonProperty("effectiveVersionSequence")]
        public Int32? EffectiveVersionSequenceId
        {
            get { return this.m_effectiveVersionSequenceId; }
            set
            {
                this.m_effectiveVersionSequenceId = value;
            }
        }

        /// <summary>
        /// Gets or sets the sequence identifier of the source when this association is no longer active
        /// </summary>
        [XmlElement("obsoleteVersionSequence"), JsonProperty("obsoleteVersionSequence")]
        public Int32? ObsoleteVersionSequenceId
        {
            get { return this.m_obsoleteVersionSequenceId; }
            set
            {
                this.m_obsoleteVersionSequenceId = value;
            }
        }

        /// <summary>
        /// When true, instructs that the sequences be serialized
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool VersionSeqeuncesSpecified { get; set; }

        /// <summary>
        /// Should serialize obsolete
        /// </summary>
        public bool ShouldSerializeObsoleteVersionSequenceId()
        {
            return this.VersionSeqeuncesSpecified || this.m_obsoleteVersionSequenceId.HasValue;
        }

        /// <summary>
        /// Should serialize obsolete
        /// </summary>
        public bool ShouldSerializeEffectiveVersionSequenceId()
        {
            return this.VersionSeqeuncesSpecified || this.m_effectiveVersionSequenceId.HasValue &&
                this.m_obsoleteVersionSequenceId.HasValue;
        }


        /// <summary>
        /// Determines equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as VersionedAssociation<TSourceType>;
            if (other == null) return false;
            return base.SemanticEquals(obj);
        }
    }
}
