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
 * Date: 2023-3-10
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Security;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Updateable entity data which is not versioned
    /// </summary>
    /// <remarks>
    /// <para>In SanteDB, objects which are non-versioned don't track changes over time, and therefore provide an updated time
    /// to allow the tracking of the last update to the object</para>
    /// <para>Because these objects are non-versioned, updates are destructive in that data is replaced in the database
    /// at a field level.</para></remarks>
    [XmlType(nameof(NonVersionedEntityData), Namespace = "http://santedb.org/model")]
    [JsonObject(Id = nameof(NonVersionedEntityData))]
    public class NonVersionedEntityData : BaseEntityData, INonVersionedData
    {

        /// <summary>
        /// Updated time
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the time that this object was last modified in ISO format
        /// </summary>
        [XmlElement("updatedTime"), JsonProperty("updatedTime"), SerializationMetadata()]
        public String UpdatedTimeXml
        {
            get { return this.UpdatedTime?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                    {
                        this.UpdatedTime = val;
                    }
                    else
                    {
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                    }
                }
                else
                {
                    this.UpdatedTime = null;
                }
            }
        }

        /// <summary>
        /// Gets the time this item was modified
        /// </summary>
        public override DateTimeOffset ModifiedOn => this.UpdatedTime ?? this.CreationTime;

        /// <summary>
        /// Gets or sets the user that updated this base data
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationMetadata(), SerializationReference(nameof(UpdatedByKey))]
        public SecurityProvenance UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the provenance identifier associated with the last update of this object
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("updatedBy"), JsonProperty("updatedBy")]
        public Guid? UpdatedByKey { get; set; }

        /// <summary>
        /// True if key should be serialized
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeUpdatedByKey() => this.UpdatedByKey.HasValue;

    }
}