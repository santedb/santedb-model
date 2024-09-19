/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents an act which indicates why data was created/changed
    /// </summary>
    /// <remarks>
    /// Control acts are typically container acts which are used to illustrate an event which occurs on the system to 
    /// change state, create data, or update it. The use of control acts in SanteDB are not required however may be 
    /// useful in contexts where event metadata is collected (similar to the EVN segment in HL7v2).
    /// </remarks>
    [XmlType(nameof(ControlAct), Namespace = "http://santedb.org/model"), JsonObject("ControlAct")]
    [ClassConceptKey(ActClassKeyStrings.ControlAct)]
    public class ControlAct : Act
    {
        /// <summary>
        /// Control act
        /// </summary>
        public ControlAct()
        {
            this.m_classConceptKey = ActClassKeys.ControlAct;
        }

        /// <summary>
        /// Gets or sets the class concept key
        /// </summary>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == ActClassKeys.ControlAct;
    }
}
