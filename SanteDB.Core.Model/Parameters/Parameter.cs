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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Collection;
using SanteDB.Core.Model.Roles;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Parameters
{
    /// <summary>
    /// REST service fault wrapper
    /// </summary>
    [XmlType(nameof(Parameter), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(Parameter))]
    public class Parameter
    {
        // Value of the object
        private object m_value;

        /// <summary>
        /// Serialization ctor
        /// </summary>
        public Parameter()
        {
        }

        /// <summary>
        /// Creates a new parameter with specified <paramref name="name"/> and <paramref name="value"/>
        /// </summary>
        /// <param name="name">The name of hte parameter</param>
        /// <param name="value">The value of the parameter</param>
        public Parameter(String name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the name of the operation
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter
        /// </summary>
        [JsonProperty("value"),
            XmlElement("int", typeof(int)),
            XmlElement("bool", typeof(bool)),
            XmlElement("array", typeof(string[])),
            XmlElement("date", typeof(DateTime)),
            XmlElement("string", typeof(string)),
            XmlElement("float", typeof(float)),
            XmlElement("uuid", typeof(Guid)),
            XmlElement("patient", typeof(Patient)),
            XmlElement("act", typeof(Act)),
            XmlElement("substanceAdministration", typeof(SubstanceAdministration)),
            XmlElement("quantityObservation", typeof(QuantityObservation)),
            XmlElement("codedObservation", typeof(CodedObservation)),
            XmlElement("textObservation", typeof(TextObservation)),
            XmlElement("procedure", typeof(Procedure)),
            XmlElement("encounter", typeof(PatientEncounter)),
            XmlElement("narrative", typeof(Narrative)),
            XmlElement("bundle", typeof(Bundle)),
            XmlElement("other", typeof(object))
        ]
        public Object Value
        {
            get => this.m_value;
            set
            {
                if (value is JToken jt)
                {
                    if (jt.Type == JTokenType.Array)
                    {
                        this.m_value = jt.Select(o => o.ToString()).ToArray();
                    }
                    else if (jt.Type == JTokenType.Boolean)
                    {
                        this.m_value = jt.Value<Boolean>();
                    }
                    else if (jt.Type == JTokenType.Date)
                    {
                        this.m_value = jt.Value<DateTime>();
                    }
                    else if (jt.Type == JTokenType.String)
                    {
                        this.m_value = jt.Value<String>();
                    }
                    else
                    {
                        this.m_value = jt;
                    }
                }
                else
                {
                    this.m_value = value;
                }
            }
        }
    }
}