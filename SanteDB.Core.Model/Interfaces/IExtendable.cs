/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-27
 */
using System;
using System.Collections.Generic;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents a object that can be extended with IModelExtensions
    /// </summary>
    public interface IExtendable : IIdentifiedData
    {

        /// <summary>
        /// Gets the list of extensions
        /// </summary>
        IEnumerable<IModelExtension> Extensions { get; }

        /// <summary>
        /// Remove an extension from the extension object
        /// </summary>
        void RemoveExtension(Guid extensionType);

        /// <summary>
        /// Add an extension from the extension object
        /// </summary>
        /// <param name="extensionType">The type of extension to add</param>
        /// <param name="handlerType">The handler to serialize the <paramref name="value"/></param>
        /// <param name="value">The value of the extension to set</param>
        void AddExtension(Guid extensionType, Type handlerType, object value);
    }
}
