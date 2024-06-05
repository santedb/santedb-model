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
    /// Status concepts represent the current status of entities, acts, and concepts.
    /// </summary>
    public static class StatusKeyStrings
    {
        /// <summary>
        /// When an entity or act is active, it means the information or entity is currently correct and ongoing
        /// </summary>
        public const String Active = "C8064CBD-FA06-4530-B430-1A52F1530C27";

        /// <summary>
        /// Indicates that an act has been completed and now represents an act in the past
        /// </summary>
        public const String Completed = "AFC33800-8225-4061-B168-BACC09CDBAE3";

        /// <summary>
        /// Indicates that the data is new, and may require additional verification or actions
        /// </summary>
        public const String New = "C34FCBF1-E0FE-4989-90FD-0DC49E1B9685";

        /// <summary>
        /// Indicates that the entity or act never existed, and was entered in error
        /// </summary>
        public const String Nullified = "CD4AA3C4-02D5-4CC9-9088-EF8F31E321C5";

        /// <summary>
        /// Indicates that the act was cancelled before being completed
        /// </summary>
        public const String Cancelled = "3EFD3B6E-02D5-4CC9-9088-EF8F31E321C5";

        /// <summary>
        /// Indicates that the entity or act did exist at one point, however it is no longer current
        /// </summary>
        public const String Obsolete = "BDEF5F90-5497-4F26-956C-8F818CCE2BD2";

        /// <summary>
        /// Indicates that the data was logically deleted (i.e. it did exist, however no longer exists)
        /// </summary>
        public const String Purged = "39995C08-0A5C-4549-8BA7-D187F9B3C4FD";

        /// <summary>
        /// Indicates the record is deleted (inactive)
        /// </summary>
        public const String Inactive = "0BBEC253-21A1-49CB-B376-7FE4D0592CDA";
    }

    /// <summary>
    /// Status concepts represent the current status of entities, acts, and concepts.
    /// </summary>
    public static class StatusKeys
    {
        /// <summary>
        /// When an entity or act is active, it means the information or entity is currently correct and ongoing
        /// </summary>
        public static readonly Guid Active = Guid.Parse(StatusKeyStrings.Active);

        /// <summary>
        /// Indicates that an act has been completed and now represents an act in the past
        /// </summary>
        public static readonly Guid Completed = Guid.Parse(StatusKeyStrings.Completed);

        /// <summary>
        /// Indicates that the data is new, and may require additional verification or actions
        /// </summary>
        public static readonly Guid New = Guid.Parse(StatusKeyStrings.New);

        /// <summary>
        /// Indicates that the entity or act never existed, and was entered in error
        /// </summary>
        public static readonly Guid Nullified = Guid.Parse(StatusKeyStrings.Nullified);

        /// <summary>
        /// Indicates that the act was cancelled before being completed
        /// </summary>
        public static readonly Guid Cancelled = Guid.Parse(StatusKeyStrings.Cancelled);

        /// <summary>
        /// Indicates that the entity or act did exist at one point, however the data is no long considered accurate or the most up to date
        /// </summary>
        public static readonly Guid Obsolete = Guid.Parse(StatusKeyStrings.Obsolete);

        /// <summary>
        /// Indicates that the entity or act did exist at one point, however it no longer exists
        /// </summary>
        public static readonly Guid Purged = Guid.Parse(StatusKeyStrings.Purged);

        /// <summary>
        /// Indicates that the entity or act did exist at one point, however it no longer exists - and the reason is unknown
        /// </summary>
        public static readonly Guid Inactive = Guid.Parse(StatusKeyStrings.Inactive);

        /// <summary>
        /// States which indicate that a record is active
        /// </summary>
        public static readonly Guid[] ActiveStates = new Guid[]
        {
            StatusKeys.Active,
            StatusKeys.Completed,
            StatusKeys.New
        };

        /// <summary>
        /// States which indicate that a record is inactive and should not be included in results
        /// </summary>
        public static readonly Guid[] InactiveStates = new Guid[]
        {
            StatusKeys.Inactive,
            StatusKeys.Nullified,
            StatusKeys.Obsolete
        };

        /// <summary>
        /// Any status
        /// </summary>
        public static readonly Guid[] AllStates = new Guid[]
        {
            StatusKeys.Active,
            StatusKeys.Completed,
            StatusKeys.New,
            StatusKeys.Inactive,
            StatusKeys.Nullified,
            StatusKeys.Obsolete,
            StatusKeys.Purged
        };
    }
}