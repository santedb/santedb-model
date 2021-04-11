using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Relationship target
    /// </summary>
    public interface IHasRelationships
    {

        /// <summary>
        /// Gets the relationshp
        /// </summary>
        IEnumerable<ITargetedAssociation> Relationships { get; }
    }
}
