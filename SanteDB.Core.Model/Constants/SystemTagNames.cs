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
namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// A list of tag names used by the system
    /// </summary>
    public static class SystemTagNames
    {

        /// <summary>
        /// Used to convey back to the caller any BRE errors
        /// </summary>
        public const string BreErrorTag = "$bre.error";
        /// <summary>
        /// The matching score tag for a query result
        /// </summary>
        public const string MatchScoreTag = "$match.score";
        /// <summary>
        /// Indicates that data originated from the configured upstream
        /// </summary>
        public const string UpstreamDataTag = "$upstream";
        /// <summary>
        /// The privacy protection method
        /// </summary>
        public const string PrivacyProtectionMethodTag = "$pep.method";
        /// <summary>
        /// The privacy masking tag
        /// </summary>
        public const string PrivacyMaskingTag = "$pep.masked";
        /// <summary>
        /// Alternate keys tag for loading alternate objects from the database
        /// </summary>
        public const string AlternateKeysTag = "$alt.keys";

        /// <summary>
        /// Indicates that a dCDR needs to re-query or re-fetch the data
        /// </summary>
        public const string DcdrRefetchTag = "$dcdr.refetch";

        /// <summary>
        /// Indicates that an object is checked out
        /// </summary>
        public const string CheckoutStatusTag = "$checkoutState";
        /// <summary>
        /// Indicates that data was synthesized from a variety of other records and does not actually exist
        /// </summary>
        public const string GeneratedDataTag = "$generated";
        /// <summary>
        /// Indicates that the data is tagged for back-entry
        /// </summary>
        public const string BackEntry = "isBackEntry";

        /// <summary>
        /// Minimum value allowed for an observation
        /// </summary>
        public const string CdssMaxValue = "$cdss.maxValue";
        /// <summary>
        /// Minimum value allowed for an observation
        /// </summary>
        public const string CdssMinValue = "$cdss.minValue";
        /// <summary>
        /// Only allow on of the protocol emissions per visit
        /// </summary>
        public const string CdssOnePerVisit = "$cdss.onePerVisit";
        /// <summary>
        /// Overwrite a component
        /// </summary>
        public const string CdssOverwriteComponent = "$cdss.overwriteComponent";

        /// <summary>
        /// CDSS order by tag
        /// </summary>
        public const string CdssOrderTag = "$cdss.order";

        /// <summary>
        /// Skip matching
        /// </summary>
        public const string SkipDuplicateCheck = "$match.skip";
    }
}
