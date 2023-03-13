/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-3-10
 */
using System;

namespace SanteDB.Core.Model.Map.Builder
{

    /// <summary>
    /// Represents a model mapper instance
    /// </summary>
    public interface IModelMapper
    {

        /// <summary>
        /// Source type
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// Target type
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Map from source to target
        /// </summary>
        Object MapToTarget(Object source);

        /// <summary>
        /// Map from target to source
        /// </summary>
        Object MapToSource(Object target);

    }

    /// <summary>
    /// Model mapper
    /// </summary>
    public interface IModelMapper<TSource, TTarget> : IModelMapper
        where TSource : new()
        where TTarget : new()
    {

        /// <summary>
        /// Maps <paramref name="source"/> from <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>
        /// </summary>
        /// <param name="source"></param>
        TTarget MapToTarget(TSource source);

        /// <summary>
        /// Performs a reverse map on <paramref name="target"/> to <typeparamref name="TTarget"/> from <typeparamref name="TSource"/>
        /// </summary>
        TSource MapToSource(TTarget target);

    }
}
