/*
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2021-2-9
 */
using System.Xml.Serialization;

namespace SanteDB.Core.Auditing
{
    /// <summary>
    /// Auditable object lifecycle
    /// </summary>
    [XmlType(nameof(AuditableObjectLifecycle), Namespace = "http://santedb.org/audit")]
	public enum AuditableObjectLifecycle
	{
        /// <summary>
        /// Not set
        /// </summary>
		[XmlEnum("none")]
        NotSet = 0x0,

        /// <summary>
        /// An object was created
        /// </summary>
		[XmlEnum("create")]
		Creation = 0x01,

        /// <summary>
        /// An object was imported from an external source
        /// </summary>
		[XmlEnum("import")]
		Import = 0x02,

        /// <summary>
        /// An object was amended (updated)
        /// </summary>
		[XmlEnum("amend")]
		Amendment = 0x03,

        /// <summary>
        /// An object was verified
        /// </summary>
		[XmlEnum("verif")]
		Verification = 0x04,

        /// <summary>
        /// An object wsa transformed
        /// </summary>
		[XmlEnum("xfrm")]
		Translation = 0x05,

        /// <summary>
        /// An object was accessed
        /// </summary>
		[XmlEnum("access")]
		Access = 0x06,

        /// <summary>
        /// An object was de-identified
        /// </summary>
		[XmlEnum("deid")]
		Deidentification = 0x07,

        /// <summary>
        /// An object was aggregated with another group of objects
        /// </summary>
		[XmlEnum("agg")]
		Aggregation = 0x08,

        /// <summary>
        /// An object was reported on
        /// </summary>
		[XmlEnum("rpt")]
		Report = 0x09,

        /// <summary>
        /// An object was exported to another system
        /// </summary>
		[XmlEnum("export")]
		Export = 0x0a,

        /// <summary>
        /// An object was disclosed to a user
        /// </summary>
		[XmlEnum("disclose")]
		Disclosure = 0x0b,

        /// <summary>
        /// The object was the receipt of a disclosure
        /// </summary>
		[XmlEnum("rcpdisclose")]
		ReceiptOfDisclosure = 0x0c,

        /// <summary>
        /// The object was archived
        /// </summary>
		[XmlEnum("arch")]
		Archiving = 0x0d,

        /// <summary>
        /// The object was obsoleted (logically deleted)
        /// </summary>
		[XmlEnum("obsolete")]
		LogicalDeletion = 0x0e,

        /// <summary>
        /// The object was perminently deleted
        /// </summary>
		[XmlEnum("delete")]
		PermanentErasure = 0x0f
	}
}