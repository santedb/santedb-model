using SanteDB.Core.i18n;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SanteDB.Core.Model.Query.FilterExtension
{
    /// <summary>
    /// Allows for the query of an object based on a modified since
    /// </summary>
    public class LastModifiedFilterExtension : IQueryFilterConverterExtension
    {
        /// <inheritdoc/>
        public string Name => "lastModified";

        /// <inheritdoc/>
        public MethodInfo ExtensionMethod => typeof(ExtensionMethods).GetMethod(nameof(ExtensionMethods.LastModified));

        /// <inheritdoc/>
        public BinaryExpression Compose(Expression scope, ExpressionType comparison, Expression valueExpression, Expression[] parms)
        {
            
            // Next we want to call our method and pass that as the first parameter
            return Expression.MakeBinary(comparison,
                Expression.Call(this.ExtensionMethod, scope),  // Calls the extension method for getting last modified
                valueExpression); // Pass the filter expression
        }
    }
}
