﻿/*
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
 * User: fyfej
 * Date: 2023-6-21
 */
using System;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Implementers declare they can be identified by a key, have a tag and support getting details on the modified date.
    /// </summary>
    public interface IIdentifiedResource
    {
        /// <summary>
        /// Gets or sets the resource key for this resource.
        /// </summary>
        Guid? Key { get; set; }

        /// <summary>
        /// Gets the tag for this resource. The tag is used to calculate whether a resource has changed or not.
        /// </summary>
        string Tag { get; }

        /// <summary>
        /// Gets the last modified timestamp for this resource.
        /// </summary>
        DateTimeOffset ModifiedOn { get; }
    }
}
