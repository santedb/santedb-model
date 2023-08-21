using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Attributes
{

    /// <summary>
    /// Classifications of resources
    /// </summary>
    public enum ResourceSensitivityClassification
    {
        /// <summary>
        /// Resource is PHI
        /// </summary>
        PersonalHealthInformation,
        /// <summary>
        /// Resource contains adminstrative data
        /// </summary>
        Administrative,
        /// <summary>
        /// Resource is metadata
        /// </summary>
        Metadata
    }

    /// <summary>
    /// Tags a resource's class as either:
    ///     * Clinical
    ///     * Administrative 
    ///     * Metadata
    ///     
    /// This allows other generic processes to understand the resource's sensitivity and to take appropriate 
    /// auditing and/or privacy decisions (such as export, etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourceSensitivityAttribute : Attribute
    {

        /// <summary>
        /// Create a new sensitivity attribute
        /// </summary>
        public ResourceSensitivityAttribute(ResourceSensitivityClassification classificationType)
        {
            this.Classification = classificationType;
        }

        /// <summary>
        /// Gets the classification of this reosurce
        /// </summary>
        public ResourceSensitivityClassification Classification { get; }
    }
}
