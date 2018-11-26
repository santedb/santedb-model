﻿using System.Xml.Serialization;

namespace SanteDB.Core.Auditing
{
    /// <summary>
    /// Represents the type of network access point.
    /// </summary>
    [XmlType(nameof(NetworkAccessPointType), Namespace = "http://santedb.org/audit")]
	public enum NetworkAccessPointType
	{
		/// <summary>
		/// Represents an identifier which is a machine name.
		/// </summary>
		[XmlEnum("name")]
		MachineName = 0x1,

		/// <summary>
		/// Represents an identifier which is an IP address.
		/// </summary>
		[XmlEnum("ip")]
		IPAddress = 0x2,

		/// <summary>
		/// Represents an identifier which is a telephone number.
		/// </summary>
		[XmlEnum("tel")]
		TelephoneNumber = 0x3
	}
}