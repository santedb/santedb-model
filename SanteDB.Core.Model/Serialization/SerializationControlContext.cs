/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors
 * Portions Copyright (C) 2015-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2024-6-21
 */
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
