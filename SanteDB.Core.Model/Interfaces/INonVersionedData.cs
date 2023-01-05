using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// <see cref="IBaseData"/> that is not versioned
    /// </summary>
    public interface INonVersionedData : IBaseData
    {
        /// <summary>
        /// Gets or sets the provenance of updating
        /// </summary>
        [QueryParameter("updatedBy")]
        Guid? UpdatedByKey { get; }

        /// <summary>
        /// Gets or sets the time of updating
        /// </summary>
        [QueryParameter("updatedTime")]
        DateTimeOffset? UpdatedTime { get; }
    }
}
