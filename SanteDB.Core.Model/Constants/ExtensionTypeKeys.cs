﻿/*
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2021-2-9
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
        public static readonly Guid JpegPhotoExtension = Guid.Parse("77B53CBA-C32F-442B-B7A7-ED08184A0FA5");

        /// <summary>
        /// Data quality issue extension
        /// </summary>
        public static readonly Guid DataQualityExtension = Guid.Parse("6DE54C16-8D1C-48E4-B750-7A2F4552E86D");
    }
}
