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
 * Date: 2022-5-30
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Security Entity base class
    /// </summary>
    [XmlType(Namespace = "http://santedb.org/model", TypeName = "SecurityEntity")]
    [JsonObject(nameof(SecurityEntity))]
    [NonCached]
    public class SecurityEntity : NonVersionedEntityData, ISecurable
    {

        /// <summary>
        /// Policies applied to this entity
        /// </summary>
        protected List<SecurityPolicyInstance> m_policies = new List<SecurityPolicyInstance>();

        /// <summary>
        /// Policies associated with the entity
        /// </summary>
        [XmlIgnore, JsonIgnore, Obsolete("Use IPolicyInformationService.GetPolicies", true)]
        public virtual List<SecurityPolicyInstance> Policies
        {
            get
            {
                return this.m_policies;
            }
            set
            {
                this.m_policies = value;
            }
        }

        /// <summary>
        /// Add a policy to this act
        /// </summary>
        [Obsolete("Use IPolicyInformationService.AddPolicies", true)]
        public void AddPolicy(string policyId)
        {
        }

        /// <summary>
        /// Returns true if this object has the specified policy applied
        /// </summary>
        [Obsolete("Use IPolicyInformationService.HasPolicy", true)]
        public bool HasPolicy(string policyId)
        {
            return this.Policies.Any(o => o.Policy.Oid == policyId);
        }
    }
}
