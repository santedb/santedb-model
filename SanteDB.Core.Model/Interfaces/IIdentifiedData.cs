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
using System;
using System.Collections.Generic;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Identified entity
    /// </summary>
    public interface IIdentifiedData
    {


        /// <summary>
        /// Gets the identifier for the entity
        /// </summary>
        Guid? Key { get; set; }

        /// <summary>
        /// Remove annotation
        /// </summary>
        void RemoveAnnotation(Object annotation);

        /// <summary>
        /// Remove annotations of <typeparamref name="T"/>
        /// </summary>
        void RemoveAnnotations<T>();

        /// <summary>
        /// Get annotations of specified <typeparamref name="T"/>
        /// </summary>
        IEnumerable<T> GetAnnotations<T>();

        /// <summary>
        /// Add an annotated object
        /// </summary>
        void AddAnnotation<T>(T annotation);

        /// <summary>
        /// Copy annotations
        /// </summary>
        IIdentifiedData CopyAnnotations(IIdentifiedData other);
    }
}