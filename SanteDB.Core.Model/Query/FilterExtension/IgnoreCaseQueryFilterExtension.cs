using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

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
            return Expression.MakeBinary(comparison,
                Expression.Call(scope, this.ExtensionMethod), Expression.Call(valueExpression, this.ExtensionMethod));
        }
    }
}
