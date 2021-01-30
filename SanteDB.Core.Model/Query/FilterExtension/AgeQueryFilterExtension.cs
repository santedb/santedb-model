using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

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
        public MethodInfo ExtensionMethod => typeof(ExtensionMethods).GetMethod(nameof(ExtensionMethods.Age));

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
