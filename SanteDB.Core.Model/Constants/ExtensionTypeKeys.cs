﻿/*
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
 * Date: 2023-5-19
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

    }
}