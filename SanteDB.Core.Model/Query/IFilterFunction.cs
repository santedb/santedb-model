using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Represents a query filter function
    /// </summary>
    public interface IFilterFunction
    {

        /// <summary>
        /// The name of the filter function
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the extension method which should be appended onto LINQ queries (and from which LINQ queries should be added)
        /// </summary>
        MethodInfo ExtensionMethod { get; }

    }
}
