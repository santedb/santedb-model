﻿/*
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
    /// Null reason strings
    /// </summary>
    public static class NullReasonStrings
    {
        /// <summary>
        /// The reason that the requested value was not provided is that it is not available
        /// </summary>
        public const String Unavailable = "31E01921-82DC-4622-B3DB-21429EA9E406";

        /// <summary>
        /// The reason that the requested value was not provided is that it is not applicable (for example last menstrual period of a male)
        /// </summary>
        public const String NotApplicable = "FEA2CFB1-F231-413D-B113-372779092E56";

        /// <summary>
        /// The reason that the value is not provided is that it can be derived from other information
        /// </summary>
        public const String Derived = "8EF137B3-E717-492B-8D8F-3817C99AED88";

        /// <summary>
        /// The value was not provided because it does not fall within the acceptable values 
        /// </summary>
        public const String Other = "6052712A-340E-4480-AD6B-409BA320DB4F";

        /// <summary>
        /// The value was asked for but the target did not know that answer
        /// </summary>
        public const String AskedUnknown = "21B0FFC8-CA4E-408D-A104-41FC924D3A39";

        /// <summary>
        /// The value was entered but it is invalid according to business rules
        /// </summary>
        public const String Invalid = "D3F92EB1-FECE-4DEA-BED2-515AF2B0FB38";

        /// <summary>
        /// There is a value present, but the quantity of the value is so small that it cannot be registered
        /// </summary>
        public const String Trace = "085069D8-0CA8-4771-986B-5EB3466580FF";

        /// <summary>
        /// The value is not prvovided because it is negative infinity
        /// </summary>
        public const String NegativeInfinity = "FED3FE1B-B2C7-480B-B0AF-5FD2E0200CE5";

        /// <summary>
        /// The exact value is not known, but there is sufficient quantity to perform an act
        /// </summary>
        public const String SufficientQuantity = "C139841A-7D5A-40BA-9AC7-7628A7CDF443";

        /// <summary>
        /// The value is available however it cannot be encoded in the desired format
        /// </summary>
        public const String UnEncoded = "7DA45C51-EB8E-4C75-A40B-7DB66CB3F3CB";

        /// <summary>
        /// The value is unavailable because it was not asked for
        /// </summary>
        public const String NotAsked = "09919A72-808C-44C4-8B44-86FD3725F100";

        /// <summary>
        /// The value may have been asked for and was not known or is unknown (this differes from AskedUnknown)
        /// </summary>
        public const String Unknown = "70FE34CE-CAFF-4F46-B6E6-9CD6D8F289D6";

        /// <summary>
        /// The value is not provided because it is positive infinity
        /// </summary>
        public const String PositiveInfinity = "E6D6FEE2-FA53-4027-8EB8-9DD0F35D053D";

        /// <summary>
        /// The value is not provided because there is no available information
        /// </summary>
        public const String NoInformation = "61D8F65C-747E-4A99-982F-A42AC5437473";

        /// <summary>
        /// The value is available however it has been masked due to privacy concerns
        /// </summary>
        public const String Masked = "9B16BF12-073E-4EA4-B6C5-E1B93E8FD490";
    }

    /// <summary>
    /// In SanteDB, any concept can be replaced with a null reason. A null reason indicates why a particular field is not present rather than being null
    /// </summary>
    public static class NullReasonKeys
    {
        /// <summary>
        /// The reason that the requested value was not provided is that it is not available
        /// </summary>
        public static Guid Unavailable = Guid.Parse(NullReasonStrings.Unavailable);

        /// <summary>
        /// The reason that the requested value was not provided is that it is not applicable (for example last menstrual period of a male)
        /// </summary>
        public static Guid NotApplicable = Guid.Parse(NullReasonStrings.NotApplicable);

        /// <summary>
        /// The reason that the value is not provided is that it can be derived from other information
        /// </summary>
        public static Guid Derived = Guid.Parse(NullReasonStrings.Derived);

        /// <summary>
        /// The value was not provided because it does not fall within the acceptable values 
        /// </summary>
        public static Guid Other = Guid.Parse(NullReasonStrings.Other);

        /// <summary>
        /// The value was asked for but the target did not know that answer
        /// </summary>
        public static Guid AskedUnknown = Guid.Parse(NullReasonStrings.AskedUnknown);

        /// <summary>
        /// The value was entered but it is invalid according to business rules
        /// </summary>
        public static Guid Invalid = Guid.Parse(NullReasonStrings.Invalid);

        /// <summary>
        /// There is a value present, but the quantity of the value is so small that it cannot be registered
        /// </summary>
        public static Guid Trace = Guid.Parse(NullReasonStrings.Trace);

        /// <summary>
        /// The value is not prvovided because it is negative infinity
        /// </summary>
        public static Guid NegativeInfinity = Guid.Parse(NullReasonStrings.NegativeInfinity);

        /// <summary>
        /// The exact value is not known, but there is sufficient quantity to perform an act
        /// </summary>
        public static Guid SufficientQuantity = Guid.Parse(NullReasonStrings.SufficientQuantity);

        /// <summary>
        /// The value is available however it cannot be encoded in the desired format
        /// </summary>
        public static Guid UnEncoded = Guid.Parse(NullReasonStrings.UnEncoded);

        /// <summary>
        /// The value is unavailable because it was not asked for
        /// </summary>
        public static Guid NotAsked = Guid.Parse(NullReasonStrings.NotAsked);

        /// <summary>
        /// The value may have been asked for and was not known or is unknown (this differes from AskedUnknown)
        /// </summary>
        public static Guid Unknown = Guid.Parse(NullReasonStrings.Unknown);

        /// <summary>
        /// The value is not provided because it is positive infinity
        /// </summary>
        public static Guid PositiveInfinity = Guid.Parse(NullReasonStrings.PositiveInfinity);

        /// <summary>
        /// The value is not provided because there is no available information
        /// </summary>
        public static Guid NoInformation = Guid.Parse(NullReasonStrings.NoInformation);

        /// <summary>
        /// The value is available however it has been masked due to privacy concerns
        /// </summary>
        public static Guid Masked = Guid.Parse(NullReasonStrings.Masked);
    }
}
