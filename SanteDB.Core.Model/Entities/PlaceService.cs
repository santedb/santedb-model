/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-27
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.DataTypes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents a service for a place
    /// </summary>

    [XmlType("PlaceService", Namespace = "http://santedb.org/model"), JsonObject("PlaceService")]
    public class PlaceService : VersionedAssociation<Entity>
    {
        private Concept m_service;

        // Service key
        private Guid? m_serviceConceptKey;

        /// <summary>
        /// Gets or sets the service concept
        /// </summary>
        [SerializationReference(nameof(ServiceConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept ServiceConcept
        {
            get
            {
                this.m_service = base.DelayLoad(this.m_serviceConceptKey, this.m_service);
                return this.m_service;
            }
            set
            {
                this.m_service = value;
                this.m_serviceConceptKey = value?.Key;
            }
        }

        /// <summary>
        /// Gets or sets the service concept key
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("serviceConcept"), JsonProperty("serviceConcept")]
        public Guid? ServiceConceptKey
        {
            get { return this.m_serviceConceptKey; }
            set
            {
                if (this.m_serviceConceptKey != value)
                {
                    this.m_serviceConceptKey = value;
                    this.m_service = null;
                }
            }
        }

        // Service
        /// <summary>
        /// The schedule that the service is offered
        /// </summary>
        [XmlElement("serviceSchedule"), JsonProperty("serviceSchedule")]
        public String ServiceSchedule { get; set; }

        /// <summary>
        /// Semantic equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as PlaceService;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.ServiceConceptKey == other.ServiceConceptKey &&
                this.ServiceSchedule == other.ServiceSchedule;
        }
    }
}