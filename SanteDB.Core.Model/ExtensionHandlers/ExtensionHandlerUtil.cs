/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-6-21
 */
using SanteDB.Core.Interfaces;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Concurrent;

namespace SanteDB.Core.Model.ExtensionHandlers
{
    /// <summary>
    /// Extension handler utility
    /// </summary>
    internal static class ExtensionHandlerUtil
    {
        // Extension handlers
        private static ConcurrentDictionary<Guid, IExtensionHandler> s_extensionHandlers = new ConcurrentDictionary<Guid, IExtensionHandler>();

        /// <summary>
        /// Get the extension handler in a cached manner
        /// </summary>
        public static IExtensionHandler GetExtensionHandler<TBoundModel>(this Extension<TBoundModel> me) where TBoundModel : VersionedEntityData<TBoundModel>, new()
        {

            if (!s_extensionHandlers.TryGetValue(me.ExtensionTypeKey.GetValueOrDefault(), out var extensionHandler))
            {
                // Loading from DB = slow
                var extensionType = me.LoadProperty(o => o.ExtensionType);
                extensionHandler = extensionType?.ExtensionHandlerInstance;
                s_extensionHandlers.TryAdd(me.ExtensionTypeKey.GetValueOrDefault(), extensionHandler);
            }
            return extensionHandler;
        }

    }
}
