﻿/*
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
using System;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Classifier attribute used to mark a class' classifier
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassifierAttribute : Attribute, IPropertyReference
    {

        /// <summary>
        /// Classifier attribute property
        /// </summary>
        /// <param name="classProperty">The classifier property named classifier</param>
        public ClassifierAttribute(string classProperty)
        {
            this.ClassifierProperty = classProperty;
        }

        /// <summary>
        /// Classifier attribute property
        /// </summary>
        /// <param name="classProperty">The classifier property named classifier</param>
        /// <param name="keyProperty">The key property for the classifier used when UUIDs are queried</param>
        public ClassifierAttribute(string classProperty, string keyProperty)
        {
            this.ClassifierProperty = classProperty;
            this.ClassifierKeyProperty = keyProperty;
        }

        /// <summary>
        /// Gets or sets the classifier property
        /// </summary>
        public string ClassifierProperty { get; set; }

        /// <summary>
        /// Gets or sets the property to set as key when a UUID is used
        /// </summary>
        public string ClassifierKeyProperty { get; set; }

        /// <summary>
        /// Gets the classifier property
        /// </summary>
        string IPropertyReference.PropertyName => this.ClassifierProperty;

    }
}
