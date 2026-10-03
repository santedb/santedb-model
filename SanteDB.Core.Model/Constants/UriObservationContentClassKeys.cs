using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Link observation 
    /// </summary>
    public static class UriObservationContentClassKeys
    {
        /// <summary>
        /// Diagnostic imaging (WADO) or a PACS/RIS URL
        /// </summary>
        public static readonly Guid Imaging = Guid.Parse("f31eb278-89ed-4000-89a4-cae7f690d9a0");

        /// <summary>
        /// A remote structured data API object accessed via an API call
        /// </summary>
        public static readonly Guid StructuredDataApi = Guid.Parse("6e0f94ad-f35d-48a4-8bf4-effa415208f7");

        /// <summary>
        /// A viewer or other dynamic content - intended to be embedded or launched by an application user interface 
        /// </summary>
        public static readonly Guid UserInteractive = Guid.Parse("83e7d87a-ff0d-4b11-9f27-c799e7ce01d1");

        /// <summary>
        /// Streaming data from a device (EEG lead, continuous monitor, etc.)
        /// </summary>
        public static readonly Guid StreamingData = Guid.Parse("839e528f-4533-40b3-99e0-79923c1e0ab3");

        /// <summary>
        /// Static data which can be fetched and downloaded (a document, a page, a blob, a picture)
        /// </summary>
        public static readonly Guid StaticData = Guid.Parse("36ed1e75-5346-4a87-bc5d-78f054b51303");

    }
}
