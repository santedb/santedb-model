using System;
using System.Collections.Generic;
using System.Text;

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
