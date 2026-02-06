using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Hack - This allows a resource to provide an API name for the resource
    /// </summary>
    public class ResourceNameAttribute : Attribute
    {

        /// <summary>
        /// Create a new resource name attribute
        /// </summary>
        public ResourceNameAttribute(string resourceName)
        {
            this.ResourceName = resourceName;
        }
        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        public string ResourceName { get; set; }
    }
}
