/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 * Copyright 2019-2019 SanteSuite Contributors (See NOTICE)
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
 * User: Justin Fyfe
 * Date: 2019-8-8
 */
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        /// Build a LINQ expression
        /// </summary>
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, Dictionary<String, Delegate> variables)
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
        public static Expression<Func<TModelType, bool>> BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, Dictionary<String, Delegate> variables, bool safeNullable)
        {
            var expression = BuildLinqExpression<TModelType>(httpQueryParameters, "o", variables, safeNullable);

            if (expression == null) // No query!
                return (TModelType o) => true;
            else
                return Expression.Lambda<Func<TModelType, bool>>(expression.Body, expression.Parameters);
        }

        /// <summary>
        /// Build LINQ expression
        /// </summary>
        /// <param name="forceLoad">When true, will assume the object is working on memory objects and will call LoadProperty on guards</param>
        public static LambdaExpression BuildLinqExpression<TModelType>(NameValueCollection httpQueryParameters, string parameterName, Dictionary<String, Delegate> variables = null, bool safeNullable = true, bool forceLoad = false)
        {
            var parameterExpression = Expression.Parameter(typeof(TModelType), parameterName);
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
                        memberInfo = accessExpression.Type.GetRuntimeProperties().FirstOrDefault(p => p.GetCustomAttributes<XmlElementAttribute>()?.Any(a => a.ElementName == pMember) == true || p.GetCustomAttribute<QueryParameterAttribute>()?.ParameterName == pMember);
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
                    else if (pMember != memberPath.Last())
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

                    accessExpression = Expression.MakeMemberAccess(accessExpression, memberInfo);


                    if (!String.IsNullOrEmpty(cast))
                    {

                        Type castType = null;
                        if (!m_castCache.TryGetValue(cast, out castType))
                        {
                            castType = typeof(QueryExpressionParser).GetTypeInfo().Assembly.ExportedTypes.FirstOrDefault(o => o.GetTypeInfo().GetCustomAttribute<XmlTypeAttribute>()?.TypeName == cast);
                            if (castType == null)
                                throw new ArgumentOutOfRangeException(nameof(castType), cast);

                            lock (m_castCache)
                                if (!m_castCache.ContainsKey(cast))
                                    m_castCache.Add(cast, castType);
                        }
                        accessExpression = Expression.TypeAs(accessExpression, castType);
                    }
                    if (coalesce)
                    {
                        accessExpression = Expression.Coalesce(accessExpression, Expression.New(accessExpression.Type));
                    }

                    // Guard on classifier?
                    if (!String.IsNullOrEmpty(guard))
                    {
                        Type itemType = accessExpression.Type.GenericTypeArguments[0];
                        Type predicateType = typeof(Func<,>).MakeGenericType(itemType, typeof(bool));
                        ParameterExpression guardParameter = Expression.Parameter(itemType, "guard");
                        if (guard == "null")
                            guard = null;

                        // Cascade the Classifiers to get the access
                        ClassifierAttribute classAttr = itemType.GetTypeInfo().GetCustomAttribute<ClassifierAttribute>();
                        if (classAttr == null)
                            throw new InvalidOperationException("No classifier found for guard expression");
                        PropertyInfo classifierProperty = itemType.GetRuntimeProperty(classAttr.ClassifierProperty);
                        // Handle XML props
                        if (classifierProperty.Name.EndsWith("Xml"))
                            classifierProperty = itemType.GetRuntimeProperty(classifierProperty.Name.Replace("Xml", ""));

                        Expression guardAccessor = guardParameter;
                        while (classifierProperty != null && classAttr != null)
                        {
                            if (typeof(IdentifiedData).GetTypeInfo().IsAssignableFrom(classifierProperty.PropertyType.GetTypeInfo()) && guard != null)
                            {
                                if (forceLoad) {
                                    var loadMethod = (MethodInfo)typeof(ExtensionMethods).GetGenericMethod(nameof(ExtensionMethods.LoadProperty), new Type[] { classifierProperty.PropertyType }, new Type[] { typeof(IdentifiedData), typeof(String) });
                                    var loadExpression = Expression.Call(loadMethod, guardAccessor, Expression.Constant(classifierProperty.Name));
                                    guardAccessor = Expression.Coalesce(loadExpression, Expression.New(classifierProperty.PropertyType));
                                }
                                else
                                    guardAccessor = Expression.Coalesce(Expression.MakeMemberAccess(guardAccessor, classifierProperty), Expression.New(classifierProperty.PropertyType));

                            }
                            else
                                guardAccessor = Expression.MakeMemberAccess(guardAccessor, classifierProperty);


                            classAttr = classifierProperty.PropertyType.GetTypeInfo().GetCustomAttribute<ClassifierAttribute>();
                            if (classAttr != null && guard != null)
                                classifierProperty = classifierProperty.PropertyType.GetRuntimeProperty(classAttr.ClassifierProperty);
                            else if (guard == null)
                                break;
                        }

                        MethodInfo whereMethod = typeof(Enumerable).GetGenericMethod("Where",
                            new Type[] { itemType },
                            new Type[] { accessExpression.Type, predicateType }) as MethodInfo;

                        // Now make expression
                        Expression guardExpression = null;
                        if (guard != null)
                            foreach (var g in guard.Split('|'))
                            {
                                // HACK: Some types use enums as their classifier 
                                object value = g;
                                if (guardAccessor.Type.GetTypeInfo().IsEnum)
                                    value = Enum.Parse(guardAccessor.Type, g);
                                
                                var expr = Expression.MakeBinary(ExpressionType.Equal, guardAccessor, Expression.Constant(value));
                                if (guardExpression == null)
                                    guardExpression = expr;
                                else
                                    guardExpression = Expression.MakeBinary(ExpressionType.Or, guardExpression, expr);
                            }
                        else
                            guardExpression = Expression.MakeBinary(ExpressionType.Equal, guardAccessor, Expression.Constant(null));

                        var guardLambda = Expression.Lambda(guardExpression, guardParameter);
                        accessExpression = Expression.Call(whereMethod, accessExpression, guardLambda);

                        if (currentValue.Value?.Length == 1 && currentValue.Value[0].EndsWith("null") && i == memberPath.Length)
                        {
                            var anyMethod = typeof(Enumerable).GetGenericMethod("Any",
                                new Type[] { itemType },
                                new Type[] { accessExpression.Type }) as MethodInfo;
                            accessExpression = Expression.Call(anyMethod, accessExpression);
                            currentValue.Value[0] = currentValue.Value[0].Replace("null", "false");
                        }

                    }
                    // List expression, we want the Any() operator
                    if (accessExpression.Type.GetTypeInfo().ImplementedInterfaces.Any(o => o == typeof(IEnumerable)) &&
                        accessExpression.Type.GetTypeInfo().IsGenericType)
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
                            if(currentValue.Key.Length + 1 > path.Length)
                                subFilter.Add(currentValue.Key.Substring(path.Length), new List<String>(currentValue.Value));
                            else
                                subFilter.Add("id", new List<String>(currentValue.Value));

                            // Add collect other parameters
                            foreach (var wv in workingValues.Where(o => o.Key.StartsWith(path)).ToList())
                            {
                                subFilter.Add(wv.Key.Substring(path.Length), new List<String>(wv.Value));
                                workingValues.Remove(wv);
                            }

                            var builderMethod = typeof(QueryExpressionParser).GetGenericMethod(nameof(BuildLinqExpression), new Type[] { itemType }, new Type[] { typeof(NameValueCollection), typeof(String), typeof(Dictionary<String, Delegate>), typeof(bool), typeof(bool) });

                            Expression predicate = (builderMethod.Invoke(null, new object[] { subFilter, pMember, variables, safeNullable, forceLoad }) as LambdaExpression);
                            if (predicate == null)
                                continue;
                            keyExpression = Expression.Call(anyMethod, accessExpression, predicate);
                            currentValue = new KeyValuePair<string, string[]>("", new string[0]);
                            break;  // skip
                        }
                    }

                    // Is this an access expression?
                    if (currentValue.Value == null && typeof(IdentifiedData).GetTypeInfo().IsAssignableFrom(accessExpression.Type.GetTypeInfo()))
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
                            !value.Contains("null")
                            )
                            value = "~" + value;

                        Expression nullCheckExpr = null;
                        Type operandType = thisAccessExpression.Type;

                        // Correct for nullable
                        if (value != "null" && thisAccessExpression.Type.GetTypeInfo().IsGenericType && thisAccessExpression.Type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                            safeNullable)
                        {
                            nullCheckExpr = Expression.MakeBinary(ExpressionType.NotEqual, thisAccessExpression, Expression.Constant(null));
                            thisAccessExpression = Expression.MakeMemberAccess(thisAccessExpression, accessExpression.Type.GetRuntimeProperty("Value"));
                        }

                        // Process value
                        ExpressionType et = ExpressionType.Equal;
                        IQueryFilterExtension extendedFilter = null;
                        String[] extendedParms = null;

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
                                extendedParms = parms.Split(',');
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
                                    thisAccessExpression = Expression.Call(thisAccessExpression, typeof(String).GetRuntimeMethod("Contains", new Type[] { typeof(String) }), Expression.Constant(pValue.Substring(1)));
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

                                    Expression lowerBound = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, thisAccessExpression, Expression.Convert(Expression.Constant(dateLow), thisAccessExpression.Type.StripNullable())),
                                        upperBound = Expression.MakeBinary(ExpressionType.LessThanOrEqual, thisAccessExpression, Expression.Convert(Expression.Constant(dateHigh), thisAccessExpression.Type.StripNullable()));
                                    thisAccessExpression = Expression.MakeBinary(ExpressionType.AndAlso, lowerBound, upperBound);
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
                            valueExpr = GetVariableExpression(pValue.Substring(1), thisAccessExpression.Type, variables, parameterExpression) ?? Expression.Constant(pValue);
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
                        else if (operandType.GetTypeInfo().IsEnum)
                        {
                            int tryParse = 0;
                            if (Int32.TryParse(pValue, out tryParse))
                                valueExpr = Expression.Constant(Enum.ToObject(operandType, Int32.Parse(pValue)));
                            else
                                valueExpr = Expression.Constant(Enum.Parse(operandType, pValue));

                        }
                        else
                        {
                            Object converted = null;
                            if (MapUtil.TryConvert(pValue, operandType, out converted))
                            {
                                valueExpr = Expression.Constant(converted);
                            }
                            else if(typeof(IdentifiedData).GetTypeInfo().IsAssignableFrom(operandType.GetTypeInfo()) && Guid.TryParse(pValue, out Guid uuid)) // Assign to key
                            {
                                valueExpr = Expression.Constant(uuid);
                                thisAccessExpression = accessExpression = Expression.MakeMemberAccess(accessExpression, operandType.GetRuntimeProperty(nameof(IdentifiedData.Key)));
                            }
                            else
                                valueExpr = Expression.Constant(Convert.ChangeType(pValue, operandType));
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
                                if (parmList.Length > parmNo) {
                                    var parmType = parmList[parmNo++];
                                    if (p.StartsWith("$")) // variable
                                        return GetVariableExpression(p.Substring(1), thisAccessExpression.Type, variables, parameterExpression) ?? Expression.Constant(p);
                                    else if (parmType.ParameterType != typeof(String) && MapUtil.TryConvert(p, parmType.ParameterType, out object res)) // convert parameter type
                                        return Expression.Constant(res);
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
        /// Get variable expression
        /// </summary>
        private static Expression GetVariableExpression(string variablePath, Type expectedReturn, Dictionary<string, Delegate> variables, ParameterExpression parameterExpression)
        {
            Delegate val = null;
            String varName = variablePath.Contains(".") ? variablePath.Substring(0, variablePath.IndexOf(".")) : variablePath,
                varPath = variablePath.Substring(varName.Length);

            Expression scope = null;
            if (varName == "_")
                scope = parameterExpression;
            else if (variables.TryGetValue(varName, out val))
            {
                if (val.GetMethodInfo().GetParameters().Length > 0)
                    scope = Expression.Invoke(Expression.Constant(val));
                else
                    scope = Expression.Call(val.Target == null ? null : Expression.Constant(val.Target), val.GetMethodInfo());
            }
            else
                return null;

            Expression retVal = scope;

            if (String.IsNullOrEmpty(varPath))
                return Expression.Convert(retVal, expectedReturn);
            else
            {
                var builderMethod = typeof(QueryExpressionParser).GetGenericMethod(nameof(BuildPropertySelector), new Type[] { scope.Type }, new Type[] { typeof(String), typeof(Boolean) });
                retVal = Expression.Invoke(builderMethod.Invoke(null, new object[]
                {
                        varPath.Substring(1),
                        true
                }) as Expression, retVal);
                if (retVal.Type.IsConstructedGenericType &&
                    retVal.Type.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>))
                    retVal = Expression.Coalesce(retVal, Expression.Default(retVal.Type.GetTypeInfo().GenericTypeArguments[0]));
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
        public static LambdaExpression BuildPropertySelector(Type type, String propertyName, bool forceLoad = false) { 
            var builderMethod = typeof(QueryExpressionParser).GetGenericMethod(nameof(BuildLinqExpression), new Type[] { type }, new Type[] { typeof(NameValueCollection), typeof(String), typeof(Dictionary<String, Delegate>), typeof(bool), typeof(bool) });
            var nvc = new NameValueCollection();
            nvc.Add(propertyName, "null");
            nvc[propertyName] = null;
            return builderMethod.Invoke(null, new object[]
            {
                nvc, "__xinstance", null, false, forceLoad
            }) as LambdaExpression;
        }
    }
}
