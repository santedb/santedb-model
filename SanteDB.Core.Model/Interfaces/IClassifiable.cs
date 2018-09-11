using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents a classifiable entity
    /// </summary>
    public interface IClassifiable
    {

        /// <summary>
        /// Gets the class concept key
        /// </summary>
        Guid? ClassConceptKey { get; }
    }
}
