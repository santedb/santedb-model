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
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents a class which has an immediate and primary outcome and is an alteration 
    /// of the physical condition of the subject.
    /// </summary>
    [XmlType(nameof(Procedure), Namespace = "http://santedb.org/model"), JsonObject(nameof(Procedure))]
    [XmlRoot(nameof(Procedure), Namespace = "http://santedb.org/model")]
    [ClassConceptKey(ActClassKeyStrings.Procedure)]
    public class Procedure : Act
    {

        /// <summary>
        /// Default ctor for procedure
        /// </summary>
        public Procedure()
        {
            base.ClassConceptKey = ActClassKeys.Procedure;
        }

        /// <summary>
        /// Gets or sets the class concept key
        /// </summary>
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        public override Guid? ClassConceptKey { get => ActClassKeys.Procedure; set => base.ClassConceptKey = ActClassKeys.Procedure; }

        /// <summary>
        /// Gets or sets te method/technique used to perform the procedure
        /// </summary>
        [XmlElement("method"), JsonProperty("method")]
        public Guid? MethodKey { get; set; }

        /// <summary>
        /// Gets or sets the anatomical site or system through which the procedure was performed
        /// </summary>
        [XmlElement("approachSite"), JsonProperty("approachSite")]
        public Guid? ApproachSiteKey { get; set; }

        /// <summary>
        /// Gets or sets the anatomical site or system which is the target of the procedure
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("targetSite"), JsonProperty("targetSite")]
        public Guid? TargetSiteKey { get; set; }

        /// <summary>
        /// Gets or sets te method/technique used to perform the procedure
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(MethodKey))]
        public Concept Method { get; set; }

        /// <summary>
        /// Gets or sets the anatomical site or system which is the target of the procedure
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ApproachSiteKey))]
        public Concept ApproachSite { get; set; }

        /// <summary>
        /// Gets or sets te method/technique used to perform the procedure
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(TargetSiteKey))]
        public Concept TargetSite { get; set; }

    }
}
