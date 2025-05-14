/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
    /// Keys of the unit of measure of obserations
    /// </summary>
    public static class UnitOfMeasureKeys
    {
        /// <summary>
        /// Unit of measure is MMHG (for blood pressure)
        /// </summary>
        public static readonly Guid MillimetersOfMercury = Guid.Parse("BB1D639D-AEA8-4750-A24C-B2548D2B6FE4");
        /// <summary>
        /// Millimeters of Mercury
        /// </summary>
        public static readonly Guid MmHg = MillimetersOfMercury;
        /// <summary>
        /// Unit of measure is a /min
        /// </summary>
        public static readonly Guid PerMinute = Guid.Parse("564EBC64-1B56-48CD-907D-41BB47B7834F");
        /// <summary>
        /// Unit of measure is %
        /// </summary>
        public static readonly Guid Percentage = Guid.Parse("7BEA593B-454A-4DE4-BAD9-4866FD14FE09");
        /// <summary>
        /// Unit of measure is degrees C
        /// </summary>
        public static readonly Guid DegreesCelsius = Guid.Parse("83792D73-6A3A-4A72-B13A-BA95EF4A83EA");
        /// <summary>
        /// Unit of measure is centimeter
        /// </summary>
        public static readonly Guid Centimetre = Guid.Parse("EEEFB6BB-18B1-478D-9CC5-575325B947A8");
        /// <summary>
        /// Unit of measure is 1000 g
        /// </summary>
        public static readonly Guid Kilograms = Guid.Parse("A0A8D4DB-DB72-4BC7-9B8C-C07CEF7BC796");
        /// <summary>
        /// Unit of measure is 1 g
        /// </summary>
        public static readonly Guid Grams = Guid.Parse("93424B4A-36F2-4376-B374-1C086C5EBF52");
        /// <summary>
        ///  Unit of measure is degrees fahrenheit
        /// </summary>
        public static readonly Guid DegreesFahrenheit = Guid.Parse("4400EC99-E5AE-4FF8-BC4B-69A6AC7D77D7");
        /// <summary>
        /// Unit of measure is US standard inches
        /// </summary>
        public static readonly Guid UsInches = Guid.Parse("9E974584-7C48-457E-A79C-031CDD62714A");
        /// <summary>
        /// Unit of measure is 1 dose
        /// </summary>
        public static readonly Guid Dose = Guid.Parse("A4FC5C93-31C2-4F87-990E-C5A4E5EA2E76");
        /// <summary>
        /// Unit of measure is 1 puff
        /// </summary>
        public static readonly Guid Puff = Guid.Parse("2CEDFE61-4549-4D93-A936-79C942E683CC");
        /// <summary>
        /// Unit of measure is 1 per or 1 each
        /// </summary>
        public static readonly Guid Each = Guid.Parse("B9424B4A-36F2-4376-B374-1C086C5EBF52");
    }
}
