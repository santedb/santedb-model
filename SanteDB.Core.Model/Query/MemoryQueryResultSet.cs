/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors
 * Portions Copyright (C) 2015-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2022-5-30
 */
using SanteDB.Core.i18n;
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
    public class MemoryQueryResultSet : IQueryResultSet, IOrderableQueryResultSet
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
        /// Intersect the sets
        /// </summary>
        public IQueryResultSet Intersect(IQueryResultSet other)
        {
            return new MemoryQueryResultSet(this.m_wrapped.Intersect(other.OfType<object>()));
        }

        /// <summary>
        /// Return object of the specified type
        /// </summary>
        public IEnumerable<TType> OfType<TType>()
        {
            return this.m_wrapped.OfType<TType>();
        }

        /// <summary>
        /// Order by descending order
        /// </summary>
        public IOrderableQueryResultSet OrderBy(Expression expression)
        {
            if (expression is LambdaExpression le)
            {
                var lex = le.Compile();
                return new MemoryQueryResultSet(this.m_wrapped.OrderBy(o => lex.DynamicInvoke(o)));
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(LambdaExpression), expression.GetType()));
            }
        }

        /// <summary>
        /// Order by descending
        /// </summary>
        public IOrderableQueryResultSet OrderByDescending(Expression expression)
        {
            if (expression is LambdaExpression le)
            {
                var lex = le.Compile();
                return new MemoryQueryResultSet(this.m_wrapped.OrderByDescending(o => lex.DynamicInvoke(o)));
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(LambdaExpression), expression.GetType()));
            }
        }

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
        /// Union records
        /// </summary>
        public IQueryResultSet Union(IQueryResultSet other)
        {
            return new MemoryQueryResultSet(this.m_wrapped.Union(other.OfType<object>()));
        }

        /// <summary>
        /// Where clause
        /// </summary>
        public IQueryResultSet Where(Expression query)
        {
            if (query is LambdaExpression le)
            {
                var lex = le.Compile();
                return new MemoryQueryResultSet(this.m_wrapped.Where(o => lex.DynamicInvoke(o).Equals(true)));
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(LambdaExpression), query.GetType()));
            }
        }
    }

    /// <summary>
    /// A memory query result set
    /// </summary>
    public class MemoryQueryResultSet<TData> : MemoryQueryResultSet, IQueryResultSet<TData>, IOrderableQueryResultSet<TData>
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
        public IOrderableQueryResultSet<TData> OrderBy<TKey>(Expression<Func<TData, TKey>> sortExpression)
        {
            return new MemoryQueryResultSet<TData>(this.m_wrapped.OrderBy(sortExpression.Compile()));
        }

        /// <summary>
        /// Order by descending or
        /// </summary>
        public IOrderableQueryResultSet<TData> OrderByDescending<TKey>(Expression<Func<TData, TKey>> sortExpression)
        {
            return new MemoryQueryResultSet<TData>(this.m_wrapped.OrderByDescending(sortExpression.Compile()));
        }

        /// <summary>
        /// Perform a select
        /// </summary>
        public IEnumerable<TReturn> Select<TReturn>(Expression<Func<TData, TReturn>> selector)
        {
            return this.m_wrapped.Select(selector.Compile());
        }

        /// <summary>
        /// Select a C# expressions
        /// </summary>
        public IEnumerable<TReturn> Select<TReturn>(Func<TData, TReturn> selector)
        {
            return this.m_wrapped.Select(selector);
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