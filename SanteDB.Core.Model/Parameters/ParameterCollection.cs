using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
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
            value = (TValue)(p?.Value ?? default(TValue));
            return p != null;
        }
    }
}