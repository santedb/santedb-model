/*
 * Copyright (C) 2021 - 2021, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-5
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
    /// of the source object at time of assoication carries meaning. A versioned association has
    /// an effective and obsolete version sequence indicator which allows callers to determine
    /// at which version of a particular object the relationship was active (or became active)</para>
    /// </remarks>
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("VersionedAssociation")]
    public abstract class VersionedAssociation<TSourceType> : Association<TSourceType>, IVersionedAssociation where TSourceType : VersionedEntityData<TSourceType>, new()
    {
        /// <summary>
        /// Gets or sets the version sequence of the source object when this assoication became active
        /// </summary>
        [XmlElement("effectiveVersionSequence"), JsonProperty("effectiveVersionSequence")]
        public Int64? EffectiveVersionSequenceId { get; set; }

        /// <summary>
        /// Gets or sets the sequence identifier of the source when this association is no longer active
        /// </summary>
        [XmlElement("obsoleteVersionSequence"), JsonProperty("obsoleteVersionSequence")]
        public Int64? ObsoleteVersionSequenceId { get; set; }

        /// <summary>
        /// Should serialize obsolete
        /// </summary>
        public bool ShouldSerializeObsoleteVersionSequenceId() => this.VersionSeqeuncesSpecified || this.ObsoleteVersionSequenceId.HasValue;

        /// <summary>
        /// Should serialize obsolete
        /// </summary>
        public bool ShouldSerializeEffectiveVersionSequenceId() => this.VersionSeqeuncesSpecified || this.EffectiveVersionSequenceId.HasValue;

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