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
using SanteDB.Core.Model.Map;
using System;
using System.Linq.Expressions;

namespace SanteDB.Core.Model.Query
{

    /// <summary>
    /// Sorting instruction
    /// </summary>
    public struct ModelSort<TData>
    {

        /// <summary>
        /// Creates a new sort structure
        /// </summary>
        public ModelSort(LambdaExpression property, SortOrderType order)
        {
            this.SortProperty = Expression.Lambda<Func<TData, dynamic>>(Expression.Convert(property.Body, typeof(Object)), property.Parameters[0]);
            this.SortOrder = order;
        }

        /// <summary>
        /// Creates a new sort structure
        /// </summary>
        public ModelSort(Expression<Func<TData, dynamic>> property, SortOrderType order)
        {
            if (property.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException($"{nameof(property)} must be a LambdaExpression");
            }

            this.SortProperty = property;
            this.SortOrder = order;
        }

        /// <summary>
        /// Gets the sort order
        /// </summary>
        public SortOrderType SortOrder { get; }

        /// <summary>
        /// Sort properties
        /// </summary>
        public Expression<Func<TData, dynamic>> SortProperty { get; }
    }
}
