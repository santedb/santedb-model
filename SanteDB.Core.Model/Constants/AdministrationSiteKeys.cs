/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
    /// Keys for administration sites
    /// </summary>
    public static class AdministrationSiteKeys
    {
        /// <summary>
        /// Administration site is left arm
        /// </summary>
        public static readonly Guid LeftArm = Guid.Parse("DD5DB8ED-0D97-4728-BD94-27AACD79EA02"); // Left Arm                                                                                                                                                                                                                                                                                                                                                                                    
        /// <summary>
        /// Administration site is right arm
        /// </summary>
        public static readonly Guid RightArm = Guid.Parse("13C8EC88-F0B8-47A3-850A-522616855106"); // Right Arm                                                                                                                                                                                                                                                                                                                                                                                  
        /// <summary>
        /// Administration site is left thigh
        /// </summary>
        public static readonly Guid LeftThigh = Guid.Parse("1EDC643D-A93A-47ED-8737-06EDE53D0E1F"); // Left Thigh                                                                                                                                                                                                                                                                                                                                                                                
        /// <summary>
        /// Administration site is right thigh
        /// </summary>
        public static readonly Guid RightThigh = Guid.Parse("6D607C13-5A87-40B2-91CD-79F000BD82E4"); // Right Thigh                                                                                                                                                                                                                                                                                                                                                                              
        /// <summary>
        /// Administration site is buttox
        /// </summary>
        public static readonly Guid Buttox = Guid.Parse("D1A20F09-E5A8-4030-80BA-6060FCA3A268"); // Buttox                                                                                                                                                                                                                                                                                                                                                                                        
        /// <summary>
        /// Administration site is Oral
        /// </summary>
        public static readonly Guid Oral = Guid.Parse("E5B6847E-91E0-4FCA-AC2E-753962008080"); // Oral        
    }
}
