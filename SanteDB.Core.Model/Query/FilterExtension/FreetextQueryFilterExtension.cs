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
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SanteDB.Core.Model.Query.FilterExtension
{
    /// <summary>
    /// Represents a filter extension that uses the age of an object
    /// </summary>
    public class FreetextQueryFilterExtension : IQueryFilterExtension
    {

        /// <summary>
        /// The name of this filter extension
        /// </summary>
        public const string FilterName = "freetext";

        /// <summary>
        /// Get the name of the extendion
        /// </summary>
        public string Name => FilterName;

        /// <summary>
        /// Get the extension method
        /// </summary>
        public MethodInfo ExtensionMethod => typeof(QueryModelExtensions).GetMethod(nameof(QueryModelExtensions.FreetextSearch));

        /// <summary>
        /// Compose the expression
        /// </summary>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            // Is there a reference data?
            var rawParm = parms[0] as ConstantExpression;
            if (rawParm == null)
            {
                throw new InvalidOperationException("Freetext requires a constant string expression");
            }

            if (scope.Type.StripNullable() == typeof(Guid) && scope is MemberExpression mae) // We are anchored to the holder
            {
                while (mae != null && !typeof(IdentifiedData).IsAssignableFrom(mae.Expression.Type))
                {
                    mae = mae.Expression as MemberExpression;
                }
                if (mae != null)
                {
                    return Expression.MakeBinary(comparison, Expression.Call(null, this.ExtensionMethod, mae.Expression, rawParm), valueExpression);
                }
                else
                {
                    throw new InvalidOperationException("Freetext searches must be anchored on an id property - example: relationship.target.id=:(freetext... or id=:(freetext...");
                }
            }
            else
            {
                return Expression.MakeBinary(comparison, Expression.Call(null, this.ExtensionMethod, scope, rawParm), valueExpression);
            }
        }
    }
}
