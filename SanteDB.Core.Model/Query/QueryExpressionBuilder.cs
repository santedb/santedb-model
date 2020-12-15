/*
 * Copyright (C) 2019 - 2020, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2020-5-1
 */
using SanteDB.Core.Model.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Query
{
    /// <summary>
    /// Expression visitor which turns a LINQ expression against a query type to an HTTP header
    /// </summary>
    public class QueryExpressionBuilder
    {
        /// <summary>
        /// Http query expression visitor.
        /// </summary>
        private class HttpQueryExpressionVisitor : ExpressionVisitor
        {

            // Readonly names
            private static readonly String[] s_reservedNames =
            {
                "Any", "Where", "Contains", "StartsWith", "EndsWith", "ToLower", "ToUpper"
            };

            // The dictionary
            private List<KeyValuePair<String, Object>> m_query;

            // Interface hints
            private Dictionary<Type, Type> m_interfaceHints = new Dictionary<Type, Type>();

            /// <summary>
            /// Initializes a new instance of the <see cref="HttpQueryExpressionVisitor"/> class.
            /// </summary>
            /// <param name="workingDictionary">The working dictionary.</param>
            public HttpQueryExpressionVisitor(List<KeyValuePair<String, Object>> workingDictionary, Type modelType)
            {
                this.m_query = workingDictionary;

                foreach (var itm in modelType.GetTypeInfo().ImplementedInterfaces)
                    this.m_interfaceHints.Add(itm, modelType);
            }

            /// <summary>
            /// Add a condition if not already present
            /// </summary>
            private void AddCondition(String key, Object value)
            {
                var cvalue = this.m_query.FirstOrDefault(o => o.Key == key);
                if (cvalue.Value == null)
                    this.m_query.Add(new KeyValuePair<string, object>(key, value));
                else if (cvalue.Value is IList)
                {
                    if(!(cvalue.Value as IList).Contains(value))
                        (cvalue.Value as IList).Add(value);
                }
                else
                {
                    this.m_query.Remove(cvalue);
                    this.m_query.Add(new KeyValuePair<String, Object>(key, new List<Object>() { cvalue.Value, value }));
                }
            }

            /// <summary>
            /// Visit a query expression
            /// </summary>
            /// <returns>The modified expression list, if any one of the elements were modified; otherwise, returns the original
            /// expression list.</returns>
            /// <param name="node">Node.</param>
            public override Expression Visit(Expression node)
            {
                if (node == null)
                    return node;

                // Convert node type
                switch (node.NodeType)
                {
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        return this.VisitBinary((BinaryExpression)node);
                    case ExpressionType.MemberAccess:
                        return this.VisitMemberAccess((MemberExpression)node);
                    case ExpressionType.Parameter:
                        return this.VisitParameter((ParameterExpression)node);
                    case ExpressionType.Call:
                        return this.VisitMethodCall((MethodCallExpression)node);
                    case ExpressionType.Lambda:
                        return this.VisitLambdaGeneric((LambdaExpression)node);
                    case ExpressionType.Invoke:
                        return this.VisitInvocation((InvocationExpression)node);
                    case ExpressionType.Constant:
                    case ExpressionType.Convert:
                    case ExpressionType.TypeAs:
                        return node;
                    case ExpressionType.Not:
                        return this.VisitUnary((UnaryExpression)node);
                    case ExpressionType.Coalesce:
                        // We coalesce for nulls so we want the first
                        return this.Visit(((BinaryExpression)node).Left);
                    default:
                        throw new InvalidOperationException($"Don't know how to conver type of {node.Type}");
                }
            }

            /// <summary>
            /// Strips a Convert() to get the internal
            /// </summary>
            private Expression StripConvert(Expression node)
            {
                while (node.NodeType == ExpressionType.Convert ||
                    node.NodeType == ExpressionType.ConvertChecked)
                    node = (node as UnaryExpression).Operand;
                return node;
            }

            /// <summary>
            /// Visit the invocation expression (expand this and compile the expression if it is a lambda and invoke / dynamic invoke , capturing the results)
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                if(node.Expression is LambdaExpression)
                {
                    var callee = (node.Expression as LambdaExpression).Compile();
                    var args = node.Arguments.Select(o =>
                    {
                        o = this.StripConvert(o);
                        switch (o.NodeType)
                        {
                            case ExpressionType.Constant:
                                return (o as ConstantExpression).Value;
                            case ExpressionType.Call:
                                {
                                    var ie = o as MethodCallExpression;
                                    var obj = (ie.Object as ConstantExpression)?.Value;
                                    return ie.Method.Invoke(obj, new object[0]);
                                }
                            default:
                                throw new InvalidOperationException($"Cannot expand parameter {o} ({o.NodeType})");
                        }
                    });
                    return Expression.Constant(callee.DynamicInvoke(args.ToArray()));
                }
                else
                {
                    throw new InvalidOperationException("Cannot invoke a non-Lambda expression");
                }
            }

            /// <summary>
            /// Visit unary expression
            /// </summary>
            protected override Expression VisitUnary(UnaryExpression node)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Not:
                        var parmName = this.ExtractPath(node.Operand, true);
                        if (!parmName.EndsWith("]"))
                            throw new InvalidOperationException("Only guard conditions can (i.e. Any statements) support unary NOT");

                        this.AddCondition(parmName, "null");
                        return null;
                    default:
                        throw new NotSupportedException("Unary expressions cannot be represented as key/value pair filters");
                }
            }

            /// <summary>
            /// Visit method call
            /// </summary>
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                switch (node.Method.Name)
                {
                    case "Contains":
                        {
                            if (node.Object == null && node.Method.DeclaringType == typeof(Enumerable))
                            {
                                // Array contains 
                                // value=X&value=Y&value=Z
                                Expression array = node.Arguments[0],
                                    bindParameter = node.Arguments[1];
                                var parmName = this.ExtractPath(bindParameter, false);
                                var valueList = this.ExtractValue(array);
                                if (valueList is IEnumerable enumerable)
                                {
                                    foreach (var i in enumerable)
                                        this.AddCondition(parmName, i);
                                    return null;
                                }
                                else
                                    throw new InvalidOperationException("Cannot understand this enumerable object");
                            }
                            else
                            {
                                var parmName = this.ExtractPath(node.Object, false);
                                object parmValue = this.ExtractValue(node.Arguments[0]);
                                this.AddCondition(parmName, "~" + parmValue.ToString());
                                return null;
                            }
                        }
                    case "StartsWith":
                        {
                            var parmName = this.ExtractPath(node.Object, false);
                            object parmValue = this.ExtractValue(node.Arguments[0]);
                            this.AddCondition(parmName, "^" + parmValue.ToString());
                            return null;
                        }
                    case "Any":
                        {
                            var parmName = this.ExtractPath(node.Arguments[0], false);
                            // Process lambda
                            var result = new List<KeyValuePair<string, object>>();
                            var subQueryExpressionVisitor = new HttpQueryExpressionVisitor(result, node.Arguments[0].Type);
                            if (node.Arguments.Count == 2)
                            {
                                subQueryExpressionVisitor.Visit(node.Arguments[1]);

                                // Result
                                foreach (var itm in result)
                                    this.AddCondition(String.Format("{0}.{1}", parmName, itm.Key), itm.Value);
                                return null;
                            }
                            else
                                return null;

                        }
                    default: // extended fn?
                        {
                            var methodCall = node as MethodCallExpression;
                            var extendedFn = QueryFilterExtensions.GetExtendedFilterByMethod(methodCall.Method);
                            if (extendedFn == null)
                                throw new MissingMemberException($"Cannot find extension method {methodCall.Method}");
                        }
                        return null;

                }

            }

            /// <summary>
            /// Visits the member access.
            /// </summary>
            /// <returns>The member access.</returns>
            /// <param name="node">The node being visited</param>
            protected virtual Expression VisitMemberAccess(MemberExpression node)
            {
                this.Visit(node.Expression);
                return node;
            }

            /// <summary>
            /// Visits the lambda generic.
            /// </summary>
            /// <returns>The lambda generic.</returns>
            /// <param name="node">Node.</param>
            protected virtual Expression VisitLambdaGeneric(LambdaExpression node)
            {
                this.Visit(node.Body);
                return node;
            }

            /// <summary>
            /// Visit a binary expression which is in the form of A(operator)B
            /// </summary>
            /// <returns>The binary.</returns>
            /// <param name="node">Node.</param>
            protected override Expression VisitBinary(BinaryExpression node)
            {
                this.Visit(node.Left);
                this.Visit(node.Right);

                String parmName = this.ExtractPath(node.Left, false);
                Object parmValue = this.ExtractValue(node.Right);

                // Not able to map
                if ((node.Left as MethodCallExpression)?.Method.Name == "Any" &&
                    (node.Left as MethodCallExpression)?.Arguments.Count == 1)
                {
                    // Special exists method call - i.e. HAS X
                    var mci = (node.Left as MethodCallExpression);
                    parmName = this.ExtractPath(mci.Arguments[0], false);
                    if (node.Right is ConstantExpression cci)
                    {
                        if (node.NodeType == ExpressionType.NotEqual)
                            this.AddCondition(parmName, true.Equals(cci.Value) ? "null" : "!null");
                        else if (node.NodeType == ExpressionType.Equal)
                            this.AddCondition(parmName, true.Equals(cci.Value) ? "!null" : "null");
                        else
                            throw new InvalidOperationException($"Cannot determine how to convert ANY() function '{node}'");
                    }
                    else
                        this.AddCondition(parmName, "null");
                }
                else if (!String.IsNullOrEmpty(parmName))
                {
                    Object fParmValue = this.PrepareValue(parmValue, false);
                    if (parmValue is DateTime)
                        fParmValue = ((DateTime)parmValue).ToString("o");
                    else if (parmValue is DateTimeOffset)
                        fParmValue = ((DateTimeOffset)parmValue).ToString("o");
                    else if (parmValue == null)
                        fParmValue = "null";

                    // Node type
                    switch (node.NodeType)
                    {
                        case ExpressionType.GreaterThan:
                            fParmValue = ">" + fParmValue;
                            break;
                        case ExpressionType.GreaterThanOrEqual:
                            fParmValue = ">=" + fParmValue;
                            break;
                        case ExpressionType.LessThan:
                            fParmValue = "<" + fParmValue;
                            break;
                        case ExpressionType.LessThanOrEqual:
                            fParmValue = "<=" + fParmValue;
                            break;
                        case ExpressionType.NotEqual:
                            fParmValue = "!" + fParmValue;
                            break;
                    }

                    // Is this an extended method?
                    if (node.Left is MethodCallExpression)
                    {
                        var methodCall = node.Left as MethodCallExpression;
                        var extendedFn = QueryFilterExtensions.GetExtendedFilterByMethod(methodCall.Method);
                        if (extendedFn != null)
                        {
                            var callValue = $":({extendedFn.Name}";
                            if (methodCall.Arguments.Count > 1)
                                callValue += $"|{String.Join(",", methodCall.Arguments.Skip(1).Select(o => this.PrepareValue(this.ExtractValue(o), true)))}";
                            callValue += ")";
                            fParmValue = callValue + fParmValue;
                        }
                    }

                    this.AddCondition(parmName, fParmValue);
                }

                return node;
            }

            /// <summary>
            /// Prepare the specified parameter value
            /// </summary>
            private object PrepareValue(object parmValue, bool quoteStrings)
            {
                Object fParmValue = parmValue;
                if (parmValue is DateTime)
                    fParmValue = ((DateTime)parmValue).ToString("o");
                else if (parmValue is DateTimeOffset)
                    fParmValue = ((DateTimeOffset)parmValue).ToString("o");
                else if (parmValue == null)
                    fParmValue = "null";
                else if (parmValue is String && !"null".Equals(parmValue) && quoteStrings)
                    fParmValue = $"\"{parmValue.ToString().Replace("\"","\\\"")}\"";
                return fParmValue;
            }

            /// <summary>
            /// Extract a value
            /// </summary>
            /// <returns>The value.</returns>
            /// <param name="access">Access.</param>
            protected Object ExtractValue(Expression access)
            {
                if (access == null)
                    return null;

                access = this.StripConvert(access);

                switch(access.NodeType)
                {
                    case ExpressionType.Parameter:
                        return "$_";
                    case ExpressionType.Constant:
                        return ((ConstantExpression)access).Value;
                    case ExpressionType.MemberAccess:
                        MemberExpression expr = access as MemberExpression;
                        var expressionValue = this.ExtractValue(expr.Expression);
                        // Is this a ref to a parameter or variable? If so, we must extract it
                        if (expressionValue?.ToString().StartsWith("$_") == true)
                            return this.ExtractPath(expr, false, true);
                        else if (expr.Member is PropertyInfo)
                        {
                            try
                            {
                                return (expr.Member as PropertyInfo).GetValue(expressionValue);
                            }
                            catch
                            {
                                return null;
                            }
                        }
                        else if (expr.Member is FieldInfo)
                        {
                            return (expr.Member as FieldInfo).GetValue(expressionValue);
                        }
                        break;
                    case ExpressionType.Coalesce:
                        return this.ExtractValue((access as BinaryExpression).Left);
                    case ExpressionType.Invoke:
                        return this.ExtractValue(this.VisitInvocation(access as InvocationExpression));
                }
                return null; 
            }

            /// <summary>
            /// Extract the path
            /// </summary>
            /// <returns>The path.</returns>
            /// <param name="access">Access.</param>
            /// <param name="fromUnary">Extract the path from a unuary or binary expression</param>
            /// <param name="fromOperand">Indicates the extraction should occur from an operand and not the operator</param>
            protected String ExtractPath(Expression access, bool fromUnary, bool fromOperand = false)
            {
                access = this.StripConvert(access);
                if (access.NodeType == ExpressionType.MemberAccess)
                {
                    MemberExpression memberExpr = access as MemberExpression;
                    String path = this.ExtractPath(memberExpr.Expression, fromUnary, fromOperand); // get the chain if required
                    if (memberExpr.Expression.Type.GetTypeInfo().IsGenericType && memberExpr.Expression.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        return path;

                    // XML property?
                    var memberInfo = memberExpr.Expression.Type.GetRuntimeProperty(memberExpr.Member.Name + "Xml") ??
                                     memberExpr.Member;

                    // Member information is declread on interface
                    Type mapType = null;
                    if(memberInfo.DeclaringType.GetTypeInfo().IsInterface && this.m_interfaceHints.TryGetValue(memberInfo.DeclaringType, out mapType))
                        memberInfo = mapType.GetRuntimeProperty(memberInfo.Name) ?? memberInfo;
                    
                    // Is this a delay load?
                    var serializationReferenceAttribute = memberExpr.Member.GetCustomAttribute<SerializationReferenceAttribute>();
                    var queryParameterAttribute = memberExpr.Member.GetCustomAttribute<QueryParameterAttribute>();
                    var xmlIgnoreAttribute = memberExpr.Member.GetCustomAttribute<XmlIgnoreAttribute>();
                    if (xmlIgnoreAttribute != null && serializationReferenceAttribute != null && !String.IsNullOrEmpty(serializationReferenceAttribute.RedirectProperty))
                        memberInfo = memberExpr.Expression.Type.GetRuntimeProperty(serializationReferenceAttribute.RedirectProperty);

                    // TODO: Delay and bound properties!!
                    var memberXattribute = memberInfo.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault();
                    if (memberXattribute == null && queryParameterAttribute != null)
                        memberXattribute = new XmlElementAttribute(queryParameterAttribute.ParameterName); // We don't serialize but it does exist
                    else if(memberExpr.Expression is ConstantExpression)
                    {
                        return (memberExpr.Expression as ConstantExpression).Value.ToString();
                    }
                    else if (memberXattribute == null)
                    {
                        if (memberExpr.Expression.Type.StripNullable() == typeof(DateTimeOffset) &&
                            memberExpr.Member.Name == "DateTime")
                            return path;
                        throw new InvalidOperationException($"The path {access} cannot be translated, ensure the property is XML navigable or has a QueryParameter attribute"); // TODO: When this occurs?
                    }

                    // Return path
                    if (String.IsNullOrEmpty(path))
                        return memberXattribute.ElementName;
                    else if (memberXattribute.ElementName == "id") // ID can be ignored
                        return path;
                    else
                        return String.Format("{0}.{1}", path, memberXattribute.ElementName);
                }
                else if (access.NodeType == ExpressionType.Call)
                {
                    //CallExpression callExpr = access as MemberExpression;
                    MethodCallExpression callExpr = access as MethodCallExpression;

                    if (callExpr.Method.Name == "Where" ||
                        fromUnary && callExpr.Method.Name == "Any")
                    {
                        String path = this.ExtractPath(callExpr.Arguments[0], false, fromOperand); // get the chain if required
                        var guardExpression = callExpr.Arguments[1] as LambdaExpression;
                        // Where should be a guard so we just grab the unary equals only!
                        var binaryExpression = guardExpression.Body as BinaryExpression;
                        if (binaryExpression == null)
                            throw new InvalidOperationException("Cannot translate non-binary expression guards");

                        // Is the expression the guard?
                        String guardString = this.BuildGuardExpression(binaryExpression);
                        return String.Format("{0}[{1}]", path, guardString);

                    }
                    else
                    {
                        var extendedFilter = QueryFilterExtensions.GetExtendedFilterByMethod(callExpr.Method);
                        if (extendedFilter != null)
                            return this.ExtractPath(callExpr.Arguments[0], false, fromOperand); // get the chain if required
                        else if (!s_reservedNames.Contains(callExpr.Method.Name))
                            throw new InvalidOperationException($"Can't find extended method handler for {callExpr.Method.Name}");
                    }

                }
                
                else if (access.NodeType == ExpressionType.TypeAs)
                {
                    UnaryExpression ua = (UnaryExpression)access;
                    return String.Format("{0}@{1}", this.ExtractPath(ua.Operand, false, fromOperand), ua.Type.GetTypeInfo().GetCustomAttribute<XmlTypeAttribute>().TypeName);
                }
                else if (access.NodeType == ExpressionType.Parameter && fromOperand)
                    return "$_";
                return null;
            }

            /// <summary>
            /// Build a guard expression
            /// </summary>
            private string BuildGuardExpression(BinaryExpression binaryExpression)
            {

                switch (binaryExpression.NodeType)
                {
                    case ExpressionType.Or:
                        return $"{this.BuildGuardExpression(binaryExpression.Left as BinaryExpression)}|{this.BuildGuardExpression(binaryExpression.Right as BinaryExpression)}";
                    case ExpressionType.Equal:
                        var expressionMember = binaryExpression.Left as MemberExpression;
                        var valueExpression = binaryExpression.Right as ConstantExpression;
                        var classifierAttribute = expressionMember.Member.DeclaringType.GetTypeInfo().GetCustomAttribute<ClassifierAttribute>();
                        if (classifierAttribute?.ClassifierProperty != expressionMember.Member.Name)
                            throw new InvalidOperationException($"Classifier for type on {expressionMember.Member.DeclaringType.FullName} is property {classifierAttribute?.ClassifierProperty} however expression uses property {expressionMember.Member.Name}. Only {classifierAttribute?.ClassifierProperty} may be used in guard expression");
                        if (valueExpression == null)
                            throw new InvalidOperationException("Only constant expressions are supported on guards");
                        return valueExpression.Value.ToString();
                    default:
                        throw new InvalidOperationException($"Binary expressions of {binaryExpression.NodeType} are not permitted");

                }

            }
        }

        /// <summary>
        /// Builds the query dictionary .
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="model">Model.</param>
        /// <param name="stripNullChecks">True if null checks should not be included in the output</param>
        /// <typeparam name="TModel">The 1st type parameter.</typeparam>
        public static IEnumerable<KeyValuePair<String, Object>> BuildQuery<TModel>(Expression<Func<TModel, bool>> model, bool stripNullChecks = false)
        {
            List<KeyValuePair<String, Object>> retVal = new List<KeyValuePair<string, Object>>();
            var visitor = new HttpQueryExpressionVisitor(retVal, typeof(TModel));
            visitor.Visit(model);
            if (stripNullChecks)
                retVal.RemoveAll(o => retVal.Any(c => c.Key == o.Key && c.Value != o.Value) && o.Value.Equals("!null"));
            return retVal;
        }


        /// <summary>
        /// Builds an HTTP sorting expression
        /// </summary>
        /// <returns></returns>
        /// TODO: Handle chained sort properties
        public static String BuildSortExpression<TModel>(ModelSort<TModel> sort)
        {
            var memberExpression = (sort.SortProperty as LambdaExpression)?.Body;
            while (!(memberExpression is MemberExpression) && memberExpression != null)
                memberExpression = (memberExpression as UnaryExpression)?.Operand;

            if (memberExpression == null)
                throw new InvalidOperationException("Cannot convert sort expression");
            else
                return $"{((memberExpression as MemberExpression).Member as PropertyInfo).GetSerializationName()}:{(sort.SortOrder == Map.SortOrderType.OrderBy ? "asc" : "desc")}";

        }
    }
}

