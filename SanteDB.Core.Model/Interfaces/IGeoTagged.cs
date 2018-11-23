using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
