using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// A reference to any identified data object
    /// </summary>
    /// <remarks>
    /// The identified data reference class is not persisted as an object per se, rather it
    /// serves as a link to a piece of data which already exists where the type may not
    /// be known or where the type is known but limited fields are required to reference the data
    /// </remarks>
    [XmlType("Reference", Namespace = "http://santedb.org/model"), JsonObject("Reference")]
    [XmlRoot("Reference", Namespace = "http://santedb.org/model")]
    public class IdentifiedDataReference : IdentifiedData
    {
        /// <summary>
        /// Get the type identifier
        /// </summary>
        [DataIgnore, XmlIgnore, JsonProperty("$type")]
        public override string Type { get => "Reference"; set { } }

        /// <summary>
        /// Modified on
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public override DateTimeOffset ModifiedOn => DateTimeOffset.MinValue;
    }
}