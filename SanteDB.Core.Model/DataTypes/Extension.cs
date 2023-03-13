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
using SanteDB.Core.Interfaces;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// Represents a base entity extension
    /// </summary>
    [Classifier(nameof(ExtensionType)), SimpleValue(nameof(ExtensionValueString))]
    [XmlType(Namespace = "http://santedb.org/model"), JsonObject("Extension")]
    public abstract class Extension<TBoundModel> :
        VersionedAssociation<TBoundModel>, IModelExtension where TBoundModel : VersionedEntityData<TBoundModel>, new()
    {

        /// <summary>
        /// Gets or sets the value of the extension
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public byte[] ExtensionValueXml { get; set; }

        /// <summary>
        /// Empty
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return this.ExtensionValueXml == null || this.ExtensionValueXml.Length == 0;
        }

        /// <summary>
        /// Value as string of bytes
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(ExtensionValueXml))]
        public string ExtensionValueString
        {
            get
            {
                if (this.ExtensionValueXml == null)
                {
                    return null;
                }

                try
                {
                    return BitConverter.ToString(this.ExtensionValueXml).Replace("-", "");
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    this.ExtensionValueXml = null;
                }

                try
                {
                    if (value.Length % 2 == 1)
                    {
                        value = "0" + value;
                    }

                    this.ExtensionValueXml = Enumerable.Range(0, value.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(value.Substring(x, 2), 16)).ToArray();
                }
                catch
                {
                    this.ExtensionValueXml = Encoding.UTF8.GetBytes(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the ignore value
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationMetadataAttribute]
        public Object ExtensionValue
        {
            get
            {
                return this.LoadProperty(o => o.ExtensionType)?.ExtensionHandlerInstance?.DeSerialize(this.ExtensionValueXml);
            }
            set
            {
                if (this.LoadProperty(o => o.ExtensionType).ExtensionHandlerInstance != null)
                {
                    this.ExtensionValueXml = this.ExtensionType?.ExtensionHandlerInstance?.Serialize(value);
                }
            }
        }

        /// <summary>
        /// Get the value of the extension
        /// </summary>
        /// <returns></returns>
        public Object GetValue()
        {
            return this.LoadProperty<ExtensionType>("ExtensionType")?.ExtensionHandlerInstance?.DeSerialize(this.ExtensionValueXml);
        }

        /// <summary>
        /// Get the specified value as a particular type
        /// </summary>
        /// <typeparam name="T">The type to retrieve the value as</typeparam>
        /// <returns>The de-searlized object</returns>
        public T GetValue<T>()
        {
            var handler = this.LoadProperty<ExtensionType>("ExtensionType")?.ExtensionHandlerInstance;
            if (handler != null)
            {
                return handler.DeSerialize<T>(this.ExtensionValueXml);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets or sets an extension displayable value
        /// </summary>
        [XmlIgnore, JsonIgnore, QueryParameter("display")]
        public String ExtensionDisplay
        {
            get
            {
                this.ExtensionType = this.LoadProperty<ExtensionType>(nameof(ExtensionType));
                return this.ExtensionType?.ExtensionHandlerInstance?.GetDisplay(this.ExtensionValue);
            }
            set { }
        }

        /// <summary>
        /// Gets or sets the extension type
        /// </summary>
        [SerializationReference(nameof(ExtensionTypeKey))]
        [XmlIgnore, JsonIgnore]

        public ExtensionType ExtensionType { get; set; }

        /// <summary>
        /// Gets or sets the extension type key
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("extensionType"), JsonProperty("extensionType")]
        public Guid? ExtensionTypeKey { get; set; }

        /// <summary>
        /// Get the type key
        /// </summary>
        Guid IModelExtension.ExtensionTypeKey
        {
            get
            {
                return this.ExtensionTypeKey.Value;
            }
        }

        /// <summary>
        /// Gets the data
        /// </summary>
        byte[] IModelExtension.Data
        {
            get
            {
                return this.ExtensionValueXml;
            }
        }

        /// <summary>
        /// Get the display
        /// </summary>
        string IModelExtension.Display
        {
            get
            {
                return this.ExtensionDisplay;
            }
        }

        /// <summary>
        /// Get the value of the extension
        /// </summary>
        object IModelExtension.Value
        {
            get
            {
                return this.GetValue();
            }
        }

        /// <summary>
        /// Determine equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            Extension<TBoundModel> other = obj as Extension<TBoundModel>;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) && other.ExtensionTypeKey == this.ExtensionTypeKey &&
                this.ExtensionValueString == other.ExtensionValueString;
        }
    }

    /// <summary>
    /// Extension bound to entity
    /// </summary>

    [XmlType("EntityExtension", Namespace = "http://santedb.org/model"), JsonObject("EntityExtension")]
    public class EntityExtension : Extension<Entity>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityExtension()
        {
        }

        /// <summary>
        /// Creates an entity extension
        /// </summary>
        public EntityExtension(Guid extensionType, byte[] value)
        {
            this.ExtensionTypeKey = extensionType;
            this.ExtensionValueXml = value;
        }

        /// <summary>
        /// Creates an entity extension
        /// </summary>
        public EntityExtension(Guid extensionType, Type extensionHandlerType, object value)
        {
            this.ExtensionTypeKey = extensionType;
            this.ExtensionValueXml = (Activator.CreateInstance(extensionHandlerType) as IExtensionHandler)?.Serialize(value);
        }
    }

    /// <summary>
    /// Act extension
    /// </summary>

    [XmlType("ActExtension", Namespace = "http://santedb.org/model"), JsonObject("ActExtension")]
    public class ActExtension : Extension<Act>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ActExtension()
        {
        }

        /// <summary>
        /// Creates an entity extension
        /// </summary>
        public ActExtension(Guid extensionType, byte[] value)
        {
            this.ExtensionTypeKey = extensionType;
            this.ExtensionValueXml = value;
        }

        /// <summary>
        /// Creates an entity extension
        /// </summary>
        public ActExtension(Guid extensionType, Type extensionHandlerType, object value)
        {
            this.ExtensionTypeKey = extensionType;
            this.ExtensionValueXml = (Activator.CreateInstance(extensionHandlerType) as IExtensionHandler)?.Serialize(value);
        }
    }
}