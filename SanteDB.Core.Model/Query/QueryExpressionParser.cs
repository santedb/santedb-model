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
 * Date: 2021-8-27
 */
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Query
{


    /// <summary>
    /// A class which is responsible for translating a series of Query Parmaeters to a LINQ expression
    /// to be passed to the persistence layer
    /// </summary>
    public static class QueryExpressionParser
    {

        // Member cache
        private static Dictionary<Type, Dictionary<String, PropertyInfo>> m_memberCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        // Cast cache
        private static Dictionary<String, Type> m_castCache = new Dictionary<string, Type>();
        // Redirect cache
        private static Dictionary<Type, Dictionary<String, PropertyInfo>> m_redirectCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        /// <summary>
        /// Gets the default built in variables
        /// </summary>
        private static readonly Dictionary<String, Func<object>> m_builtInVars = new Dictionary<string, Func<object>>();

        /// Static CTOR        
        static QueryExpressionParser()
        {
            m_builtInVars.Add("now", () => DateTime.Now);
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
        public static Expression BuildLinqExpression(Type modelType, NameValueCollection httpQueryParameters)
        {

            var methodInfo = typeof(QueryExpressionParser).GetGenericMethod(nameof(QueryExpressionParser.BuildLinqExpression), new Type[] { modelType }, new Type[] { typeof(NameValueCollection) });
            return methodInfo.Invoke(null, new object[] { httpQueryParameters }) as Expression;
        }

        /// <summary>
        /// Build expression for specified type
        /// </summary>
        public static LambdaExpression BuildLinqExpression(Type modelType, NameValueCollection httpQueryParameters, string parameterName, Dictionary<String, Func<object>> variables = null, bool safeNullable = true, bool forceLoad = false, bool lazyExpandVariables = true)
        {
            var parameterExpression = Expression.Parameter(modelType, parameterName);
            Expression retVal = null;
            List<KeyValuePair<String, String[]>> workingValues = new List<KeyValuePair<string, string[]>>();
            // Iterate 
            foreach (var nvc in httpQueryParameters.Where(p => !p.Key.StartsWith("_")).Distinct())
                workingValues.Add(new KeyValuePair<string, string[]>(nvc.Key, nvc.Value?.ToArray()));

            // Get the first values
            while (workingValues.Count > 0)
            {
                var currentValue = workingValues.FirstOrDefault();
                workingValues.Remove(currentValue);

                if (currentValue.Value?.Count(o => !String.IsNullOrEmpty(o)) == 0)
                    continue;

                // Create accessor expression
                Expression keyExpression = null;
                Expression accessExpression = parameterExpression;
                String[] memberPath = currentValue.Key.Split('.');
                String path = "";

                for (int i = 0; i < memberPath.Length; i++)
                {
                    var rawMember = memberPath[i];

                    // No access path - so they are just referencing the core parameter
                    if (String.IsNullOrEmpty(rawMember))
                        continue;

                    var pMember = rawMember;
                    String guard = String.Empty,
                        cast = String.Empty;

                    // Guard token incomplete?
                    if (pMember.Contains("[") && !pMember.Contains("]"))
                    {
                        while (!pMember.Contains("]") && i < memberPath.Length)
                            pMember += "." + memberPath[++i];
                    }

                    // Update path
                    path += pMember + ".";
                    bool coalesce = false;

                    // Guard token?
                    if (pMember.Contains("[") && pMember.EndsWith("]"))
                    {
                        guard = pMember.Substring(pMember.IndexOf("[") + 1, pMember.Length - pMember.IndexOf("[") - 2);
                        pMember = pMember.Substring(0, pMember.IndexOf("["));
                    }
                    if (pMember.EndsWith("?"))
                    {
                        coalesce = true;
                        pMember = pMember.Substring(0, pMember.Length - 1);
                    }
                    if (pMember.Contains("@"))
                    {
                        cast = pMember.Substring(pMember.IndexOf("@") + 1);
                        pMember = pMember.Substring(0, pMember.IndexOf("@"));
                    }

                    // Get member cache for data
                    Dictionary<String, PropertyInfo> memberCache = null;
                    if (!m_memberCache.TryGetValue(accessExpression.Type, out memberCache))
                    {
                        memberCache = new Dictionary<string, PropertyInfo>();
                        lock (m_memberCache)
                            if (!m_memberCache.ContainsKey(accessExpression.Type))
                                m_memberCache.Add(accessExpression.Type, memberCache);
                    }

                    // Add member info
                    PropertyInfo memberInfo = null;
                    if (!memberCache.TryGetValue(pMember, out memberInfo))
                    {
                        memberInfo = accessExpression.Type.GetRuntimeProperties().FirstOrDefault(p => p.GetQueryName() == pMember);
                        if (memberInfo == null)
                            throw new ArgumentOutOfRangeException(currentValue.Key);

                        // Member cache
                        lock (memberCache)
                            if (!memberCache.ContainsKey(pMember))
                                memberCache.Add(pMember, memberInfo);
                    }

                    // Handle XML props
                    if (memberInfo.Name.EndsWith("Xml"))
                        memberInfo = accessExpression.Type.GetRuntimeProperty(memberInfo.Name.Replace("Xml", ""));
                    else if (i != memberPath.Length - 1)
                    {
                        PropertyInfo backingFor = null;

                        // Look in member cache
                        if (!m_redirectCache.TryGetValue(accessExpression.Type, out memberCache))
                        {
                            memberCache = new Dictionary<string, PropertyInfo>();
                            lock (m_redirectCache)
                                if (!m_redirectCache.ContainsKey(accessExpression.Type))
                                    m_redirectCache.Add(accessExpression.Type, memberCache);
                        }

                        // Now find backing
                        if (!memberCache.TryGetValue(pMember, out backingFor))
                        {
                            backingFor = accessExpression.Type.GetRuntimeProperties().FirstOrDefault(p => p.GetCustomAttribute<SerializationReferenceAttribute>()?.RedirectProperty == memberInfo.Name);
                            // Member cache
                            lock (memberCache)
                                if (!memberCache.ContainsKey(pMember))
                                    memberCache.Add(pMember, backingFor);
                        }

                        if (backingFor != null)
                            memberInfo = backingFor;
                    }


                    // Force loading of properties
                    if (forceLoad)
                    {
                        if (typeof(IList).IsAssignableFrom(memberInfo.PropertyType))
                        {
                            var loadMethod = (MethodInfo)typeof(ExtensionMethods).GetGenericMethod(nameof(ExtensionMethods.LoadCollection), new Type[] { memberInfo.PropertyType.GetGenericArguments()[0] }, new Type[] { typeof(IdentifiedData), typeof(String), typeof(bool) });
                            accessExpression = Expression.Call(loadMethod, accessExpression, Expression.Constant(memberInfo.Name), Expression.Constant(false));
                        }
                        else if (typeof(IIdentifiedEntity).IsAssignableFrom(memberInfo.PropertyType))
                        {
                            var loadMethod = (MethodInfo)typeof(ExtensionMethods).GetGenericMethod(nameof(ExtensionMethods.LoadProperty), new Type[] { memberInfo.PropertyType }, new Type[] { typeof(IdentifiedData), typeof(String), typeof(bool) });
                            accessExpression = Expression.Call(loadMethod, accessExpression, Expression.Constant(memberInfo.Name), Expression.Constant(false));
                        }
                        else
                            accessExpression = Expression.MakeMemberAccess(accessExpression, memberInfo);
                    }
                    else
                        accessExpression = Expression.MakeMemberAccess(accessExpression, memberInfo);

                    if (!String.IsNullOrEmpty(cast))
                    {

                        Type castType = null;
                        if (!m_castCache.TryGetValue(cast, out castType))
                        {
                            castType = typeof(QueryExpressionParser).Assembly.ExportedTypes.FirstOrDefault(o => o.GetCustomAttribute<XmlTypeAttribute>()?.TypeName == cast);
                            if (castType == null)
                                throw new ArgumentOutOfRangeException(nameof(castType), cast);

                            lock (m_castCache)
                                if (!m_castCache.ContainsKey(cast))
                                    m_castCache.Add(cast, castType);
                        }
                        accessExpression = Expression.TypeAs(accessExpression, castType);
                    }
                    if (coalesce && accessExpression.Type.GetConstructor(Type.EmptyTypes) != null)
                    {
                        accessExpression = Expression.Coalesce(accessExpression, Expression.New(accessExpression.Type));
                    }

                    // Guard on classifier?
                    if (!String.IsNullOrEmpty(guard))
                    {
                        Type itemType = accessExpression.Type.GenericTypeArguments[0];
                        Type predicateType = typeof(Func<,>).MakeGenericType(itemType, typeof(bool));
                        ParameterExpression guardParameter = Expression.Parameter(itemType, "guard");
                        // Cascade the Classifiers to get the access
                        ClassifierAttribute classAttr = itemType.GetCustomAttribute<ClassifierAttribute>();
                        if (classAttr == null)
                            throw new InvalidOperationException("No classifier found for guard expression");
                        PropertyInfo classifierProperty = itemType.GetRuntimeProperty(classAttr.ClassifierProperty);
                        Expression guardExpression = null;


                        if (guard.Split('|').All(o => Guid.TryParse(o, out Guid _))) // TODO: Refactor this (and the entire class)
                        {
                            if (!String.IsNullOrEmpty(classAttr.ClassifierKeyProperty)) // attempt to get via classifier key
                                classifierProperty = itemType.GetRuntimeProperty(classAttr.ClassifierKeyProperty);
                            else
                                classifierProperty = classifierProperty.GetSerializationRedirectProperty();

                            if (classifierProperty == null)
                                throw new ArgumentOutOfRangeException("Cannot use UUID filter for Guard on this context");

                            foreach (var g in guard.Split('|'))
                            {
                                var expr = Expression.MakeBinary(ExpressionType.Equal, Expression.MakeMemberAccess(guardParameter, classifierProperty), Expression.Convert(Expression.Constant(Guid.Parse(g)), classifierProperty.PropertyType));
                                if (guardExpression == null)
                                    guardExpression = expr;
                                else
                                    guardExpression = Expression.MakeBinary(ExpressionType.OrElse, guardExpression, expr);
                            }


                        }
                        else
                        {
                            if (guard == "null")
                                guard = null;

                            // Handle XML props
                            if (classifierProperty.Name.EndsWith("Xml"))
                            {
                                classifierProperty = itemType.GetRuntimeProperty(classifierProperty.Name.Replace("Xml", ""));
                            }

                            Expression guardAccessor = guardParameter;
                            while (classifierProperty != null && classAttr != null)
                            {
                                if (typeof(IdentifiedData).IsAssignableFrom(classifierProperty.PropertyType) && guard != null)
                                {
                                    if (forceLoad)
                                    {
                                        var loadMethod = (MethodInfo)typeof(ExtensionMethods).GetGenericMethod(nameof(ExtensionMethods.LoadProperty), new Type[] { classifierProperty.PropertyType }, new Type[] { typeof(IdentifiedData), typeof(String), typeof(bool) });
                                        var loadExpression = Expression.Call(loadMethod, guardAccessor, Expression.Constant(classifierProperty.Name), Expression.Constant(false));
                                        guardAccessor = Expression.Coalesce(loadExpression, Expression.New(classifierProperty.PropertyType));
                                    }
                                    else
                                        guardAccessor = Expression.Coalesce(Expression.MakeMemberAccess(guardAccessor, classifierProperty), Expression.New(classifierProperty.PropertyType));

                                }
                                else
                                    guardAccessor = Expression.MakeMemberAccess(guardAccessor, classifierProperty);


                                classAttr = classifierProperty.PropertyType.GetCustomAttribute<ClassifierAttribute>();
                                if (classAttr != null && guard != null)
                                    classifierProperty = classifierProperty.PropertyType.GetRuntimeProperty(classAttr.ClassifierProperty);
                                else if (guard == null)
                                    break;
                            }

                            // Now make expression
                            if (guard == null)
                                guardExpression = Expression.MakeBinary(ExpressionType.Equal, guardAccessor, Expression.Constant(null));
                            else
                                foreach (var g in guard.Split('|'))
                                {
                                    // HACK: Some types use enums as their classifier 
                                    object value = g;
                                    if (guardAccessor.Type.IsEnum)
                                        value = Enum.Parse(guardAccessor.Type, g);

                                    var expr = Expression.MakeBinary(ExpressionType.Equal, guardAccessor, Expression.Constant(value));
                                    if (guardExpression == null)
                                        guardExpression = expr;
                                    else
                                        guardExpression = Expression.MakeBinary(ExpressionType.Or, guardExpression, expr);
                                }

                        }

                        MethodInfo whereMethod = typeof(Enumerable).GetGenericMethod("Where",
                                new Type[] { itemType },
                                new Type[] { accessExpression.Type, predicateType }) as MethodInfo;
                        var guardLambda = Expression.Lambda(guardExpression, guardParameter);
                        accessExpression = Expression.Call(whereMethod, accessExpression, guardLambda);

                        if (currentValue.Value?.Length == 1 && currentValue.Value[0].EndsWith("null") && i == memberPath.Length - 1)
                        {
                            var anyMethod = typeof(Enumerable).GetGenericMethod("Any",
                                new Type[] { itemType },
                                new Type[] { accessExpression.Type }) as MethodInfo;
                            accessExpression = Expression.Call(anyMethod, accessExpression);
                            currentValue.Value[0] = currentValue.Value[0].Replace("null", "false");
                        }

                    }
                    // List expression, we want the Any() operator
                    if (accessExpression.Type.IsEnumerable() &&
                        accessExpression.Type.IsGenericType)
                    {


                        // First or default
                        if (currentValue.Value == null)
                        {
                            Type itemType = accessExpression.Type.GenericTypeArguments[0];
                            Type predicateType = typeof(Func<,>).MakeGenericType(itemType, typeof(bool));
                            var firstMethod = typeof(Enumerable).GetGenericMethod("FirstOrDefault", new Type[] { itemType }, new Type[] { accessExpression.Type }) as MethodInfo;
                            accessExpression = Expression.Call(firstMethod, accessExpression);
                        }
                        else // We're filtering so guard
                        {
                            Type itemType = accessExpression.Type.GenericTypeArguments[0];
                            Type predicateType = typeof(Func<,>).MakeGenericType(itemType, typeof(bool));

                            var anyMethod = typeof(Enumerable).GetGenericMethod("Any",
                                new Type[] { itemType },
                                new Type[] { accessExpression.Type, predicateType }) as MethodInfo;

                            // Add sub-filter
                            NameValueCollection subFilter = new NameValueCollection();

                            // Default to id
                            if (currentValue.Key.Length + 1 > path.Length)
                                subFilter.Add(currentValue.Key.Substring(path.Length), new List<String>(currentValue.Value));
                            else if (currentValue.Value.All(o => Guid.TryParse(o, out Guid _)))
                                subFilter.Add("id", new List<String>(currentValue.Value));
                            else if (currentValue.Key.Length == path.Length - 1) // just the same property so is simple
                                subFilter.Add("", new List<String>(currentValue.Value));

                            // Add collect other parameters
                            foreach (var wv in workingValues.Where(o => o.Key.StartsWith(path)).ToList())
                            {
                                subFilter.Add(wv.Key.Substring(path.Length), new List<String>(wv.Value));
                                workingValues.Remove(wv);
                            }

                            var builderMethod = typeof(QueryExpressionParser).GetGenericMethod(nameof(BuildLinqExpression), new Type[] { itemType }, new Type[] { typeof(NameValueCollection), typeof(String), typeof(Dictionary<String, Func<Object>>), typeof(bool), typeof(bool), typeof(bool) });

                            Expression predicate = BuildLinqExpression(itemType, subFilter, pMember, variables, safeNullable, forceLoad, lazyExpandVariables);
                            if (predicate == null) // No predicate so just ANY()
                                continue;

                            keyExpression = Expression.Call(anyMethod, accessExpression, predicate);
                            currentValue = new KeyValuePair<string, string[]>("", new string[0]);
                            break;  // skip
                        }
                    }

                    // Is this an access expression?
                    if (currentValue.Value == null && typeof(IdentifiedData).IsAssignableFrom(accessExpression.Type))
                        accessExpression = Expression.Coalesce(accessExpression, Expression.New(accessExpression.Type));
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
                            value = "~" + value;

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

                        if (String.IsNullOrEmpty(value)) continue;
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
                                    throw new MissingMemberException(fnName);
                                value = String.IsNullOrEmpty(operand) ? "true" : operand;
                                operandType = extendedFilter.ExtensionMethod.ReturnType;
                            }
                        }

                        String pValue = value;

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
                                    throw new InvalidOperationException("^ can only be applied to string properties");

                                break;
                            case '~':
                                et = ExpressionType.Equal;
                                if (thisAccessExpression.Type == typeof(String))
                                {
                                    pValue = pValue.Substring(1);
                                    if (pValue.StartsWith("$"))
                                        thisAccessExpression = Expression.Call(thisAccessExpression, typeof(String).GetRuntimeMethod("Contains", new Type[] { typeof(String) }), GetVariableExpression(pValue.Substring(1), thisAccessExpression.Type, variables, parameterExpression, lazyExpandVariables) ?? Expression.Constant(pValue));
                                    else
                                        thisAccessExpression = Expression.Call(thisAccessExpression, typeof(String).GetRuntimeMethod("Contains", new Type[] { typeof(String) }), Expression.Constant(pValue));
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
                                        dateHigh = new DateTime(dateLow.Year, 12, 31, 23, 59, 59);
                                    else if (pValue.Length == 7)
                                        dateHigh = new DateTime(dateLow.Year, dateLow.Month, DateTime.DaysInMonth(dateLow.Year, dateLow.Month), 23, 59, 59);
                                    else if (pValue.Length == 10)
                                        dateHigh = new DateTime(dateLow.Year, dateLow.Month, dateLow.Day, 23, 59, 59);
                                    
                                    if (thisAccessExpression.Type == typeof(DateTime?) || thisAccessExpression.Type == typeof(DateTimeOffset?))
                                        thisAccessExpression = Expression.MakeMemberAccess(thisAccessExpression, thisAccessExpression.Type.GetRuntimeProperty("Value"));

                                    // Correct for DTO if originally is
                                    if(thisAccessExpression.Type == typeof(DateTimeOffset) || thisAccessExpression.Type == typeof(DateTimeOffset?))
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
                                    throw new InvalidOperationException($"~ can only be applied to string and date properties not {thisAccessExpression.Type}");

                                break;
                            case '!':
                                et = ExpressionType.NotEqual;
                                pValue = value.Substring(1);
                                break;
                        }

                        // The expression
                        Expression valueExpr = null;
                        if (pValue == "null")
                            valueExpr = Expression.Constant(null);
                        else if (pValue.StartsWith("$"))
                            valueExpr = GetVariableExpression(pValue.Substring(1), thisAccessExpression.Type, variables, parameterExpression, lazyExpandVariables) ?? Expression.Constant(pValue);
                        else if (operandType == typeof(String))
                            valueExpr = Expression.Constant(pValue);
                        else if (operandType == typeof(DateTime) || operandType == typeof(DateTime?))
                            valueExpr = Expression.Constant(DateTime.Parse(pValue));
                        else if (operandType == typeof(DateTimeOffset) || operandType == typeof(DateTimeOffset?))
                            valueExpr = Expression.Constant(DateTimeOffset.Parse(pValue));
                        else if (operandType == typeof(Guid))
                            valueExpr = Expression.Constant(Guid.Parse(pValue));
                        else if (operandType == typeof(Guid?))
                            valueExpr = Expression.Convert(Expression.Constant(Guid.Parse(pValue)), typeof(Guid?));
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
                                valueExpr = Expression.Constant(Enum.ToObject(operandType, Int32.Parse(pValue)));
                            else
                                valueExpr = Expression.Constant(Enum.Parse(operandType, pValue));

                        }
                        else
                        {
                            try
                            {
                                Object converted = null;
                                if (MapUtil.TryConvert(pValue, operandType, out converted))
                                {
                                    valueExpr = Expression.Constant(converted);
                                }
                                else if (typeof(IdentifiedData).IsAssignableFrom(operandType) && Guid.TryParse(pValue, out Guid uuid)) // Assign to key
                                {
                                    valueExpr = Expression.Constant(uuid);
                                    thisAccessExpression = accessExpression = Expression.MakeMemberAccess(accessExpression, operandType.GetRuntimeProperty(nameof(IdentifiedData.Key)));
                                }
                                else
                                    valueExpr = Expression.Constant(Convert.ChangeType(pValue, operandType));
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
                                        return GetVariableExpression(p.Substring(1), thisAccessExpression.Type, variables, parameterExpression, lazyExpandVariables) ?? Expression.Constant(p);
                                    else if (parmType.ParameterType != typeof(String) && MapUtil.TryConvert(p, parmType.ParameterType, out object res)) // convert parameter type
                                        return Expression.Constant(res);
                                    else if (p.StartsWith("\"") && p.EndsWith("\""))
                                        return Expression.Constant(p.Substring(1, p.Length - 2).Replace("\\\"", "\""));
                                    else
                                        return Expression.Constant(p);
                                }
                                return Expression.Constant(p);
                            }).ToArray();

                            singleExpression = extendedFilter.Compose(thisAccessExpression, et, valueExpr, parms);
                            
                        }
                        else
                        {
                            if (valueExpr.Type != thisAccessExpression.Type)
                                valueExpr = Expression.Convert(valueExpr, thisAccessExpression.Type);
                            singleExpression = Expression.MakeBinary(et, thisAccessExpression, valueExpr);
                        }

                        if (nullCheckExpr != null)
                            singleExpression = Expression.MakeBinary(ExpressionType.AndAlso, nullCheckExpr, singleExpression);

                        if (keyExpression == null)
                            keyExpression = singleExpression;
                        else if (et == ExpressionType.Equal)
                            keyExpression = Expression.MakeBinary(ExpressionType.OrElse, keyExpression, singleExpression);
                        else
                            keyExpression = Expression.MakeBinary(ExpressionType.AndAlso, keyExpression, singleExpression);
                    }
                }
                else
                    keyExpression = accessExpression;

                if (retVal == null)
                    retVal = keyExpression;
                else
                    retVal = Expression.MakeBinary(ExpressionType.AndAlso, retVal, keyExpression);

            }
            //Debug.WriteLine(String.Format("Converted {0} to {1}", httpQueryParameters, retVal));

            if (retVal == null)
                return null;
            return Expression.Lambda(retVal, parameterExpression);

        }


        /// <summary>
        /// Buidl linq expression
        /// </summary>
        /// <typeparam name="TModelType"></typeparam>
        /// <param name="httpQueryParameters"></param>
        /// <returns></returns>
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters)
        {
            return BuildLinqExpression<TModelType>(httpQueryParameters, null);
        }

        /// <summary>
        /// Build linq expression from string
        /// </summary>
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(String filter)
        {
            return BuildLinqExpression<TModelType>(NameValueCollection.ParseQueryString(filter), null);
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
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, Dictionary<String, Func<object>> variables, bool safeNullable, bool lazyExpandVariables = true)
        {

            var expression = BuildLinqExpression<TModelType>(httpQueryParameters, "o", variables, safeNullable: safeNullable, lazyExpandVariables: lazyExpandVariables);

            if (expression == null) // No query!
                return (TModelType o) => true;
            else
                return Expression.Lambda<Func<TModelType, bool>>(expression.Body, expression.Parameters);
        }

        /// <summary>
        /// Build LINQ expression
        /// </summary>
        /// <param name="forceLoad">When true, will assume the object is working on memory objects and will call LoadProperty on guards</param>
        /// <param name="httpQueryParameters">The query parameters formatter as HTTP query</param>
        /// <param name="lazyExpandVariables">When true, variables should be expanded in the LINQ expression otherwise they are realized when conversion is done</param>
        /// <param name="parameterName">The name of the parameter on the resulting Lambda</param>
        /// <param name="safeNullable">When true, coalesce operations will be injected into the LINQ to ensure object in-memory collections don't throw NRE</param>
        /// <param name="variables">A list of variables which are accessed in the LambdaExpression via $variable</param>
        public static LambdaExpression BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, string parameterName, Dictionary<String, Func<object>> variables = null, bool safeNullable = true, bool forceLoad = false, bool lazyExpandVariables = true)
        {
            return BuildLinqExpression(typeof(TModelType), httpQueryParameters, parameterName, variables, safeNullable, forceLoad, lazyExpandVariables);
        }


        /// <summary>
        /// Get variable expression
        /// </summary>
        private static Expression GetVariableExpression(string variablePath, Type expectedReturn, Dictionary<string, Func<object>> variables, ParameterExpression parameterExpression, bool lazyExpandVariables)
        {
            Func<object> val = null;
            String varName = variablePath.Contains(".") ? variablePath.Substring(0, variablePath.IndexOf(".")) : variablePath,
                varPath = variablePath.Substring(varName.Length);

            Expression scope = null;
            if (varName == "_")
                scope = parameterExpression;
            else if (variables?.TryGetValue(varName, out val) == true || m_builtInVars.TryGetValue(varName, out val))
            {
                if (val.GetMethodInfo().GetParameters().Length > 0)
                    scope = Expression.Invoke(Expression.Constant(val));
                else if (!lazyExpandVariables)
                {
                    // Expand value
                    var value = val();
                    var propertySelector = BuildPropertySelector(value.GetType(), varPath.Substring(1), true);
                    scope = Expression.Constant(propertySelector.Compile().DynamicInvoke(value));
                    varPath = String.Empty;
                }
                else
                {
                    var value = val();
                    scope = Expression.Convert(Expression.Call(val.Target == null ? null : Expression.Constant(val.Target), val.GetMethodInfo()), value?.GetType() ?? expectedReturn);

                }
            }
            else
                return null;

            Expression retVal = scope;


            if (String.IsNullOrEmpty(varPath))
                return Expression.Convert(retVal, expectedReturn);
            else
            {
                retVal = Expression.Invoke(
                    BuildPropertySelector(scope.Type, varPath.Substring(1), true)
                    , retVal);
                if (retVal.Type.IsConstructedGenericType &&
                    retVal.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    retVal = Expression.Coalesce(retVal, Expression.Default(retVal.Type.GenericTypeArguments[0]));

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
        public static LambdaExpression BuildPropertySelector(Type type, String propertyName, bool forceLoad = false)
        {
            var nvc = new NameValueCollection();
            nvc.Add(propertyName, "null");
            nvc[propertyName] = null;
            return BuildLinqExpression(type, nvc, "__xinstance", null, false, forceLoad,false);
        }
    }
}
