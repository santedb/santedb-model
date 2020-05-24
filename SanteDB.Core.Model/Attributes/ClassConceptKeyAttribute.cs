using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Maps a class concept key to this type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ClassConceptKeyAttribute : Attribute
    {

        /// <summary>
        /// Gets or set teh class concept key
        /// </summary>
        public String ClassConcept { get; set; }

        /// <summary>
        /// Creates a new class concept key attribute
        /// </summary>
        public ClassConceptKeyAttribute(String classConceptGuid)
        {
            this.ClassConcept = classConceptGuid;
        }
    }
}
