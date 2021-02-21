﻿/*
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2021-2-9
 */
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Warehouse
{
    /// <summary>
    /// Represents a stored query creation statement
    /// </summary>
    [XmlType(nameof(DatamartStoredQuery), Namespace = "http://santedb.org/warehousing"), JsonObject(nameof(DatamartStoredQuery))]
    [XmlRoot(nameof(DatamartStoredQuery), Namespace = "http://santedb.org/warehousing")]
    public class DatamartStoredQuery : IDatamartSchemaPropertyContainer
    {

        /// <summary>
        /// Stored query
        /// </summary>
        public DatamartStoredQuery()
        {
            this.Definition = new List<DatamartStoredQueryDefinition>();
            this.Properties = new List<DatamartSchemaProperty>();
        }

        /// <summary>
        /// Attachments
        /// </summary>
        [XmlElement("connection"), JsonProperty("connection")]
        public String DataSource { get; set; }

        /// <summary>
        /// Gets or sets the provider identifier
        /// </summary>
        [XmlAttribute("id"), JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Definition of the query
        /// </summary>
        [XmlAttribute("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the property names for the schema element
        /// </summary>
        [XmlElement("property"), JsonProperty("property")]
        public List<DatamartSchemaProperty> Properties { get; set; }

        /// <summary>
        /// Definition of the query
        /// </summary>
        [XmlElement("sql"), JsonProperty("select")]
        public List<DatamartStoredQueryDefinition> Definition { get; set; }

    }
}
