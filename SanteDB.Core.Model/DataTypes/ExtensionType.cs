﻿/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using Newtonsoft.Json;
using SanteDB.Core.Interfaces;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.EntityLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Instructions on how an extensionshould be handled
    /// </summary>
    [Classifier(nameof(Uri)), KeyLookup(nameof(Uri))]
    [XmlType(nameof(ExtensionType), Namespace = "http://santedb.org/model"), JsonObject("ExtensionType")]
    [XmlRoot(nameof(ExtensionType), Namespace = "http://santedb.org/model")]
    public class ExtensionType : NonVersionedEntityData
    {

        /// <summary>
        /// Extension type ctor
        /// </summary>
        public ExtensionType()
        {

        }

        /// <summary>
        /// Creates  a new extension type
        /// </summary>
        public ExtensionType(String name, Type handlerClass)
        {
            this.Uri = name;
            this.ExtensionHandler = handlerClass;
        }

        /// <summary>
        /// Gets or sets the extension handler
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Type ExtensionHandler { get; set; }

        /// <summary>
        /// Extension handler instance
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public IExtensionHandler ExtensionHandlerInstance
        {
            get
            {
                try
                {
                    return Activator.CreateInstance(this.ExtensionHandler) as IExtensionHandler;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [XmlElement("handlerClass"), JsonProperty("handlerClass")]
        public String ExtensionHandlerXml
        {
            get { return this.ExtensionHandler?.AssemblyQualifiedNameWithoutVersion(); }
            set
            {
                if (value == null)
                {
                    this.ExtensionHandler = null;
                }
                else
                {
                    this.ExtensionHandler = System.Type.GetType(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the uri of the extension
        /// </summary>
        [XmlElement("uri"), JsonProperty("uri")]
        public String Uri { get; set; }

        /// <summary>
        /// Gets or sets the name of the extension
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Represents scopes to which the authority is bound
        /// </summary>
        [JsonProperty("scope"), XmlElement("scope")]
        public List<Guid> ScopeXml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets concept sets to which this concept is a member
        /// </summary>
        [SerializationMetadata, XmlIgnore, JsonIgnore, SerializationReference(nameof(ScopeXml))]
        public List<Concept> Scope
        {
            get
            {
                return this.ScopeXml?.Select(o => EntitySource.Current.Get<Concept>(o)).ToList();
            }
            set
            {
                this.ScopeXml = value?.Where(o => o.Key.HasValue).Select(o => o.Key.Value).ToList();
            }
        }


    }
}
