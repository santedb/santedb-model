/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-3-10
 */
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Parameters
{
    /// <summary>
    /// Gets the operation invokation
    /// </summary>
    [XmlType(nameof(ParameterCollection), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(ParameterCollection), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(ParameterCollection))]
    public class ParameterCollection
    {
        /// <summary>
        /// Parameter collection serialization ctor
        /// </summary>
        public ParameterCollection()
        {
        }

        /// <summary>
        /// Create parameter collection with specified parameters
        /// </summary>
        public ParameterCollection(params Parameter[] parameters)
        {
            this.Parameters = new List<Parameter>(parameters);
        }

        /// <summary>
        /// Gets or sets the parameters
        /// </summary>
        [XmlElement("parameter"), JsonProperty("parameter")]
        public List<Parameter> Parameters { get; set; }

        /// <summary>
        /// Try to get the specified <paramref name="parameterName"/> from the parameter list
        /// </summary>
        /// <typeparam name="TValue">The type of value</typeparam>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns></returns>
        public bool TryGet<TValue>(String parameterName, out TValue value)
        {
            var p = this.Parameters?.Find(o => o.Name == parameterName);
            // HACK: Sometimes the value is an LONG but the type is INT and this does not like that condition
            if(Map.MapUtil.TryConvert(p?.Value, typeof(TValue), out var tvalue))
            {
                value = (TValue)tvalue;
            }
            else
            {
                value = (TValue)(p?.Value ?? default(TValue));
            }

            return p != null;
        }

        /// <summary>
        /// Set the specified <paramref name="parameterName"/>
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set</param>
        /// <param name="value">The value of the parameter</param>
        public void Set(string parameterName, object value)
        {
            var existingParameter = this.Parameters.Find(o => o.Name == parameterName);
            if(existingParameter == null)
            {
                existingParameter = new Parameter(parameterName, value);
                this.Parameters.Add(existingParameter);
            }
            else
            {
                existingParameter.Value = value;
            }

        }
    }
}