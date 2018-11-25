using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a simple geographic tagging attribute
    /// </summary>
    [XmlType(nameof(GeoTag), Namespace = "http://santedb.org/model"), JsonObject(nameof(GeoTag))]
    public class GeoTag
    {

        /// <summary>
        /// Creates a new geo-tag
        /// </summary>
        public GeoTag()
        {

        }

        /// <summary>
        /// Creates a new geo tag
        /// </summary>
        public GeoTag(double lat, double lng, bool? precise)
        {
            this.Lat = lat;
            this.Lng = lng;
            this.Precise = precise;
        }

        /// <summary>
        /// Gets the latitude
        /// </summary>
        [XmlElement("lat"), JsonProperty("lat")]
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the object
        /// </summary>
        [XmlElement("lng"), JsonProperty("lng")]
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the accuracy of the geo-tag
        /// </summary>
        [XmlElement("precise"), JsonProperty("precise")]
        public bool? Precise { get; set; }

    }
}
