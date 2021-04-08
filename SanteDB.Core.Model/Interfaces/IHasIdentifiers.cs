using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Entity has external identifiers
    /// </summary>
    public interface IHasIdentifiers
    {

        /// <summary>
        /// Get the xternal identifiers
        /// </summary>
        IEnumerable<IExternalIdentifier> Identifiers { get; }
    }
}
