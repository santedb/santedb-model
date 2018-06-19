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
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Updateable entity data which is not versioned
    /// </summary>
    [XmlType(nameof(NonVersionedEntityData), Namespace = "http://santedb.org/model")]
    [JsonObject(Id = nameof(NonVersionedEntityData))]
    public class NonVersionedEntityData : BaseEntityData
    {

        // The updated by id
        private Guid? m_updatedById;
        // The updated by user
        private SecurityUser m_updatedBy;

        /// <summary>
        /// Updated time
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public DateTimeOffset? UpdatedTime { get; set; }


        /// <summary>
        /// Gets or sets the creation time in XML format
        /// </summary>
        [XmlElement("updatedTime"), JsonProperty("updatedTime"), DataIgnore()]
        public String UpdatedTimeXml
        {
            get { return this.UpdatedTime?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                if (value != null)
                    this.UpdatedTime = DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture);
                else
                    this.UpdatedTime = null;
            }
        }

        /// <summary>
        /// Gets the time this item was modified
        /// </summary>
        public override DateTimeOffset ModifiedOn
        {
            get
            {
                return this.UpdatedTime ?? this.CreationTime;
            }
        }

        /// <summary>
        /// Gets or sets the user that updated this base data
        /// </summary>

        [XmlIgnore, JsonIgnore, DataIgnore()]
        public SecurityUser UpdatedBy
        {
            get
            {
                this.m_updatedBy = base.DelayLoad(this.m_updatedById, this.m_updatedBy);
                return m_updatedBy;
            }
            set
            {
                this.m_updatedBy = value;
                this.m_updatedById = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the created by identifier
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("updatedBy"), JsonProperty("updatedBy")]
        public Guid? UpdatedByKey
        {
            get { return this.m_updatedById; }
            set
            {
                if (this.m_updatedById != value)
                    this.m_updatedBy = null;
                this.m_updatedById = value;
            }
        }


        /// <summary>
        /// True if key should be serialized
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeUpdatedByKey()
        {
            return this.UpdatedByKey.HasValue;
        }

        /// <summary>
        /// Forces refresh
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.m_updatedBy = null;
        }
    }
}
