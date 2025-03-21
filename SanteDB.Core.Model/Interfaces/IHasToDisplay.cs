using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents a <see cref="Exception"/> or any object that produces a human friendly message about its status
    /// </summary>
    public interface IHasToDisplay
    {

        /// <summary>
        /// Get the message for rendering in the user interface
        /// </summary>
        String ToDisplay();
    }
}
