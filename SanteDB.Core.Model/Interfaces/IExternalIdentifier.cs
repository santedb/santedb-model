using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Describes a class that is an external identifier
    /// </summary>
    public interface IExternalIdentifier
    {

        /// <summary>
        /// Gets the authority
        /// </summary>
        AssigningAuthority Authority { get; }

        /// <summary>
        /// Gets the value of the identity 
        /// </summary>
        String Value { get; }
    }
}
