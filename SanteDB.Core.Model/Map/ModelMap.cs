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
using SanteDB.Core.Exceptions;
using SanteDB.Core.Model.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Map
{
    /// <summary>
    /// Model map format class
    /// </summary>
    [XmlRoot(Namespace = "http://santedb.org/model/map", ElementName = "modelMap")]
    [XmlType(nameof(ModelMap), Namespace = "http://santedb.org/model/map")]
    public class ModelMap
    {

        // Class cache
        private Dictionary<KeyValuePair<Type, Type>, ClassMap> m_classCache = new Dictionary<KeyValuePair<Type, Type>, ClassMap>();
        // Lock object
        private Object m_lockObject = new Object();
        private static XmlSerializer s_xsz = XmlModelSerializerFactory.Current.CreateSerializer(typeof(ModelMap));


        /// <summary>
        /// Creates the specified model mmap
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <returns>The constructed model map</returns>
        public static ModelMap Load(Stream sourceStream)
        {
            var retVal = s_xsz.Deserialize(sourceStream) as ModelMap;
            var validation = retVal.Validate();
            if (validation.Any(o => o.Level == ResultDetailType.Error))
            {
                throw new ModelMapValidationException(validation);
            }

            return retVal;
        }

        /// <summary>
        /// Gets or sets the class mapping
        /// </summary>
        [XmlElement("class")]
        public List<ClassMap> Class { get; set; }



        /// <summary>
        /// Get a class map for the specified type
        /// </summary>
        public ClassMap GetModelClassMap(Type type)
        {
            return this.GetModelClassMap(type, null);
        }

        /// <summary>
        /// Validate the map
        /// </summary>
        public IEnumerable<ValidationResultDetail> Validate()
        {
            foreach (var cls in this.Class)
            {
                foreach(var itm in cls.Validate())
                {
                    yield return itm;
                }

                // Add the parent map types
                var par = cls.ModelType.BaseType;
                cls.ParentMap = new List<ClassMap>();
                while(par != typeof(Object) && par != null)
                {
                    var map = this.Class.Find(o => o.ModelType == par);
                    if(map != null)
                    {
                        cls.ParentMap.Add(map);
                    }
                    par = par.BaseType;
                }
            }

        }

        /// <summary>
        /// Get the model class map between two types
        /// </summary>
        internal ClassMap GetModelClassMap(Type modelType, Type domainType)
        {
            KeyValuePair<Type, Type> key = new KeyValuePair<Type, Type>(modelType, domainType);
            if (!this.m_classCache.TryGetValue(key, out ClassMap retVal))
            {
                retVal = this.Class.Find(o => o.ModelType == modelType && o.DomainType == (domainType ?? o.DomainType));

                // Look up the object hierarchy
                if (retVal == null)
                {
                    while (modelType != typeof(Object) && retVal?.DomainType != (domainType ?? retVal?.DomainType))
                    {
                        modelType = modelType.BaseType;
                        retVal = this.Class.Find(o => o.ModelType == modelType && o.DomainType == domainType);
                    }
                }

                if (retVal != null)
                {
                    lock (this.m_lockObject)
                    {
                        if (!this.m_classCache.ContainsKey(key))
                        {
                            this.m_classCache.Add(key, retVal);
                        }
                    }
                }
            }

            return retVal;
        }
    }
}
