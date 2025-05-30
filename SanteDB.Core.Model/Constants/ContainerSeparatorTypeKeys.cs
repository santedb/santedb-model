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
    /// Represents a collection of container separator type keys.
    /// A material in a blood collection container that facilitates the separation of of blood cells from serum or plasma
    /// </summary>
    public static class ContainerSeparatorTypeKeys
    {
        /// <summary>
        /// Represents a gelatinous type of separator material.
        /// </summary>
        public static readonly Guid Gel = Guid.Parse("EE450FF6-9BED-4C47-90D2-671AB3041756");

        /// <summary>
        /// Represents no separator material is present in the container.
        /// </summary>
        public static readonly Guid None = Guid.Parse("472524EB-C8D4-49F8-862A-EF4BD7CA0395");
    }
}
