/*
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
 * User: fyfej
 * Date: 2023-6-21
 */
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Query;
using System;
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
        TObject Get<TObject>(Guid? key, Guid? versionKey) where TObject : IdentifiedData, new();

        /// <summary>
        /// Query the specified data from the delay load provider
        /// </summary>
        IQueryResultSet<TObject> Query<TObject>(Expression<Func<TObject, bool>> query) where TObject : IdentifiedData, new();


        /// <summary>
        /// Get relationships
        /// </summary>
        IQueryResultSet<TObject> GetRelations<TObject>(params Guid?[] sourceKey) where TObject : IdentifiedData, ISimpleAssociation, new();


        /// <summary>
        /// Get relationships
        /// </summary>
        /// <param name="relatedType">The related type to load</param>
        /// <param name="sourceKey">The source keys to load relationships for</param>
        IQueryResultSet GetRelations(Type relatedType, params Guid?[] sourceKey);

    }
}