﻿/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-6-21
 */
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Audit
{
    /// <summary>
    /// Classifies the type of identifier that a auditable object may have
    /// </summary>
    [XmlType(nameof(AuditableObjectIdType), Namespace = "http://santedb.org/audit")]
    public enum AuditableObjectIdType
    {


        /// <remarks>Use with object type code Person</remarks>
        [XmlEnum("na")]
        NotSpecified = 0x00,

        /// <remarks>Use with object type code Person</remarks>
        [XmlEnum("mrn")]
        MedicalRecord = 0x01,

        /// <remarks>Use with object type code Person</remarks>
        [XmlEnum("pid")]
        PatientNumber = 0x02,

        /// <remarks>Use with object type code Person</remarks>
        [XmlEnum("ern")]
        EncounterNumber = 0x03,

        /// <remarks>Use with object type code Person</remarks>
        [XmlEnum("enrl")]
        EnrolleeNumber = 0x04,

        /// <remarks>Use with object type code Person</remarks>
        [XmlEnum("ssn")]
        SocialSecurityNumber = 0x05,

        /// <remarks>Use with object type code Person</remarks>
        [XmlEnum("acct")]
        AccountNumber = 0x06,

        /// <remarks>Use with object type code Person or Organization</remarks>
        [XmlEnum("guar")]
        GuarantorNumber = 0x07,

        /// <remarks>Use with object type code SystemObject</remarks>
        [XmlEnum("rpt")]
        ReportName = 0x08,

        /// <remarks>Use with object type code SystemObject</remarks>
        [XmlEnum("rpn")]
        ReportNumber = 0x09,

        /// <remarks>Use with object type code SystemObject</remarks>
        [XmlEnum("srch")]
        SearchCritereon = 0x0a,

        /// <remarks>Use with object type code Person or SystemObject</remarks>
        [XmlEnum("uid")]
        UserIdentifier = 0x0b,

        /// <remarks>Use with object type code SystemObject</remarks>
        [XmlEnum("uri")]
        Uri = 0x0c,

        /// <summary>
        /// Custom code
        /// </summary>
        [XmlEnum("ext")]
        Custom = 0x0d
    }
}