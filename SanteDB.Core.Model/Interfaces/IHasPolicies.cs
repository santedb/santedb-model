using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents an object that has security policies associated with it
    /// </summary>
    public interface IHasPolicies
    {

        /// <summary>
        /// Get the security policies associated with this object
        /// </summary>
        IEnumerable<SecurityPolicyInstance> Policies { get; }

    }
}
