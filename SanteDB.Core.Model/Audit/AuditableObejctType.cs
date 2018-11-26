﻿using System.Xml.Serialization;

namespace SanteDB.Core.Auditing
{
    /// <summary>
    /// Identifies the type of auditable objects in the system
    /// </summary>
    [XmlType(nameof(AuditableObjectType), Namespace = "http://santedb.org/audit")]
	public enum AuditableObjectType
	{
		/// <summary>
		/// Represents a person.
		/// </summary>
		[XmlEnum("p")]
		Person = 1,

		/// <summary>
		/// Represents a system object.
		/// </summary>
		[XmlEnum("s")]
		SystemObject = 2,

		/// <summary>
		/// Represents an organization.
		/// </summary>
		[XmlEnum("o")]
		Organization = 3,

		/// <summary>
		/// Represents an other object type.
		/// </summary>
		[XmlEnum("x")]
		Other = 4
	}
}