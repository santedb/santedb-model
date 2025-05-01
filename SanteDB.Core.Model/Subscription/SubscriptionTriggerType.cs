/*
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
using System;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Subscription
{

    /// <summary>
    /// Represents synchronization pull triggers
    /// </summary>
    [XmlType(nameof(SubscriptionTriggerType), Namespace = "http://santedb.org/subscription")]
    [Flags]
    public enum SubscriptionTriggerType
    {
        /// <summary>
        /// Never execute the trigger
        /// </summary>
        [XmlEnum("never")]
        Never = 0x0,
        /// <summary>
        /// Always execute the trigger
        /// </summary>
        [XmlEnum("always")]
        Always = OnStart | OnCommit | OnStop | OnPush | OnNetworkChange | PeriodicPoll,
        /// <summary>
        /// Only on start
        /// </summary>
        [XmlEnum("on-start")]
        OnStart = 0x01,
        /// <summary>
        /// Only on commit
        /// </summary>
        [XmlEnum("on-commit")]
        OnCommit = 0x02,
        /// <summary>
        /// Only on stop
        /// </summary>
        [XmlEnum("on-stop")]
        OnStop = 0x04,
        /// <summary>
        /// Only on push of data
        /// </summary>
        [XmlEnum("on-push")]
        OnPush = 0x08,
        /// <summary>
        /// Only when the network changes
        /// </summary>
        [XmlEnum("on-x-net")]
        OnNetworkChange = 0x10,
        /// <summary>
        /// Periodically poll
        /// </summary>
        [XmlEnum("periodic")]
        PeriodicPoll = 0x20,
        /// <summary>
        /// Only when manually pulling
        /// </summary>
        [XmlEnum("manual")]
        Manual = 0x40
    }
}
