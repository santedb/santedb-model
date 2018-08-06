using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Gets or sets the query filter extensions on the application context
    /// </summary>
    public static class QueryFilterExtensions
    {

        // The extension methods
        private static Dictionary<String, MethodInfo> s_extensionMethods = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Create filter extensions
        /// </summary>
        static QueryFilterExtensions()
        {
            AddExtendedFilter(typeof(QueryFilterExtensions).GetRuntimeMethod(nameof(First), new Type[] { typeof(String), typeof(int), typeof(String) }));
            AddExtendedFilter(typeof(QueryFilterExtensions).GetRuntimeMethod(nameof(Last), new Type[] { typeof(String), typeof(int), typeof(String) }));
        }

        /// <summary>
        /// Add an extended filter to the list
        /// </summary>
        /// <param name="method">The extension method that should be added to the filter</param>
        public static void AddExtendedFilter(MethodInfo method)
        {
            // Validate
            if (method.GetParameters().Count() == 0)
                throw new ArgumentOutOfRangeException("Extension method must have one parameter");

            string methName = method.Name.ToLower();

            lock (s_extensionMethods)
                if (!s_extensionMethods.ContainsKey(methName))
                    s_extensionMethods.Add(methName, method);

        }

        /// <summary>
        /// Get an extended filter name
        /// </summary>
        public static MethodInfo GetExtendedFilter(String name)
        {
            MethodInfo retVal = null;
            s_extensionMethods.TryGetValue(name, out retVal);
            return retVal;
        }

        /// <summary>
        /// Begins with 
        /// </summary>
        public static bool First(this String me, int length, String compare)
        {
            return me.Substring(length) == compare.Substring(length);
        }

        /// <summary>
        /// Ends with
        /// </summary>
        public static bool Last(this String me, int length, String compare)
        {
            return me.Substring(me.Length - length, length) == compare.Substring(compare.Length - length, length);
        }
    }
}
