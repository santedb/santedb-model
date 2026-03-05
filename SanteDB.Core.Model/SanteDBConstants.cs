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
using Newtonsoft.Json.Converters;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class SanteDBModelConstants
    {

        /// <summary>
        /// When set on an object, no dynamic loading via LoadProperty() will work on the object
        /// </summary>
        public const string NoDynamicLoadAnnotation = "No_Dyna_load";

        /// <summary>
        /// XHTML namespace
        /// </summary>
        public const string NS_XHTML = "http://www.w3.org/1999/xhtml";

        /// <summary>
        /// Class concept type map
        /// </summary>
        internal static readonly Dictionary<Guid, Type> CLASS_CONCEPT_TYPE_MAP;

        /// <summary>
        /// Model constants
        /// </summary>
        static SanteDBModelConstants()
        {
            CLASS_CONCEPT_TYPE_MAP = AppDomain.CurrentDomain.GetAllTypes()
                .Where(t => !t.IsGenericType && !t.IsAbstract && !t.IsInterface)
                .Where(t => t.HasCustomAttribute<ClassConceptKeyAttribute>())
                .SelectMany(t => t.GetCustomAttributes<ClassConceptKeyAttribute>().Select(k => new { Type = t, ClassKey = k.ClassConcept }))
                .ToDictionaryIgnoringDuplicates(k => Guid.Parse(k.ClassKey), t => t.Type);

            // Fill in missing generic types
            foreach(var fi in typeof(EntityClassKeys).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var ck = (Guid?)fi.GetValue(null);
                if(ck.HasValue && !CLASS_CONCEPT_TYPE_MAP.ContainsKey(ck.Value))
                {
                    CLASS_CONCEPT_TYPE_MAP.Add(ck.Value, typeof(Entity));
                }
            }

            // Fill in missing generic types
            foreach (var fi in typeof(ActClassKeys).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var ck = (Guid?)fi.GetValue(null);
                if (ck.HasValue && !CLASS_CONCEPT_TYPE_MAP.ContainsKey(ck.Value))
                {
                    CLASS_CONCEPT_TYPE_MAP.Add(ck.Value, typeof(Act));
                }
            }

        }
    }
}