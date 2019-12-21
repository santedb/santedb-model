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
using System.Reflection;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Identifies where tools can find the serialization information
    /// for an ignored property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializationReferenceAttribute : Attribute
    {
        /// <summary>
        /// The redirection attribute
        /// </summary>
        public SerializationReferenceAttribute(String redirectProperty)
        {
            this.RedirectProperty = redirectProperty;
        }

        /// <summary>
        /// Identifies where the serialization information can be found
        /// </summary>
        public String RedirectProperty { get; set; }

        /// <summary>
        /// Get property from the type
        /// </summary>
        public PropertyInfo GetProperty(Type hostType)
        {
            return hostType.GetRuntimeProperty(this.RedirectProperty);
        }
    }
}
