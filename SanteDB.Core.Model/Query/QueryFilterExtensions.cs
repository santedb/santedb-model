﻿/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
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
 * User: justin
 * Date: 2018-8-8
 */
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
