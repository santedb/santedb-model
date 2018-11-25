using SanteDB.Core.Model.DataTypes;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents data that is tagged with a geographic location
    /// </summary>
    public interface IGeoTagged
    {
        /// <summary>
        /// Geographic tag
        /// </summary>
        GeoTag GeoTag { get; set; }

    }
}
