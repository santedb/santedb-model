/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 */
using Newtonsoft.Json;
using System;
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
