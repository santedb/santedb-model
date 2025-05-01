/*
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
using SanteDB.Core.Model.Attributes;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents the model of a clinical protocol for CDR 
    /// </summary>
    /// <remarks>
    /// <para>The protocol type is used to store and retrieve information about a clinical protocol which was indicated on data submitted 
    /// to the CDR. This particular type makes no supposition that SanteDB can "execute" the protocol, rather this class is used 
    /// to store metadata about the clinical protocol in the SanteDB database</para>
    /// <para>
    /// The use of the CDSS execution engine to generate <see cref="CarePlan"/> references rely on executable definitions for clinical 
    /// protocols to be registered with SanteDB (for example: from an applet, from a database definition, etc.). These are stored separately 
    /// and apart from this class.
    /// </para>
    /// </remarks>
    [XmlType(nameof(Protocol), Namespace = "http://santedb.org/model"), JsonObject(nameof(Protocol))]
    [XmlRoot(nameof(Protocol), Namespace = "http://santedb.org/model")]
    [KeyLookup(nameof(Name))]
    public class Protocol : BaseEntityData
    {
        /// <summary>
        /// Gets or sets the name of the protocol
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the OID
        /// </summary>
        [XmlElement("oid"), JsonProperty("oid")]
        public String Oid { get; set; }

        /// <summary>
        /// Semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Protocol;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.Name == other.Name;
        }
    }
}