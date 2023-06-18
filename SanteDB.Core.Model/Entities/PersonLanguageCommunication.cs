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
 * Date: 2023-5-19
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a single preferred communication method for the entity
    /// </summary>

    [XmlType("PersonLanguageCommunication", Namespace = "http://santedb.org/model"), JsonObject("PersonLanguageCommunication")]
    public class PersonLanguageCommunication : VersionedAssociation<Entity>, IHasExternalKey
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public PersonLanguageCommunication()
        {
        }

        /// <summary>
        /// Language communication code ctor with specified code and preference
        /// </summary>
        public PersonLanguageCommunication(String languageCode, bool isPreferred)
        {
            this.LanguageCode = languageCode;
            this.IsPreferred = isPreferred;
        }

        /// <summary>
        /// Gets or sets the external key for the object
        /// </summary>
        /// <remarks>Sometimes, when communicating with an external communications another system needs to 
        /// refer to this by a particular key</remarks>
        [XmlElement("externId"), JsonProperty("externId")]
        public string ExternalKey { get; set; }

        /// <summary>
        /// Gets or set the user's preference indicator
        /// </summary>
        [XmlElement("isPreferred"), JsonProperty("isPreferred")]
        public bool IsPreferred { get; set; }

        /// <summary>
        /// Gets or sets the language code
        /// </summary>
        [XmlElement("languageCode"), JsonProperty("languageCode")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as PersonLanguageCommunication;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) &&
                this.IsPreferred == other.IsPreferred &&
                this.LanguageCode == other.LanguageCode;
        }
    }
}