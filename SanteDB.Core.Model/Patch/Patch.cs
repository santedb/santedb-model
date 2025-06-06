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
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Patch
{

    /// <summary>
    /// Represents a collection of patch instructions
    /// </summary>
    [XmlType(nameof(PatchCollection), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(PatchCollection), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(PatchCollection))]
    public class PatchCollection : IdentifiedData
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public PatchCollection()
        {
            this.Patches = new List<Patch>();
        }

        /// <summary>
        /// Gets or sets the patches 
        /// </summary>
        [XmlElement("patch"), JsonProperty("patch")]
        public List<Patch> Patches { get; set; }

        /// <inheritdoc/>
        public override DateTimeOffset ModifiedOn => DateTimeOffset.Now;
    }

    /// <summary>
    /// Represents a series of patch instructions 
    /// </summary>
    [XmlType(nameof(Patch), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(Patch), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(Patch))]
    public class Patch : BaseEntityData
    {
        /// <summary>
        /// Patch
        /// </summary>
        public Patch()
        {
            this.Version = typeof(Patch).Assembly.GetName().Version.ToString();
            this.Operation = new List<PatchOperation>();
        }

        /// <summary>
        /// Gets or sets the version of the patch file
        /// </summary>
        [XmlAttribute("version"), JsonProperty("version")]
        public String Version { get; set; }

        /// <summary>
        /// Application version
        /// </summary>
        [XmlElement("appliesTo"), JsonProperty("appliesTo")]
        public PatchTarget AppliesTo { get; set; }

        /// <summary>
        /// A list of patch operations to be applied to the object
        /// </summary>
        [XmlElement("change"), JsonProperty("change")]
        public List<PatchOperation> Operation { get; set; }

        /// <summary>
        /// To string representation
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var itm in this.Operation)
            {
                builder.AppendFormat("{0}\r\n", itm);
            }

            return builder.ToString();
        }
    }
}
