using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Wrapped query result set
    /// </summary>
    public class MemoryQueryResultSet : IQueryResultSet
    {
        // The underlying result set
        private IEnumerable<Object> m_wrapped;

        /// <summary>
        /// Creates a new wrapped memory result set
        /// </summary>
        public MemoryQueryResultSet(IEnumerable wrapped)
        {
            this.m_wrapped = wrapped.OfType<Object>();
        }

        /// <summary>
        /// True if any results
        /// </summary>
        public bool Any() => this.m_wrapped.Any();

        /// <summary>
        /// As stateful query
        /// </summary>
        public IQueryResultSet AsStateful(Guid stateId) => this;

        /// <summary>
        /// Count the objects
        /// </summary>
        public int Count() => this.m_wrapped.Count();

        /// <summary>
        /// Get the first
        /// </summary>
        public object First() => this.m_wrapped.First();

        /// <summary>
        /// First or default
        /// </summary>
        public object FirstOrDefault() => this.m_wrapped.FirstOrDefault();

        /// <summary>
        /// Get enumerator
        /// </summary>
        public IEnumerator GetEnumerator() => this.m_wrapped.GetEnumerator();

        /// <summary>
        /// Get single result
        /// </summary>
        public object Single() => this.m_wrapped.Single();

        /// <summary>
        /// Get single result or return default
        /// </summary>
        public object SingleOrDefault() => this.m_wrapped.SingleOrDefault();

        /// <summary>
        /// Skip n records
        /// </summary>
        public IQueryResultSet Skip(int count) => new MemoryQueryResultSet(this.m_wrapped.Skip(count));

        /// <summary>
        /// Take n records
        /// </summary>
        public IQueryResultSet Take(int count) => new MemoryQueryResultSet(this.m_wrapped.Take(count));

        /// <summary>
        /// Where clause
        /// </summary>
        public IQueryResultSet Where(Expression query)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A memory query result set
    /// </summary>
    public class MemoryQueryResultSet<TData> : MemoryQueryResultSet, IQueryResultSet<TData>
    {
        // Wraped object
        private IEnumerable<TData> m_wrapped;

        /// <summary>
        /// New query result set
        /// </summary>
        public MemoryQueryResultSet() : base(new TData[0])
        {
            this.m_wrapped = new TData[0];
        }

        /// <summary>
        /// Create a new wrapped result set
        /// </summary>
        public MemoryQueryResultSet(IEnumerable<TData> wrapped) : base(wrapped)
        {
            this.m_wrapped = wrapped;
        }

        /// <summary>
        /// Inersect with query
        /// </summary>
        public IQueryResultSet<TData> Intersect(Expression<Func<TData, bool>> query)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Intersect with another
        /// </summary>
        public IQueryResultSet<TData> Intersect(IQueryResultSet<TData> other)
        {
            return new MemoryQueryResultSet<TData>(this.m_wrapped.Intersect(other));
        }

        /// <summary>
        /// Order by
        /// </summary>
        public IQueryResultSet<TData> OrderBy(Expression<Func<TData, dynamic>> sortExpression)
        {
            return new MemoryQueryResultSet<TData>(this.m_wrapped.OrderBy(sortExpression.Compile()));
        }

        /// <summary>
        /// Order by descending or
        /// </summary>
        public IQueryResultSet<TData> OrderByDescending(Expression<Func<TData, dynamic>> sortExpression)
        {
            return new MemoryQueryResultSet<TData>(this.m_wrapped.OrderByDescending(sortExpression.Compile()));
        }

        /// <summary>
        /// Union with another
        /// </summary>
        public IQueryResultSet<TData> Union(Expression<Func<TData, bool>> query)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Union with another
        /// </summary>
        public IQueryResultSet<TData> Union(IQueryResultSet<TData> other)
        {
            return new MemoryQueryResultSet<TData>(other.Union(this.m_wrapped));
        }

        /// <summary>
        /// Where clause filter
        /// </summary>
        public IQueryResultSet<TData> Where(Expression<Func<TData, bool>> query)
        {
            return new MemoryQueryResultSet<TData>(this.m_wrapped.Where(query.Compile()));
        }

        /// <summary>
        /// As a stateful query
        /// </summary>
        IQueryResultSet<TData> IQueryResultSet<TData>.AsStateful(Guid stateId) => this;

        /// <summary>
        /// First object
        /// </summary>
        TData IQueryResultSet<TData>.First() => this.m_wrapped.First();

        /// <summary>
        /// First or default
        /// </summary>
        TData IQueryResultSet<TData>.FirstOrDefault() => this.m_wrapped.FirstOrDefault();

        /// <summary>
        /// Get enumerator
        /// </summary>
        IEnumerator<TData> IEnumerable<TData>.GetEnumerator() => this.m_wrapped.GetEnumerator();

        /// <summary>
        /// Single object
        /// </summary>
        TData IQueryResultSet<TData>.Single() => this.m_wrapped.Single();

        /// <summary>
        /// Single object or default
        /// </summary>
        TData IQueryResultSet<TData>.SingleOrDefault() => this.m_wrapped.SingleOrDefault();

        /// <summary>
        /// Skip the specified results
        /// </summary>
        IQueryResultSet<TData> IQueryResultSet<TData>.Skip(int count) => new MemoryQueryResultSet<TData>(this.m_wrapped.Skip(count));

        /// <summary>
        /// Take only <paramref name="count"/>
        /// </summary>
        IQueryResultSet<TData> IQueryResultSet<TData>.Take(int count) => new MemoryQueryResultSet<TData>(this.m_wrapped.Take(count));
    }
}