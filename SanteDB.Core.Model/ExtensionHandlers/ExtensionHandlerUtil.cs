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
