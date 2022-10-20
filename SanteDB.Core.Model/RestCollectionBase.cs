using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Base type for collections returned from Rest Endpoints. 
    /// </summary>
    public abstract class RestCollectionBase
    {
        /// <summary>
        /// Initializes an empty list of collection items.
        /// </summary>
        public RestCollectionBase()
        {
            CollectionItem = new List<object>();
        }

        /// <summary>
        /// Initializes with a specific collection of items.
        /// </summary>
        public RestCollectionBase(IEnumerable<object> collectionItems)
        {
            CollectionItem = new List<object>(collectionItems);
        }

        /// <summary>
        /// Initializes with a specific collection of items and a known offset and total count.
        /// </summary>
        public RestCollectionBase(IEnumerable<object> collectionItems, int offset, int totalCount)
        {
            CollectionItem = new List<object>(collectionItems);
            Offset = offset;
            Size = totalCount;
        }

        /// <summary>
        /// Gets or sets a list of collection items.
        /// </summary>
        [XmlElement("resource"), JsonProperty("resource")]
        public List<object> CollectionItem { get; set; }

        /// <summary>
        /// Gets or sets the total offset.
        /// </summary>
        [XmlAttribute("offset"), JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the total collection size.
        /// </summary>
        [XmlAttribute("size"), JsonProperty("size")]
        public int Size { get; set; }
    }
}
