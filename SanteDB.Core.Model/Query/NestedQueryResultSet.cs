/*
 * Copyright (C) 2021 - 2026, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-6-21
 */
using SanteDB.Core.i18n;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// A query result set which wraps another query result set and allows for delayed execution
    /// </summary>
    public class NestedQueryResultSet : IQueryResultSet, IOrderableQueryResultSet
    {
        // The underlying result set
        private IQueryResultSet m_wrapped;

        // Iterator to yield
        private Func<object, object> m_yielder;

        /// <inheritdoc/>
        public Type ElementType => this.m_wrapped.ElementType;

        /// <summary>
        /// Creates a new wrapped memory result set
        /// </summary>
        public NestedQueryResultSet(IQueryResultSet wrapped, Func<object, object> yielder)
        {
            this.m_wrapped = wrapped;
            this.m_yielder = yielder;
        }

        /// <summary>
        /// True if any results
        /// </summary>
        public bool Any() => this.m_wrapped.Any();


        /// <summary>
        /// As stateful query
        /// </summary>
        public IQueryResultSet AsStateful(Guid stateId)
        {
            return new NestedQueryResultSet(this.m_wrapped.AsStateful(stateId), this.m_yielder);
        }

        /// <summary>
        /// Count the objects
        /// </summary>
        public int Count() => this.m_wrapped.Count();

        /// <summary>
        /// Get the first
        /// </summary>
        public object First() => this.m_yielder(this.m_wrapped.First());

        /// <summary>
        /// First or default
        /// </summary>
        public object FirstOrDefault() => this.m_yielder(this.m_wrapped.FirstOrDefault());

        /// <summary>
        /// Get enumerator
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            foreach (var itm in this.m_wrapped)
            {
                yield return this.m_yielder(itm);
            }
        }

        /// <summary>
        /// Intersect the sets
        /// </summary>
        public IQueryResultSet Intersect(IQueryResultSet other)
        {
            if (other is NestedQueryResultSet nr)
            {
                return new NestedQueryResultSet(this.m_wrapped.Intersect(nr.m_wrapped), (o) => nr.m_yielder(this.m_yielder(o)));
            }
            else
            {
                return new NestedQueryResultSet(this.m_wrapped.Intersect(other), this.m_yielder);
            }
        }

        /// <summary>
        /// Return this object of the specified <typeparamref name="TType"/>
        /// </summary>
        public IEnumerable<TType> OfType<TType>()
        {
            foreach (var itm in this)
            {
                if (itm is TType typ)
                {
                    yield return typ;
                }
            }
        }

        /// <summary>
        /// Non-generic select method
        /// </summary>
        public IEnumerable<TReturn> Select<TReturn>(Expression selector) => this.m_wrapped.Select<TReturn>(selector);

        /// <summary>
        /// Order the wrapped result set
        /// </summary>
        public IOrderableQueryResultSet OrderBy(Expression expression)
        {
            if (this.m_wrapped is IOrderableQueryResultSet orderable)
            {
                return new NestedQueryResultSet(orderable.OrderBy(expression), this.m_yielder);
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.NOT_SUPPORTED_IMPLEMENTATION, typeof(IOrderableQueryResultSet)));
            }
        }

        /// <summary>
        /// Order wrapped result set by descending
        /// </summary>
        public IOrderableQueryResultSet OrderByDescending(Expression expression)
        {
            if (this.m_wrapped is IOrderableQueryResultSet orderable)
            {
                return new NestedQueryResultSet(orderable.OrderByDescending(expression), this.m_yielder);
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.NOT_SUPPORTED_IMPLEMENTATION, typeof(IOrderableQueryResultSet)));
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
        public IQueryResultSet Skip(int count) => new NestedQueryResultSet(this.m_wrapped.Skip(count), this.m_yielder);

        /// <summary>
        /// Take n records
        /// </summary>
        public IQueryResultSet Take(int count) => new NestedQueryResultSet(this.m_wrapped.Take(count), this.m_yielder);

        /// <summary>
        /// Union the two datasets
        /// </summary>
        public IQueryResultSet Union(IQueryResultSet other)
        {
            if (other is NestedQueryResultSet nr)
            {
                return new NestedQueryResultSet(this.m_wrapped.Union(nr.m_wrapped), (o) => nr.m_yielder(this.m_yielder(o)));
            }
            else
            {
                return new NestedQueryResultSet(this.m_wrapped.Union(other), this.m_yielder);
            }
        }

        /// <summary>
        /// Where clause
        /// </summary>
        public IQueryResultSet Where(Expression query)
        {
            return new NestedQueryResultSet(this.m_wrapped.Where(query), this.m_yielder);
        }
    }

    /// <summary>
    /// A query result set which wraps another and allows a constructor to execute a Func on each
    /// yield return iteration of the underlying result set.
    /// </summary>
    public class NestedQueryResultSet<TData> : NestedQueryResultSet, IQueryResultSet<TData>, IOrderableQueryResultSet<TData>
    {
        // Wraped object
        private IQueryResultSet<TData> m_wrapped;

        // Yielder
        private Func<TData, TData> m_yielder;

        /// <summary>
        /// Create a new wrapped result set
        /// </summary>
        public NestedQueryResultSet(IQueryResultSet<TData> wrapped, Func<TData, TData> yielder) : base(wrapped, (o) => yielder((TData)o))
        {
            this.m_wrapped = wrapped;
            this.m_yielder = yielder;
        }

        /// <summary>
        /// Inersect with query
        /// </summary>
        public IQueryResultSet<TData> Intersect(Expression<Func<TData, bool>> query)
        {
            return new NestedQueryResultSet<TData>(this.m_wrapped.Intersect(query), this.m_yielder);
        }


        /// <inheritdoc/>
        public IQueryResultSet<TData> Distinct() => new NestedQueryResultSet<TData>(this.m_wrapped.Distinct(), this.m_yielder);


        /// <summary>
        /// Intersect with another
        /// </summary>
        public IQueryResultSet<TData> Intersect(IQueryResultSet<TData> other)
        {
            return new NestedQueryResultSet<TData>(this.m_wrapped.Intersect(other), this.m_yielder);
        }

        /// <summary>
        /// Order by
        /// </summary>
        public IOrderableQueryResultSet<TData> OrderBy<TKey>(Expression<Func<TData, TKey>> sortExpression)
        {
            if (this.m_wrapped is IOrderableQueryResultSet<TData> orderable)
            {
                return new NestedQueryResultSet<TData>(orderable.OrderBy(sortExpression), this.m_yielder);
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.NOT_SUPPORTED_IMPLEMENTATION, typeof(IOrderableQueryResultSet<TData>)));
            }
        }

        /// <summary>
        /// Order by descending or
        /// </summary>
        public IOrderableQueryResultSet<TData> OrderByDescending<TKey>(Expression<Func<TData, TKey>> sortExpression)
        {
            if (this.m_wrapped is IOrderableQueryResultSet<TData> orderable)
            {
                return new NestedQueryResultSet<TData>(orderable.OrderByDescending(sortExpression), this.m_yielder);
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.NOT_SUPPORTED_IMPLEMENTATION, typeof(IOrderableQueryResultSet<TData>)));
            }
        }

        /// <summary>
        /// Perform a select
        /// </summary>
        public IEnumerable<TReturn> Select<TReturn>(Expression<Func<TData, TReturn>> selector)
        {
            var selectFn = selector.Compile();
            foreach (var itm in this.m_wrapped)
            {
                yield return selectFn(this.m_yielder(itm));
            }
        }

        /// <summary>
        /// Union with another
        /// </summary>
        public IQueryResultSet<TData> Union(Expression<Func<TData, bool>> query)
        {
            return new NestedQueryResultSet<TData>(this.m_wrapped.Union(query), this.m_yielder);
        }

        /// <summary>
        /// Except with another
        /// </summary>
        public IQueryResultSet<TData> Except(Expression<Func<TData, bool>> query)
        {
            return new NestedQueryResultSet<TData>(this.m_wrapped.Except(query), this.m_yielder);
        }

        /// <summary>
        /// Union with another
        /// </summary>
        public IQueryResultSet<TData> Union(IQueryResultSet<TData> other)
        {
            return new NestedQueryResultSet<TData>(this.m_wrapped.Union(other), this.m_yielder);
        }

        /// <summary>
        /// Where clause filter
        /// </summary>
        public IQueryResultSet<TData> Where(Expression<Func<TData, bool>> query)
        {
            return new NestedQueryResultSet<TData>(this.m_wrapped.Where(query), this.m_yielder);
        }

        /// <summary>
        /// As a stateful query
        /// </summary>
        IQueryResultSet<TData> IQueryResultSet<TData>.AsStateful(Guid stateId)
        {
            return new NestedQueryResultSet<TData>(this.m_wrapped.AsStateful(stateId), this.m_yielder);
        }

        /// <summary>
        /// First object
        /// </summary>
        TData IQueryResultSet<TData>.First() => this.m_yielder(this.m_wrapped.First());

        /// <summary>
        /// First or default
        /// </summary>
        TData IQueryResultSet<TData>.FirstOrDefault() => this.m_yielder(this.m_wrapped.FirstOrDefault());

        /// <summary>
        /// Get enumerator
        /// </summary>
        IEnumerator<TData> IEnumerable<TData>.GetEnumerator()
        {
            foreach (var itm in this.m_wrapped)
            {
                yield return this.m_yielder(itm);
            }
        }

        /// <summary>
        /// Single object
        /// </summary>
        TData IQueryResultSet<TData>.Single() => this.m_yielder(this.m_wrapped.Single());

        /// <summary>
        /// Single object or default
        /// </summary>
        TData IQueryResultSet<TData>.SingleOrDefault() => this.m_yielder(this.m_wrapped.SingleOrDefault());

        /// <summary>
        /// Skip the specified results
        /// </summary>
        IQueryResultSet<TData> IQueryResultSet<TData>.Skip(int count) => new NestedQueryResultSet<TData>(this.m_wrapped.Skip(count), this.m_yielder);

        /// <summary>
        /// Take only <paramref name="count"/>
        /// </summary>
        IQueryResultSet<TData> IQueryResultSet<TData>.Take(int count) => new NestedQueryResultSet<TData>(this.m_wrapped.Take(count), this.m_yielder);
    }
}