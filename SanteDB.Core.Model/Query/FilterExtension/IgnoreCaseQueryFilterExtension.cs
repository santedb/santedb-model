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
using System.Linq.Expressions;
using System.Reflection;

namespace SanteDB.Core.Model.Query.FilterExtension
{
    /// <summary>
    /// Represents the lower query filter extension
    /// </summary>
    public class IgnoreCaseQueryFilterExtension : IQueryFilterExtension
    {

        /// <summary>
        /// Gets the lower extension name
        /// </summary>
        public string Name => "nocase";

        /// <summary>
        /// The actual method
        /// </summary>
        public MethodInfo ExtensionMethod => typeof(String).GetRuntimeMethod(nameof(String.ToLowerInvariant), Type.EmptyTypes);

        /// <summary>
        /// Compose the expression
        /// </summary>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            if (scope is MethodCallExpression callExpression) // We're calling a method so use the internal argument
            {
                return Expression.MakeBinary(comparison, Expression.Call(
                    Expression.Call(callExpression.Object, this.ExtensionMethod),
                    callExpression.Method,
                    Expression.Call(callExpression.Arguments[0], this.ExtensionMethod)
                ), Expression.Constant(true));
            }
            else
                return Expression.MakeBinary(comparison,
                    Expression.Call(scope, this.ExtensionMethod), Expression.Call(valueExpression, this.ExtensionMethod));
        }
    }
}
