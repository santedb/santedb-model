using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents a narrative object in the reference model
    /// </summary>
    /// <remarks>
    /// <para>The narrative class allows for the composition of documents (with class code DOCCLIN) or a section
    /// (with class code DOCSECT) within a broader narrative. It also allows for the generation and storage of CDA</para>
    /// </remarks>
    [XmlType(nameof(Narrative), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(Narrative), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(Narrative))]
    public class Narrative : Act
    {
        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == ActClassKeys.Document || classKey == ActClassKeys.DocumentSection;

        /// <summary>
        /// The external version number of the structured document
        /// </summary>
        [XmlElement("versionCode"), JsonProperty("versionCode")]
        public String VersionNumber { get; set; }

        /// <summary>
        /// The language in which the document content is written
        /// </summary>
        [XmlElement("language"), JsonProperty("language")]
        public String LanguageCode { get; set; }

        /// <summary>
        /// The title of the clinical document
        /// </summary>
        [XmlElement("title"), JsonProperty("title")]
        public String Title { get; set; }

        /// <summary>
        /// Gets or sets the text of the document
        /// </summary>
        [XmlElement("text"), JsonProperty("text")]
        public String Text { get; set; }
    }
}