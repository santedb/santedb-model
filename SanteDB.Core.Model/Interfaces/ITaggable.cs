/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-27
 */
using System;
using System.Collections.Generic;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents a class which can hold tags
    /// </summary>
    public interface ITaggable
    {
        /// <summary>
        /// Gets the tags associated with the object
        /// </summary>
        IEnumerable<ITag> Tags { get; }

        /// <summary>
        /// Add a tag
        /// </summary>
        ITag AddTag(String tagKey, String tagValue);

        /// <summary>
        /// Get the tag
        /// </summary>
        string GetTag(string tagKey);

        /// <summary>
        /// Remove the specified tag
        /// </summary>
        void RemoveTag(string tagKey);

        /// <summary>
        /// Try to get a tag
        /// </summary>
        /// <param name="tagName">The name of the tag to try to get</param>
        /// <param name="tagValue">The value retrieved</param>
        bool TryGetTag(string tagName, out ITag tagValue);
    }

    /// <summary>
    /// Represents a tag
    /// </summary>
    public interface ITag : ISimpleAssociation
    {
        /// <summary>
        /// Gets the key for the tag
        /// </summary>
        String TagKey { get; }

        /// <summary>
        /// Gets the value for the tag
        /// </summary>
        String Value { get; }
    }
}