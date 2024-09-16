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
 */
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SanteDB.Core.Model.Query.FilterExtension
{
    /// <summary>
    /// Filter expression that truncates a date 
    /// </summary>
    public class DateTruncateFilterExtension : IQueryFilterExtension
    {
        /// <summary>
        /// The name of the query filter extension
        /// </summary>
        public string Name => "date_trunc";

        /// <summary>
        /// Gets the extension method
        /// </summary>
        public MethodInfo ExtensionMethod => typeof(QueryModelExtensions).GetMethod(nameof(QueryModelExtensions.DateTrunc), new Type[] { typeof(DateTimeOffset), typeof(String) });

        /// <summary>
        /// Compose the expression
        /// </summary>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            if (parms.Length == 0)
            {
                throw new InvalidOperationException("Date truncation requires precision");
            }
            else if (valueExpression.Type == typeof(DateTime))
            {
                var func = typeof(QueryModelExtensions).GetMethod(nameof(QueryModelExtensions.DateTrunc), new Type[] { typeof(DateTime), typeof(String) });
                return Expression.MakeBinary(ExpressionType.Equal,
                    Expression.Call(func, Expression.Convert(scope, typeof(DateTime)), parms[0]),
                    Expression.Call(func, valueExpression, parms[0]));
            }
            else
            {
                return Expression.MakeBinary(ExpressionType.Equal,
                    Expression.Call(this.ExtensionMethod, Expression.Convert(scope, typeof(DateTimeOffset)), parms[0]),
                    Expression.Call(this.ExtensionMethod, valueExpression, parms[0]));
            }
        }
    }
}
