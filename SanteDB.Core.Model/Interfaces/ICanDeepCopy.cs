using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// An interface which defines an object which can deep-copy itself
    /// </summary>
    public interface ICanDeepCopy 
    {

        /// <summary>
        /// Deep copy this object
        /// </summary>
        /// <returns>The deep-copy of the object</returns>
        ICanDeepCopy DeepCopy();
    }
}
