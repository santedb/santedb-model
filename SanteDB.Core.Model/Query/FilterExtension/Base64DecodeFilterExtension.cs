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
    public class Base64DecodeFilterExtension : IQueryFilterConverterExtension
    {
        /// <inheritdoc/>
        public string Name => "b64decode";

        /// <inheritdoc/>
        public MethodInfo ExtensionMethod => typeof(ExtensionMethods).GetMethod(nameof(ExtensionMethods.ParseBase64UrlEncode));

        /// <inheritdoc/>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            return Expression.MakeBinary(comparison,
                scope,
                Expression.Call(this.ExtensionMethod, valueExpression));
        }
    }
}
