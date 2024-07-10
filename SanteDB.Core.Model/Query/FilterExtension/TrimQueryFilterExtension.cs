using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SanteDB.Core.Model.Query.FilterExtension
{
    /// <summary>
    /// Trims the whitespace from a string
    /// </summary>
    public class TrimQueryFilterExtension : IQueryFilterExtension
    {
        /// <inheritdoc/>
        public string Name => "trim";

        /// <inheritdoc/>
        public MethodInfo ExtensionMethod => typeof(String).GetRuntimeMethod(nameof(String.Trim), Type.EmptyTypes);

        /// <inheritdoc/>
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
            {
                return Expression.MakeBinary(comparison,
                    Expression.Call(scope, this.ExtensionMethod), Expression.Call(valueExpression, this.ExtensionMethod));
            }
        }
    }
}
