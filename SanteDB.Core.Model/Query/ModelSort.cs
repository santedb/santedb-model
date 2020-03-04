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
using SanteDB.Core.Model;
using SanteDB.Core.Model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
                throw new ArgumentException($"{nameof(property)} must be a LambdaExpression");
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
