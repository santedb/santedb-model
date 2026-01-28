/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SanteDB.Core.Model.Serialization
{
    /// <summary>
    /// Model binding
    /// </summary>
    public class ModelSerializationBinder : ISerializationBinder
    {
        private static readonly object s_lock = new object();

        private static readonly Dictionary<string, Type> s_typeCache = new Dictionary<string, Type>();
        private readonly Type m_hintType;

        /// <summary>
        /// Create a new model serialization binder
        /// </summary>
        public ModelSerializationBinder()
        {
        }

        /// <summary>
        /// Model serialization binding with hint
        /// </summary>
        /// <param name="hintType"></param>
        public ModelSerializationBinder(Type hintType)
        {
            this.m_hintType = hintType;
        }

        /// <summary>
        /// Bind the type to a name
        /// </summary>
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            // Attempt to see if the type was registered
            typeName = s_typeCache.FirstOrDefault(o => o.Value == serializedType).Key;
            if (typeName == null)
            {
                typeName = serializedType.GetSerializationName() ?? serializedType.Name;
            }

            assemblyName = null;
        }

        /// <summary>
        /// Bind to type
        /// </summary>
        public Type BindToType(string assemblyName, string typeName)
        {
            // Assembly to search
            var asm = typeof(ModelSerializationBinder).Assembly;
            if (!string.IsNullOrEmpty(assemblyName))
            {
                asm = Assembly.Load(new AssemblyName(assemblyName));
            }
            else if (this.m_hintType != null) // use hint type
            {
                asm = this.m_hintType.Assembly;
                if (this.m_hintType.GetSerializationName() == typeName)
                {
                    return this.m_hintType;
                }
            }

            // Is the type an array 
            var isArray = typeName.StartsWith("ArrayOf");
            typeName = isArray ? typeName.Substring(7) : typeName;

            // The type
            Type type = null;
            if (!s_typeCache.TryGetValue(typeName, out type))
            {
                lock (s_lock)
                {
                    type = typeof(ModelSerializationBinder).Assembly.GetExportedTypesSafe().Union(asm.GetExportedTypesSafe()).FirstOrDefault(
                        t => t.GetSerializationName() == typeName
                    );
                    if (type == null) // deep look
                    {
                        type = AppDomain.CurrentDomain.GetAllTypes().FirstOrDefault(o => o.GetSerializationName() == typeName);
                    }
                    if (!s_typeCache.ContainsKey(typeName) && type != null)
                    {
                        s_typeCache.Add(typeName, type);
                    }
                }
            }

            if (type == null)
            {
                type = asm.GetType(typeName);
            }

            return isArray ? type?.MakeArrayType() : type ?? null;
        }

        /// <summary>
        /// Gets all registered types
        /// </summary>
        public static IEnumerable<Type> GetRegisteredTypes()
        {
            return s_typeCache.Values;
        }

        /// <summary>
        /// Register the model type
        /// </summary>
        public static void RegisterModelType(Type type)
        {
            var typeName = type.GetSerializationName() ?? type.Name;
            if (!s_typeCache.ContainsKey(typeName))
            {
                lock (s_lock)
                {
                    s_typeCache.Add(typeName, type);
                }
            }

            //else
            //    throw new ArgumentException($"Type {typeName} is already registered");
        }

        /// <summary>
        /// Register the model type
        /// </summary>
        public static void RegisterModelType(string typeName, Type type)
        {
            if (!s_typeCache.ContainsKey(typeName))
            {
                lock (s_lock)
                {
                    s_typeCache.Add(typeName, type);
                }
            }

            //else
            //    throw new ArgumentException($"Type {typeName} is already registered");
        }
    }
}