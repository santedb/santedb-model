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
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Base type for collections returned from Rest Endpoints. 
    /// </summary>
    public abstract class RestCollectionBase
    {
        /// <summary>
        /// Initializes an empty list of collection items.
        /// </summary>
        public RestCollectionBase()
        {
            CollectionItem = new List<object>();
        }

        /// <summary>
        /// Initializes with a specific collection of items.
        /// </summary>
        public RestCollectionBase(IEnumerable<object> collectionItems)
        {
            CollectionItem = new List<object>(collectionItems);
        }

        /// <summary>
        /// Initializes with a specific collection of items and a known offset and total count.
        /// </summary>
        public RestCollectionBase(IEnumerable<object> collectionItems, int offset, int totalCount)
        {
            CollectionItem = new List<object>(collectionItems);
            Offset = offset;
            Size = totalCount;
        }

        /// <summary>
        /// Gets or sets a list of collection items.
        /// </summary>
        [XmlElement("resource"), JsonProperty("resource")]
        public List<object> CollectionItem { get; set; }

        /// <summary>
        /// Gets or sets the total offset.
        /// </summary>
        [XmlAttribute("offset"), JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the total collection size.
        /// </summary>
        [XmlAttribute("size"), JsonProperty("size")]
        public int Size { get; set; }
    }
}
