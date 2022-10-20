﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Tickles
{
    /// <summary>
    /// Represents a tickle message
    /// </summary>
    [JsonObject(nameof(Tickle))]
    [XmlType(nameof(Tickle), Namespace = "http://santedb.org/appService/tickle")]
    [XmlRoot(nameof(Tickle), Namespace = "http://santedb.org/appService/tickle")]
    public class Tickle
    {

        /// <summary>
        /// Creates a an empty tickle
        /// </summary>
        public Tickle()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
        }

        /// <summary>
        /// Creates a new tickle
        /// </summary>
        public Tickle(Guid to, TickleType type, string text, DateTime? expiry = null) : this()
        {
            Target = to;
            Type = type;
            Text = text;
            Expiry = expiry ?? DateTime.MaxValue;
        }

        /// <summary>
        /// Identifier of the tickle
        /// </summary>
        [JsonProperty("id"), XmlAttribute("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        [JsonProperty("text"), XmlText]
        public string Text { get; set; }

        /// <summary>
        /// Gets the type of tickle
        /// </summary>
        [JsonProperty("type"), XmlAttribute("type")]
        public TickleType Type { get; set; }

        /// <summary>
        /// Gets or sets the expiration of the tickle
        /// </summary>
        [JsonProperty("exp"), XmlAttribute("exp")]
        public DateTime Expiry { get; set; }

        /// <summary>
        /// The time the tickle was created
        /// </summary>
        [JsonProperty("creationTime"), XmlAttribute("creationTime")]
        public DateTime Created { get; set; }

        /// <summary>
        /// The target of the tickle
        /// </summary>
        [JsonProperty("target"), XmlAttribute("to")]
        public Guid Target { get; set; }

    }
}
