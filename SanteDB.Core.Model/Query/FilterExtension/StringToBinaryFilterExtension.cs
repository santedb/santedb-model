using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SanteDB.Core.Model.Query.FilterExtension
{
    /// <summary>
    /// Allows a user to query for binary data by passing a HEX, Base64 or plain-text
    /// </summary>
    public class StringToBinaryFilterExtension : IQueryFilterConverterExtension
    {
        /// <inheritdoc/>
        public string Name => "tobytes";

        /// <inheritdoc/>
        public MethodInfo ExtensionMethod => typeof(QueryModelExtensions).GetMethod(nameof(QueryModelExtensions.StringToBinary));

        /// <inheritdoc/>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            return Expression.MakeBinary(comparison,
                scope,
                Expression.Call(this.ExtensionMethod, valueExpression));
        }
    }
}
