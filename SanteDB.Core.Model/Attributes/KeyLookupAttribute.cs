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

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Identifies to the persistence layer what property can be used for lookup when a key is not present
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class KeyLookupAttribute : Attribute
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public KeyLookupAttribute(String uniqueProperty)
        {
            this.UniqueProperty = uniqueProperty;
        }

        /// <summary>
        /// Gets or sets whether the persistence engine should throw an exception when persisting duplicates
        /// </summary>
        public String UniqueProperty { get; set; }

    }
}
