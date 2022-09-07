using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Converts <typeparamref name="TFrom"/> to <typeparamref name="TTo"/>
    /// </summary>
    /// <remarks>
    /// Quick and dirty solution from https://stackoverflow.com/questions/14437239/change-a-linq-expression-predicate-from-one-type-to-another
    /// </remarks>
    public class ExpressionReturnRewriter<TFrom, TTo, TReturn> : ExpressionVisitor
    {
        private Dictionary<string, ParameterExpression> convertedParameters;
        private Expression<Func<TFrom, TReturn>> expression;

        /// <summary>
        /// Creates a new expression return visitor
        /// </summary>
        /// <param name="expresionToConvert">The type of expression to conviert</param>
        public ExpressionReturnRewriter(Expression<Func<TFrom, TReturn>> expresionToConvert)
        {
            //for each parameter in the original expression creates a new parameter with the same name but with changed type
            convertedParameters = expresionToConvert.Parameters
                .ToDictionary(
                    x => x.Name,
                    x => Expression.Parameter(typeof(TTo), x.Name)
                );

            expression = expresionToConvert;
        }

        /// <summary>
        /// Perform the conversion logic 
        /// </summary>
        /// <returns>The converted expression with converted return type</returns>
        public Expression<Func<TTo, TReturn>> Convert()
        {
            return (Expression<Func<TTo, TReturn>>)Visit(expression);
        }

        /// <summary>
        /// Visit the specified member
        /// </summary>
        /// <param name="node">The node to visit</param>
        /// <returns>The mapped expressio</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TFrom))
            {
                var memeberInfo = typeof(TTo).GetMember(node.Member.Name).First();
                var newExp = Visit(node.Expression);
                return Expression.MakeMemberAccess(newExp, memeberInfo);
            }
            else
            {
                return base.VisitMember(node);
            }
        }

        /// <inheritdoc/>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if(convertedParameters.TryGetValue(node.Name, out var newNode)) {
                return newNode;
            }
            return node;
        }

        /// <inheritdoc/>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var newExp = Visit(node.Body);
            return Expression.Lambda(newExp, convertedParameters.Select(x => x.Value));
        }
    }
}