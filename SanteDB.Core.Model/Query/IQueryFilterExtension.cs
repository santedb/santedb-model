using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
