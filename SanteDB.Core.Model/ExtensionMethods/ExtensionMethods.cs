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
 * Date: 2023-3-10
 */
using Newtonsoft.Json;
using SanteDB.Core.Model;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Map;
using SanteDB.Core.Model.Query;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SanteDB
{
    /// <summary>
    /// Reflection tools
    /// </summary>
    public static class ExtensionMethods
    {

        // Revers lookup cache for resource names
        private static IDictionary<String, Type> s_resourceNames;

        /// <summary>
        /// Indicates a properly was load/checked
        /// </summary>
        private struct PropertyLoadCheck
        {
            /// <summary>
            /// Creates a new laod check struct
            /// </summary>
            public PropertyLoadCheck(String propertyName)
            {
                this.PropertyName = propertyName;
            }

            /// <summary>
            /// Gets or sets the name of the property
            /// </summary>
            public String PropertyName { get; set; }

            /// <summary>
            /// Get the property hash code
            /// </summary>
            public override int GetHashCode() => this.PropertyName.GetHashCode();
        }

        private static ConcurrentDictionary<String, MethodBase> s_genericMethodCache = new ConcurrentDictionary<string, MethodBase>();


        // Property cache
        private static ConcurrentDictionary<String, PropertyInfo> s_propertyCache = new ConcurrentDictionary<string, PropertyInfo>();

        // Type property cache
        private static ConcurrentDictionary<Type, PropertyInfo[]> s_typePropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        // Parameterless ctor cache
        private static ConcurrentDictionary<Type, bool> s_parameterlessCtor = new ConcurrentDictionary<Type, bool>();

        // Enumerable types
        private static ConcurrentDictionary<Type, bool> m_enumerableTypes = new ConcurrentDictionary<Type, bool>();

        // Skip assemblies
        private static ConcurrentBag<Assembly> m_skipAsm = new ConcurrentBag<Assembly>();

        // Types
        private static ConcurrentDictionary<Assembly, Type[]> m_types = new ConcurrentDictionary<Assembly, Type[]>();


        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}"/>
        ///     according to specified key selector function. Diplicate keys will not be added to the dictionary.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>
        ///     A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TSource"/> selected from the input sequence.
        /// </returns>
        public static Dictionary<TKey, TSource> ToDictionaryIgnoringDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            => ToDictionaryIgnoringDuplicates(source, keySelector, v => v);


        /// <summary>
        ///     Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}"/>
        ///     according to specified key selector and element selector functions. Diplicate keys will not be added to the dictionary.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <typeparam name="TElement">The type of the value returned by elementSelector.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <returns>
        ///     A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TElement"/> selected from the input sequence.
        /// </returns>
        public static Dictionary<TKey, TElement> ToDictionaryIgnoringDuplicates<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> valueSelector)
        {
            if (null == source)
            {
                return null;
            }

            var dict = new Dictionary<TKey, TElement>();

            foreach (var item in source)
            {
                var key = keySelector(item);
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, valueSelector(item));
                }
            }

            return dict;
        }

        /// <summary>
        /// For each item in an enumerable
        /// </summary>
        public static void ForEach<T>(IEnumerable<T> me, Action<T> action)
        {
            foreach (var itm in me)
            {
                action(itm);
            }
        }

        /// <summary>
        /// Returns true if the <see cref="IList"/> is null or has no elements
        /// </summary>
        public static bool IsNullOrEmpty(this IEnumerable me) => me == null || me.OfType<Object>().Count() == 0;

        /// <summary>
        /// Add <paramref name="itemsToAdd"/> to <paramref name="me"/>
        /// </summary>
        public static void AddRange<T>(this IList<T> me, IEnumerable<T> itemsToAdd)
        {
            foreach (var itm in itemsToAdd)
            {
                me.Add(itm);
            }
        }

        /// <summary>
        /// Safe method for loading exported types from assemblies where <see cref="ReflectionTypeLoadException"/> is thrown
        /// </summary>
        public static Type[] GetExportedTypesSafe(this Assembly me)
        {
            try
            {
                return me.GetTypes().Where(t => t.IsPublic).ToArray();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(o => o?.IsPublic == true).ToArray();
            }
        }

        /// <summary>
        /// Get all types
        /// </summary>
        public static IEnumerable<Type> GetAllTypes(this AppDomain me)
        {
            // HACK: The weird TRY/CATCH in select many is to prevent mono from throwning a fit
            return me.GetAssemblies()
                .Where(a => !a.IsDynamic && !m_skipAsm.Contains(a))
                .SelectMany(a =>
                {
                    try
                    {
                        if (!m_types.TryGetValue(a, out var typ))
                        {
                            typ = a.GetExportedTypesSafe().Where(t => t.GetCustomAttribute<ObsoleteAttribute>() == null).ToArray();
                            m_types.TryAdd(a, typ);
                        }
                        return typ;
                    }
                    catch
                    {
                        m_skipAsm.Add(a); return Type.EmptyTypes;
                    }
                });
        }



        /// <summary>
        /// As result set
        /// </summary>
        public static IQueryResultSet AsResultSet(this IEnumerable me)
        {
            if (me is IQueryResultSet qre)
            {
                return qre;
            }
            else
            {
                return new MemoryQueryResultSet(me);
            }
        }

        /// <summary>
        /// As result set
        /// </summary>
        public static IQueryResultSet<TData> AsResultSet<TData>(this IEnumerable<TData> me)
        {
            if (me is IQueryResultSet<TData> qre)
            {
                return qre;
            }
            else
            {
                return new MemoryQueryResultSet<TData>(me);
            }
        }

        /// <summary>
        /// Order result set
        /// </summary>
        /// <remarks>
        /// This interface prevents the default .NET IEnumerable OrderByDescending being called on a normal IQueryResultSet
        /// </remarks>
        public static IOrderableQueryResultSet<TData> OrderByDescending<TData, TKey>(this IQueryResultSet<TData> me, Expression<Func<TData, TKey>> sortExpression)
        {
            if (me is IOrderableQueryResultSet<TData> qre)
            {
                return qre.OrderByDescending(sortExpression);
            }
            else
            {
                return me.OrderByDescending(sortExpression);
            }
        }

        /// <summary>
        /// Order result set
        /// </summary>
        /// <remarks>
        /// This interface prevents the default .NET IEnumerable OrderByDescending being called on a normal IQueryResultSet
        /// </remarks>
        public static IOrderableQueryResultSet<TData> OrderBy<TData, TKey>(this IQueryResultSet<TData> me, Expression<Func<TData, TKey>> sortExpression)
        {
            if (me is IOrderableQueryResultSet<TData> qre)
            {
                return qre.OrderBy(sortExpression);
            }
            else
            {
                return me.OrderBy(sortExpression);
            }
        }

        /// <summary>
        /// As a result set
        /// </summary>
        public static IQueryResultSet<TTo> TransformResultSet<TFrom, TTo>(this IQueryResultSet<TFrom> me)
        {
            if (me is IQueryResultSet<TTo> qre)
            {
                return qre;
            }
            else if (typeof(TTo).IsAssignableFrom(typeof(TFrom)))
            {
                return new TransformQueryResultSet<TFrom, TTo>(me, (o) => (TTo)(object)o);
            }
            else
            {
                throw new InvalidOperationException($"Type {typeof(TFrom)} and {typeof(TTo)} are not compatible");
            }
        }

        /// <summary>
        /// Parse base 64 encode
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static byte[] ParseBase64UrlEncode(this String base64String)
        {
            string incoming = base64String.Replace('_', '/').Replace('-', '+');
            switch (base64String.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            return System.Convert.FromBase64String(incoming);
        }

        /// <summary>
        /// Convert to base 64
        /// </summary>
        public static String Base64UrlEncode(this byte[] array)
        {
            return System.Convert.ToBase64String(array)
                    .TrimEnd(new Char[] { '=' }).Replace('+', '-').Replace('/', '_');
        }

        const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567=";

        /// <summary>
        /// Encodes <paramref name="buffer"/> as a Base32 string
        /// </summary>
        /// <param name="buffer">The array of bytes to encode</param>
        /// <returns>The encoded Base32 string</returns>
        public static String Base32Encode(this byte[] buffer)
        {
            if (buffer.Length < 1)
            {
                return string.Empty;
            }

            int roffset = 0, woffset = 0;
            var chars = new char[((int)(buffer.Length / 5d + 1d)) * 8];

            var bufferspan = new byte[8];

            while (roffset < buffer.Length)
            {
                var length = roffset + 5 > buffer.Length ? (buffer.Length - roffset) : 5;

                Array.Clear(bufferspan, 0, 8);
                //bufferspan.Fill(0);
                //buffer.Slice(roffset, length).CopyTo(bufferspan.Slice(3));
                Buffer.BlockCopy(buffer, roffset, bufferspan, 3, length);

                if (System.BitConverter.IsLittleEndian)
                {
                    //This is intentionally not in a loop.
                    byte t1 = bufferspan[0];
                    byte t2 = bufferspan[7];
                    bufferspan[0] = t2;
                    bufferspan[7] = t1;
                    t1 = bufferspan[1];
                    t2 = bufferspan[6];
                    bufferspan[1] = t2;
                    bufferspan[6] = t1;
                    t1 = bufferspan[2];
                    t2 = bufferspan[5];
                    bufferspan[2] = t2;
                    bufferspan[5] = t1;
                    t1 = bufferspan[3];
                    t2 = bufferspan[4];
                    bufferspan[3] = t2;
                    bufferspan[4] = t1;
                }


                ulong l = BitConverter.ToUInt64(bufferspan, 0); //BinaryPrimitives.ReadUInt64BigEndian(bufferspan);

                switch (length)
                {
                    case 5:
                        chars[woffset++] = Base32Alphabet[(int)(l >> 35) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 30) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 25) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 20) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 15) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 10) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 5) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l) & 0x1F];
                        break;
                    case 4:
                        chars[woffset++] = Base32Alphabet[(int)(l >> 35) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 30) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 25) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 20) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 15) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 10) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 5) & 0x1F];
                        chars[woffset++] = Base32Alphabet[32]; //Pad Char
                        break;
                    case 3:
                        chars[woffset++] = Base32Alphabet[(int)(l >> 35) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 30) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 25) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 20) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 15) & 0x1F];
                        chars[woffset++] = Base32Alphabet[32]; //Pad Char
                        chars[woffset++] = Base32Alphabet[32];
                        chars[woffset++] = Base32Alphabet[32];
                        break;
                    case 2:
                        chars[woffset++] = Base32Alphabet[(int)(l >> 35) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 30) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 25) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 20) & 0x1F];
                        chars[woffset++] = Base32Alphabet[32];
                        chars[woffset++] = Base32Alphabet[32]; //Pad Char
                        chars[woffset++] = Base32Alphabet[32];
                        chars[woffset++] = Base32Alphabet[32];
                        break;
                    case 1:
                        chars[woffset++] = Base32Alphabet[(int)(l >> 35) & 0x1F];
                        chars[woffset++] = Base32Alphabet[(int)(l >> 30) & 0x1F];
                        chars[woffset++] = Base32Alphabet[32];
                        chars[woffset++] = Base32Alphabet[32];
                        chars[woffset++] = Base32Alphabet[32];
                        chars[woffset++] = Base32Alphabet[32]; //Pad Char
                        chars[woffset++] = Base32Alphabet[32];
                        chars[woffset++] = Base32Alphabet[32];
                        break;
                    default:
                        break;
                }
                roffset += length;
            }

            return new string(chars, 0, woffset);
        }

        /// <summary>
        /// Convert a hex string to byte array
        /// </summary>
        public static String HexEncode(this byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }

        /// <summary>
        /// Decode <paramref name="encodedData"/> to a byte array
        /// </summary>
        public static byte[] HexDecode(this string encodedData) => Enumerable.Range(0, encodedData.Length)
                                    .Where(x => x % 2 == 0)
                                    .Select(x => System.Convert.ToByte(encodedData.Substring(x, 2), 16))
                                    .ToArray();

        /// <summary>
        /// Gets the value of a component of an <see cref="EntityAddress"/> with the given <paramref name="addressType"/>.
        /// </summary>
        public static String Value(this EntityAddress me, Guid addressType)
        {
            return me.LoadCollection<EntityAddressComponent>("Component").FirstOrDefault(o => o.ComponentTypeKey == addressType)?.Value;
        }

        /// <summary>
        /// Set the load state
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetLoaded<TSource, TReturn>(this TSource me, Expression<Func<TSource, TReturn>> propertySelector)
            where TSource : IAnnotatedResource
        {
            me.SetLoaded(propertySelector.GetMember().Name);
        }


        /// <summary>
        /// Returns true if the property is loaded
        /// </summary>
        public static bool WasLoaded<TSource, TReturn>(this TSource me, Expression<Func<TSource, TReturn>> propertySelector)
            where TSource : IAnnotatedResource
        {
            return me.WasLoaded(propertySelector.GetMember().Name);
        }

        /// <summary>
        /// Returns true if the property has been loaded
        /// </summary>
        public static bool WasLoaded(this IAnnotatedResource me, String propertyName)
        {
            var loadCheck = new PropertyLoadCheck(propertyName);
            return me.GetAnnotations<PropertyLoadCheck>().Contains(loadCheck);
        }

        /// <summary>
        /// Delay load property
        /// </summary>
        public static TReturn LoadProperty<TReturn>(this IAnnotatedResource me, string propertyName, bool forceReload = false)
        {
            return (TReturn)me.LoadProperty(propertyName, forceReload);
        }

        /// <summary>
        /// Delay load property
        /// </summary>
        public static IEnumerable<TReturn> LoadCollection<TReturn>(this IAnnotatedResource me, string propertyName, bool forceReload = false)
        {
            return me.LoadProperty(propertyName, forceReload) as IEnumerable<TReturn> ?? new List<TReturn>();
        }

        /// <summary>
        /// Get member
        /// </summary>
        public static MemberInfo GetMember(this Expression me)
        {
            if (me is UnaryExpression ue)
            {
                return ue.Operand.GetMember();
            }
            else if (me is LambdaExpression le)
            {
                return le.Body.GetMember();
            }
            else if (me is MemberExpression ma)
            {
                if (ma.Member.Name == "Value" && ma.Expression.Type.IsGenericType && ma.Expression.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return ma.Expression.GetMember();
                }
                else
                {
                    return ma.Member;
                }
            }
            else
            {
                throw new InvalidOperationException($"{me} not supported, please use a member access expression");
            }
        }


        /// <summary>
        /// Load collection of <typeparamref name="TReturn"/> from <typeparamref name="TSource"/>
        /// </summary>
        public static IEnumerable<TReturn> LoadCollection<TSource, TReturn>(this TSource me, Expression<Func<TSource, IEnumerable<TReturn>>> selector, bool forceReload = false)
            where TSource : IAnnotatedResource
        {
            if (selector is LambdaExpression lambda)
            {
                var body = lambda.Body as MemberExpression;
                if (body != null)
                {
                    return me.LoadCollection<TReturn>(body.Member.Name, forceReload);
                }
                else
                {
                    throw new InvalidOperationException("Must be simple property selector");
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown expression passed");
            }
        }

        /// <summary>
        /// Load collection of <typeparamref name="TReturn"/> from <typeparamref name="TSource"/>
        /// </summary>
        public static TReturn LoadProperty<TSource, TReturn>(this TSource me, Expression<Func<TSource, TReturn>> selector, bool forceReload = false)
            where TSource : IAnnotatedResource
        {
            if (selector is LambdaExpression lambda)
            {
                var body = lambda.Body as MemberExpression;
                if (body != null)
                {
                    return me.LoadProperty<TReturn>(body.Member.Name, forceReload);
                }
                else
                {
                    throw new InvalidOperationException("Must be simple property selector");
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown expression passed");
            }
        }


        /// <summary>
        /// Set the necessary annotation on <paramref name="me"/> to indicate that <paramref name="propertyName"/> has
        /// been loaded
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetLoaded(this IAnnotatedResource me, string propertyName)
        {
            var loadCheck = new PropertyLoadCheck(propertyName);
            if (!me.GetAnnotations<PropertyLoadCheck>().Contains(loadCheck))
            {
                me.AddAnnotation(loadCheck);
            }
        }

        /// <summary>
        /// Delay load property
        /// </summary>
        public static object LoadProperty(this IAnnotatedResource me, string propertyName, bool forceReload = false)
        {
            if (me == null)
            {
                return null;
            }

            var propertyToLoad = me.GetType().GetProperty(propertyName);
            if (propertyToLoad == null)
            {
                return null;
            }

            var currentValue = propertyToLoad.GetValue(me);
            var loadCheck = new PropertyLoadCheck(propertyName);

            
            if (!forceReload && (me.GetAnnotations<PropertyLoadCheck>().Contains(loadCheck) || me.GetAnnotations<String>().Contains(SanteDBModelConstants.NoDynamicLoadAnnotation)))
            {
                // HACK: For some reason load check can be set but a list property is null
                if (currentValue == null &&
                    propertyToLoad.PropertyType.Implements(typeof(IList)))
                {
                    forceReload = true;
                }
                else
                {
                    return currentValue;
                }
            }
            else if (forceReload)
            {
                currentValue = null;
            }

            try
            {
                if (typeof(IList).IsAssignableFrom(propertyToLoad.PropertyType)) // Collection we load by key
                {
                    if ((currentValue == null || currentValue is IList cle && cle.IsNullOrEmpty()) &&
                        typeof(IdentifiedData).IsAssignableFrom(propertyToLoad.PropertyType.StripGeneric()))
                    {
                        IList loaded = Activator.CreateInstance(propertyToLoad.PropertyType) as IList;
                        if (me.Key.HasValue)
                        {
                            if (me is ITaggable taggable && taggable.TryGetTag(SystemTagNames.AlternateKeysTag, out ITag altKeys))
                            {
                                var loadedData = EntitySource.Current.Provider.GetRelations(propertyToLoad.PropertyType.StripGeneric(), altKeys.Value.Split(',').Select(o => (Guid?)Guid.Parse(o)).ToArray());
                                foreach (var itm in loadedData)
                                {
                                    loaded.Add(itm);
                                }
                            }
                            else
                            {
                                var delayLoad = EntitySource.Current.Provider.GetRelations(propertyToLoad.PropertyType.StripGeneric(), me.Key.Value);
                                if (delayLoad != null)
                                {
                                    foreach (var itm in delayLoad)
                                    {
                                        loaded.Add(itm);
                                    }
                                }
                            }

                        }
                        propertyToLoad.SetValue(me, loaded);
                        return loaded;
                    }
                    return currentValue;
                }
                else if (currentValue == null)
                {
                    var keyValue = propertyToLoad.GetSerializationRedirectProperty()?.GetValue(me) as Guid?;
                    if (keyValue.GetValueOrDefault() == default(Guid))
                    {
                        return currentValue;
                    }
                    else
                    {
                        var mi = typeof(IEntitySourceProvider).GetGenericMethod(nameof(IEntitySourceProvider.Get), new Type[] { propertyToLoad.PropertyType }, new Type[] { typeof(Guid?) });
                        var loaded = mi.Invoke(EntitySource.Current.Provider, new object[] { keyValue });
                        propertyToLoad.SetValue(me, loaded);
                        return loaded;
                    }
                }
                else
                {
                    return currentValue;
                }
            }
            finally
            {
                me.AddAnnotation(loadCheck);
            }
            // Now we want to
        }

        /// <summary>
        /// Gets the serialization runtime property
        /// </summary>
        public static PropertyInfo GetSerializationRedirectProperty(this PropertyInfo me)
        {
            var xsr = me.GetCustomAttribute<SerializationReferenceAttribute>();
            if (xsr != null)
            {
                return me.DeclaringType.GetProperty(xsr.RedirectProperty);
            }
            else
            {
                return me.DeclaringType.GetProperty($"{me.Name}Key");
            }
        }

        /// <summary>
        /// Gets the serialization runtime property
        /// </summary>
        public static PropertyInfo GetSerializationModelProperty(this PropertyInfo me)
        {
            if(me.Name.EndsWith("Key"))
            {
                return me.DeclaringType.GetProperty(me.Name.Substring(0, me.Name.Length - 3));
            }
            return null;
        }

        /// <summary>
        /// Create aggregation functions
        /// </summary>
        public static Expression Aggregate(this Expression me, AggregationFunctionType aggregation)
        {
            var aggregateMethod = typeof(Enumerable).GetGenericMethod(aggregation.ToString(),
               new Type[] { me.Type.GenericTypeArguments[0] },
               new Type[] { me.Type });
            return Expression.Call(aggregateMethod as MethodInfo, me);
        }

        /// <summary>
        /// The purpose of this method is to convert object <paramref name="me"/> to <typeparamref name="TReturn"/>. Why?
        /// Because if you have an instance of Act that actually needs to be a SubstanceAdministration we can't just cast
        /// so we have to copy.
        /// </summary>
        public static TReturn Convert<TReturn>(this Object me) where TReturn : new()
        {
            if (me is TReturn)
            {
                return (TReturn)me;
            }
            else
            {
                var retVal = new TReturn();
                retVal.CopyObjectData(me);
                return retVal;
            }
        }

        /// <summary>
        /// Copy object data from one object instance to another
        /// </summary>
        public static TObject CopyObjectData<TObject>(this TObject toEntity, TObject fromEntity)
        {
            return toEntity.CopyObjectData(fromEntity, false);
        }

        /// <summary>
        /// Strips all version information from the specified object.
        /// </summary>
        public static TObject StripAssociatedItemSources<TObject>(this TObject toEntity)
        {
            if (toEntity == null)
            {
                throw new ArgumentNullException(nameof(toEntity));
            }

            if (!(toEntity is IdentifiedData identifiedData))
            {
                return toEntity;
            }

            PropertyInfo[] properties = null;
            if (!s_typePropertyCache.TryGetValue(toEntity.GetType(), out properties))
            {
                properties = toEntity.GetType().GetProperties().Where(destinationPi => destinationPi.GetCustomAttribute<SerializationMetadataAttribute>() == null &&
                    destinationPi.CanWrite).ToArray();
                s_typePropertyCache.TryAdd(toEntity.GetType(), properties);
            }
            foreach (var destinationPi in properties)
            {
                if (typeof(ISimpleAssociation).IsAssignableFrom(destinationPi.PropertyType.StripGeneric())) // Versioned association
                {
                    var value = destinationPi.GetValue(toEntity);
                    if (value is IList list)
                    {
                        foreach (ISimpleAssociation itm in list)
                        {
                            if (itm is ITargetedAssociation itgt && itgt.TargetEntityKey == identifiedData.Key)
                            {
                                continue; // We're just pointing at ourselves and we don't want to change the direction of flow
                            }
                            //
                            if (itm.SourceEntityKey != identifiedData.Key)
                            {
                                itm.Key = null;
                                itm.SourceEntityKey = null;

                                if (itm is IVersionedAssociation va)
                                {
                                    va.EffectiveVersionSequenceId = null;
                                }

                                itm.StripAssociatedItemSources();
                            }
                        }
                    }
                    else if (value is ISimpleAssociation assoc)
                    {
                        if (assoc.SourceEntityKey != identifiedData.Key)
                        {
                            assoc.Key = null;
                            assoc.SourceEntityKey = null;
                            if (assoc is IVersionedAssociation va)
                            {
                                va.EffectiveVersionSequenceId = null;
                            }

                            assoc.StripAssociatedItemSources();
                        }
                    }
                }
            }

            return toEntity;
        }

        /// <summary>
        /// Update property data if required
        /// </summary>
        /// TODO: Write a version of this that can use the CODE-DOM to pre-compile a copy function instead of using reflection which is slow
        public static TObject CopyObjectData<TObject>(this TObject toEntity, TObject fromEntity, bool overwritePopulatedWithNull = false, bool ignoreTypeMismatch = false, bool declaredOnly = false, bool onlyNullFields = false)
        {
            if (toEntity == null)
            {
                throw new ArgumentNullException(nameof(toEntity));
            }
            else if (fromEntity == null)
            {
                return toEntity;// nothing to copy
            }
            else if (!ignoreTypeMismatch && !fromEntity.GetType().IsAssignableFrom(toEntity.GetType()))
            {
                throw new ArgumentException($"Type mismatch {toEntity.GetType().FullName} != {fromEntity.GetType().FullName}", nameof(fromEntity));
            }

            PropertyInfo[] properties = null;
            if (declaredOnly)
            {
                properties = typeof(TObject).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }
            else if (!s_typePropertyCache.TryGetValue(toEntity.GetType(), out properties))
            {
                properties = toEntity.GetType().GetProperties().Where(destinationPi => destinationPi.GetCustomAttribute<SerializationMetadataAttribute>() == null &&
                    destinationPi.CanWrite).ToArray();
                s_typePropertyCache.TryAdd(toEntity.GetType(), properties);
            }

            var sameType = fromEntity.GetType() == toEntity.GetType();

            foreach (var destinationPi in properties)
            {

                var sourcePi = sameType ? destinationPi : fromEntity.GetType().GetProperty(destinationPi.Name);
                // Skip properties no in the source
                if (sourcePi == null)
                {
                    continue;
                }

                object newValue = sourcePi.GetValue(fromEntity),
                    oldValue = destinationPi.GetValue(toEntity);

                // HACK: New value wrap for nullables
                if (newValue is IList lst && lst.Count == 0)
                {
                    newValue = null;
                }

                object defaultValue = null;
                var hasConstructor = false;
                if (newValue != null && !s_parameterlessCtor.TryGetValue(newValue.GetType(), out hasConstructor))
                {
                    // HACK: Some .NET types don't like to be constructed
                    hasConstructor = newValue.GetType().GetConstructor(Type.EmptyTypes) != null;
                    s_parameterlessCtor.TryAdd(newValue.GetType(), hasConstructor);
                }
                if (newValue != null && hasConstructor)
                {
                    defaultValue = Activator.CreateInstance(newValue.GetType());
                }

                if (onlyNullFields &&
                    oldValue != null &&
                    oldValue != defaultValue &&
                    !default(bool).Equals(oldValue) &&
                    !default(int).Equals(oldValue) &&
                    !default(DateTime).Equals(oldValue) &&
                    !default(decimal).Equals(oldValue) &&
                    !default(double).Equals(oldValue))
                {
                    continue;
                }

                if (newValue is IList && oldValue is IList)
                {
                    if (!Enumerable.SequenceEqual<Object>(((IList)newValue).OfType<Object>(), ((IList)oldValue).OfType<Object>()))
                    {
                        destinationPi.SetValue(toEntity, newValue);
                    }
                }
                else if (
                    newValue != null &&
                    !newValue.Equals(oldValue) == true &&
                    (destinationPi.PropertyType.StripNullable() != destinationPi.PropertyType || typeof(String) == destinationPi.PropertyType && !String.IsNullOrEmpty(newValue.ToString()) || !newValue.Equals(defaultValue) || !destinationPi.PropertyType.IsValueType))
                {
                    destinationPi.SetValue(toEntity, newValue);
                }
                else if (newValue == null && oldValue != null && overwritePopulatedWithNull)
                {
                    destinationPi.SetValue(toEntity, newValue);
                }
            }
            return toEntity;
        }

        /// <summary>
        /// Update property data with data from <paramref name="fromEntities"/> if the property is not semantically equal
        /// </summary>
        public static TObject SemanticCopy<TObject>(this TObject toEntity, params TObject[] fromEntities) where TObject : IdentifiedData
        {
            return SemanticCopyInternal(toEntity, false, null, fromEntities);
        }

        /// <summary>
        /// Update property data with data from <paramref name="fromEntities"/> if the property is not semantically equal - only if the field on the inbound <paramref name="toEntity"/> is null
        /// </summary>
        public static TObject SemanticCopyNullFields<TObject>(this TObject toEntity, params TObject[] fromEntities) where TObject : IdentifiedData
        {
            return SemanticCopyInternal(toEntity, true, null, fromEntities);
        }

        /// <summary>
        /// Update property data with data from <paramref name="fromEntities"/> if and only if the source property is null
        /// in the original toEntity
        /// </summary>
        public static TObject SemanticCopyFields<TObject>(this TObject toEntity, TObject[] fromEntities, params String[] fieldNames) where TObject : IdentifiedData
        {
            return SemanticCopyInternal(toEntity, false, fieldNames, fromEntities);
        }

        /// <summary>
        /// Perform semantic copy
        /// </summary>
        private static TObject SemanticCopyInternal<TObject>(this TObject toEntity, bool onlyIfNull, String[] fieldNames, params TObject[] fromEntities) where TObject : IdentifiedData
        {
            if (toEntity == null)
            {
                throw new ArgumentNullException(nameof(toEntity));
            }
            else if (fromEntities == null)
            {
                throw new ArgumentNullException(nameof(fromEntities));
            }
            else if (fromEntities.Length == 0)
            {
                return toEntity;
            }
            else if (!fromEntities.Any(e => e.GetType().IsAssignableFrom(toEntity.GetType())))
            {
                throw new ArgumentException($"Type mismatch {toEntity.GetType().FullName} != {fromEntities.GetType().FullName}", nameof(fromEntities));
            }

            PropertyInfo[] properties = null;
            if (!s_typePropertyCache.TryGetValue(toEntity.GetType(), out properties))
            {
                properties = toEntity.GetType().GetProperties().Where(destinationPi => destinationPi.GetCustomAttribute<SerializationMetadataAttribute>() == null &&
                    destinationPi.CanWrite).ToArray();
                s_typePropertyCache.TryAdd(toEntity.GetType(), properties);
            }

            // Properties which are lists that have had data added from the fromEntities array
            Dictionary<PropertyInfo, List<IdentifiedData>> mergedListProperties = new Dictionary<PropertyInfo, List<IdentifiedData>>();
            String[] ignoreProperties = { "Key", "VersionKey", "VersionSequenceId", "CreationTime", "CreatedByKey", "CreatedBy", "UpdatedBy", "UpdatedByKey", "UpdatedTime", "ObsoletedBy", "ObsoletedByKey", "ObsoletionTime", "ReplacesVersionKey", "ReplacesVersion" };

            // Destination properties
            foreach (var destinationPi in properties.Where(p => !ignoreProperties.Contains(p.Name)))
            {
                object oldValue = destinationPi.GetValue(toEntity);
                if ((onlyIfNull && (oldValue == null || (oldValue as IList)?.Count == 0) || !onlyIfNull) &&
                    (fieldNames == null || fieldNames.Contains(destinationPi.Name)))
                {
                    foreach (var fromEntity in fromEntities.OrderBy(k => k.ModifiedOn))
                    {
                        var sourcePi = fromEntity.GetType().GetProperty(destinationPi.Name);
                        // Skip properties no in the source
                        if (sourcePi == null)
                        {
                            continue;
                        }

                        // Skip data ignore
                        if (destinationPi.PropertyType.IsGenericType &&
                            destinationPi.PropertyType.GetGenericTypeDefinition().Namespace.StartsWith("System.Data.Linq") ||
                            destinationPi.PropertyType.Namespace.StartsWith("SanteDB.Persistence"))
                        {
                            continue;
                        }

                        object newValue = sourcePi.GetValue(fromEntity);

                        // HACK: New value wrap for nullables
                        if (newValue is Guid? && newValue != null)
                        {
                            newValue = (newValue as Guid?).Value;
                        }

                        // HACK: Empty lists are NULL
                        if ((newValue as IList)?.Count == 0)
                        {
                            newValue = null;
                        }

                        if (newValue != null) // The new value has something
                        {
                            // The destination is a list, so we should add if the item doesn't exist in the list
                            if (typeof(IList).IsAssignableFrom(destinationPi.PropertyType))
                            {
                                // No current value
                                if (oldValue == null)
                                {
                                    oldValue = Activator.CreateInstance(destinationPi.PropertyType);
                                    destinationPi.SetValue(toEntity, oldValue);
                                }

                                // Cast lists
                                IList<IdentifiedData> oldList = (oldValue as IList).OfType<IdentifiedData>().ToList(),
                                    newList = (newValue as IList).OfType<IdentifiedData>().ToList();
                                IList modifyList = oldValue as IList;

                                // Initialize list if needed
                                List<IdentifiedData> addedObjects = null;
                                if (!mergedListProperties.TryGetValue(destinationPi, out addedObjects))
                                {
                                    addedObjects = new List<IdentifiedData>();
                                    mergedListProperties.Add(destinationPi, addedObjects);
                                }

                                // Copy / update items
                                foreach (var itm in newList)
                                {
                                    // Get an existing item that matches this
                                    var existing = oldList.FirstOrDefault(o => o.SemanticEquals(itm));
                                    // Couldn't find something that "means the same" as the current
                                    if (existing == null)
                                    {
                                        var citm = itm.Clone();
                                        //citm.Key = null;
                                        modifyList.Add(citm);
                                    }
                                    else if (existing is IVersionedAssociation ive)
                                    {
                                        var cve = itm as IVersionedAssociation;
                                        ive.ObsoleteVersionSequenceId = cve.ObsoleteVersionSequenceId;
                                        ive.EffectiveVersionSequenceId = cve.EffectiveVersionSequenceId;
                                    }
                                    addedObjects.Add(itm);
                                }
                            }
                            else if (newValue is IdentifiedData &&
                                !(newValue as IdentifiedData).SemanticEquals(oldValue))
                            {
                                destinationPi.SetValue(toEntity, newValue);
                            }
                            else if (!newValue.Equals(oldValue) &&
                                (destinationPi.PropertyType.StripNullable() != destinationPi.PropertyType || !newValue.Equals(Activator.CreateInstance(newValue.GetType())) || !destinationPi.PropertyType.IsValueType))
                            {
                                destinationPi.SetValue(toEntity, newValue);
                            }
                        }
                    }
                }
            }

            return toEntity;
        }

        /// <summary>
        /// Get generic method
        /// </summary>
        public static MethodBase GetGenericMethod(this Type type, string name, Type[] typeArgs, Type[] argTypes)
        {
            int typeArity = typeArgs.Length;
            var methods = type.GetMethods()
                .Where(m => m.Name == name)
                .Where(m => m.GetGenericArguments().Length == typeArity)
                .Where(m => m.GetParameters().Length == argTypes.Length)
                .Select(m => m.MakeGenericMethod(typeArgs)).ToList();

            methods = methods.Where(m => Enumerable.Range(0, argTypes.Length).All(i => m.GetParameters()[i].IsOut || m.GetParameters()[i].ParameterType.IsAssignableFrom(argTypes[i]))).ToList();

            return methods.FirstOrDefault();
        }

        /// <summary>
        /// True if enumerable
        /// </summary>
        public static bool IsEnumerable(this Type me)
        {
            if (!m_enumerableTypes.TryGetValue(me, out bool retVal))
            {
                retVal = typeof(IEnumerable).IsAssignableFrom(me);
                m_enumerableTypes.TryAdd(me, retVal);
            }
            return retVal;
        }

        /// <summary>
        /// Get the serialization name
        /// </summary>
        public static string GetSerializationName(this PropertyInfo me)
        {
            var xmlName = me.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault()?.ElementName ?? me.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
            if (xmlName == null)
            {
                var refName = me.GetCustomAttribute<SerializationReferenceAttribute>()?.RedirectProperty;
                if (refName == null)
                {
                    return me.GetQueryName();
                }

                xmlName = me.DeclaringType.GetProperty(refName)?.GetCustomAttribute<XmlElementAttribute>()?.ElementName;
            }
            else if (xmlName == String.Empty)
            {
                xmlName = me.Name;
            }

            return xmlName;
        }

        /// <summary>
        /// Determine if <paramref name="me"/> implements all of <paramref name="interfaces"/>
        /// </summary>
        public static bool Implements(this Type me, params Type[] interfaces)
        {
            return interfaces.All(o => o.IsAssignableFrom(me));
        }

        /// <summary>
        /// Get the serialization name
        /// </summary>
        public static string GetQueryName(this PropertyInfo me)
        {
            return me.GetCustomAttributes<QueryParameterAttribute>().FirstOrDefault()?.ParameterName
                ?? me.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault()?.ElementName
                ?? me.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
        }

        /// <summary>
        /// Get all allowed class keys for the specified type
        /// </summary>
        public static Guid[] GetClassKeys(this Type type)
        {
            return type.GetCustomAttributes<ClassConceptKeyAttribute>().Select(o => Guid.Parse(o.ClassConcept)).ToArray();
        }

        /// <summary>
        /// Get the serialization type name
        /// </summary>
        public static String GetSerializationName(this Type type)
        {
            return type.GetCustomAttribute<XmlRootAttribute>(false)?.ElementName ?? type.GetCustomAttribute<JsonObjectAttribute>(false)?.Id ?? type.GetCustomAttribute<XmlTypeAttribute>(false)?.TypeName ?? type.FullName;
        }

        /// <summary>
        /// Get the resource type from the serialization name
        /// </summary>
        /// <param name="rootElement">The serialization name</param>
        /// <returns>The resource type</returns>
        public static Type GetResourceType(this XName rootElement)
        {
            if(s_resourceNames == null)
            {
                s_resourceNames = AppDomain.CurrentDomain.GetAllTypes()
                    .Where(o => o.GetCustomAttribute<XmlRootAttribute>() != null)
                    .ToDictionaryIgnoringDuplicates(o => $"{o.GetCustomAttribute<XmlRootAttribute>().Namespace}#{o.GetCustomAttribute<XmlRootAttribute>().ElementName}", o => o);
            }
            return s_resourceNames.TryGetValue($"{rootElement.Namespace}#{rootElement.LocalName}", out var retVal) ? retVal : null;
        }

        /// <summary>
        /// Get the classifier property on <paramref name="type"/>
        /// </summary>
        public static PropertyInfo GetSanteDBProperty<AttributeType>(this Type type) where AttributeType : Attribute, IPropertyReference
        {
            var classifierAttribute = type.GetCustomAttribute<AttributeType>();
            if (classifierAttribute == null)
            {
                return null;
            }
            else
            {
                return type.GetProperty(classifierAttribute.PropertyName);
            }
        }


        /// <summary>
        /// Get a property based on XML property and/or serialization redirect and/or query parameter
        /// </summary>
        public static PropertyInfo GetQueryProperty(this Type type, string propertyName, bool followReferences = false, bool dropXmlSuffix = true)
        {
            PropertyInfo retVal = null;
            var key = String.Format("{0}.{1}[{2}]", type.FullName, propertyName, followReferences);
            if (!s_propertyCache.TryGetValue(key, out retVal))
            {
                retVal = type.GetProperties().FirstOrDefault(o => o.GetCustomAttributes<XmlElementAttribute>()?.FirstOrDefault()?.ElementName == propertyName || o.GetCustomAttribute<QueryParameterAttribute>()?.ParameterName == propertyName || o.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName == propertyName);
                if (retVal == null)
                {
                    return null;
                }

                if (followReferences)
                {
                    retVal = type.GetProperties().FirstOrDefault(o => o.GetCustomAttribute<SerializationReferenceAttribute>()?.RedirectProperty == retVal.Name) ?? retVal;
                }

                if (retVal.Name.EndsWith("Xml") && dropXmlSuffix)
                {
                    retVal = type.GetProperty(retVal.Name.Substring(0, retVal.Name.Length - 3));
                }

                s_propertyCache.TryAdd(key, retVal);
            }
            return retVal;
        }

        /// <summary>
        /// Compute a basic hash string
        /// </summary>
        public static String HashCode(this byte[] me)
        {
            long hash = 1009;
            foreach (var b in me)
            {
                hash = ((hash << 5) + hash) ^ b;
            }

            return BitConverter.ToString(BitConverter.GetBytes(hash)).Replace("-", "");
        }

        /// <summary>
        /// Determines whether the specified property name has property.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the specified property name has property; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">source - Value cannot be null</exception>
        /// <exception cref="System.ArgumentException">Value cannot be null or empty - propertyName</exception>
        public static bool HasProperty(this ExpandoObject source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Value cannot be null");
            }

            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Value cannot be null or empty", nameof(propertyName));
            }

            return ((IDictionary<string, object>)source).ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets the full name of the user entity.
        /// </summary>
        /// <param name="entity">The user entity.</param>
        /// <param name="nameUseKey">The name use key.</param>
        /// <returns>Returns the full name of the user entity.</returns>
        /// <exception cref="System.ArgumentNullException">If the entity is null.</exception>
        public static string GetFullName(this UserEntity entity, Guid nameUseKey)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Value cannot be null");
            }

            var given = entity.Names.Where(n => n.NameUseKey == nameUseKey).SelectMany(n => n.Component).Where(c => c.ComponentTypeKey == NameComponentKeys.Given).Select(c => c.Value).ToList();
            var family = entity.Names.Where(n => n.NameUseKey == nameUseKey).SelectMany(n => n.Component).Where(c => c.ComponentTypeKey == NameComponentKeys.Family).Select(c => c.Value).ToList();

            return string.Join(" ", given) + " " + string.Join(" ", family);
        }

        /// <summary>
        /// Determine semantic equality of each item in me and other
        /// </summary>
        public static bool SemanticEquals<TEntity>(this IEnumerable<TEntity> me, IEnumerable<TEntity> other) where TEntity : IdentifiedData
        {
            if (other == null)
            {
                return false;
            }

            bool equals = me.Count() == other.Count();
            foreach (var itm in me)
            {
                equals &= other.Any(o => o.SemanticEquals(itm));
            }

            foreach (var itm in other)
            {
                equals &= me.Any(o => o.SemanticEquals(itm));
            }

            return equals;
        }

        /// <summary>
        /// Strips list
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>Returns the type.</returns>
        public static Type StripGeneric(this Type t)
        {
            return t.IsGenericType ? t.GenericTypeArguments[0] : t;
        }

        /// <summary>
        /// Strips any nullable typing.
        /// </summary>
        /// <param name="t">The typ.</param>
        /// <returns>Returns the type.</returns>
        public static Type StripNullable(this Type t)
        {
            return Nullable.GetUnderlyingType(t) ?? t;
        }

        /// <summary>
        /// Determine if <paramref name="t"/> is a nullable type
        /// </summary>
        /// <param name="t">The type of the nullable</param>
        /// <returns>True if the type is nullable</returns>
        public static bool IsNullable(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Validates that this object has a target entity
        /// </summary>
        public static IEnumerable<ValidationResultDetail> Validate<TSourceType>(this VersionedAssociation<TSourceType> me) where TSourceType : VersionedEntityData<TSourceType>, new()
        {
            var validResults = new List<ValidationResultDetail>();
            if (me.SourceEntityKey == Guid.Empty)
            {
                validResults.Add(new ValidationResultDetail(ResultDetailType.Error, String.Format("({0}).{1} required", me.GetType().Name, "SourceEntityKey"), null, null));
            }

            return validResults;
        }

        /// <summary>
        /// Returns true if <paramref name="p"/> has <typeparamref name="TAttribute"/>
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to check for</typeparam>
        /// <param name="p">The type on which the attribute should be checked</param>
        /// <returns>True if <paramref name="p"/> is annoted with <typeparamref name="TAttribute"/></returns>
        public static bool HasCustomAttribute<TAttribute>(this PropertyInfo p)
            => HasCustomAttribute(p, typeof(TAttribute));

        /// <summary>
        /// Non generic implementation of <see cref="HasCustomAttribute{TAttribute}(Type)"/>
        /// </summary>
        /// <param name="p">The type on which <paramref name="attributeType"/> should be checked</param>
        /// <param name="attributeType">The type of attribute to check on <paramref name="p"/></param>
        /// <returns>True if <paramref name="p"/> is annotated with <paramref name="attributeType"/></returns>
        public static bool HasCustomAttribute(this PropertyInfo p, Type attributeType)
            => p?.GetCustomAttribute(attributeType) != null;

        /// <summary>
        /// Returns true if <paramref name="t"/> has <typeparamref name="TAttribute"/>
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to check for</typeparam>
        /// <param name="t">The type on which the attribute should be checked</param>
        /// <returns>True if <paramref name="t"/> is annoted with <typeparamref name="TAttribute"/></returns>
        public static bool HasCustomAttribute<TAttribute>(this Type t)
            => HasCustomAttribute(t, typeof(TAttribute));

        /// <summary>
        /// Non generic implementation of <see cref="HasCustomAttribute{TAttribute}(Type)"/>
        /// </summary>
        /// <param name="t">The type on which <paramref name="attributeType"/> should be checked</param>
        /// <param name="attributeType">The type of attribute to check on <paramref name="t"/></param>
        /// <returns>True if <paramref name="t"/> is annotated with <paramref name="attributeType"/></returns>
        public static bool HasCustomAttribute(this Type t, Type attributeType)
            => t?.GetCustomAttribute(attributeType) != null;
        //=> t?.CustomAttributes?.Any(cad => cad.AttributeType == attributeType) ?? false;
        

        /// <summary>
        /// Convert the exception to a human readable string
        /// </summary>
        public static string ToHumanReadableString(this Exception e)
        {
            StringBuilder retVal = new StringBuilder($"{e.GetType().Name} : {e.Message}");
            while(e.InnerException != null)
            {
                retVal.AppendFormat("\r\nCAUSE: {0}: {1}", e.InnerException.GetType().Name, e.InnerException.Message);
                e = e.InnerException;
            }
            return retVal.ToString();
        }

    }
}