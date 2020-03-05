/*
 * Based on OpenIZ, Copyright (C) 2015 - 2019 Mohawk College of Applied Arts and Technology
 * Copyright (C) 2019 - 2020, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2019-11-27
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Subscription
{

    /// <summary>
    /// The subscription modes in which a filter definition applies
    /// </summary>
    [XmlType(nameof(SubscriptionModeType), Namespace = "http://santedb.org/subscription")]
    public enum SubscriptionModeType
    {
        /// <summary>
        /// Only visible when subscription mode is selected
        /// </summary>
        [XmlEnum("subscription")]
        Subscription = 1,
        /// <summary>
        /// Only visible when all data (no specific item) is selected
        /// </summary>
        [XmlEnum("all")]
        All = 2,
        /// <summary>
        /// Always visible
        /// </summary>
        [XmlEnum("*")]
        AllOrSubscription = 3
    }


}
