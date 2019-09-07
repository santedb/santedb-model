/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: justi
 * Date: 2019-1-12
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Security role
    /// </summary>
    [XmlType(Namespace = "http://santedb.org/model", TypeName = "SecurityRole")]
    [KeyLookup(nameof(Name))]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityRole")]
    [JsonObject(nameof(SecurityRole))]
    public class SecurityRole : SecurityEntity
    {
        /// <summary>
        /// Users in teh group
        /// </summary>
        public SecurityRole()
        {
            this.Users = new List<SecurityUser>();
        }

        /// <summary>
        /// Gets or sets the name of the security role
        /// </summary>
        [XmlElement("name"), JsonProperty("name"), NoCase]
        public String Name { get; set; }

        /// <summary>
        /// Description of the role
        /// </summary>
        [XmlElement("description"), JsonProperty("description")]
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the security users in the role
        /// </summary>
        [XmlIgnore, JsonIgnore, QueryParameter("users")]
        public List<SecurityUser> Users { get; set; }

        /// <summary>
        /// Determine semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as SecurityRole;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.Name == other.Name;
        }
    }
}
