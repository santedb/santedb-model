using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Implementers declare they can be identified by a key, have a tag and support getting details on the modified date.
    /// </summary>
    public interface IIdentifiedResource
    {
        /// <summary>
        /// Gets or sets the resource key for this resource.
        /// </summary>
        Guid? Key { get; set;  }

        /// <summary>
        /// Gets the tag for this resource. The tag is used to calculate whether a resource has changed or not.
        /// </summary>
        string Tag { get; }

        /// <summary>
        /// Gets the last modified timestamp for this resource.
        /// </summary>
        DateTimeOffset ModifiedOn { get; }
    }
}
