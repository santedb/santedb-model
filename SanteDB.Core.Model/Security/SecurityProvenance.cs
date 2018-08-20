using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Represents security provenance information
    /// </summary>
    [XmlType(nameof(SecurityProvenance), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(SecurityProvenance), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(SecurityProvenance))]
    public class SecurityProvenance : IdentifiedData
    {

        // User
        private SecurityUser m_user;

        /// <summary>
        /// Gets the time that the provenance was modified / created
        /// </summary>
        public override DateTimeOffset ModifiedOn => this.CreationTime;

        /// <summary>
        /// Gets or sets the time at which the data was created
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(CreationTimeXml))]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time in XML format
        /// </summary>
        [XmlElement("creationTime"), JsonProperty("creationTime"), DataIgnore()]
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
                if (value != null)
                    this.CreationTime = DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture);
                else this.CreationTime = default(DateTimeOffset);
            }
        }

        /// <summary>
        /// Gets or sets the application key
        /// </summary>
        [XmlElement("application"), JsonProperty("application")]
        public Guid? ApplicationKey { get; set; }

        /// <summary>
        /// Gets or sets the user key
        /// </summary>
        [XmlElement("user"), JsonProperty("user")]
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Gets or sets the device key 
        /// </summary>
        [XmlElement("device"), JsonProperty("device")]
        public Guid? DeviceKey { get; set; }

        /// <summary>
        /// Gets or sets the session
        /// </summary>
        [XmlElement("session"), JsonProperty("session")]
        public Guid? SessionKey { get; set; }

        /// <summary>
        /// Gets the security user for the provenance if applicable
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(UserKey))]
        public SecurityUser User {
            get
            {
                this.m_user = base.DelayLoad(this.UserKey, this.m_user);
                return this.m_user;
            }
            set
            {
                this.m_user = value;
                this.UserKey = value?.Key;
            }
        }
    }
}
