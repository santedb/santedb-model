using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

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
        public MethodInfo ExtensionMethod => typeof(QueryModelExtensions).GetMethod(nameof(QueryModelExtensions.DateTrunc));

        /// <summary>
        /// Compose the expression
        /// </summary>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            if (parms.Length == 0)
                throw new InvalidOperationException("Date truncation requires precision");
            else
            {
                return Expression.MakeBinary(ExpressionType.Equal,
                    Expression.Call(this.ExtensionMethod, Expression.Convert(scope, typeof(DateTime)), parms[0]),
                    Expression.Call(this.ExtensionMethod, valueExpression, parms[0]));
            }
        }
    }
}
