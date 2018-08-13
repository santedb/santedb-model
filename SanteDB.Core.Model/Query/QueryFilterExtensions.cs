using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Gets or sets the query filter extensions on the application context
    /// </summary>
    public static class QueryFilterExtensions
    {
        /// <summary>
        /// Filter regular expression for query parameter syntax
        /// </summary>
        public static readonly Regex ExtendedFilterRegex = new Regex(@"^:\((\w*?)(\|(.*?)\)|\))(.*)");


        // The extension methods
        private static Dictionary<String, IQueryFilterExtension> s_extensionMethods = new Dictionary<string, IQueryFilterExtension>();

        /// <summary>
        /// Create filter extensions
        /// </summary>
        static QueryFilterExtensions()
        {
        }

        /// <summary>
        /// Add an extended filter to the list
        /// </summary>
        /// <param name="extensionInfo">The extension method that should be added to the filter</param>
        public static void AddExtendedFilter(IQueryFilterExtension extensionInfo)
        {
            string methName = extensionInfo.Name;
            lock (s_extensionMethods)
                if (!s_extensionMethods.ContainsKey(methName))
                    s_extensionMethods.Add(methName, extensionInfo);

        }

        /// <summary>
        /// Get an extended filter name
        /// </summary>
        public static IQueryFilterExtension GetExtendedFilter(String name)
        {
            IQueryFilterExtension retVal = null;
            s_extensionMethods.TryGetValue(name, out retVal);
            return retVal;
        }

        /// <summary>
        /// Get extneded filter by the method is uses in LINQ
        /// </summary>
        internal static IQueryFilterExtension GetExtendedFilterByMethod(MethodInfo method)
        {
            return s_extensionMethods.Values.FirstOrDefault(o => o.ExtensionMethod == method);
        }
    }
}
