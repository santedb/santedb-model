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
using System.Text.RegularExpressions;
using System.Xml;

namespace SanteDB.Core.Model.Map
{
    /// <summary>
    /// SanteDB conversion helper functions
    /// </summary>
    public class SanteDBConvert
    {

        /// <summary>
        /// Guid > Byte[]
        /// </summary>
        public static byte[] NullGuidToByte(Guid? g)
        {
            return g.HasValue ? g.Value.ToByteArray() : null;
        }

        /// <summary>
        /// Byte[] > GUID
        /// </summary>
        public static Guid? ByteToNullGuid(byte[] b)
        {
            return b == null ? null : (Guid?)new Guid(b);
        }

        /// <summary>
        /// Boolean to int
        /// </summary>
        public static int BooleanToInt(Boolean b)
        {
            return b ? 1 : 0;
        }

        /// <summary>
        /// Boolean to int
        /// </summary>
        public static Boolean IntToBoolean(Int32 i)
        {
            return i != 0;
        }

        /// <summary>
        /// Guid > Byte[]
        /// </summary>
        public static byte[] GuidToByte(Guid g)
        {
            return g.ToByteArray();
        }

        /// <summary>
        /// Byte[] > GUID
        /// </summary>
        public static Guid ByteToGuid(byte[] b)
        {
            return new Guid(b);
        }


        /// <summary>
        /// DT > DTO
        /// </summary>
        public static DateTimeOffset? DateTimeToDateTimeOffset(DateTime? dt)
        {
            return dt;
        }

        /// <summary>
        /// DTO > DT
        /// </summary>
        public static DateTime? DateTimeOffsetToDateTime(DateTimeOffset? dto)
        {
            return dto?.DateTime;
        }

        /// <summary>
        /// DT > DTO
        /// </summary>
        public static DateTimeOffset DateTimeToDateTimeOffset(DateTime dt)
        {
            return dt;
        }

        /// <summary>
        /// DTO > DT
        /// </summary>
        public static DateTime DateTimeOffsetToDateTime(DateTimeOffset dto)
        {
            return dto.DateTime;
        }

        // Constant epoch
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Parse a date time into an object
        /// </summary>
        public static Int32 ToDateTime(DateTime date)
        {
            return (int)(date.ToUniversalTime() - EPOCH).TotalSeconds;
        }

        /// <summary>
        /// Parse a date time from an object
        /// </summary>
        public static DateTime ParseDateTime(Int32 date)
        {
            return EPOCH.AddSeconds(date).ToLocalTime();
        }

        /// <summary>
        /// Parse a date time into an object
        /// </summary>
        public static Int32 ToDateTimeOffset(DateTimeOffset date)
        {
            return (int)(date.ToLocalTime() - EPOCH).TotalSeconds;
        }

        /// <summary>
        /// Parse a date time from an object
        /// </summary>
        public static DateTimeOffset ParseDateTimeOffset(Int32 date)
        {
            return EPOCH.AddSeconds(date).ToLocalTime();
        }

        /// <summary>
        /// Convert Int16 to decimal
        /// </summary>
        public static Decimal Int16ToDecimal(Int16 val)
        {
            return (Decimal)val;
        }

        /// <summary>
        /// Convert int16 to 32
        /// </summary>
        public static Int32 Int16ToInt32(Int16 val)
        {
            return (Int32)val;
        }

        /// <summary>
        /// Convert int16 to 32
        /// </summary>
        public static Int16 Int32ToInt16(Int32 val)
        {
            return (Int16)val;
        }

        /// <summary>
        /// Convert Int32 to decimal
        /// </summary>
        public static Decimal Int32ToDecimal(Int32 val)
        {
            return (Decimal)val;
        }

        /// <summary>
        /// Convert Int64 to decimal
        /// </summary>
        public static Decimal Int64ToDecimal(Int64 val)
        {
            return (Decimal)val;
        }

        /// <summary>
        /// Convert 64-bit long to int
        /// </summary>
        public static Int32 Int64ToInt32(Int64 val)
        {
            return (Int32)val;
        }

        /// <summary>
        /// Timespan converted to string
        /// </summary>
        public static String TimeSpanToString(TimeSpan ts)
        {
            return ts.ToString();
        }

        /// <summary>
        /// String to timespan
        /// </summary>
        public static TimeSpan StringToTimespan(String value)
        {
            var match = new Regex(@"^(\d*?)([yMdwhms])$").Match(value);
            if (match.Success) // in format 3w, 2d, etc. 
            {
                long qty = long.Parse(match.Groups[1].Value);
                switch (match.Groups[2].Value)
                {
                    case "y":
                        return new TimeSpan((long)(TimeSpan.TicksPerDay * 365.25d * qty));
                    case "M":
                        return new TimeSpan((long)(TimeSpan.TicksPerDay * 30.473d * qty));
                    case "d":
                        return new TimeSpan((int)qty, 0, 0, 0);
                    case "w":
                        return new TimeSpan((int)qty * 7, 0, 0, 0);
                    case "h":
                        return new TimeSpan(0, (int)qty, 0, 0);
                    case "m":
                        return new TimeSpan(0, (int)qty, 0);
                    case "s":
                        return new TimeSpan(0, 0, (int)qty);
                    case "t":
                        return new TimeSpan((long)qty);
                    default:
                        throw new ArgumentOutOfRangeException($"Don't understand unit {match.Groups[2].Value}");
                }
            }
            else
            {
                try
                {
                    return XmlConvert.ToTimeSpan(value);
                }
                catch
                {
                    return TimeSpan.Parse(value);
                }
            }
        }
    }
}