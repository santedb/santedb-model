﻿/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using SanteDB.Core.i18n;
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace SanteDB.Core.Model.Map
{
    /// <summary>
    /// Map utility
    /// </summary>
    public static class MapUtil
    {

        /// <summary>
        /// Maps from wire format to real format
        /// Key - string in the format {FROM}>{TO}
        /// Value - MethodInfo of the method that will perform the operation to convert
        /// </summary>
        private static Dictionary<string, MethodInfo> s_wireMaps = new Dictionary<string, MethodInfo>();

        // Class key maps
        private static Dictionary<Guid, Type> s_classKeyMaps;

        // Non convertable
        private static HashSet<String> m_nonConvertable = new HashSet<String>();

        // Conversion maps
        private static Dictionary<String, MethodInfo> m_converterMaps = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// Get the model type from the classification key
        /// </summary>
        public static Type GetModelTypeFromClassKey(Guid classKey)
        {
            if (s_classKeyMaps == null)
            {
                s_classKeyMaps = new Dictionary<Guid, Type>();

                var types = typeof(MapUtil).Assembly.GetTypes()
                    .Where(t => t != null && !t.IsAbstract)
                    .Select(t => (type: t, direct: t.GetCustomAttributes<ClassConceptKeyAttribute>(inherit: false), inherited: t.GetCustomAttributes<ClassConceptKeyAttribute>(inherit: true)))
                    .Where(tup => tup.direct?.Any() == true || tup.inherited?.Any() == true);

                //Direct assignments take priority since they are expressed by the model.
                foreach (var t in types)
                {
                    foreach (var clsCode in t.direct)
                    {
                        var key = Guid.Parse(clsCode.ClassConcept);

                        //Two direct assignments of the same class key is not permitted. Throw an exception showing which two types have the same direct assignment and need to be fixed.
                        if (s_classKeyMaps.ContainsKey(key))
                        {
                            throw new ArgumentException($"Model exception: Two model types contain the same class concept in the ClassConceptKey attribute: '{t.type.FullName}' and '{s_classKeyMaps[key].FullName}'.");
                        }
                        else
                        {
                            s_classKeyMaps.Add(key, t.type);
                        }
                    }
                }

                //Inherited assignments account for the first instance
                foreach (var t in types)
                {
                    foreach (var clsCode in t.inherited)
                    {
                        var key = Guid.Parse(clsCode.ClassConcept);

                        if (!s_classKeyMaps.ContainsKey(key))
                            s_classKeyMaps.Add(key, t.type);
                    }
                }
            }

            s_classKeyMaps.TryGetValue(classKey, out Type retVal);
            return retVal;
        }

        /// <summary>
        /// Register a map
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destType"></param>
        /// <param name="method"></param>
        public static void RegisterMap(Type sourceType, Type destType, MethodInfo method)
        {
            lock (s_wireMaps)
            {
                s_wireMaps.Add(String.Format("{0}>{1}", sourceType, destType), method);
            }
        }

        /// <summary>
        /// Returns true if the maps contains 
        /// </summary>
        public static bool HasMap(Type sourceType, Type destType)
        {
            if (sourceType == null || destType == null)
            {
                return false;
            }

            return s_wireMaps.ContainsKey(String.Format("{0}>{1}", sourceType, destType));
        }

        /// <summary>
        /// Find the converter for the types specified
        /// </summary>
        /// <param name="scanType">The type to scan in</param>
        /// <param name="sourceType">The source type</param>
        /// <param name="destType">The destination type</param>
        /// <returns></returns>
        internal static MethodInfo FindConverter(Type scanType, Type sourceType, Type destType)
        {
            var key = $"{sourceType.FullName}>{destType.FullName}";
            MethodInfo retVal = null;
            if (!m_converterMaps.TryGetValue(key, out retVal))
            {
                var rtm = scanType.GetRuntimeMethods();
                foreach (MethodInfo mi in rtm)
                {
                    if (mi.GetParameters().Length == 2 &&
                                       (mi.ReturnType.IsSubclassOf(destType) || destType == mi.ReturnType) &&
                                       mi.GetParameters()[0].ParameterType.FullName == sourceType.FullName &&
                                       mi.GetParameters()[1].ParameterType.FullName == typeof(IFormatProvider).FullName)
                    {
                        retVal = mi;
                    }
                    else if (mi.GetParameters().Length == 1 &&
                                            (mi.ReturnType.IsSubclassOf(destType) || destType == mi.ReturnType) &&
                                            mi.GetParameters()[0].ParameterType.FullName == sourceType.FullName && retVal == null)
                    {
                        retVal = mi;
                    }
                }

                if (retVal != null)
                {
                    lock (m_converterMaps)
                    {
                        if (!m_converterMaps.ContainsKey(key))
                        {
                            m_converterMaps.Add(key, retVal);
                        }
                    }
                }
            }

            return retVal;

        }

        /// <summary>
        /// Convert the object <paramref name="value"/> to type <typeparamref name="T"/>
        /// </summary>
        public static T Convert<T>(object value)
        {
            if (TryConvert(value, typeof(T), out var retVal))
            {
                return (T)retVal;
            }
            else
            {
                throw new InvalidOperationException(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE);
            }
        }
        /// <summary>
        /// Attempt casting <paramref name="value"/> to <paramref name="destType"/> placing the result 
        /// in <paramref name="result"/>
        /// </summary>
        public static bool TryConvert(object value, Type destType, out object result)
        {

            // The type represents a wrapper for an enumeration
            Type m_destType = destType;

            if (value == null)
            {
                result = null;
                return m_destType.IsNullable();
            }

            String convertKey = $"{value.GetType().FullName}>{destType.FullName}";

            if (m_nonConvertable.Contains(convertKey))
            {
                result = null;
                return false;
            }
            else if (m_destType.IsGenericType && !value.GetType().IsEnum)
            {
                m_destType = m_destType.GenericTypeArguments[0];
            }

            // Is there a cast?
            if ("null".Equals(value?.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                result = null;
                return m_destType.IsNullable();
            }
            else if (destType.IsAssignableFrom(value.GetType())) //  (m_destType.IsAssignableFrom(value.GetType())) // Same type
            {
                result = value;
                return true;
            }
            else if ((m_destType == typeof(int) || m_destType == typeof(long)) && value.GetType().IsEnum)
            {
                result = (int)value;
                return true;
            }
            else if (m_destType.IsEnum && (value is int || value is long))
            {
                result = Enum.ToObject(m_destType, value);
                return true;
            }
            else if (m_destType.IsEnum && value.GetType() == typeof(String)) // No map exists yet
            {
                try
                {
                    result = Enum.Parse(m_destType, value.ToString(), true);
                    return true;
                }
                catch
                {
                    result = null;
                    return false;
                }

            }
            else if (destType.FullName.StartsWith("System.Nullable"))
            {
                destType = m_destType; // Transparency for nullable types
            }

            // Is there a built in method that can convert this
            MethodInfo mi;
            string converterKey = string.Format(CultureInfo.InvariantCulture, "{0}>{1}", value.GetType().FullName, destType.FullName);
            if (!s_wireMaps.TryGetValue(converterKey, out mi))
            {
                // Try to find a map first...
                // Using an operator overload
                mi = FindConverter(typeof(SanteDBConvert), value.GetType(), destType);
                if (mi == null)
                {
                    mi = FindConverter(m_destType, value.GetType(), destType);
                }

                if (mi == null)
                {
                    mi = FindConverter(value.GetType(), value.GetType(), destType);
                }

                if (mi == null && m_destType != destType) // Using container type
                {
                    mi = FindConverter(destType, value.GetType(), destType);
                }

                if (mi == null) // Using System.Xml.XmlConvert 
                {
                    mi = FindConverter(typeof(System.Xml.XmlConvert), value.GetType(), destType);
                }

                if (mi == null) // Using System.Convert as a last resort
                {
                    mi = FindConverter(typeof(System.Convert), value.GetType(), destType);
                }

                if (mi != null)
                {
                    lock (s_wireMaps)
                    {
                        if (!s_wireMaps.ContainsKey(converterKey))
                        {
                            s_wireMaps.Add(converterKey, mi);
                        }
                    }
                }
                else
                {
                    // Last ditch effort to parse
                    // Compare apples to apples
                    // We have two generic types, however the dest type generic doesn't match the 
                    // value type generic. This is common with type overrides where the object container
                    // doesn't match the value. For example, attempting to assign a CV<String> to a CS<ResponseMode>
                    if (value.GetType().IsGenericType &&
                        destType.IsGenericType &&
                        destType.GenericTypeArguments[0] != value.GetType().GenericTypeArguments[0] &&
                        destType.GetGenericTypeDefinition() != value.GetType().GetGenericTypeDefinition())
                    {
                        Type valueCastType = value.GetType().GetGenericTypeDefinition().MakeGenericType(
                            destType.GenericTypeArguments
                        );
                        try
                        {
                            var retVal = TryConvert(value, valueCastType, out result);
                            if (!retVal)
                            {
                                lock (m_nonConvertable)
                                {
                                    m_nonConvertable.Add(convertKey);
                                }
                            }

                            return retVal;
                        }
                        catch
                        {
                            result = null;
                            return false;

                        }
                    }
                    else
                    {
                        lock (m_nonConvertable)
                        {
                            m_nonConvertable.Add(convertKey);
                        }

                        result = null;
                        return false;
                    }

                }
            }


            try
            {
                if (mi.GetParameters().Length == 2)
                {
                    result = mi.Invoke(null, new object[] { value, CultureInfo.InvariantCulture }); // Invoke the conversion method;
                }
                else
                {
                    result = mi.Invoke(null, new object[] { value }); // Invoke the conversion method
                }

                var retVal = result != null;
                if (!retVal) // non convertable
                {
                    lock (m_nonConvertable)
                    {
                        m_nonConvertable.Add(convertKey);
                    }
                }

                return retVal;
            }
            catch { result = null; return false; }
        }

    }
}
