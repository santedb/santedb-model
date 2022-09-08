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
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Security;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Represents the root of all model classes in the SanteDB Core
    /// </summary>
    /// <remarks>
    /// <para>This abstract class is used to encapsulate the key properties of base data elements in the SanteDB
    /// model, namely it keeps track of which entities created and obsoleted a particular resource and when those
    /// events occurred.</para>
    /// </remarks>
    [XmlType("BaseEntityData", Namespace = "http://santedb.org/model"), JsonObject("BaseEntityData")]
    public abstract class BaseEntityData : IdentifiedData, IBaseData
    {

        /// <summary>
        /// Constructs a new base entity data
        /// </summary>
        public BaseEntityData()
        {
        }

        /// <summary>
        /// Gets or sets the time at which the data was created
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CreationTimeXml))]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time as an ISO date format
        /// </summary>
        [XmlElement("creationTime"), JsonProperty("creationTime"), SerializationMetadata()]
        public String CreationTimeXml
        {
            get
            {
                if (this.CreationTime == default(DateTimeOffset))
                    return null;
                else
                    return this.CreationTime.ToString("o", CultureInfo.InvariantCulture);
            }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.CreationTime = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else this.CreationTime = default(DateTimeOffset);
            }
        }

        /// <summary>
        /// Gets or sets the time when the data is or will become invalid
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(ObsoletionTimeXml))]
        public DateTimeOffset? ObsoletionTime { get; set; }

        /// <summary>
        /// Gets or sets the time that the data is no longer valid (was deleted/obsoleted) in ISO format
        /// </summary>
        [XmlElement("obsoletionTime"), JsonProperty("obsoletionTime"), SerializationMetadata()]
        public String ObsoletionTimeXml
        {
            get { return this.ObsoletionTime?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.ObsoletionTime = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else this.ObsoletionTime = null;
            }
        }

        /// <summary>
        /// Gets or sets the user that created this base data
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CreatedByKey)), SerializationMetadata]
        public virtual SecurityProvenance CreatedBy { get; set; }

        /// <summary>
        /// Gets the time that the object was last modified (from base data, default to CreationTime)
        /// </summary>
        [JsonIgnore, XmlIgnore]

        public override DateTimeOffset ModifiedOn => this.CreationTime;

        /// <summary>
        /// True if key should be serialized
        /// </summary>
        public bool ShouldSerializeCreatedByKey() => this.CreatedByKey.HasValue;

        /// <summary>
        /// True if key should be serialized
        /// </summary>
        public bool ShouldSerializeObsoletedByKey()=>this.ObsoletedByKey.HasValue;

        /// <summary>
        /// Gets or sets the user that obsoleted this base data
        /// </summary>
        [SerializationReference(nameof(ObsoletedByKey)), SerializationMetadata()]
        [XmlIgnore, JsonIgnore]
        public virtual SecurityProvenance ObsoletedBy { get; set; }

        /// <summary>
        /// Gets or sets the security provenance object which represents the creation of this object
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("createdBy"), JsonProperty("createdBy")]
        public virtual Guid? CreatedByKey { get; set; }

        /// <summary>
        /// Gets or sets the security provenance object which represents the obsoletion of this data
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("obsoletedBy"), JsonProperty("obsoletedBy")]
        public virtual Guid? ObsoletedByKey { get; set; }

        /// <summary>
        /// Represent the data as a string
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} (K:{1})", this.GetType().Name, this.Key);
        }
    }
}