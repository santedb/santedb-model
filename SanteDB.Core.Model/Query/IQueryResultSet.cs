using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Query result set without type parameters
    /// </summary>
    public interface IQueryResultSet : IEnumerable
    {
        /// <summary>
        /// Filter the result set where the specified condition matches
        /// </summary>
        /// <param name="query">The query to apply</param>
        /// <returns>The filtered result collection</returns>
        IQueryResultSet Where(Expression query);

        /// <summary>
        /// Retrieve the first result otherwise throw exception
        /// </summary>
        object First();

        /// <summary>
        /// Retrieve the first result otherwise return default
        /// </summary>
        object FirstOrDefault();

        /// <summary>
        /// Retrieve one (and only one) result otherwise throw exception
        /// </summary>
        object Single();

        /// <summary>
        /// Retrieve one (and only one) otherwise return null if not found or throw if more than one
        /// </summary>
        object SingleOrDefault();

        /// <summary>
        /// Take only <paramref name="count"/> results
        /// </summary>
        IQueryResultSet Take(int count);

        /// <summary>
        /// Skip <paramref name="count"/> results
        /// </summary>
        IQueryResultSet Skip(int count);

        /// <summary>
        /// Get the result set as a stateful query
        /// </summary>
        IQueryResultSet AsStateful(Guid stateId);

        /// <summary>
        /// Returns true if any results match
        /// </summary>
        bool Any();

        /// <summary>
        /// Return only the count of the objects
        /// </summary>
        int Count();
    }

    /// <summary>
    /// Represents a query result collection which allows for delay loading of results
    /// </summary>
    public interface IQueryResultSet<TData> : IQueryResultSet, IEnumerable<TData>
    {
        /// <summary>
        /// Filter the result set where the specified condition matches
        /// </summary>
        /// <param name="query">The query to apply</param>
        /// <returns>The filtered result collection</returns>
        IQueryResultSet<TData> Where(Expression<Func<TData, bool>> query);

        /// <summary>
        /// Intersect this result set with another
        /// </summary>
        IQueryResultSet<TData> Intersect(Expression<Func<TData, bool>> query);

        /// <summary>
        /// Intersect this result set with another
        /// </summary>
        IQueryResultSet<TData> Intersect(IQueryResultSet<TData> other);

        /// <summary>
        /// Retrieve the first result otherwise throw exception
        /// </summary>
        TData First();

        /// <summary>
        /// Retrieve the first result otherwise return default
        /// </summary>
        TData FirstOrDefault();

        /// <summary>
        /// Retrieve one (and only one) result otherwise throw exception
        /// </summary>
        TData Single();

        /// <summary>
        /// Retrieve one (and only one) otherwise return null if not found or throw if more than one
        /// </summary>
        TData SingleOrDefault();

        /// <summary>
        /// Union the results in this set with those matching <paramref name="query"/>
        /// </summary>
        IQueryResultSet<TData> Union(Expression<Func<TData, bool>> query);

        /// <summary>
        /// Union the results in this set with those in the <paramref name="other"/>
        /// </summary>
        IQueryResultSet<TData> Union(IQueryResultSet<TData> other);

        /// <summary>
        /// Take only <paramref name="count"/> results
        /// </summary>
        IQueryResultSet<TData> Take(int count);

        /// <summary>
        /// Skip <paramref name="count"/> results
        /// </summary>
        IQueryResultSet<TData> Skip(int count);

        /// <summary>
        /// Order the result set by the specified sort expression
        /// </summary>
        /// <param name="sortExpression">The expression to sort by</param>
        IQueryResultSet<TData> OrderBy(Expression<Func<TData, dynamic>> sortExpression);

        /// <summary>
        /// Order the result set by descending
        /// </summary>
        /// <param name="sortExpression">The sort expression</param>
        IQueryResultSet<TData> OrderByDescending(Expression<Func<TData, dynamic>> sortExpression);

        /// <summary>
        /// Get the result set as a stateful query
        /// </summary>
        IQueryResultSet<TData> AsStateful(Guid stateId);
    }
}