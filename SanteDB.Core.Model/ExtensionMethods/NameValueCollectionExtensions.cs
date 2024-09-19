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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SanteDB
{
    /// <summary>
    /// Extension methods on <see cref="NameValueCollection"/>
    /// </summary>
    public static class NameValueCollectionExtensions
    {

        /// <summary>
        /// Try to get values for <paramref name="key"/> from <paramref name="me"/>
        /// </summary>
        /// <param name="me">The <see cref="NameValueCollection"/> from which the data should be retrieved</param>
        /// <param name="key">The key to retrieve</param>
        /// <param name="values">The values</param>
        /// <returns>True if the value was retrieved</returns>
        public static bool TryGetValue(this NameValueCollection me, String key, out String[] values)
        {
            values = me.GetValues(key);
            return values != null;
        }

        /// <summary>
        /// Convert the name value collection to a dictionary
        /// </summary>
        /// <param name="me">The <see cref="NameValueCollection"/> to convert</param>
        /// <returns>The converted dictionary</returns>
        public static IDictionary<String, String[]> ToDictionary(this NameValueCollection me) => me.ToDictionary(o => o, o => o);

        /// <summary>
        /// Convert the collection of key value pairs into a proper dictionary
        /// </summary>
        /// <param name="me">The dictionary to convert</param>
        /// <returns>The converted dictionary</returns>
        /// <remarks>In OpenIZ (SanteDB 1.0) there were mixed use of dictionaries, collections and <see cref="NameValueCollection"/> this is a helper method to correct 
        /// duplicate keys in some of these collections.</remarks>
        public static IDictionary<String, String[]> ToParameterDictionary(this IEnumerable<KeyValuePair<String, String[]>> me) => me.GroupBy(o => o.Key).ToDictionary(o => o.Key, o => o.SelectMany(v => v.Value).ToArray());

        /// <summary>
        /// Convert the name value collection to a dictionary
        /// </summary>
        /// <param name="me">The <see cref="NameValueCollection"/> to convert</param>
        /// <param name="keySelector">The transformer for the key</param>
        /// <param name="valueSelector">The transformer for the value</param>
        /// <returns>The converted dictionary</returns>
        public static IDictionary<String, TValue> ToDictionary<TValue>(this NameValueCollection me, Func<String, String> keySelector, Func<String[], TValue> valueSelector) => me.AllKeys.ToDictionary(o => keySelector(o), o => valueSelector(me.GetValues(o)));


        /// <summary>
        /// Creates a new name value collection from the kvp array
        /// </summary>
        public static NameValueCollection ToNameValueCollection(this IEnumerable<KeyValuePair<string, string>> kvpa)
        {
            var retVal = new NameValueCollection();
            foreach (var kv in kvpa)
            {
                retVal.Add(kv.Key, kv.Value?.ToString());
            }

            return retVal;
        }


        /// <summary>
        /// Creates a new name value collection from the kvp array
        /// </summary>
        public static NameValueCollection ToNameValueCollection(this IEnumerable<KeyValuePair<string, object>> kvpa)
        {
            var retVal = new NameValueCollection();
            foreach (var kv in kvpa)
            {
                if (kv.Value is IList le)
                {
                    foreach (var val in le)
                    {
                        retVal.Add(kv.Key, val.ToString());
                    }
                }
                else
                {
                    retVal.Add(kv.Key, kv.Value?.ToString());
                }
            }
            return retVal;
        }

        /// <summary>
        /// Parse a query string
        /// </summary>
        public static NameValueCollection ParseQueryString(this String me)
        {
            NameValueCollection retVal = new NameValueCollection();
            if (String.IsNullOrEmpty(me))
            {
                return retVal;
            }

            if (me.StartsWith("?"))
            {
                me = me.Substring(1);
            }

            // Escape regex
            var escapeRegex = new Regex("%([A-Fa-f0-9]{2})");

            foreach (var itm in me.Split('&'))
            {
                var expr = itm.Split('=');
                expr[0] = Uri.UnescapeDataString(expr[0]).Trim();

                if(expr.Length > 2)
                {
                    for(int i = 2; i < expr.Length; i++)
                    {
                        expr[1] += $"={expr[i]}";
                    }
                }
                expr[1] = Uri.UnescapeDataString(expr[1]).Trim();

                if (expr[0].EndsWith("[]")) // JQUERY Hack:
                {
                    expr[0] = expr[0].Substring(0, expr[0].Length - 2);
                }

                var value = escapeRegex.Replace(expr[1], (m) => System.Text.Encoding.UTF8.GetString(new byte[] { Convert.ToByte(m.Groups[1].Value, 16) }, 0, 1));
                // HACK: Replace this later
                if (!String.IsNullOrEmpty(value))
                {
                    retVal.Add(expr[0].Trim(), value);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Add the specified <paramref name="values"/> as <paramref name="name"/> to <paramref name="me"/>
        /// </summary>
        /// <param name="me">The <see cref="NameValueCollection"/> to add the values to</param>
        /// <param name="name">The name of the values</param>
        /// <param name="values">The values</param>
        public static void Add(this NameValueCollection me, String name, params String[] values)
        {
            if (values != null)
            {
                me.Add(name, values.AsEnumerable());
            }
        }


        /// <summary>
        /// Add the specified <paramref name="values"/> as <paramref name="name"/> to <paramref name="me"/>
        /// </summary>
        /// <param name="me">The <see cref="NameValueCollection"/> to add the values to</param>
        /// <param name="name">The name of the values</param>
        /// <param name="values">The values</param>
        public static void Add(this NameValueCollection me, String name, IEnumerable<String> values)
        {
            foreach (var v in values)
            {
                me.Add(name, v);
            }
        }

        /// <summary>
        /// Represent the <see cref="NameValueCollection"/> as a URI query string
        /// </summary>
        /// <param name="me">The name value collection to represent as a URI</param>
        /// <returns>The string in URI format</returns>
        public static String ToHttpString(this NameValueCollection me)
        {
            StringBuilder queryString = new StringBuilder();
            foreach (var kv in me.AllKeys)
            {
                foreach (var val in me.GetValues(kv))
                {
                    if (string.IsNullOrEmpty(val))
                    {
                        continue;
                    }

                    queryString.AppendFormat("{0}={1}&", kv, Uri.EscapeDataString(val));
                }
            }
            if (me.AllKeys.Any())
            {
                queryString.Remove(queryString.Length - 1, 1);
            }
            return queryString.ToString();
        }

        /// <summary>
        /// Convert query types
        /// </summary>
        public static List<KeyValuePair<String, String>> ToList(this System.Collections.Specialized.NameValueCollection nvc)
        {
            var retVal = new List<KeyValuePair<String, String>>();
            foreach (var k in nvc.AllKeys)
            {
                foreach (var v in nvc.GetValues(k) ?? new string[0])
                {
                    retVal.Add(new KeyValuePair<String, String>(k, v));
                }
            }

            return retVal;
        }

        /// <summary>
        /// Convert the name value collection to an array
        /// </summary>
        /// <param name="me">The <see cref="NameValueCollection"/> to be converted</param>
        /// <returns>The converted namevalue collection</returns>
        public static KeyValuePair<String, String>[] ToArray(this NameValueCollection me) => me.ToList().ToArray();
    }
}
