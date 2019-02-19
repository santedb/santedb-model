using SanteDB.Core.Model;
using SanteDB.Core.Model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Query
{

    /// <summary>
    /// Sorting instruction
    /// </summary>
    public struct ModelSort<TData>
    {

        /// <summary>
        /// Creates a new sort structure
        /// </summary>
        public ModelSort(LambdaExpression property, SortOrderType order)
        {
            this.SortProperty = Expression.Lambda<Func<TData, dynamic>>(Expression.Convert(property.Body, typeof(Object)), property.Parameters[0]);
            this.SortOrder = order;
        }

        /// <summary>
        /// Creates a new sort structure
        /// </summary>
        public ModelSort(Expression<Func<TData, dynamic>> property, SortOrderType order)
        {
            if (property.NodeType != ExpressionType.Lambda)
                throw new ArgumentException($"{nameof(property)} must be a LambdaExpression");
            this.SortProperty = property;
            this.SortOrder = order;
        }

        /// <summary>
        /// Gets the sort order
        /// </summary>
        public SortOrderType SortOrder { get; }

        /// <summary>
        /// Sort properties
        /// </summary>
        public Expression<Func<TData, dynamic>> SortProperty { get; }
    }
}
