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

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Mood concept keys
    /// </summary>
    public static class MoodConceptKeyStrings
    {
        /// <summary>
        /// The act represents a goal to be acheived
        /// </summary>
        public const String Goal = "13925967-E748-4DD6-B562-1E1DA3DDFB06";
        /// <summary>
        /// The act represents a promise to do something
        /// </summary>
        public const String Promise = "B389DEDF-BE61-456B-AA70-786E1A5A69E0";
        /// <summary>
        /// The act represents a request by a person to perfrom some action
        /// </summary>
        public const String Request = "E658CA72-3B6A-4099-AB6E-7CF6861A5B61";
        /// <summary>
        /// The act represents an event that actually occurred
        /// </summary>
        public const String Eventoccurrence = "EC74541F-87C4-4327-A4B9-97F325501747";
        /// <summary>
        /// The act represents an intent that a human WILL do something
        /// </summary>
        public const String Intent = "099BCC5E-8E2F-4D50-B509-9F9D5BBEB58E";
        /// <summary>
        /// The act represents a proposal to do something
        /// </summary>
        public const String Proposal = "ACF7BAF2-221F-4BC2-8116-CEB5165BE079";
    }

    /// <summary>
    /// Mood concept keys
    /// </summary>
    public static class MoodConceptKeys
    {
        /// <summary>
        /// The act represents a goal to be acheived
        /// </summary>
        public static readonly Guid Goal = Guid.Parse(MoodConceptKeyStrings.Goal);
        /// <summary>
        /// The act represents a promise to do something
        /// </summary>
        public static readonly Guid Promise = Guid.Parse(MoodConceptKeyStrings.Promise);
        /// <summary>
        /// The act represents a request by a person to perfrom some action
        /// </summary>
        public static readonly Guid Request = Guid.Parse(MoodConceptKeyStrings.Request);
        /// <summary>
        /// The act represents an event that actually occurred
        /// </summary>
        public static readonly Guid Eventoccurrence = Guid.Parse(MoodConceptKeyStrings.Eventoccurrence);
        /// <summary>
        /// The act represents an intent that a human WILL do something
        /// </summary>
        public static readonly Guid Intent = Guid.Parse(MoodConceptKeyStrings.Intent);
        /// <summary>
        /// The act represents a proposal to do something
        /// </summary>
        public static readonly Guid Proposal = Guid.Parse(MoodConceptKeyStrings.Proposal);
    }
}
