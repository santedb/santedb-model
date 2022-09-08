﻿/*
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
    /// Represents a filter extension that uses the age of an object
    /// </summary>
    public class AgeQueryFilterExtension : IQueryFilterExtension
    {
        /// <summary>
        /// Get the name of the extendion
        /// </summary>
        public string Name => "age";

        /// <summary>
        /// Get the extension method
        /// </summary>
        public MethodInfo ExtensionMethod => typeof(QueryModelExtensions).GetMethod(nameof(QueryModelExtensions.Age));

        /// <summary>
        /// Compose the expression
        /// </summary>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            // Is there a reference data?
            var rawParm = parms[0] as ConstantExpression;
            if (rawParm == null || !DateTime.TryParse(rawParm.Value.ToString(), out DateTime parsedParm))
                parsedParm = DateTime.Now;


            if (scope.Type == typeof(DateTimeOffset))
                scope = Expression.MakeMemberAccess(scope, typeof(DateTimeOffset).GetProperty(nameof(DateTimeOffset.DateTime)));
            return Expression.MakeBinary(comparison, Expression.Call(null, this.ExtensionMethod, scope, Expression.Constant(parsedParm)), valueExpression);

        }
    }
}
