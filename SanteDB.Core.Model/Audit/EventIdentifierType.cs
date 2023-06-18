/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-5-19
 */
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Audit
{
    /// <summary>
    /// Represents an event identifier type.
    /// </summary>
    [XmlType(nameof(EventIdentifierType), Namespace = "http://santedb.org/audit"), Flags]
    public enum EventIdentifierType
    {
        /// <summary>
        /// Represents a provisioning event.
        /// </summary>
        [XmlEnum("provision")]
        ProvisioningEvent = 0x01,

        /// <summary>
        /// Represents a medication event.
        /// </summary>
        [XmlEnum("medication")]
        MedicationEvent = 0x02,

        /// <summary>
        /// Represents a resource assignment.
        /// </summary>
        [XmlEnum("resource")]
        ResourceAssignment = 0x04,

        /// <summary>
        /// Represents a care episode.
        /// </summary>
        [XmlEnum("careep")]
        CareEpisode = 0x08,

        /// <summary>
        /// Represents a care protocol.
        /// </summary>
        [XmlEnum("careprotocol")]
        CareProtocol = 0x10,

        /// <summary>
        /// Represents a procedure record.
        /// </summary>
        [XmlEnum("procedure")]
        ProcedureRecord = 0x20,

        /// <summary>
        /// Represents a query.
        /// </summary>
        [XmlEnum("query")]
        Query = 0x40,

        /// <summary>
        /// Represents a patient record.
        /// </summary>
        [XmlEnum("patient")]
        PatientRecord = 0x80,

        /// <summary>
        /// Represents an order record.
        /// </summary>
        [XmlEnum("order")]
        OrderRecord = 0x100,

        /// <summary>
        /// Represents a network entry.
        /// </summary>
        [XmlEnum("network")]
        NetworkActivity = 0x200,

        /// <summary>
        /// Represents an import.
        /// </summary>
        [XmlEnum("import")]
        Import = 0x400,

        /// <summary>
        /// Represents an export.
        /// </summary>
        [XmlEnum("export")]
        Export = 0x800,

        /// <summary>
        /// Represents application activity.
        /// </summary>
        [XmlEnum("application")]
        ApplicationActivity = 0x1000,

        /// <summary>
        /// Represents a security alert.
        /// </summary>
        [XmlEnum("security")]
        SecurityAlert = 0x2000,

        /// <summary>
        /// Represents user authentication.
        /// </summary>
        [XmlEnum("auth")]
        UserAuthentication = 0x4000,

        /// <summary>
        /// Represents that an emergency override started.
        /// </summary>
        [XmlEnum("btg")]
        EmergencyOverrideStarted = 0x8000,

        /// <summary>
        /// Represents the use of a restricted function.
        /// </summary>
        [XmlEnum("restrictedFn")]
        UseOfRestrictedFunction = 0x10000,

        /// <summary>
        /// Represents a login.
        /// </summary>
        [XmlEnum("login")]
        Login = 0x20000,

        /// <summary>
        /// Represents a logout.
        /// </summary>
        [XmlEnum("logout")]
        Logout = 0x40000
    }

}
