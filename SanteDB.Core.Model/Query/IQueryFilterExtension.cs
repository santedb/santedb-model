/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: Justin Fyfe
 * Date: 2019-8-8
 */
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Represents a query filter extension
    /// </summary>
    public interface IQueryFilterExtension
    {

        /// <summary>
        /// Gets the name of the extension
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the return type of the function
        /// </summary>
        MethodInfo ExtensionMethod { get; }

        /// <summary>
        /// Construct the expression from the parameters on the query string
        /// </summary>
        /// <param name="scope">The scope of the current property</param>
        /// <param name="parms">The parameters on the query string</param>
        /// <param name="valueExpression">The operand</param>
        /// <param name="comparison">The type of comparison to be made</param>
        /// <returns></returns>
        /// <remarks>
        /// Basically this will take seomthing like <code>dateOfBirth=:(diff|&lt;=3w)2018-01-01</code> and
        /// turn it into <code>o.DateOfBirth.Diff("2018-01-01", "w") &lt;= 3</code>
        /// </remarks>
        BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms);

    }
}
