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
using SanteDB.Core.Model.Attributes;
using System;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents base entity data
    /// </summary>
    public interface IBaseData : IAnnotatedResource
    {
        /// <summary>
        /// Gets or sets the creator of the data
        /// </summary>
        [QueryParameter("createdBy")]
        Guid? CreatedByKey { get; }

        /// <summary>
        /// Gets or sets teh obsoletor of the data
        /// </summary>
        [QueryParameter("obsoletedBy")]
        Guid? ObsoletedByKey { get; }

        /// <summary>
        /// Gets or sets the time when the data was created
        /// </summary>
        [QueryParameter("creationTime")]
        DateTimeOffset CreationTime { get; }

        /// <summary>
        /// Gets or sets the time with the data was obsoleted.
        /// </summary>
        [QueryParameter("obsoletionTime")]
        DateTimeOffset? ObsoletionTime { get; }
    }
}
