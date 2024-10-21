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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

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
        public static readonly Regex ExtendedFilterRegex = new Regex(@":\((\w*?)(\|((?:"".*(?:[^\\]""))|(?:.*?))\)|\))(.*)", RegexOptions.Compiled);

        /// <summary>
        /// Filter for parameter extraction
        /// </summary>
        public static readonly Regex ParameterExtractRegex = new Regex(@"((?:"".*?(?:[^\\]""))|(?:.*?)),(.*)", RegexOptions.Compiled);

        // The extension methods
        private static Dictionary<String, IQueryFilterExtension> s_extensionMethods = new Dictionary<string, IQueryFilterExtension>();

        /// <summary>
        /// Create filter extensions
        /// </summary>
        static QueryFilterExtensions()
        {
            InitializeFilters();
        }

        /// <summary>
        /// With control parameter is used as a wrapper for _ parameters
        /// </summary>
        public static object WithControl(this object me, string controlParameter, object value)
        {
            return true;
        }

        /// <summary>
        /// Initialize filters
        /// </summary>
        private static void InitializeFilters()
        {
            // Try to init extended filters
            foreach (var ext in AppDomain.CurrentDomain.GetAllTypes()
                    .Where(t => typeof(IQueryFilterExtension).IsAssignableFrom(t) && !t.IsAbstract)
                    .Select(t => Activator.CreateInstance(t) as IQueryFilterExtension))
            {
                QueryFilterExtensions.AddExtendedFilter(ext);
            }
        }

        /// <summary>
        /// Add an extended filter to the list
        /// </summary>
        /// <param name="extensionInfo">The extension method that should be added to the filter</param>
        public static void AddExtendedFilter(IQueryFilterExtension extensionInfo)
        {
            string methName = extensionInfo.Name;
            lock (s_extensionMethods)
            {
                if (!s_extensionMethods.ContainsKey(methName))
                {
                    s_extensionMethods.Add(methName, extensionInfo);
                }
            }
        }

        /// <summary>
        /// Get an extended filter name
        /// </summary>
        public static IQueryFilterExtension GetExtendedFilter(String name)
        {
            if (s_extensionMethods.Count == 0)
            {
                InitializeFilters();
            }

            IQueryFilterExtension retVal = null;
            s_extensionMethods.TryGetValue(name, out retVal);
            return retVal;
        }

        /// <summary>
        /// Get extended filters
        /// </summary>
        public static IEnumerable<IQueryFilterExtension> GetExtendedFilters()
        {
            return s_extensionMethods.Values;
        }

        /// <summary>
        /// Get extneded filter by the method is uses in LINQ
        /// </summary>
        internal static IQueryFilterExtension GetExtendedFilterByMethod(MethodInfo method)
        {
            return s_extensionMethods.Values.FirstOrDefault(o => o.ExtensionMethod.Name == method.Name && o.ExtensionMethod.GetParameters().Length == method.GetParameters().Length);
        }
    }
}