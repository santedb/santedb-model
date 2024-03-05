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
 * User: fyfej
 * Date: 2023-6-21
 */
using System;

namespace SanteDB.Core.Model.Constants
{

    /// <summary>
    /// Base entity relationship type keys
    /// </summary>
    public static class RelationshipClassKeyStrings
    {

        /// <summary>
        /// The target is a sub-object of the holder (i.e. it doesn't exist within its own right)
        /// </summary>
        public const string ContainedObjectLink = "B23A00BB-34B0-4704-AC5B-53330A8852B3";

        /// <summary>
        /// Entity is a referenced link of the holder (i.e. should be )
        /// </summary>
        public const string ReferencedObjectLink = "724B1FC7-94FC-43E5-B597-B6ED2FB8F660";

        /// <summary>
        /// Entity referenced is for internal use only (i.e. should not be disclosed on external callers)
        /// </summary>
        public const string PrivateLink = "CA3057C3-CE83-4CA5-A0C4-FA0480B7F991";

        /// <summary>
        /// Played role
        /// </summary>
        public const string PlayedRoleLink = "8E7BEFBC-56D9-49F2-A758-7085CA72D03D";

    }

    /// <summary>
    /// Base entity relationship type keys
    /// </summary>
    public static class RelationshipClassKeys
    {

        /// <summary>
        /// The target is referenced by the holder however the target exists as an independent object
        /// </summary>
        public static readonly Guid ReferencedObjectLink = Guid.Parse(RelationshipClassKeyStrings.ReferencedObjectLink);

        /// <summary>
        /// The target is referenced by the holder, however the target cannot exist without the holder object (it relies on the holder to give it context)
        /// </summary>
        public static readonly Guid ContainedObjectLink = Guid.Parse(RelationshipClassKeyStrings.ContainedObjectLink);

        /// <summary>
        /// The target is referenced by the holder by a system process and should not be disclosed on non-internal APIs
        /// </summary>
        public static readonly Guid PrivateLink = Guid.Parse(RelationshipClassKeyStrings.PrivateLink);
        /// <summary>
        /// The holder of the relationship plays the role of the target of the relationship
        /// </summary>
        public static readonly Guid PlayedRoleLink = Guid.Parse(RelationshipClassKeyStrings.PlayedRoleLink);
    }
}