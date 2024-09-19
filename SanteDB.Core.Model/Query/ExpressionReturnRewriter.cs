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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Converts <typeparamref name="TFrom"/> to <typeparamref name="TTo"/>
    /// </summary>
    /// <remarks>
    /// Quick and dirty solution from https://stackoverflow.com/questions/14437239/change-a-linq-expression-predicate-from-one-type-to-another
    /// </remarks>
    public class ExpressionParameterRewriter<TFrom, TTo, TReturn> : ExpressionVisitor
    {
        private Dictionary<string, ParameterExpression> convertedParameters;
        private Expression<Func<TFrom, TReturn>> expression;

        /// <summary>
        /// Creates a new expression return visitor
        /// </summary>
        /// <param name="expresionToConvert">The type of expression to conviert</param>
        public ExpressionParameterRewriter(Expression<Func<TFrom, TReturn>> expresionToConvert)
        {
            //for each parameter in the original expression creates a new parameter with the same name but with changed type
            convertedParameters = expresionToConvert.Parameters
                .ToDictionary(
                    x => x.Name ?? "o",
                    x => Expression.Parameter(typeof(TTo), x.Name)
                );

            expression = expresionToConvert;
        }

        /// <summary>
        /// Perform the conversion logic 
        /// </summary>
        /// <returns>The converted expression with converted return type</returns>
        public Expression<Func<TTo, TReturn>> Convert()
        {
            return (Expression<Func<TTo, TReturn>>)Visit(expression);
        }

        /// <summary>
        /// Visit the specified member
        /// </summary>
        /// <param name="node">The node to visit</param>
        /// <returns>The mapped expressio</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TFrom))
            {
                var memeberInfo = typeof(TTo).GetMember(node.Member.Name).First();
                var newExp = Visit(node.Expression);
                return Expression.MakeMemberAccess(newExp, memeberInfo);
            }
            else
            {
                return base.VisitMember(node);
            }
        }

        /// <inheritdoc/>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (convertedParameters.TryGetValue(node.Name, out var newNode))
            {
                return newNode;
            }
            return node;
        }

        /// <inheritdoc/>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var newExp = Visit(node.Body);
            return Expression.Lambda(newExp, convertedParameters.Select(x => x.Value));
        }
    }
}