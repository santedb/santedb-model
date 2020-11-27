/*
 * Copyright (C) 2019 - 2020, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2019-11-27
 */
using Newtonsoft.Json;
using SanteDB.Core.Interfaces;
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Instructions on how an extensionshould be handled
    /// </summary>
    [Classifier(nameof(Name)), KeyLookup(nameof(Name))]
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
            this.Name = name;
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
            get { return this.ExtensionHandler?.AssemblyQualifiedName; }
            set
            {
                if (value == null)
                    this.ExtensionHandler = null;
                else
                    this.ExtensionHandler = System.Type.GetType(value);
            }
        }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Whether the extension is enabled
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool IsEnabled { get; set; }


    }
}
