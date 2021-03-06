﻿/*
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
    /// Identifies the type of auditable objects in the system
    /// </summary>
    [XmlType(nameof(AuditableObjectType), Namespace = "http://santedb.org/audit")]
	public enum AuditableObjectType
	{
        /// <summary>
        /// Not specified
        /// </summary>
        [XmlEnum("u")]
        NotSpecified = 0,

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