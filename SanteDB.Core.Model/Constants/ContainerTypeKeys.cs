﻿/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
    /// Represents a collection of container type keys.
    /// </summary>
    public static class ContainerTypeKeys
    {
        /// <summary>
        /// Represents a container type of box.
        /// </summary>
        public static readonly Guid Boxes = Guid.Parse("6CFDA504-ED76-4BC9-B77B-40D4ED09E93D");

        /// <summary>
        /// Represents a container type of vial.
        /// </summary>
        public static readonly Guid Vials = Guid.Parse("907DDBFB-91B3-439E-8DF2-67925DCF4625");
    }
}
