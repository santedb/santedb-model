using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// Construct the expression from the parameters on the query string
        /// </summary>
        /// <param name="scope">The scope of the current property</param>
        /// <param name="parms">The parameters on the query string</param>
        /// <param name="operand">The operand</param>
        /// <returns></returns>
        /// <remarks>
        /// Basically this will take seomthing like <code>dateOfBirth=:(diff|&lt;=3w)2018-01-01</code> and
        /// turn it into <code>o.DateOfBirth.Diff("2018-01-01", "w") &lt;= 3</code>
        /// </remarks>
        BinaryExpression Compose(Expression scope, String[] parms, Object operand);

        /// <summary>
        /// Allows the filter extension to detect if it is present for 
        /// </summary>
        /// <param name="expression">The binary expression to detect if this filter extension has been generated on</param>
        /// <returns>True if the filter extension detects its own presence on the expression</returns>
        bool Detect(BinaryExpression expression);

        /// <summary>
        /// De-compose the LINQ expression to a string representation of the function
        /// </summary>
        /// <param name="expression">The expression to decompose</param>
        /// <returns>The query data for the binary expression</returns>
        KeyValuePair<String, Object> DeCompose(BinaryExpression expression);
    }
}
