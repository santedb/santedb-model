using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents an association where the caller can traverse between the source and target
    /// </summary>
    public interface ITargetedAssociation : ISimpleAssociation
    {

        /// <summary>
        /// The target (where the association points)
        /// </summary>
        Guid? TargetEntityKey { get; set; }

        /// <summary>
        /// The target entity object
        /// </summary>
        object TargetEntity { get; set; }
    }
}
