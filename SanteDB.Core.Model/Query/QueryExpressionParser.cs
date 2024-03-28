/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Map;
using SanteDB.Core.Model.Serialization;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Query
{


    /// <summary>
    /// A class which is responsible for translating a series of Query Parameters to a LINQ expression
    /// to be passed to the persistence layer
    /// </summary>
    public static class QueryExpressionParser
    {

        private static readonly Regex m_valueExtractionRegex = new Regex(@"^((?:(?:\w+?|\w+?\[(?:[^\]]*)\])(?:\.|\@\w+?\.?)?)*)\=(.*)$", RegexOptions.Compiled);
        private static readonly Regex m_propertyExtractionRegex = new Regex(@"^(([_$\w]+)(?:\[([^\]]+)\])?(?:@(\w+))?(\??))\.?(.*?)$", RegexOptions.Compiled);
        private static readonly ModelSerializationBinder m_modelBinder = new ModelSerializationBinder();

        // Member cache
        private static ConcurrentDictionary<Type, ConcurrentDictionary<String, PropertyInfo>> m_memberCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertyInfo>>();
        // Cast cache
        private static ConcurrentDictionary<String, Type> m_castCache = new ConcurrentDictionary<string, Type>();
        // Redirect cache
        private static ConcurrentDictionary<Type, Dictionary<String, PropertyInfo>> m_redirectCache = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        /// <summary>
        /// Gets the default built in variables
        /// </summary>
        private static readonly Dictionary<String, Func<object>> m_builtInVars = new Dictionary<string, Func<object>>();

        /// Static CTOR        
        static QueryExpressionParser()
        {
            m_builtInVars.Add("now", () => DateTimeOffset.Now);
            m_builtInVars.Add("today", () => DateTime.Now.Date);
        }

        /// <summary>
        /// Build the order by expression
        /// </summary>
        public static ModelSort<TModelType>[] BuildSort<TModelType>(List<String> orderBy)
        {
            // Order by
            List<ModelSort<TModelType>> retVal = new List<ModelSort<TModelType>>();
            foreach (var itm in orderBy)
            {
                var sortData = itm.Split(':');
                retVal.Add(new ModelSort<TModelType>(
                    QueryExpressionParser.BuildPropertySelector<TModelType>(sortData[0]),
                    sortData.Length == 1 || sortData[1] == "asc" ? Core.Model.Map.SortOrderType.OrderBy : Core.Model.Map.SortOrderType.OrderByDescending
                ));
            }
            return retVal.ToArray();
        }

        /// <summary>
        /// Build expression for specified type
        /// </summary>
        public static LambdaExpression BuildLinqExpression(Type modelType, NameValueCollection httpQueryParameters)
        {
            return BuildLinqExpression(modelType, httpQueryParameters, "o");
        }

        /// <summary>
        /// Build expression for specified type
        /// </summary>
        /// <param name="relayControlVariables">When true, the <see cref="QueryFilterExtensions.WithControl(object, string, object)"/> extension method should be used to convey the control variables</param>
        /// <param name="forceLoad">When true, use the <see cref="ExtensionMethods.LoadProperty(IAnnotatedResource, string, bool)"/> on all calls to load values on the path. This is useful if the resulting Linq expression is intended to be executed in memory</param>
        /// <param name="httpQueryParameters">The HTTP query parameter collection to parse into a LINQ expression</param>
        /// <param name="lazyExpandVariables">When true, all variables in <paramref name="variables"/> should be called in the LINQ expression, when false the variables are evaluated when the expression is created</param>
        /// <param name="modelType">The type of model to which the returned LINQ expression should accept as a parameter</param>
        /// <param name="parameterName">The name of the parameter in the Lambda expression</param>
        /// <param name="safeNullable">When true, use coalesce operators in the Lambda expression to provide a default value. This is useful if the LINQ expression will be used on memory objects</param>
        /// <param name="coalesceOutput">When true, all outputs on selectors should be coalesced with a new object (always return a value)</param>
        /// <param name="collectionResolutionMethod">When provided resolve the terminal statement of a collection with the specified function</param>
        /// <param name="alwaysCoalesce">When true, all values in the created pat should be coalesced</param>
        /// <param name="variables">The variable evaluators to use when expanding <c>$variable</c> expressions in the HDSI path</param>
        public static LambdaExpression BuildLinqExpression(Type modelType, NameValueCollection httpQueryParameters, string parameterName, Dictionary<String, Func<object>> variables = null, bool safeNullable = true, bool alwaysCoalesce = false, bool forceLoad = false, bool lazyExpandVariables = true, bool relayControlVariables = false, bool coalesceOutput = true, string collectionResolutionMethod = nameof(Enumerable.FirstOrDefault))
        {
            var controlMethod = typeof(QueryFilterExtensions).GetMethod(nameof(QueryFilterExtensions.WithControl), BindingFlags.Static | BindingFlags.NonPublic);

            var parameterExpression = Expression.Parameter(modelType, parameterName);
            Expression retVal = null;

            List<KeyValuePair<String, String[]>> workingValues = new List<KeyValuePair<string, string[]>>();
            // Iterate 
            foreach (var nvc in httpQueryParameters?.AllKeys ?? new string[0])
            {
                workingValues.Add(new KeyValuePair<string, string[]>(nvc, httpQueryParameters.GetValues(nvc)));
            }

            // Get the first values
            while (workingValues.Count > 0)
            {
                var currentValue = workingValues.FirstOrDefault();
                workingValues.Remove(currentValue);

                if (currentValue.Value?.Count(o => !String.IsNullOrEmpty(o)) == 0)
                {
                    continue;
                }

                // Create accessor expression
                Expression keyExpression = null;
                Expression accessExpression = parameterExpression;


                var currentPathString = currentValue.Key;

                var accessResult = m_propertyExtractionRegex.Match(currentPathString);
                while(accessResult.Success)
                {

                    // Get the raw member to the path 
                    var propertyGroup = accessResult.Groups[1].Value;
                   
                    var propertyPathRaw = accessResult.Groups[2].Value;
                    var guardExpressionRaw = accessResult.Groups[3].Value;
                    var castExpressionRaw = accessResult.Groups[4].Value;
                    var isCoalesced = accessResult.Groups[5].Value == "?" || alwaysCoalesce;
                    var isControlParameter = propertyPathRaw.StartsWith("_");
                    var remainderExpressionRaw = accessResult.Groups[6].Value;

                    if (isControlParameter && relayControlVariables) // Control of parameter 
                    {
                        accessExpression = Expression.Call(null, controlMethod, accessExpression, Expression.Constant(propertyPathRaw), Expression.Constant(null));
                    }
                    else if (!isControlParameter)
                    {
                        // Attempt to get property cache
                        if (!m_memberCache.TryGetValue(accessExpression.Type, out var memberCache))
                        {
                            memberCache = new ConcurrentDictionary<string, PropertyInfo>();
                            m_memberCache.TryAdd(accessExpression.Type, memberCache);
                        }

                        // Get or member info
                        var followRefs = !String.IsNullOrEmpty(remainderExpressionRaw) || !String.IsNullOrEmpty(castExpressionRaw);
                        var propertyPathKey = $"{propertyPathRaw}{followRefs}";
                        if (!memberCache.TryGetValue(propertyPathKey, out var memberInfo))
                        {
                            // Get the property
                            memberInfo = accessExpression.Type.GetQueryProperty(propertyPathRaw, followReferences: followRefs) ??
                                    accessExpression.Type.GetInterfaces().Select(o => o.GetQueryProperty(propertyPathRaw, followReferences: followRefs)).OfType<PropertyInfo>().FirstOrDefault() ??
                                    throw new ArgumentOutOfRangeException(currentValue.Key, ErrorMessages.HDSI_PATH_INVALID);

                            if (memberInfo.Name.EndsWith("Xml")) // Handle XML properties
                            {
                                memberInfo = accessExpression.Type.GetRuntimeProperty(memberInfo.Name.Replace("Xml", ""));
                            }

                            // Member cache
                            memberCache.TryAdd(propertyPathKey, memberInfo);

                        }

                        // Force loading of properties
                        if (forceLoad)
                        {
                            if (typeof(IList).IsAssignableFrom(memberInfo.PropertyType))
                            {
                                var loadMethod = (MethodInfo)typeof(ExtensionMethods).GetGenericMethod(nameof(ExtensionMethods.LoadCollection), new Type[] { memberInfo.PropertyType.GetGenericArguments()[0] }, new Type[] { typeof(IdentifiedData), typeof(String), typeof(bool) });
                                accessExpression = Expression.Call(loadMethod, accessExpression, Expression.Constant(memberInfo.Name), Expression.Constant(false));
                            }
                            else if (typeof(IAnnotatedResource).IsAssignableFrom(memberInfo.PropertyType))
                            {
                                var loadMethod = (MethodInfo)typeof(ExtensionMethods).GetGenericMethod(nameof(ExtensionMethods.LoadProperty), new Type[] { memberInfo.PropertyType }, new Type[] { typeof(IdentifiedData), typeof(String), typeof(bool) });
                                accessExpression = Expression.Call(loadMethod, accessExpression, Expression.Constant(memberInfo.Name), Expression.Constant(false));
                            }
                            else
                            {
                                accessExpression = Expression.MakeMemberAccess(accessExpression, memberInfo);
                            }
                        }
                        else
                        {
                            accessExpression = Expression.MakeMemberAccess(accessExpression, memberInfo);
                        }

                        // Casting
                        if (!String.IsNullOrEmpty(castExpressionRaw))
                        {

                            Type castType = null;
                            if (!m_castCache.TryGetValue(castExpressionRaw, out castType))
                            {
                                castType = m_modelBinder.BindToType(null, castExpressionRaw);
                                if (castType == null)
                                {
                                    throw new ArgumentOutOfRangeException(nameof(castType), castExpressionRaw);
                                }

                                m_castCache.TryAdd(castExpressionRaw, castType);
                            }
                            accessExpression = Expression.TypeAs(accessExpression, castType);
                            isCoalesced |= safeNullable;
                        }

                        // Coalesce
                        if (isCoalesced && accessExpression.Type.GetConstructor(Type.EmptyTypes) != null)
                        {
                            accessExpression = Expression.Coalesce(accessExpression, Expression.New(accessExpression.Type));
                        }

                        // Guard?
                        if (!String.IsNullOrEmpty(guardExpressionRaw))
                        {
                            Type itemType = accessExpression.Type.GenericTypeArguments[0];
                            Type predicateType = typeof(Func<,>).MakeGenericType(itemType, typeof(bool));
                            ParameterExpression guardParameter = Expression.Parameter(itemType, "guard");

                            // Next we want to determine if the guard is an expression or just a list of values
                            Expression guardExpression = null;
                            if (guardExpressionRaw.Contains('='))
                            {
                                var guardFilterExpression = guardExpressionRaw.ParseQueryString();
                                guardExpression = BuildLinqExpression(itemType, guardFilterExpression, "guard", variables, safeNullable, alwaysCoalesce, forceLoad, lazyExpandVariables, relayControlVariables, coalesceOutput, collectionResolutionMethod);
                            }
                            else
                            {
                                var guardList = guardExpressionRaw.Split('|');
                                if (guardList.All(o => o.Equals("null", StringComparison.OrdinalIgnoreCase) || Guid.TryParse(o, out _))) // Explicit list of UUIDs
                                {
                                    // Does the classifier key property point at the key attribute if so we use this
                                    // property, otherwise we use the serialization redirect property
                                    var classifierProperty = itemType.GetClassifierKeyProperty();
                                    if (classifierProperty == null)
                                    {
                                        throw new ArgumentOutOfRangeException(propertyPathRaw, String.Format(ErrorMessages.HDSI_GUARD_INVALID, "UUID not permitted"));
                                    }

                                    foreach (var g in guardList)
                                    {
                                        var value = g.Equals("null", StringComparison.OrdinalIgnoreCase) ? (Guid?)null : Guid.Parse(g);
                                        var expr = Expression.MakeBinary(ExpressionType.Equal, Expression.MakeMemberAccess(guardParameter, classifierProperty), Expression.Convert(Expression.Constant(value), classifierProperty.PropertyType));
                                        if (guardExpression == null)
                                        {
                                            guardExpression = expr;
                                        }
                                        else
                                        {
                                            guardExpression = Expression.MakeBinary(ExpressionType.OrElse, guardExpression, expr);
                                        }
                                    }
                                    guardExpression = Expression.Lambda(guardExpression, guardParameter);
                                }
                                else // Named values - so we have to follow the property path - we will follow the classifiers through the sub objects
                                {
                                    var classifierProperty = itemType.GetClassifierProperty();
                                    Expression guardAccessor = guardParameter;
                                    while (classifierProperty != null)
                                    {
                                        if (typeof(IdentifiedData).IsAssignableFrom(classifierProperty.PropertyType))
                                        {
                                            if (forceLoad) // Force the loading of properties in the guard
                                            {
                                                var loadMethod = (MethodInfo)typeof(ExtensionMethods).GetGenericMethod(nameof(ExtensionMethods.LoadProperty), new Type[] { classifierProperty.PropertyType }, new Type[] { typeof(IdentifiedData), typeof(String), typeof(bool) });
                                                var loadExpression = Expression.Call(loadMethod, guardAccessor, Expression.Constant(classifierProperty.Name), Expression.Constant(false));
                                                guardAccessor = Expression.Coalesce(loadExpression, Expression.New(classifierProperty.PropertyType));
                                            }
                                            else
                                            {
                                                guardAccessor = Expression.Coalesce(Expression.MakeMemberAccess(guardAccessor, classifierProperty), Expression.New(classifierProperty.PropertyType));
                                            }
                                        }
                                        else
                                        {
                                            guardAccessor = Expression.MakeMemberAccess(guardAccessor, classifierProperty);
                                        }
                                        classifierProperty = classifierProperty.PropertyType.GetClassifierProperty();
                                    }

                                    // Now make expression for guard
                                    if (guardExpressionRaw.Equals("null", StringComparison.OrdinalIgnoreCase))
                                    {
                                        guardExpression = Expression.MakeBinary(ExpressionType.Equal, guardAccessor, Expression.Constant(null));
                                    }
                                    else
                                    {
                                        foreach (var g in guardList)
                                        {
                                            // HACK: Some types use enums as their classifier 
                                            object value = g;
                                            if (guardAccessor.Type.IsEnum)
                                            {
                                                value = Enum.Parse(guardAccessor.Type, g);
                                            }

                                            var expr = Expression.MakeBinary(ExpressionType.Equal, guardAccessor, Expression.Constant(value));
                                            if (guardExpression == null)
                                            {
                                                guardExpression = expr;
                                            }
                                            else
                                            {
                                                guardExpression = Expression.MakeBinary(ExpressionType.Or, guardExpression, expr);
                                            }
                                        }
                                    }

                                    guardExpression = Expression.Lambda(guardExpression, guardParameter);
                                }
                            }

                            if (!(guardExpression is LambdaExpression))
                            {
                                throw new NotSupportedException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(LambdaExpression), guardExpression.GetType()));
                            }

                            MethodInfo whereMethod = typeof(Enumerable).GetGenericMethod("Where",
                                    new Type[] { itemType },
                                    new Type[] { accessExpression.Type, predicateType }) as MethodInfo;
                            accessExpression = Expression.Call(whereMethod, accessExpression, guardExpression);

                            if (currentValue.Value?.Length == 1 && currentValue.Value[0].EndsWith("null") && String.IsNullOrEmpty(remainderExpressionRaw))
                            {
                                var anyMethod = typeof(Enumerable).GetGenericMethod("Any",
                                    new Type[] { itemType },
                                    new Type[] { accessExpression.Type }) as MethodInfo;
                                accessExpression = Expression.Call(anyMethod, accessExpression);
                                currentValue.Value[0] = currentValue.Value[0].Replace("null", "false");
                            }
                        }

                        // Is our access expression leaving on a collection? If so we want to use the Any() function 
                        if (accessExpression.Type.IsEnumerable() &&
                            accessExpression.Type.IsGenericType)
                        {

                            // First or default - we are not filtering on a value
                            if (currentValue.Value == null)
                            {
                                if (!String.IsNullOrEmpty(collectionResolutionMethod))
                                {
                                    Type itemType = accessExpression.Type.GenericTypeArguments[0];
                                    Type predicateType = typeof(Func<,>).MakeGenericType(itemType, typeof(bool));
                                    var firstMethod = typeof(Enumerable).GetGenericMethod(collectionResolutionMethod, new Type[] { itemType }, new Type[] { accessExpression.Type }) as MethodInfo;
                                    accessExpression = Expression.Call(firstMethod, accessExpression);
                                }
                            }
                            else // We are filtering - so we want to use ANY and pass in the rest of our sub-values
                            {
                                Type itemType = accessExpression.Type.GenericTypeArguments[0];
                                Type predicateType = typeof(Func<,>).MakeGenericType(itemType, typeof(bool));

                                var anyMethod = typeof(Enumerable).GetGenericMethod(nameof(Enumerable.Any),
                                    new Type[] { itemType },
                                    new Type[] { accessExpression.Type, predicateType }) as MethodInfo;

                                // A sub-filter
                                var subFilter = new NameValueCollection();

                                // Default to id
                                if(String.IsNullOrEmpty(remainderExpressionRaw) && currentValue.Value.All(o=>Guid.TryParse(o, out _)))
                                {
                                    Array.ForEach(currentValue.Value, o => subFilter.Add("id", o));
                                }
                                else if (!String.IsNullOrEmpty(remainderExpressionRaw))
                                {
                                    Array.ForEach(currentValue.Value, o => subFilter.Add(remainderExpressionRaw, o));
                                }
                                else  // just the same property so is simple
                                {
                                    Array.ForEach(currentValue.Value, o => subFilter.Add(String.Empty, o));
                                }

                                // Add collect other parameters
                                foreach (var wv in workingValues.Where(o => o.Key.StartsWith(propertyGroup)).ToList())
                                {
                                    var keyName = wv.Key.Substring(propertyGroup.Length + 1);
                                    Array.ForEach(wv.Value, o => subFilter.Add(keyName, o));
                                    workingValues.Remove(wv);
                                }

                                Expression predicate = BuildLinqExpression(itemType, subFilter, propertyPathRaw, variables: variables, safeNullable: safeNullable, forceLoad: forceLoad, lazyExpandVariables: lazyExpandVariables, alwaysCoalesce: alwaysCoalesce, coalesceOutput: coalesceOutput, collectionResolutionMethod: collectionResolutionMethod);
                                if (predicate == null) // No predicate so just ANY()
                                {
                                    continue;
                                }

                                keyExpression = Expression.Call(anyMethod, accessExpression, predicate);
                                currentValue = new KeyValuePair<string, string[]>("", new string[0]);
                                break;  // skip
                            }
                        }
                        // Is this an access expression?
                        if (currentValue.Value == null && typeof(IdentifiedData).IsAssignableFrom(accessExpression.Type) && coalesceOutput)
                        {
                            accessExpression = Expression.Coalesce(accessExpression, Expression.New(accessExpression.Type));
                        }

                    }

                    if (String.IsNullOrEmpty(remainderExpressionRaw))
                    {
                        break;
                    }
                    accessResult = m_propertyExtractionRegex.Match(remainderExpressionRaw);
                }
                
                // HACK: Was there any mapping done?
                if (accessExpression == parameterExpression)
                {
                    continue;
                }

                // Now expression
                var kp = currentValue.Value;
                if (kp != null)
                {
                    foreach (var qValue in kp.Where(o => !String.IsNullOrEmpty(o)))
                    {
                        var value = qValue;
                        var thisAccessExpression = accessExpression;
                        // HACK: Fuzz dates for intervals
                        if ((thisAccessExpression.Type.StripNullable() == typeof(DateTime) ||
                            thisAccessExpression.Type.StripNullable() == typeof(DateTimeOffset)) &&
                            value.Length <= 7 &&
                            !value.StartsWith("~") &&
                            !value.Contains("null") &&
                            !value.Contains("$")
                            )
                        {
                            value = "~" + value;
                        }

                        Expression nullCheckExpr = null;
                        Type operandType = thisAccessExpression.Type;

                        // Correct for nullable
                        if (value != "null" && value != "!null" && thisAccessExpression.Type.IsGenericType && thisAccessExpression.Type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                            safeNullable)
                        {
                            nullCheckExpr = Expression.MakeBinary(ExpressionType.NotEqual, thisAccessExpression, Expression.Constant(null));
                            thisAccessExpression = Expression.MakeMemberAccess(thisAccessExpression, accessExpression.Type.GetRuntimeProperty("Value"));
                        }

                        // Process value
                        ExpressionType et = ExpressionType.Equal;
                        IQueryFilterExtension extendedFilter = null;
                        List<String> extendedParms = new List<string>();

                        if (String.IsNullOrEmpty(value))
                        {
                            continue;
                        }
                        // The input parameter is an extended filter operation, let's parse it
                        else if (value[0] == ':')
                        {
                            var opMatch = QueryFilterExtensions.ExtendedFilterRegex.Match(value);
                            if (opMatch.Success)
                            {

                                // Extract
                                String fnName = opMatch.Groups[1].Value,
                                    parms = opMatch.Groups[3].Value,
                                    operand = opMatch.Groups[4].Value;

                                var parmExtract = QueryFilterExtensions.ParameterExtractRegex.Match(parms + ",");
                                while (parmExtract.Success)
                                {
                                    extendedParms.Add(parmExtract.Groups[1].Value);
                                    parmExtract = QueryFilterExtensions.ParameterExtractRegex.Match(parmExtract.Groups[2].Value);
                                }
                                //extendedParms = parms.Split(',');

                                // Now find the expression
                                extendedFilter = QueryFilterExtensions.GetExtendedFilter(fnName);
                                if (extendedFilter == null) // ensure valid reference
                                {
                                    throw new MissingMemberException(fnName);
                                }

                                value = String.IsNullOrEmpty(operand) ? "true" : operand;
                                operandType = extendedFilter.ExtensionMethod.ReturnType;
                            }
                        }

                        String pValue = value;

                        // New syntax for operators:
                        // gte:clause
                        // gt:clause
                        // 
                        var indexOfColon = value.IndexOf(':');
                        if (indexOfColon > 0 && indexOfColon < 4)
                        {
                            var op = value.Substr(0, indexOfColon);
                            value = value.Substring(indexOfColon + 1);
                            switch (op)
                            {
                                case "gt":
                                    value = $">{value}";
                                    break;
                                case "gte":
                                    value = $">={value}";
                                    break;
                                case "lt":
                                    value = $"<{value}";
                                    break;
                                case "lte":
                                    value = $"<={value}";
                                    break;
                                case "eq":
                                    value = $"{value}";
                                    break;
                                case "ne":
                                    value = $"!{value}";
                                    break;
                                case "ap":
                                    value = $"~{value}";
                                    break;
                                default:
                                    value = $"{op}:{value}"; // pass it along
                                    break;
                            }
                        }

                        // Operator type
                        switch (value[0])
                        {
                            case '<':
                                et = ExpressionType.LessThan;
                                pValue = value.Substring(1);
                                if (pValue[0] == '=')
                                {
                                    et = ExpressionType.LessThanOrEqual;
                                    pValue = pValue.Substring(1);
                                }
                                break;
                            case '>':
                                et = ExpressionType.GreaterThan;
                                pValue = value.Substring(1);
                                if (pValue[0] == '=')
                                {
                                    et = ExpressionType.GreaterThanOrEqual;
                                    pValue = pValue.Substring(1);
                                }
                                break;
                            case '^':
                                et = ExpressionType.Equal;
                                if (thisAccessExpression.Type == typeof(String))
                                {
                                    thisAccessExpression = Expression.Call(thisAccessExpression, typeof(String).GetRuntimeMethod("StartsWith", new Type[] { typeof(String) }), Expression.Constant(pValue.Substring(1)));
                                    operandType = typeof(bool);
                                    pValue = "true";
                                }
                                else
                                {
                                    throw new InvalidOperationException("^ can only be applied to string properties");
                                }

                                break;
                            case '~':
                                et = ExpressionType.Equal;
                                if (thisAccessExpression.Type == typeof(String))
                                {
                                    pValue = pValue.Substring(1);
                                    if (pValue.StartsWith("$"))
                                    {
                                        thisAccessExpression = Expression.Call(thisAccessExpression, typeof(String).GetRuntimeMethod("Contains", new Type[] { typeof(String) }), GetVariableExpression(pValue.Substring(1), thisAccessExpression.Type, variables, parameterExpression, lazyExpandVariables) ?? Expression.Constant(pValue));
                                    }
                                    else
                                    {
                                        thisAccessExpression = Expression.Call(thisAccessExpression, typeof(String).GetRuntimeMethod("Contains", new Type[] { typeof(String) }), Expression.Constant(pValue));
                                    }

                                    operandType = typeof(bool);
                                    pValue = "true";
                                }
                                else if (thisAccessExpression.Type == typeof(DateTime) ||
                                    thisAccessExpression.Type == typeof(DateTime?) ||
                                    thisAccessExpression.Type == typeof(DateTimeOffset) ||
                                    thisAccessExpression.Type == typeof(DateTimeOffset?))
                                {
                                    pValue = value.Substring(1);
                                    DateTime dateLow = DateTime.ParseExact(pValue, "yyyy-MM-dd".Substring(0, pValue.Length), CultureInfo.InvariantCulture), dateHigh = DateTime.MaxValue;
                                    if (pValue.Length == 4) // Year
                                    {
                                        dateHigh = new DateTime(dateLow.Year, 12, 31, 23, 59, 59);
                                    }
                                    else if (pValue.Length == 7)
                                    {
                                        dateHigh = new DateTime(dateLow.Year, dateLow.Month, DateTime.DaysInMonth(dateLow.Year, dateLow.Month), 23, 59, 59);
                                    }
                                    else if (pValue.Length == 10)
                                    {
                                        dateHigh = new DateTime(dateLow.Year, dateLow.Month, dateLow.Day, 23, 59, 59);
                                    }

                                    if (thisAccessExpression.Type == typeof(DateTime?) || thisAccessExpression.Type == typeof(DateTimeOffset?))
                                    {
                                        thisAccessExpression = Expression.MakeMemberAccess(thisAccessExpression, thisAccessExpression.Type.GetRuntimeProperty("Value"));
                                    }

                                    // Correct for DTO if originally is
                                    if (thisAccessExpression.Type == typeof(DateTimeOffset) || thisAccessExpression.Type == typeof(DateTimeOffset?))
                                    {
                                        Expression lowerBound = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, thisAccessExpression, Expression.Constant((DateTimeOffset)dateLow)),
                                            upperBound = Expression.MakeBinary(ExpressionType.LessThanOrEqual, thisAccessExpression, Expression.Constant((DateTimeOffset)dateHigh));
                                        thisAccessExpression = Expression.MakeBinary(ExpressionType.AndAlso, lowerBound, upperBound);

                                    }
                                    else
                                    {
                                        Expression lowerBound = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, thisAccessExpression, Expression.Constant(dateLow)),
                                            upperBound = Expression.MakeBinary(ExpressionType.LessThanOrEqual, thisAccessExpression, Expression.Constant(dateHigh));
                                        thisAccessExpression = Expression.MakeBinary(ExpressionType.AndAlso, lowerBound, upperBound);

                                    }
                                    operandType = thisAccessExpression.Type;
                                    pValue = "true";
                                }
                                else
                                {
                                    throw new InvalidOperationException($"~ can only be applied to string and date properties not {thisAccessExpression.Type}");
                                }

                                break;
                            case '!':
                                et = ExpressionType.NotEqual;
                                pValue = value.Substring(1);
                                break;
                        }

                        // The expression
                        Expression valueExpr = null;
                        if (pValue == "null")
                        {
                            valueExpr = Expression.Constant(null);
                        }
                        else if (pValue.StartsWith("$"))
                        {
                            valueExpr = GetVariableExpression(pValue.Substring(1), thisAccessExpression.Type, variables, parameterExpression, lazyExpandVariables) ?? Expression.Constant(pValue);
                        }
                        else if (operandType == typeof(String))
                        {
                            valueExpr = Expression.Constant(pValue);
                        }
                        else if (operandType == typeof(DateTime) || operandType == typeof(DateTime?))
                        {
                            valueExpr = Expression.Constant(DateTime.Parse(pValue));
                        }
                        else if (operandType == typeof(DateTimeOffset) || operandType == typeof(DateTimeOffset?))
                        {
                            valueExpr = Expression.Constant(DateTimeOffset.Parse(pValue));
                        }
                        else if (operandType == typeof(Guid))
                        {
                            valueExpr = Expression.Constant(Guid.Parse(pValue));
                        }
                        else if (operandType == typeof(Guid?))
                        {
                            valueExpr = Expression.Convert(Expression.Constant(Guid.Parse(pValue)), typeof(Guid?));
                        }
                        else if (operandType == typeof(TimeSpan) || operandType == typeof(TimeSpan?))
                        {
                            if (!TimeSpan.TryParse(pValue, out TimeSpan ts))
                            {
                                try
                                {
                                    ts = XmlConvert.ToTimeSpan(pValue);
                                }
                                catch
                                {
                                    ts = SanteDBConvert.StringToTimespan(pValue);
                                }
                            }
                            valueExpr = Expression.Constant(ts);
                        }
                        else if (operandType.IsEnum)
                        {
                            int tryParse = 0;
                            if (Int32.TryParse(pValue, out tryParse))
                            {
                                valueExpr = Expression.Constant(Enum.ToObject(operandType, Int32.Parse(pValue)));
                            }
                            else
                            {
                                valueExpr = Expression.Constant(Enum.Parse(operandType, pValue));
                            }
                        }
                        else if (extendedFilter is IQueryFilterConverterExtension) // Just converting input string to output string
                        {
                            valueExpr = Expression.Constant(pValue);
                        }
                        else
                        {
                            try
                            {
                                Object converted = null;
                                if (MapUtil.TryConvert(pValue, operandType.StripGeneric(), out converted))
                                {
                                    valueExpr = Expression.Constant(converted);
                                }
                                else if (typeof(IdentifiedData).IsAssignableFrom(operandType) && Guid.TryParse(pValue, out Guid uuid)) // Assign to key
                                {
                                    valueExpr = Expression.Constant(uuid);
                                    thisAccessExpression = accessExpression = Expression.MakeMemberAccess(accessExpression, operandType.GetRuntimeProperty(nameof(IdentifiedData.Key)));
                                }
                                else
                                {
                                    valueExpr = Expression.Constant(Convert.ChangeType(pValue, operandType));
                                }
                            }
                            catch (Exception e)
                            {
                                throw new InvalidOperationException($"Unable to convert {pValue} to {operandType}", e);
                            }
                        }

                        // Extended filters operate in a different manner, they basically are allowed to write whatever they like to the Expression
                        Expression singleExpression = null;
                        if (extendedFilter != null)
                        {
                            // Extended parms build
                            int parmNo = 1;
                            var parms = extendedParms.Select(p =>
                            {
                                var parmList = extendedFilter.ExtensionMethod.GetParameters();
                                if (parmList.Length > parmNo)
                                {
                                    var parmType = parmList[parmNo++];
                                    if (p.StartsWith("$")) // variable
                                    {
                                        return GetVariableExpression(p.Substring(1), thisAccessExpression.Type, variables, parameterExpression, lazyExpandVariables) ?? Expression.Constant(p);
                                    }
                                    else if (parmType.ParameterType != typeof(String) && MapUtil.TryConvert(p, parmType.ParameterType, out object res)) // convert parameter type
                                    {
                                        return Expression.Constant(res);
                                    }
                                    else if (p.StartsWith("\"") && p.EndsWith("\""))
                                    {
                                        return Expression.Constant(p.Substring(1, p.Length - 2).Replace("\\\"", "\""));
                                    }
                                    else
                                    {
                                        return Expression.Constant(p);
                                    }
                                }
                                return Expression.Constant(p);
                            }).ToArray();

                            singleExpression = extendedFilter.Compose(thisAccessExpression, et, valueExpr, parms);

                        }
                        else
                        {
                            if (valueExpr.Type != thisAccessExpression.Type)
                            {
                                valueExpr = Expression.Convert(valueExpr, thisAccessExpression.Type);
                            }

                            singleExpression = Expression.MakeBinary(et, thisAccessExpression, valueExpr);
                        }

                        if (nullCheckExpr != null)
                        {
                            singleExpression = Expression.MakeBinary(ExpressionType.AndAlso, nullCheckExpr, singleExpression);
                        }

                        // Passthrough the key expression 
                        if (singleExpression is BinaryExpression be && be.Left is MethodCallExpression mce && mce.Method.Name == nameof(QueryFilterExtensions.WithControl))
                        {
                            singleExpression = Expression.MakeUnary(ExpressionType.IsTrue,
                                Expression.Convert(Expression.Call(mce.Object, mce.Method, mce.Arguments[0], mce.Arguments[1], Expression.Convert(be.Right, typeof(Object))), typeof(Boolean)), typeof(Boolean));
                        }

                        if (keyExpression == null)
                        {
                            keyExpression = singleExpression;
                        }
                        else if (et == ExpressionType.Equal)
                        {
                            keyExpression = Expression.MakeBinary(ExpressionType.OrElse, keyExpression, singleExpression);
                        }
                        else
                        {
                            keyExpression = Expression.MakeBinary(ExpressionType.AndAlso, keyExpression, singleExpression);
                        }
                    }
                }
                else
                {
                    keyExpression = accessExpression;
                }


                if (retVal == null)
                {
                    retVal = keyExpression;
                }
                else
                {

                    retVal = Expression.MakeBinary(ExpressionType.AndAlso, retVal, keyExpression);
                }
            }
            //Debug.WriteLine(String.Format("Converted {0} to {1}", httpQueryParameters, retVal));

            if (retVal == null)
            {
                return Expression.Lambda(Expression.Constant(true), Expression.Parameter(modelType));
            }

            return Expression.Lambda(retVal, parameterExpression);

        }



        /// <summary>
        /// Build linq expression from string
        /// </summary>
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(String filter)
        {
            return BuildLinqExpression<TModelType>(filter.ParseQueryString(), null);
        }
        /// <summary>
        /// Build a LINQ expression
        /// </summary>
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, Dictionary<String, Func<object>> variables)
        {
            return BuildLinqExpression<TModelType>(httpQueryParameters, variables, true);

        }

        /// <summary>
        /// Builds the linq expression.
        /// </summary>
        /// <typeparam name="TModelType">The type of the t model type.</typeparam>
        /// <param name="httpQueryParameters">The HTTP query parameters.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="safeNullable">if set to <c>true</c> [safe nullable].</param>
        /// <returns>Expression&lt;Func&lt;TModelType, System.Boolean&gt;&gt;.</returns>
        /// <param name="lazyExpandVariables">When true, variables are written to be expanded on evaluation of the LINQ expression - if false then variables are realized when the conversion is done</param>
        /// <param name="relayControlVariables">True if control parameters should be conveyed in the LINQ expression</param>
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, Dictionary<String, Func<object>> variables = null, bool safeNullable = true, bool lazyExpandVariables = true, bool relayControlVariables = false)
        {

            var expression = BuildLinqExpression<TModelType>(httpQueryParameters, "o", variables: variables, safeNullable: safeNullable, lazyExpandVariables: lazyExpandVariables, relayControlVariables: relayControlVariables);

            if (expression == null) // No query!
            {
                return (TModelType o) => true;
            }
            else
            {
                return Expression.Lambda<Func<TModelType, bool>>(expression.Body, expression.Parameters);
            }
        }

        /// <summary>
        /// Build LINQ expression
        /// </summary>
        /// <param name="forceLoad">When true, will assume the object is working on memory objects and will call LoadProperty on guards</param>
        /// <param name="httpQueryParameters">The query parameters formatter as HTTP query</param>
        /// <param name="lazyExpandVariables">When true, variables should be expanded in the LINQ expression otherwise they are realized when conversion is done</param>
        /// <param name="parameterName">The name of the parameter on the resulting Lambda</param>
        /// <param name="safeNullable">When true, coalesce operations will be injected into the LINQ to ensure object in-memory collections don't throw NRE</param>
        /// <param name="relayControlVariables"></param>
        /// <param name="variables">A list of variables which are accessed in the LambdaExpression via $variable</param>
        public static LambdaExpression BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, string parameterName, Dictionary<String, Func<object>> variables = null, bool safeNullable = true, bool forceLoad = false, bool lazyExpandVariables = true, bool relayControlVariables = false)
        {
            return BuildLinqExpression(typeof(TModelType), httpQueryParameters, parameterName, variables: variables, safeNullable: safeNullable, forceLoad: forceLoad, lazyExpandVariables: lazyExpandVariables, relayControlVariables: relayControlVariables);
        }


        /// <summary>
        /// Get variable expression
        /// </summary>
        private static Expression GetVariableExpression(string variablePath, Type expectedReturn, Dictionary<string, Func<object>> variables, ParameterExpression parameterExpression, bool lazyExpandVariables)
        {
            Func<object> val = null;
            String varName = variablePath.Contains(".") ? variablePath.Substring(0, variablePath.IndexOf(".")) : variablePath,
                varPath = variablePath.Substring(varName.Length),
                castAs = String.Empty;

            Expression scope = null;

            if (varName.Contains("@"))
            {
                var varParts = varName.Split('@');
                castAs = varParts[1];
                varName = varParts[0];
            }
            if (varName == "_")
            {
                scope = parameterExpression;
            }
            else if (variables?.TryGetValue(varName.ToLowerInvariant(), out val) == true || m_builtInVars.TryGetValue(varName.ToLowerInvariant(), out val) ||
                variables?.TryGetValue(varName, out val) == true)
            {
                if (val.GetMethodInfo().GetParameters().Length > 0)
                {
                    scope = Expression.Invoke(Expression.Constant(val));
                }
                else if (!lazyExpandVariables)
                {
                    // Expand value
                    var value = val();
                    if (!String.IsNullOrEmpty(varPath))
                    {
                        var propertySelector = BuildPropertySelector(value.GetType(), varPath.Substring(1), true);
                        scope = Expression.Constant(propertySelector.Compile().DynamicInvoke(value));
                    }
                    else
                    {
                        scope = Expression.Constant(value);
                    }
                    varPath = String.Empty;
                }
                else
                {
                    var value = val();
                    if (String.IsNullOrEmpty(varPath))
                    {
                        scope = Expression.Call(null, (MethodInfo)typeof(MapUtil).GetGenericMethod(nameof(MapUtil.Convert), new Type[] { expectedReturn }, new Type[] { typeof(object) }), Expression.Call(val.Target == null ? null : Expression.Constant(val.Target), val.GetMethodInfo()));
                    }
                    scope = Expression.Convert(Expression.Call(val.Target == null ? null : Expression.Constant(val.Target), val.GetMethodInfo()), value?.GetType() ?? expectedReturn);

                }
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.VARIABLE_NOT_FOUND, varName));
            }

            Expression retVal = scope;


            if (String.IsNullOrEmpty(varPath))
            {
                if (expectedReturn == typeof(String) && retVal.Type != typeof(String))
                {
                    if (retVal is ConstantExpression ce && ce.Value == null)
                    {
                        return retVal;
                    }
                    else
                    {
                        return Expression.Call(retVal, retVal.Type.GetMethod(nameof(Object.ToString), Type.EmptyTypes));
                    }
                }
                else if (expectedReturn == typeof(Guid) && retVal.Type != typeof(Guid))
                {
                    if (retVal is ConstantExpression ce && ce.Value == null)
                    {
                        return retVal;
                    }
                    else
                    {
                        return Expression.Call(null, typeof(Guid).GetMethod(nameof(Guid.Parse), new Type[] { typeof(String) }), retVal);
                    }
                }
                else
                {
                    return Expression.Convert(retVal, expectedReturn);
                }
            }
            else
            {
                retVal = Expression.Invoke(
                    BuildPropertySelector(scope.Type, varPath.Substring(1), true)
                    , retVal);
                if (retVal.Type.IsConstructedGenericType &&
                    retVal.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    retVal = Expression.Coalesce(retVal, Expression.Default(retVal.Type.GenericTypeArguments[0]));
                }

                return retVal;
            }
        }

        /// <summary>
        /// Build property selector
        /// </summary>
        public static LambdaExpression BuildPropertySelector<T>(String propertyName)
        {
            return BuildPropertySelector(typeof(T), propertyName);
        }

        /// <summary>
        /// Build property selector
        /// </summary>
        public static LambdaExpression BuildPropertySelector<T>(String propertyName, bool forceLoad)
        {
            return BuildPropertySelector(typeof(T), propertyName, forceLoad);
        }
        /// <summary>
        /// Build a property selector 
        /// </summary>
        public static LambdaExpression BuildPropertySelector(Type type, String propertyName, bool forceLoad)
        {
            return BuildPropertySelector(type, propertyName, forceLoad: forceLoad, convertReturn: null, returnNewObjectOnNull: true);
        }

        /// <summary>
        /// Build a property selector 
        /// </summary>
        public static LambdaExpression BuildPropertySelector(Type type, String propertyName, bool forceLoad = false, Type convertReturn = null, bool returnNewObjectOnNull = true, string collectionResolutionMethod = "FirstOrDefault")
        {
            var nvc = new NameValueCollection();
            nvc.Add(propertyName, null);
            if (convertReturn == null)
            {
                return BuildLinqExpression(type, nvc, "__xinstance", null, safeNullable: false, forceLoad: forceLoad, lazyExpandVariables: false, coalesceOutput: returnNewObjectOnNull, collectionResolutionMethod: collectionResolutionMethod);
            }
            else if (String.IsNullOrEmpty(propertyName))
            {
                var parm = Expression.Parameter(type);
                return Expression.Lambda(parm, parm);
            }
            else
            {
                var le = BuildLinqExpression(type, nvc, "__xinstance", null, false, forceLoad, false);
                return Expression.Lambda(Expression.Convert(le.Body, convertReturn), le.Parameters);
            }
        }
    }
}
