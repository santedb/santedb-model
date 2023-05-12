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

    }
}
