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
 * Date: 2020-3-18
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Represents a security challenge
    /// </summary>
    [XmlType("SecurityChallenge", Namespace = "http://santedb.org/model"), JsonObject("SecurityChallenge")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityChallenge")]
    public class SecurityChallenge : NonVersionedEntityData
    {

        /// <summary>
        /// The text for the security challenge
        /// </summary>
        [XmlElement("text"), JsonProperty("text")]
        public String ChallengeText { get; set; }

    }
}
