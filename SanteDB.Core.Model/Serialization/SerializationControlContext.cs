using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Serialization
{
    /// <summary>
    /// Represents a serialization control context which controls partial serialization
    /// </summary>
    public sealed class SerializationControlContext : IDisposable
    {

        // Current control context
        [ThreadStatic]
        private static SerializationControlContext m_current;

        /// <summary>
        /// Gets the current serialization control context
        /// </summary>
        public static SerializationControlContext Current => m_current;

        /// <summary>
        /// Enter the export context
        /// </summary>
        private SerializationControlContext(bool forExport)
        {
            this.IsForExport = forExport;
        }

        /// <summary>
        /// True if the context is for export
        /// </summary>
        public bool IsForExport { get; }

        /// <summary>
        /// Enter an export context
        /// </summary>
        public static SerializationControlContext EnterExportContext() => m_current = new SerializationControlContext(true);

        /// <summary>
        /// Is the current context for export?
        /// </summary>
        internal static bool IsCurrentContextForExport() => Current?.IsForExport == true;

        /// <summary>
        /// Dispose the context
        /// </summary>
        public void Dispose()
        {
            m_current = null;
        }
    }
}
