using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Batch operation type
    /// </summary>
    [XmlType(nameof(BatchOperationType), Namespace = "http://santedb.org/model")]
    public enum BatchOperationType
    {
        /// <summary>
        /// Automatically decide 
        /// </summary>
        Auto = 0,
        /// <summary>
        /// Insert the object only
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Insert the object or update it
        /// </summary>
        InsertOrUpdate = 2,
        /// <summary>
        /// Update the object only
        /// </summary>
        Update = 3,
        /// <summary>
        /// Delete the object
        /// </summary>
        Obsolete = 4

    }
}
