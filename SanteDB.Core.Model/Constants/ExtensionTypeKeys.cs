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
    /// Represents extension type keys for ONLY CORE EXTENSIONS. Third party extensions should never be placed in core
    /// </summary>
    public static class ExtensionTypeKeys
    {
        /// <summary>
        /// An extension which can hold a JPG photo of an entity
        /// </summary>
        public static readonly Guid PatientRaceExtension = Guid.Parse("07B1A3CD-4A22-49DD-B2EC-9E4DEC234B10");

        /// <summary>
        /// Patient race extension 
        /// </summary>
        public const string PatientRaceExtensionName = "http://santedb.org/extensions/ext/raceCode";

        /// <summary>
        /// An extension which can hold a JPG photo of an entity
        /// </summary>
        public static readonly Guid JpegPhotoExtension = Guid.Parse("77B53CBA-C32F-442B-B7A7-ED08184A0FA5");

        /// <summary>
        /// JPG extension name
        /// </summary>
        public const string JpegPhotoExtensionName = "http://santedb.org/extensions/core/jpegPhoto";

        /// <summary>
        /// Data quality issue extension
        /// </summary>
        public static readonly Guid DataQualityExtension = Guid.Parse("6DE54C16-8D1C-48E4-B750-7A2F4552E86D");

        /// <summary>
        /// Data quality extension name
        /// </summary>
        public const string DataQualityExtensionName = "http://santedb.org/extensions/core/detectedIssue";

        /// <summary>
        /// Data quality issue extension
        /// </summary>
        public static readonly Guid PatientSafetyConcernIssueExtension = Guid.Parse("E8F05D38-60E2-4C8B-B3BE-08286148F077");

        /// <summary>
        /// Data quality extension name
        /// </summary>
        public const string PatientSafetyConcernIssueExtensionName = "http://santedb.org/extensions/core/safetyConcernIssue";

        /// <summary>
        /// User preference extension key
        /// </summary>
        public static readonly Guid UserPreferenceExtension = Guid.Parse("210DAD97-4F14-447B-962C-0AD6B5FC1933");

        /// <summary>
        /// User preferences extension key
        /// </summary>
        public const string UserPreferenceExtensionName = "http://santedb.org/extensions/core/userPreferences";

        /// <summary>
        /// Birthtime reference
        /// </summary>
        public static readonly Guid BirthTimeExtension = Guid.Parse("30471318-D9B7-46DF-9927-61B7438A581D");

        /// <summary>
        /// Birthtime name
        /// </summary>
        public const string BirthTimeExtensionName = "http://santedb.org/extensions/core/birthTime";

        /// <summary>
        /// Birth information is confirmed and readonly
        /// </summary>
        public static readonly Guid BirthInformationConfirmedExtension = Guid.Parse("CE6689C1-C48E-40C8-9ECD-3CD7734635DA");

        /// <summary>
        /// Death information is confirmed and readonly
        /// </summary>
        public static readonly Guid DeathInformationConfirmedExtension = Guid.Parse("D71A56C3-5E74-4929-9AD3-BA24DE56590A");

        /// <summary>
        /// Get the UUID of the uri scheme extension
        /// </summary>
        public static readonly Guid Rfc3986SchemeExtension = Guid.Parse("7948BDB9-0700-4945-A39B-23522801F8E3");

        /// <summary>
        /// Gets the name of the scheme
        /// </summary>
        public const string Rfc3986SchemeExtensionName = "http://santedb.org/extensions/core/itef/rfc3986/scheme";

    }
}