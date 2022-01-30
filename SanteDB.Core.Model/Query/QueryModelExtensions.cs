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
