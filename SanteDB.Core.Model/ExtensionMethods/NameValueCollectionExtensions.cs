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
        public static IDictionary<String, String[]> ToDictionary(this NameValueCollection me) => me.AllKeys.ToDictionary(o => o, o => me.GetValues(o));

        /// <summary>
        /// Creates a new name value collection from the kvp array
        /// </summary>
        public static NameValueCollection ToNameValueCollection(this IEnumerable<KeyValuePair<string, object>> kvpa)
        {
            var retVal = new NameValueCollection();
            foreach (var kv in kvpa)
                if (kv.Value is IList li)
                {
                    foreach (var v in li)
                    {
                        retVal.Add(kv.Key, v.ToString());
                    }
                }
                else
                    retVal.Add(kv.Key, kv.Value?.ToString());
            return retVal;
        }

        /// <summary>
        /// Parse a query string
        /// </summary>
        public static NameValueCollection ParseQueryString(this String me)
        {
            NameValueCollection retVal = new NameValueCollection();
            if (String.IsNullOrEmpty(me)) return retVal;
            if (me.StartsWith("?")) me = me.Substring(1);

            // Escape regex
            var escapeRegex = new Regex("%([A-Fa-f0-9]{2})");

            foreach (var itm in me.Split('&'))
            {
                var expr = itm.Split('=');
                expr[0] = Uri.UnescapeDataString(expr[0]).Trim();
                expr[1] = Uri.UnescapeDataString(expr[1]).Trim();

                if (expr[0].EndsWith("[]")) // JQUERY Hack:
                    expr[0] = expr[0].Substring(0, expr[0].Length - 2);
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
        public static void Add(this NameValueCollection me, String name, params String[] values) => me.Add(name, values.AsEnumerable());


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
                    if (string.IsNullOrEmpty(val)) continue;
                    queryString.AppendFormat("{0}={1}&", kv, Uri.EscapeDataString(val));
                }
            }
            queryString.Remove(queryString.Length - 1, 1);
            return queryString.ToString();
        }
    }
}
