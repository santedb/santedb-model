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
 * Date: 2024-6-21
 */
namespace SanteDB.Core.Model
{
    /// <summary>
    /// Extended mime types
    /// </summary>
    public static class SanteDBExtendedMimeTypes
    {

        /// <summary>
        /// Application root (extended registration)
        /// </summary>
        internal const string ApplicationRoot = "application/x.santedb";

        /// <summary>
        /// CDSS logic in text format
        /// </summary>
        public const string CdssTextFormat = ApplicationRoot + ".cdss";

        /// <summary>
        /// RIM based model root
        /// </summary>
        public const string RimModelRoot = ApplicationRoot + ".rim";

        /// <summary>
        /// RIM in JSON
        /// </summary>
        public const string JsonRimModel = RimModelRoot + "+json";

        /// <summary>
        /// RIM in XML
        /// </summary>
        public const string XmlRimModel = RimModelRoot + "+xml";

        /// <summary>
        /// RIM in ViewModel
        /// </summary>
        public const string JsonViewModel = RimModelRoot + ".viewModel+json";

        /// <summary>
        /// Patch 
        /// </summary>
        public const string PatchRoot = ApplicationRoot + ".patch";

        /// <summary>
        /// Patch in XML
        /// </summary>
        public const string XmlPatch = PatchRoot + "+xml";

        /// <summary>
        /// PAtch in Json
        /// </summary>
        public const string JsonPatch = PatchRoot + "+json";

        /// <summary>
        /// Visual resource pointer
        /// </summary>
        public const string VisualResourcePointer = ApplicationRoot + ".vrp";
    }
}
