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
    /// Represents a collection of container type keys.
    /// </summary>
    public static class ContainerTypeKeys
    {

        /// <summary>
        /// Represents a container type of box.
        /// </summary>
        public static readonly Guid BoxOrCrate = Guid.Parse("03d52794-756a-4f9a-a17e-edd44f36c244");

        /// <summary>
        /// Represents a container type of vial.
        /// </summary>
        public static readonly Guid Vial = Guid.Parse("035e63b7-854c-4ba8-a115-fb74ce4b6c8e");

        /// <summary>
        /// Represents a box which is insulated to keep contents cold
        /// </summary>
        public static readonly Guid ColdBoxOrFlask = Guid.Parse("6AE1F173-60FA-4540-B7CF-957775AE5986");

        /// <summary>
        /// Represents an actively powered cold storage unit which typically keeps contents between -4 and 8 degrees celsius
        /// </summary>
        public static readonly Guid Refrigerator = Guid.Parse("9077f00c-0f9a-4030-b1db-f0c3671ad8f4");

        /// <summary>
        /// Represents a freezer which typically keeps contents under -18 degrees celcius
        /// </summary>
        public static readonly Guid Freezer = Guid.Parse("e2800131-c008-4d11-b8b5-e98527cf8c22");

        /// <summary>
        /// Represents a non-refrigerated room where supplies are kept
        /// </summary>
        public static readonly Guid PantryOrStoreRoom = Guid.Parse("e2a19772-5c11-4af9-aa5c-68206040b72b");

    }
}
