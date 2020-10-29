using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Indicates an object has a template
    /// </summary>
    public interface IHasTemplate
    {

        /// <summary>
        /// Get the tamplate for the object
        /// </summary>
        TemplateDefinition Template { get; set; }

    }
}
