using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Parameters
{
    /// <summary>
    /// REST service fault wrapper
    /// </summary>
    [XmlType(nameof(Parameter), Namespace = "http://santedb.org/operation")]
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
                        this.m_value = jt.ToString();
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