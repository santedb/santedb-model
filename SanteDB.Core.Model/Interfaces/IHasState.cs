using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents an entity that has state
    /// </summary>
    public interface IHasState
    {

        /// <summary>
        /// Gets or sets the status concept
        /// </summary>
        Guid? StatusConceptKey { get; set; }
    }
}
