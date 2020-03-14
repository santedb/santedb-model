using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Represents a security challenge
    /// </summary>
    [XmlType("SecurityChallenge", Namespace = "http://santedb.org/model"), JsonObject("SecurityChallenge")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityChallenge")]
    public class SecurityChallenge : NonVersionedEntityData
    {

        /// <summary>
        /// The text for the security challenge
        /// </summary>
        [XmlElement("text"), JsonProperty("text")]
        public String ChallengeText { get; set; }

    }
}
