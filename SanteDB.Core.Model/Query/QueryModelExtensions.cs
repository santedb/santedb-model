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
 * Date: 2022-1-12
 */
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Query model extensions
    /// </summary>
    public static class QueryModelExtensions
    {

        /// <summary>
        /// Freetext search
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool FreetextSearch(this IdentifiedData me, string query)
        {
            return true;
        }

        /// <summary>
        /// Return the age
        /// </summary>
        /// <remarks>This exists for the extended query filter only</remarks>
        public static TimeSpan Age(this DateTime me, DateTime atDateTime)
        {
            return me.Subtract(atDateTime);
        }

        /// <summary>
        /// Gets the date contains function
        /// </summary>
        public static DateTime DateTrunc(this DateTime me, String precision)
        {
            switch(precision)
            {
                case "y":
                    return new DateTime(me.Year, 01, 01);
                case "M":
                    return new DateTime(me.Year, me.Month, 01);
                case "d":
                    return me.Date;
                default:
                    throw new NotImplementedException("Precision for DateContains is implemented to DAY precision");
            }
        }
    }
}
