/*
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
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SanteDB.Core.Model.EntityLoader
{
    /// <summary>
    /// Delay load provider
    /// </summary>
    public interface IEntitySourceProvider
    {

        /// <summary>
        /// Get the specified object
        /// </summary>
        TObject Get<TObject>(Guid? key) where TObject : IdentifiedData, new();

        /// <summary>
        /// Get the specified object
        /// </summary>
        TObject Get<TObject>(Guid? key, Guid? versionKey) where TObject : IdentifiedData, IVersionedEntity, new();

        /// <summary>
        /// Query the specified data from the delay load provider
        /// </summary>
        IEnumerable<TObject> Query<TObject>(Expression<Func<TObject, bool>> query) where TObject : IdentifiedData, new();

        /// <summary>
        /// Get relationships
        /// </summary>
        IEnumerable<TObject> GetRelations<TObject>(Guid? sourceKey, int? sourceVersionSequence) where TObject : IdentifiedData, IVersionedAssociation, new();

        /// <summary>
        /// Get relationships
        /// </summary>
        IEnumerable<TObject> GetRelations<TObject>(Guid? sourceKey) where TObject : IdentifiedData, ISimpleAssociation, new();


    }
}
