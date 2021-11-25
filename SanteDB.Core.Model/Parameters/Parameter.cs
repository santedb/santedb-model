using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        [XmlElement("value"), JsonProperty("value")]
        public Object Value { get; set; }
    }
}