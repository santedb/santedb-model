using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class SanteDBConstants
    {
        /// <summary>
        /// Alternate keys tag for loading alternate objects from the database
        /// </summary>
        public const string AlternateKeysTag = "$alt.keys";

        /// <summary>
        /// When set on an object, no dynamic loading via LoadProperty() will work on the object
        /// </summary>
        public const string NoDynamicLoadAnnotation = "No_Dyna_load";
    }
}