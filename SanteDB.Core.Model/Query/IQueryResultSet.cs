/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-5-19
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Query result set without type parameters
    /// </summary>
    /// <remarks>This is a non-generic version of <see cref="IQueryResultSet{TData}"/></remarks>
    /// <seealso cref="IQueryResultSet{TData}"/>
    public interface IQueryResultSet : IEnumerable
    {
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
        /// Select a single object from the object
        /// </summary>
        IEnumerable<TReturn> Select<TReturn>(Expression selector);

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

        /// <summary>
        /// Flatten this object model to an <see cref="IEnumerable"/> of type <typeparamref name="TType"/>
        /// </summary>
        IEnumerable<TType> OfType<TType>();

        /// <summary>
        /// Intersect this set and <paramref name="other"/>
        /// </summary>
        IQueryResultSet Intersect(IQueryResultSet other);

        /// <summary>
        /// Union this set and <paramref name="other"/>
        /// </summary>
        IQueryResultSet Union(IQueryResultSet other);

        /// <summary>
        /// Filter the result set where the specified condition matches
        /// </summary>
        /// <param name="query">The query to apply</param>
        /// <returns>The filtered result collection</returns>
        IQueryResultSet Where(Expression query);

        /// <summary>
        /// Gets the types of elements which can be filtered and/or manipulated in the collection
        /// </summary>
        Type ElementType { get; }
    }

    /// <summary>
    /// Represents an orderable query result set
    /// </summary>
    public interface IOrderableQueryResultSet : IQueryResultSet
    {
        /// <summary>
        /// Order by the specified expression
        /// </summary>
        IOrderableQueryResultSet OrderBy(Expression expression);

        /// <summary>
        /// Order by in descending order
        /// </summary>
        IOrderableQueryResultSet OrderByDescending(Expression expression);
    }

    /// <summary>
    /// Represents a query result collection which allows for delay loading of results
    /// </summary>
    /// <remarks>
    /// <para>Implementers of this interface support yielded or late-executed queries. For example, when calling a
    /// Query() method on a persistence class, the query to the database is not executed immediately, rather the
    /// first call to a method which iterates over this collection, or calls or otherwise fetches a record
    /// results in the query being executed</para>
    /// <para>This is done to reduce the amount of data which is loaded from the database, however it also means
    /// that careless use of the result set will result in many additional queries to the database. Consider:</para>
    /// <code lang="C#">
    /// <![CDATA[
    ///     var results = this.persistenceService.Query(o => o.StatusConceptKey == StatusKeys.Active, AuthenticationContext.Current.Principal);
    ///     var count = results.Count();
    ///     var first = results.First();
    ///     var second = results.Skip(1).First();
    /// ]]>
    /// </code>
    /// <para>
    /// Would result in 3 calls to the database:
    /// </para>
    /// <list type="number">
    ///     <item>The <c>Count()</c> call results in a SQL <c>SELECT COUNT(*)</c></item>
    ///     <item>The <c>First()</c> method results in a SQL <c>SELECT * .... FETCH FIRST 1 ROWS ONLY</c> call</item>
    ///     <item>The <c>Skip(1).First()</c> method results in a SQL <c>SELECT * .... OFFSET 1 FETCH FIRST 1 ROWS ONLY</c> call</item>
    /// </list>
    /// <para>
    /// If the caller wishes to make multiple calls to this interface, it is recommended that they realize the results into an <see cref="IEnumerable"/> instance, for example:
    /// </para>
    /// <code>
    /// <![CDATA[
    ///     var results = this.persistenceService.Query(o => o.StatusConceptKey == StatusKeys.Active, AuthenticationContext.Current.Principal).ToList();
    ///     var count = results.Count();
    ///     var first = results.First();
    ///     var second = results.Skip(1).First();
    /// ]]>
    /// </code>
    /// <para>Results in only one <c>SELECT * FROM xxx</c> method, all results are loaded into memory and then subsequent calls are made in memory.</para>
    /// <para>This may seem more efficient, however imagine a result set where only the second page is required (10 results per page), we want the COUNT of all records
    /// however only actually want to load 10 records from the database, in this case we want two SQL queries executed (one for counting and another for fetching)</para>
    /// <code>
    /// <![CDATA[
    ///     var results = this.persistenceService.Query(o => o.StatusConceptKey == StatusKeys.Active, AuthenticationContext.Current.Principal);
    ///     var count = results.Count();
    ///     var results = results.Skip(10).Take(10).ToList();
    /// ]]>
    /// </code>
    /// <para>
    /// This code executes two SQL queries, a <c>SELECT COUNT(*) FROM xxxx</c> and then another <c>SELECT * FROM ... OFFSET 10 FETCH FIRST 10 ROWS ONLY</c> , this
    /// greatly reduces the result set. We can also determine if we want to send queries to the database with <see cref="IQueryResultSet.Any"/>, which would execute a
    /// <c>SELECT 1 WHERE EXISTS ....</c>
    /// </para>
    /// <para>Note: You can use the <see cref="ExtensionMethods.AsResultSet(IEnumerable)"/> method in order to access the delay load functions from interfaces which return <see cref="IEnumerable"/></para>
    /// <code>
    /// <![CDATA[
    ///     var wouldBeEnumerable = repository.Find(o=>o.StatusConceptKey == StatusKeys.Active);
    ///     if(wouldBeEnumerable.Any()) // results in a SELECT * FROM x - which is slow
    ///     {
    ///         Console.Write("I just did a slow thing");
    ///     }
    ///
    ///     var resultSet = wouldBeEnumerable.AsResultSet();
    ///     if(resultSet.Any()) // results in SELECT EXISTS FROM
    ///     {
    ///         Console.Write("I just did a faster thing");
    ///     }
    ///
    /// ]]>
    /// </code>
    /// </remarks>
    /// <example title="Fetch second 10 newest keys with state of ACTIVE">
    /// <![CDATA[
    ///     var resultSet = repository.Find(o => o.StatusConcept.Mnemonic == "ACTIVE").AsResultSet();
    ///     resultSet = resultSet.OrderByDescending(o => o.VersionSequenceId); // adds ORDER BY vrsn_seq_id DESC
    ///     resultSet = resultSet.Skip(10); // add OFFSET 10 to the query
    ///     resultSet = resultSet.Take(10); // add FETCH FIRST 10 ROWS ONLY to query
    ///     foreach(var result in resultSet.Select(o => o.Key)) // places SELECT id to query
    ///     {
    ///         Console.WriteLine(result); // output is a UUID
    ///     }
    /// ]]>
    /// </example>
    public interface IQueryResultSet<TData> : IQueryResultSet, IEnumerable<TData>
    {
        /// <summary>
        /// Retrieve the first result otherwise throw exception
        /// </summary>
        /// <remarks>This method will execute a <code>FETCH FIRST 1 ROWS ONLY</code> clause on the current query, fetching the first
        /// record from the result set from the underlying persistence service.</remarks>
        /// <returns>The first <typeparamref name="TData"/> from the result set</returns>
        new TData First();

        /// <summary>
        /// Retrieve the first result otherwise return default
        /// </summary>
        /// <seealso cref="First"/>
        new TData FirstOrDefault();

        /// <summary>
        /// Retrieve one (and only one) result otherwise throw exception
        /// </summary>
        /// <remarks>This method will return only one record from the database, if more than one record is returned then an exception is thrown</remarks>
        new TData Single();

        /// <summary>
        /// Retrieve one (and only one) otherwise return null if not found or throw if more than one
        /// </summary>
        /// <see cref="Single"/>
        new TData SingleOrDefault();

        /// <summary>
        /// Take only <paramref name="count"/> results
        /// </summary>
        /// <param name="count">The number of records to take from the result set</param>
        /// <remarks>This method results in a new query result set which has a <code>FETCH FIRST <paramref name="count"/> ROWS ONLY</code> clause
        /// appended to it. In HTTP to upstream, this sets the <code>_count=<paramref name="count"/></code> query string variable.</remarks>
        new IQueryResultSet<TData> Take(int count);

        /// <summary>
        /// Get only distinct objects in the collection
        /// </summary>
        IQueryResultSet<TData> Distinct();


        /// <summary>
        /// Skip <paramref name="count"/> results
        /// </summary>
        /// <remarks>
        /// <para>This method skips <paramref name="count"/> records in the result set by sending an offset command to the persistence technology. In
        /// SQL this equates to <code>OFFSET <paramref name="count"/> ROWS</code> clause. In HTTP to Upstream, this sets the <code>_offset=<paramref name="count"/></code> query string variable.</para>
        /// </remarks>
        new IQueryResultSet<TData> Skip(int count);

        /// <summary>
        /// Get the result set as a stateful query
        /// </summary>
        /// <param name="stateId">The state identifier for the stateful result set</param>
        new IQueryResultSet<TData> AsStateful(Guid stateId);

        /// <summary>
        /// Select a single object from the object
        /// </summary>
        IEnumerable<TReturn> Select<TReturn>(Expression<Func<TData, TReturn>> selector);

        /// <summary>
        /// Intersect this result set with another
        /// </summary>
        /// <remarks>Like the <see cref="Intersect(Expression{Func{TData, bool}})"/> method, this method intersects an already created <see cref="IQueryResultSet{TData}"/> with this data</remarks>
        /// <returns>A new <see cref="IQueryResultSet{TData}"/> which contains the necessary persistence layer instructions to intersect this set with another</returns>
        IQueryResultSet<TData> Intersect(IQueryResultSet<TData> other);

        /// <summary>
        /// Union the results in this set with those in the <paramref name="other"/>
        /// </summary>
        /// <see cref="Union(Expression{Func{TData, bool}})"/>
        IQueryResultSet<TData> Union(IQueryResultSet<TData> other);

        /// <summary>
        /// Filter the result set where the specified condition matches
        /// </summary>
        /// <param name="query">The query to apply</param>
        /// <returns>The filtered result collection</returns>
        /// <remarks>
        /// <para>This instruction appends a SQL <code>WHERE</code> clause to your query. This can only be called once on a particular instance
        /// of a <see cref="IQueryResultSet{TData}"/> or else an invalid state is created. When calling directly from a persistence layer such as:</para>
        /// <code>
        /// <![CDATA[
        ///     var results = persistenceService.Query(o => o.StatusConceptKey == StatusKeys.Active, AuthenticationContext.Current.Princiapl);
        ///     var filter = results.Where(o=> o.CreationTime > DateTime.Now); // invalid state
        /// ]]>
        /// </code>
        /// <para>The method is intended for new result sets, for example, consider:</para>
        /// <code>
        /// <![CDATA[
        ///     var results = persistenceService.Query(o => o.StatusConceptKey == StatusKeys.Active, AuthenticationContext.Current.Princiapl);
        ///     results = results.Union(o => o.StatusConceptKey.New);
        ///     var filter = results.Where(o=> o.CreationTime > DateTime.Now);
        /// ]]>
        /// </code>
        /// <para>Since the equivalent function in SQL is:</para>
        /// <code>
        /// <![CDATA[
        ///     SELECT *
        ///     FROM (
        ///         SELECT *
        ///         FROM x
        ///         WHERE sts_cd_id = 'ACTIVE_UUID'
        ///         UNION
        ///         SELECT *
        ///         FROM x
        ///         WHERE sts_cd_id = 'NEW_UUID'
        ///    ) I
        ///    WHERE I.crt_utc > CURRENT_TIMESTAMP
        /// ]]>
        /// </code>
        /// </remarks>
        /// <exception cref="InvalidOperationException">When a WHERE clause has already been appended</exception>
        IQueryResultSet<TData> Where(Expression<Func<TData, bool>> query);

        /// <summary>
        /// Intersect this result set with another
        /// </summary>
        /// <remarks><para>This method intersecs the current result set (via SQL) with the results of a SQL expression with <paramref name="query"/>. The result is a new <see cref="IQueryResultSet{TData}"/> which
        /// only contains the records of this result set's query and the query of <paramref name="query"/></para></remarks>
        /// <param name="query">The query of records matching a second result set with which this result set should be intersected</param>
        /// <returns>A new <see cref="IQueryResultSet{TData}"/> which contains the necessary persistence layer instructions to intersect this set with another</returns>
        IQueryResultSet<TData> Intersect(Expression<Func<TData, bool>> query);

        /// <summary>
        /// Union the results in this set with those matching <paramref name="query"/>
        /// </summary>
        /// <remarks><para>This method returns a new <see cref="IQueryResultSet{TData}"/> which contains a UNION of results in the current set
        /// and a new set described by <paramref name="query"/>.</para>
        /// <para>The method used to load these results depends on the underlying persistence layer, however in SQL would be:</para>
        /// <code>
        /// <![CDATA[
        ///     var results = repository.Find(o=>o.StatusConceptKey == StatusKeys.Active).AsResultSet();
        ///     results = results.Union(o=>o.StatusConceptKey == StatusKeys.New).Where(o => o.CreationTime > DateTimeOffset.Now);
        /// ]]>
        /// </code>
        /// <para>In SQL based persistence services this results in a query similar to:</para>
        /// <para>
        /// <![CDATA[
        ///     SELECT *
        ///     FROM (
        ///         SELECT *
        ///         FROM x
        ///         WHERE sts_cd_id = 'ACTIVE_UUID'
        ///         UNION
        ///         SELECT *
        ///         FROM x
        ///         WHERE sts_cd_id = 'NEW_UUID'
        ///     ) I
        ///     WHERE crt_utc > CURRENT_TIMESTAMP;
        /// ]]>
        /// </para>
        /// </remarks>
        IQueryResultSet<TData> Union(Expression<Func<TData, bool>> query);

        /// <summary>
        /// Do not include in the result set any objects which match the <paramref name="query"/>
        /// </summary>
        /// <param name="query">The query of objects to except</param>
        /// <returns>The excepted query</returns>
        IQueryResultSet<TData> Except(Expression<Func<TData, bool>> query);
    }

    /// <summary>
    /// A <see cref="IQueryResultSet{TData}"/> which can be ordered
    /// </summary>
    public interface IOrderableQueryResultSet<TData> : IQueryResultSet<TData>, IOrderableQueryResultSet
    {
        /// <summary>
        /// Order the result set by the specified sort expression
        /// </summary>
        /// <param name="sortExpression">The expression to sort by</param>
        IOrderableQueryResultSet<TData> OrderBy<TKey>(Expression<Func<TData, TKey>> sortExpression);

        /// <summary>
        /// Order the result set by descending
        /// </summary>
        /// <param name="sortExpression">The sort expression</param>
        IOrderableQueryResultSet<TData> OrderByDescending<TKey>(Expression<Func<TData, TKey>> sortExpression);
    }
}