/*
 * Copyright (C) 2021 - 2021, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors
 * Portions Copyright (C) 2015-2018 Mohawk College of Applied Arts and Technology
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
 * Date: 2021-8-5
 */
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Audit
{
    /// <summary>
    /// Represents potential outcomes.
    /// </summary>
    [XmlType(nameof(OutcomeIndicator), Namespace = "http://santedb.org/audit"), Flags]
	public enum OutcomeIndicator
	{
		/// <summary>
		/// Successful operation.
		/// </summary>
		[XmlEnum("ok")]
		Success = 1,

		/// <summary>
		/// Minor failure, action should be restarted.
		/// </summary>
		[XmlEnum("fail.minor")]
		MinorFail= 2, 

		/// <summary>
		/// Action was terminated.
		/// </summary>
		[XmlEnum("fail.major")]
		SeriousFail = 4, 

		/// <summary>
		/// Major failure, action is made unavailable.
		/// </summary>
		[XmlEnum("fail.epic")]
		EpicFail = 8
	}
}