/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: Justin Fyfe
 * Date: 2019-8-8
 */
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Auditing
{
    /// <summary>
    /// Represents types of actions
    /// </summary>
    [XmlType(nameof(ActionType), Namespace = "http://santedb.org/audit"), Flags]
	public enum ActionType
	{
		/// <summary>
		/// Data was created in the system
		/// </summary>
		[XmlEnum("c")]
		Create = 0x01,

		/// <summary>
		/// Data was viewed, printed, displayed, etc...
		/// </summary>
		[XmlEnum("r")]
		Read = 0x02,

		/// <summary>
		/// Data was revised in the system
		/// </summary>
		[XmlEnum("u")]
		Update = 0x04,

		/// <summary>
		/// Data was removed from the system
		/// </summary>
		[XmlEnum("d")]
		Delete = 0x08,

		/// <summary>
		/// A system, or application function was performed
		/// </summary>
		[XmlEnum("x")]
		Execute = 0x10
	}
}