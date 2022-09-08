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
    /// A query result set which wraps another and allows a constructor to execute a Func on each
    /// yield return iteration of the underlying result set.
    /// </summary>
    public class TransformQueryResultSet<TSource, TDestination> : IQueryResultSet<TDestination>
    {
        // Wraped object
        private readonly IQueryResultSet<TSource> m_sourceResultSet;

        // The transformer
        private readonly Func<TSource, TDestination> m_transform;

        /// <summary>
        /// Transform the query result set
        /// </summary>
        /// <param name="sourceResultSet">The source result set</param>
        /// <param name="transformer">The destination result set</param>
        public TransformQueryResultSet(IQueryResultSet<TSource> sourceResultSet, Func<TSource, TDestination> transformer)
        {
            this.m_sourceResultSet = sourceResultSet;
            this.m_transform = transformer;
        }

        /// <summary>
        /// Return if this has any results
        /// </summary>
        public bool Any() => this.m_sourceResultSet.Any();

        /// <summary>
        /// Return as stateful
        /// </summary>
        IQueryResultSet IQueryResultSet.AsStateful(Guid stateId) => this.AsStateful(stateId);

        /// <summary>
        /// Return the count of results
        /// </summary>
        public int Count() => this.m_sourceResultSet.Count();

        /// <summary>
        /// Get the first object
        /// </summary>
        object IQueryResultSet.First() => this.First();

        /// <summary>
        /// get the first or defaut object
        /// </summary>
        object IQueryResultSet.FirstOrDefault() => this.FirstOrDefault();

        /// <summary>
        /// Get generic enumerator
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Return the object of specified type
        /// </summary>
        public IEnumerable<TType> OfType<TType>()
        {
            foreach (var itm in this)
            {
                if (itm is TType typ)
                    yield return typ;
            }
        }

        /// <summary>
        /// Select the specified objects
        /// </summary>
        public IEnumerable<TReturn> Select<TReturn>(Expression<Func<TDestination, TReturn>> selector)
        {
            // Is the <TDestination> compatible with <TReturn>
            if (typeof(TSource).IsAssignableFrom(typeof(TDestination)))
            {
                var convertedExpression = new ExpressionReturnRewriter<TDestination, TSource, TReturn>(selector).Convert();
                foreach (var itm in this.m_sourceResultSet.Select(convertedExpression)) // pass through
                {
                    yield return itm;
                }
            }
            else
            {
                var selectFn = selector.Compile();
                foreach (var itm in this)
                {
                    yield return selectFn(itm);
                }
            }
        }

        /// <summary>
        /// Get the single result
        /// </summary>
        object IQueryResultSet.Single() => this.Single();

        /// <summary>
        /// get the single result from this object or throw
        /// </summary>
        object IQueryResultSet.SingleOrDefault() => this.SingleOrDefault();

        /// <summary>
        /// Skip the first n results
        /// </summary>
        IQueryResultSet IQueryResultSet.Skip(int count) => this.Skip(count);

        /// <summary>
        /// Take the first n results
        /// </summary>
        IQueryResultSet IQueryResultSet.Take(int count) => this.Take(count);

        /// <summary>
        /// Return the result set as a stateful query
        /// </summary>
        public IQueryResultSet<TDestination> AsStateful(Guid stateId)
        {
            return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.AsStateful(stateId), this.m_transform);
        }

        /// <summary>
        /// Return the first record in the result set
        /// </summary>
        public TDestination First() => this.m_transform(this.m_sourceResultSet.First());

        /// <summary>
        /// Return the first result or default (null)
        /// </summary>
        public TDestination FirstOrDefault() => this.m_transform(this.m_sourceResultSet.FirstOrDefault());

        /// <summary>
        /// Get an enumerator for the source transformed through the transformer
        /// </summary>
        public IEnumerator<TDestination> GetEnumerator()
        {
            foreach (var itm in this.m_sourceResultSet)
            {
                yield return this.m_transform(itm);
            }
        }

        /// <summary>
        /// Return the only result
        /// </summary>
        public TDestination Single() => this.m_transform(this.m_sourceResultSet.Single());

        /// <summary>
        /// Return the only result or throw exception
        /// </summary>
        public TDestination SingleOrDefault() => this.m_transform(this.m_sourceResultSet.SingleOrDefault());

        /// <summary>
        /// Skip the first N records
        /// </summary>
        public IQueryResultSet<TDestination> Skip(int count)
        {
            return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Skip(count), this.m_transform);
        }

        /// <summary>
        /// Take <paramref name="count"/> results
        /// </summary>
        public IQueryResultSet<TDestination> Take(int count)
        {
            return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Take(count), this.m_transform);
        }

        /// <summary>
        /// Intersect the destination with the source
        /// </summary>
        public IQueryResultSet<TDestination> Intersect(IQueryResultSet<TDestination> other)
        {
            if (other is TransformQueryResultSet<TSource, TDestination> otherTx)
            {
                return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Intersect(otherTx.m_sourceResultSet), this.m_transform);
            }
            else if (other is IQueryResultSet<TSource> otherSrc)
            {
                return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Intersect(otherSrc), this.m_transform);
            }
            else
            {
                throw new NotSupportedException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(TDestination), typeof(TSource)));
            }
        }

        /// <summary>
        /// Union this set with another set
        /// </summary>
        public IQueryResultSet<TDestination> Union(IQueryResultSet<TDestination> other)
        {
            if (other is TransformQueryResultSet<TSource, TDestination> otherTx)
            {
                return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Union(otherTx.m_sourceResultSet), this.m_transform);
            }
            else if (other is IQueryResultSet<TSource> otherSrc)
            {
                return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Union(otherSrc), this.m_transform);
            }
            else
            {
                throw new NotSupportedException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(TDestination), typeof(TSource)));
            }
        }

        /// <summary>
        /// Filter wrapped result set
        /// </summary>
        public IQueryResultSet<TDestination> Where(Expression<Func<TDestination, bool>> query)
        {
            // Is the <TDestination> compatible with <TReturn>
            if (typeof(TSource).IsAssignableFrom(typeof(TDestination)))
            {
                var convertedExpression = new ExpressionReturnRewriter<TDestination, TSource, bool>(query).Convert();
                return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Where(convertedExpression), this.m_transform);
            }
            else
            {
                throw new NotSupportedException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(TDestination), typeof(TSource)));
            }
        }

        /// <summary>
        /// Intersecting of transformed results sets is not supported
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryResultSet<TDestination> Intersect(Expression<Func<TDestination, bool>> query)
        {
            // Is the <TDestination> compatible with <TReturn>
            if (typeof(TSource).IsAssignableFrom(typeof(TDestination)))
            {
                var convertedExpression = new ExpressionReturnRewriter<TDestination, TSource, bool>(query).Convert();
                return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Intersect(convertedExpression), this.m_transform);
            }
            else
            {
                throw new NotSupportedException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(TDestination), typeof(TSource)));
            }
        }

        /// <summary>
        /// Union this set with those records mathcing <paramref name="query"/>
        /// </summary>
        public IQueryResultSet<TDestination> Union(Expression<Func<TDestination, bool>> query)
        {
            // Is the <TDestination> compatible with <TReturn>
            if (typeof(TSource).IsAssignableFrom(typeof(TDestination)))
            {
                var convertedExpression = new ExpressionReturnRewriter<TDestination, TSource, bool>(query).Convert();
                return new TransformQueryResultSet<TSource, TDestination>(this.m_sourceResultSet.Union(convertedExpression), this.m_transform);
            }
            else
            {
                throw new NotSupportedException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(TDestination), typeof(TSource)));
            }
        }

        /// <summary>
        /// Intersect this set with other set
        /// </summary>
        public IQueryResultSet Intersect(IQueryResultSet other)
        {
            if (other is TransformQueryResultSet<TSource, TDestination> otherTx)
            {
                return this.Intersect(otherTx);
            }
            else
            {
                throw new NotSupportedException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(TDestination), typeof(TSource)));
            }
        }

        /// <summary>
        /// Union this set with another
        /// </summary>
        /// <returns></returns>
        public IQueryResultSet Union(IQueryResultSet other)
        {
            if (other is TransformQueryResultSet<TSource, TDestination> otherTx)
            {
                return this.Union(otherTx);
            }
            else
            {
                throw new NotSupportedException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(TDestination), typeof(TSource)));
            }
        }

        /// <summary>
        /// Filter the wrapped transformed expression using query
        /// </summary>
        public IQueryResultSet Where(Expression query)
        {
            if (query is Expression<Func<TDestination, bool>> strongFn)
            {
                return this.Where(strongFn);
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(Expression<Func<TDestination, bool>>), query.GetType()));
            }
        }
    }
}