using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Identifier validator interface
    /// </summary>
    public interface IIdentifierValidator
    {

        /// <summary>
        /// Gets the algorithm name
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Validate the specified identifier
        /// </summary>
        /// <typeparam name="TEntity">The type of entity the object is bound to</typeparam>
        /// <param name="id">The identifier to be validated</param>
        /// <returns>True if the identifier is valid, false if it is not</returns>
        bool IsValid<TEntity>(IdentifierBase<TEntity> id) where TEntity : VersionedEntityData<TEntity>, new();
    }
}
