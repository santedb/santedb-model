/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: Justin Fyfe
 * Date: 2019-8-8
 */
using Newtonsoft.Json;
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

        private static Dictionary<String, Type> s_typeCache = new Dictionary<string, Type>();
        private static object s_lock = new object();
        private Type m_hintType;


        /// <summary>
        /// Gets all registered types
        /// </summary>
        public static IEnumerable<Type> GetRegisteredTypes() => s_typeCache.Values;

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
        /// Register the model type
        /// </summary>
        public static void RegisterModelType(Type type)
        {
            var typeName = type.GetTypeInfo().GetCustomAttribute<JsonObjectAttribute>(false)?.Id ?? type.Name;
            if (!s_typeCache.ContainsKey(typeName))
                lock (s_lock)
                    s_typeCache.Add(typeName, type);
            //else
            //    throw new ArgumentException($"Type {typeName} is already registered");
        }


        /// <summary>
        /// Register the model type
        /// </summary>
        public static void RegisterModelType(String typeName, Type type)
        {
            if (!s_typeCache.ContainsKey(typeName))
                lock (s_lock)
                    s_typeCache.Add(typeName, type);
            //else
            //    throw new ArgumentException($"Type {typeName} is already registered");
        }

        /// <summary>
        /// Bind the type to a name
        /// </summary>
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            // Attempt to see if the type was registered
            typeName = s_typeCache.FirstOrDefault(o => o.Value == serializedType).Key;
            if (typeName == null)
                typeName = serializedType.GetTypeInfo().GetCustomAttribute<JsonObjectAttribute>(false)?.Id ?? serializedType.Name;
            assemblyName = null;

        }

        /// <summary>
        /// Bind to type
        /// </summary>
        public Type BindToType(string assemblyName, string typeName)
        {
            // Assembly to search
            Assembly asm = typeof(ModelSerializationBinder).GetTypeInfo().Assembly;
            if (!String.IsNullOrEmpty(assemblyName))
                asm = Assembly.Load(new AssemblyName(assemblyName));
            else if (this.m_hintType != null) // use hint type
            {
                asm = m_hintType.GetTypeInfo().Assembly;
                if (m_hintType.GetTypeInfo().GetCustomAttribute<JsonObjectAttribute>()?.Id == typeName)
                    return this.m_hintType;
            }

            // The type
            Type type = null;
            if (!s_typeCache.TryGetValue(typeName, out type))
                lock (s_lock)
                {
                    type = asm.ExportedTypes.SingleOrDefault(
                        t => t.GetTypeInfo().GetCustomAttribute<JsonObjectAttribute>(false)?.Id == typeName
                        );
                    if (!s_typeCache.ContainsKey(typeName) && type != null)
                        s_typeCache.Add(typeName, type);
                }
            if (type == null)
                type = asm.GetType(typeName);
            return type ?? null;
        }
    }
}
