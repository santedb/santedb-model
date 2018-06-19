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
 * User: fyfej
 * Date: 2017-9-1
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.EntityLoader;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a phonetic algorithm record in the model
    /// </summary>
    
    [XmlType("PhoneticAlgorithm",  Namespace = "http://santedb.org/model"), JsonObject("PhoneticAlgorithm")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "PhoneticAlgorithm")]
    public class PhoneticAlgorithm : NonVersionedEntityData
    {
        // Phonetic algorithm
        private static PhoneticAlgorithm s_nullPhoneticAlgorithm = null;
        private static object s_lockObject = new object();

        /// <summary>
        /// Gets the phonetic algorithm which is the "empty" algorithm
        /// </summary>
        public static PhoneticAlgorithm EmptyAlgorithm
        {
            get
            {
                if(s_nullPhoneticAlgorithm == null)
                    lock(s_lockObject)
                        s_nullPhoneticAlgorithm = EntitySource.Current.Get<PhoneticAlgorithm>(Guid.Parse("402CD339-D0E4-46CE-8FC2-12A4B0E17226"));
                return s_nullPhoneticAlgorithm;
            }
        }

        /// <summary>
        /// Gets the name of the phonetic algorithm
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }
        /// <summary>
        /// Gets the handler (or generator) for the phonetic algorithm
        /// </summary>
        [XmlElement("handler"), JsonProperty("handler")]
        public String Handler { get; set; }

    }
}