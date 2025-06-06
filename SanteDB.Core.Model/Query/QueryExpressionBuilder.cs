﻿/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
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

            /// <summary>
            /// When true - strip all !null checks
            /// </summary>
            public bool StripNullChecks { get; set; }

            /// <summary>
            /// When true strips any control
            /// </summary>
            public bool StripControl { get; set; }

            // The dictionary
            private NameValueCollection m_query;

            // Interface hints
            private Dictionary<Type, Type> m_interfaceHints = new Dictionary<Type, Type>();

            /// <summary>
            /// Initializes a new instance of the <see cref="HttpQueryExpressionVisitor"/> class.
            /// </summary>
            /// <param name="workingDictionary">The working dictionary.</param>
            /// <param name="modelType">The type of model this visitor is using</param>
            public HttpQueryExpressionVisitor(NameValueCollection workingDictionary, Type modelType)
            {
                this.m_query = workingDictionary;

                foreach (var itm in modelType.GetInterfaces())
                {
                    this.m_interfaceHints.Add(itm, modelType);
                }
            }

            /// <summary>
            /// Add a condition if not already present
            /// </summary>
            private void AddCondition(String key, Object value)
            {
                if ("!null".Equals(value) && this.StripNullChecks)
                {
                    return;
                }

                if (this.m_query.GetValues(key)?.Contains("value") != true)
                {
                    if (value is IList le)
                    {
                        foreach (var itm in le)
                        {
                            this.m_query.Add(key, itm.ToString());
                        }
                    }
                    else
                    {
                        this.m_query.Add(key, value.ToString());
                    }
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
                {
                    return node;
                }

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
                    case ExpressionType.OrElse:
                        return this.VisitBinary((BinaryExpression)node);
                    case ExpressionType.Or:
                        return this.VisitExplicitOr((BinaryExpression)node);
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
                    case ExpressionType.IsTrue:

                        return this.VisitUnary((UnaryExpression)node);

                    case ExpressionType.Coalesce:
                        // We coalesce for nulls so we want the first
                        return this.Visit(((BinaryExpression)node).Left);

                    default:
                        throw new InvalidOperationException($"Don't know how to convert type of {node.Type}");
                }
            }

            /// <summary>
            /// Visit a binary union instruction for an explcit OR
            /// </summary>
            private Expression VisitExplicitOr(BinaryExpression node)
            {
                var result = new NameValueCollection();
                var subExpressionVisitor = new HttpQueryExpressionVisitor(result, node.Left.Type);
                subExpressionVisitor.Visit(node.Left);
                var leftProperty = result.Keys[0];
                result.Clear();
                subExpressionVisitor.Visit(node.Right);
                var rightProperty = result.Keys[0];

                // Now we want to add to ours
                this.AddCondition($"{leftProperty}||{rightProperty}", result.GetValues(rightProperty));
                return node;
            }

            /// <summary>
            /// Strips a Convert() to get the internal
            /// </summary>
            private Expression StripConvert(Expression node)
            {
                while (node.NodeType == ExpressionType.Convert ||
                    node.NodeType == ExpressionType.ConvertChecked)
                {
                    node = (node as UnaryExpression).Operand;
                }

                return node;
            }

            /// <summary>
            /// Visit the invocation expression (expand this and compile the expression if it is a lambda and invoke / dynamic invoke , capturing the results)
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                if (node.Expression is LambdaExpression le)
                {
                    var callee = le.Compile();
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
                            case ExpressionType.Parameter:
                                return null;
                            default:
                                throw new InvalidOperationException($"Cannot expand parameter {o} ({o.NodeType})");
                        }
                    });
                    return Expression.Constant(callee.DynamicInvoke(args.ToArray()));
                }
                else if (node.Expression is ConstantExpression constant)
                {
                    var retVal = this.ExtractValue(constant);
                    if (retVal is MethodInfo mi)
                    {
                        retVal = mi.Invoke(null, new object[0]);
                    }
                    else if (retVal is Func<dynamic> fn)
                    {
                        retVal = fn();
                    }

                    return Expression.Constant(retVal);
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
                    case ExpressionType.IsFalse:
                    case ExpressionType.IsTrue:
                        if (node.Operand is MethodCallExpression callExpression)
                        {
                            this.VisitMethodCall(callExpression, true);
                        }
                        else if (node.Operand is UnaryExpression unaryExpression)
                        {
                            this.Visit(unaryExpression.Operand);
                        }
                        else
                        {
                            var parmName = this.ExtractPath(node.Operand, true);
                            if (!parmName.EndsWith("]"))
                            {
                                throw new InvalidOperationException("Only guard conditions can (i.e. Any statements) support unary NOT");
                            }

                            this.AddCondition(parmName, "null");
                        }
                        return null;

                    default:
                        throw new NotSupportedException("Unary expressions cannot be represented as key/value pair filters");
                }
            }

            /// <summary>
            /// Visit method call
            /// </summary>
            protected override Expression VisitMethodCall(MethodCallExpression node) => this.VisitMethodCall(node, false);

            /// <summary>
            /// Visit method call optionally negating any values parsed
            /// </summary>
            private Expression VisitMethodCall(MethodCallExpression node, bool negate)
            {
                switch (node.Method.Name)
                {
                    case "WithControl":
                        {
                            if (!this.StripControl)
                            {
                                object parmName = this.ExtractValue(node.Arguments[1]);
                                object parmValue = this.ExtractValue(node.Arguments[2]);
                                this.AddCondition(parmName.ToString(), parmValue);
                            }
                            return null;
                        }
                    case "Contains":
                        {
                            if (node.Object == null && node.Method.DeclaringType == typeof(Enumerable))
                            {
                                return this.ParseArrayContains(node, negate);
                            }
                            else
                            {
                                var parmName = this.ExtractPath(node.Object, false);
                                object parmValue = this.ExtractValue(node.Arguments[0]);
                                this.AddCondition(parmName, (negate ? "!" : "~") + parmValue.ToString());
                                return null;
                            }
                        }
                    case "StartsWith":
                        {
                            var parmName = this.ExtractPath(node.Object, false);
                            object parmValue = this.ExtractValue(node.Arguments[0]);
                            this.AddCondition(parmName, (negate ? "!" : "^") + parmValue.ToString());
                            return null;
                        }
                    case "Parse":
                        {

                            return null;
                        }
                    case "Any":
                        {
                            var parmName = this.ExtractPath(node.Arguments[0], false);
                            // Process lambda
                            var result = new NameValueCollection();
                            var subQueryExpressionVisitor = new HttpQueryExpressionVisitor(result, node.Arguments[0].Type);
                            if (node.Arguments.Count == 2)
                            {
                                subQueryExpressionVisitor.Visit(node.Arguments[1]);

                                // Result
                                foreach (var itm in result.AllKeys)
                                {
                                    this.AddCondition(String.Format("{0}.{1}", parmName, itm), result.GetValues(itm).Select(o => negate ? $"!{o}" : o).ToList());
                                }

                                return null;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    case "ToString":
                        {
                            return node.Object;
                        }
                    default: // extended fn?
                        {
                            var extendedFn = QueryFilterExtensions.GetExtendedFilterByMethod(node.Method);
                            if (extendedFn == null)
                            {
                                throw new MissingMemberException($"Cannot find extension method {node.Method}");
                            }

                            if (extendedFn.ExtensionMethod.ReturnType == typeof(bool))
                            {
                                var parmName = this.ExtractPath(node.Arguments[0], false);
                                if (parmName == null && typeof(IdentifiedData).IsAssignableFrom(node.Arguments[0].Type))
                                {
                                    parmName = "id";
                                }
                                else if (parmName == null)
                                {
                                    throw new InvalidOperationException($"Cannot determine how to map {extendedFn.Name}");
                                }
                                var callValue = $"{(negate ? "!" : "")}:({extendedFn.Name}";
                                if (node.Arguments.Count > 1)
                                {
                                    callValue += $"|{String.Join(",", node.Arguments.Skip(1).Select(o => this.PrepareValue(this.ExtractValue(o), true)))}";
                                }

                                callValue += ")";

                                this.AddCondition(parmName, callValue);
                                return node;
                            }
                            return null; // let another thing handle this
                        }
                }
            }

            /// <summary>
            /// Parse a Contains call
            /// </summary>
            private Expression ParseArrayContains(MethodCallExpression node, bool inverse)
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
                    {
                        if (inverse)
                        {
                            this.AddCondition(parmName, $"!{i}");
                        }
                        else
                        {
                            this.AddCondition(parmName, i);

                        }

                    }
                    return null;
                }
                else
                {
                    throw new InvalidOperationException("Cannot understand this enumerable object");
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
                var left = this.Visit(node.Left);
                var right = this.Visit(node.Right);

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
                        {
                            this.AddCondition(parmName, true.Equals(cci.Value) ? "null" : "!null");
                        }
                        else if (node.NodeType == ExpressionType.Equal)
                        {
                            this.AddCondition(parmName, true.Equals(cci.Value) ? "!null" : "null");
                        }
                        else
                        {
                            throw new InvalidOperationException($"Cannot determine how to convert ANY() function '{node}'");
                        }
                    }
                    else
                    {
                        this.AddCondition(parmName, "null");
                    }
                }
                else if (!String.IsNullOrEmpty(parmName))
                {
                    Object fParmValue = this.PrepareValue(parmValue, false);
                    if (parmValue is DateTime)
                    {
                        fParmValue = ((DateTime)parmValue).ToString("o");
                    }
                    else if (parmValue is DateTimeOffset)
                    {
                        fParmValue = ((DateTimeOffset)parmValue).ToString("o");
                    }
                    else if (parmValue == null)
                    {
                        fParmValue = "null";
                    }

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
                    if (node.Left is MethodCallExpression mce)
                    {
                        if (left != null)
                        {
                            return node;
                        }
                        else // we have to add it as it hasn't alread been added
                        {
                            var extendedFn = QueryFilterExtensions.GetExtendedFilterByMethod(mce.Method);
                            if (extendedFn != null)
                            {
                                var callValue = $":({extendedFn.Name}";
                                if (mce.Arguments.Count > 1)
                                {
                                    callValue += $"|{String.Join(",", mce.Arguments.Skip(1).Select(o => this.PrepareValue(this.ExtractValue(o), true)))}";
                                }

                                callValue += ")";
                                fParmValue = callValue + fParmValue;
                            }
                        }
                    }
                    else if (node.Right is MethodCallExpression mceRight && right == null &&
                        !String.IsNullOrEmpty(parmName)) // RHS is a method
                    {
                        var extendedFn = QueryFilterExtensions.GetExtendedFilterByMethod(mceRight.Method);
                        if (extendedFn != null)
                        {
                            var callValue = $":({extendedFn.Name}";
                            if (mceRight.Arguments.Count > 1)
                            {
                                callValue += $"|{String.Join(",", mceRight.Arguments.Skip(1).Select(o => this.PrepareValue(this.ExtractValue(o), true)))}";
                            }

                            callValue += ")";
                            fParmValue = callValue + fParmValue;
                        }
                    }

                    if (!String.IsNullOrEmpty(parmName))
                    {
                        this.AddCondition(parmName, fParmValue);
                    }
                }

                return node;
            }

            /// <summary>
            /// Prepare the specified parameter value
            /// </summary>
            private object PrepareValue(object parmValue, bool quoteStrings)
            {
                Object fParmValue = parmValue;
                switch (parmValue)
                {
                    case DateTime dt:
                        {
                            fParmValue = dt.ToString("o");
                            break;
                        }
                    case DateTimeOffset dto:
                        {
                            fParmValue = dto.ToString("o");
                            break;
                        }
                    case String str:
                        if (!str.Equals("null", StringComparison.OrdinalIgnoreCase) && quoteStrings)
                        {
                            fParmValue = $"\"{parmValue.ToString().Replace("\"", "\\\"")}\"";
                        }
                        break;
                    case byte[] bt:
                        fParmValue = bt.Base64UrlEncode();
                        break;
                    case bool blValue:
                        fParmValue = XmlConvert.ToString(blValue);
                        break;
                    default:
                        if (parmValue == null)
                        {
                            fParmValue = "null";
                        }
                        break;
                }

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
                {
                    return null;
                }

                access = this.StripConvert(access);

                switch (access.NodeType)
                {
                    case ExpressionType.Parameter:
                        return "$_";

                    case ExpressionType.TypeAs:
                        return this.ExtractValue(((UnaryExpression)access).Operand);
                    case ExpressionType.Constant:
                        return ((ConstantExpression)access).Value;

                    case ExpressionType.MemberAccess:
                        MemberExpression expr = access as MemberExpression;
                        var expressionValue = this.ExtractValue(expr.Expression);
                        // Is this a ref to a parameter or variable? If so, we must extract it
                        if (expressionValue?.ToString().StartsWith("$_") == true)
                        {
                            return this.ExtractPath(expr, false, true);
                        }
                        else if (expr.Member is PropertyInfo pi)
                        {
                            try
                            {
                                return pi.GetValue(expressionValue);
                            }
                            catch
                            {
                                return null;
                            }
                        }
                        else if (expr.Member is FieldInfo fi)
                        {
                            return fi.GetValue(expressionValue);
                        }
                        break;

                    case ExpressionType.Coalesce:
                        return this.ExtractValue((access as BinaryExpression).Left);

                    case ExpressionType.Invoke:
                        return this.ExtractValue(this.VisitInvocation(access as InvocationExpression));

                    case ExpressionType.Call:
                        var invoke = access as MethodCallExpression;
                        if (invoke.Arguments.Count > 0)
                        {
                            return this.ExtractValue(invoke.Arguments[0]);
                        }
                        else
                        {
                            return invoke.Method.Invoke(this.ExtractValue(invoke.Object), new object[0]);
                        }
                }
                return null;
            }

            /// <summary>
            /// Extract the property path 
            /// </summary>
            /// <param name="accessExpression">The expression to extract the path from</param>
            /// <returns>The extracted path</returns>
            public String ExtractPropertySelector(LambdaExpression accessExpression)
            {
                return this.ExtractPath(accessExpression.Body, true);
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
                    if (memberExpr.Expression.Type.IsGenericType && memberExpr.Expression.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return path;
                    }

                    // XML property?
                    var memberInfo = memberExpr.Expression.Type.GetRuntimeProperty(memberExpr.Member.Name + "Xml") ??
                                     memberExpr.Member;

                    // Member information is declread on interface
                    Type mapType = null;
                    if (memberInfo.DeclaringType.IsInterface && this.m_interfaceHints.TryGetValue(memberInfo.DeclaringType, out mapType))
                    {
                        memberInfo = mapType.GetRuntimeProperty(memberInfo.Name) ?? memberInfo;
                    }

                    // Is this a delay load?
                    var serializationReferenceAttribute = memberExpr.Member.GetCustomAttribute<SerializationReferenceAttribute>();
                    var queryParameterAttribute = memberExpr.Member.GetCustomAttribute<QueryParameterAttribute>();
                    var xmlIgnoreAttribute = memberExpr.Member.GetCustomAttribute<XmlIgnoreAttribute>();
                    if (xmlIgnoreAttribute != null && serializationReferenceAttribute != null && !String.IsNullOrEmpty(serializationReferenceAttribute.RedirectProperty))
                    {
                        memberInfo = memberExpr.Expression.Type.GetRuntimeProperty(serializationReferenceAttribute.RedirectProperty);
                    }

                    // TODO: Delay and bound properties!!
                    var memberXattribute = memberInfo.GetCustomAttributes<XmlElementAttribute>().FirstOrDefault();
                    if (memberXattribute == null && queryParameterAttribute != null)
                    {
                        memberXattribute = new XmlElementAttribute(queryParameterAttribute.ParameterName); // We don't serialize but it does exist
                    }
                    else if (memberExpr.Expression is ConstantExpression)
                    {
                        return (memberExpr.Expression as ConstantExpression).Value.ToString();
                    }
                    else if (memberXattribute == null)
                    {
                        if (memberExpr.Expression.Type.StripNullable() == typeof(DateTimeOffset) &&
                            memberExpr.Member.Name == "DateTime")
                        {
                            return path;
                        }

                        throw new InvalidOperationException($"The path {access} cannot be translated, ensure the property is XML navigable or has a QueryParameter attribute"); // TODO: When this occurs?
                    }

                    // Return path
                    if (String.IsNullOrEmpty(path))
                    {
                        return memberXattribute.ElementName;
                    }
                    else if (memberXattribute.ElementName == "id") // ID can be ignored
                    {
                        return path;
                    }
                    else
                    {
                        return String.Format("{0}.{1}", path, memberXattribute.ElementName);
                    }
                }
                else if (access.NodeType == ExpressionType.Call)
                {
                    //CallExpression callExpr = access as MemberExpression;
                    MethodCallExpression callExpr = access as MethodCallExpression;
                    
                    if (callExpr.Method.Name == "WithControl")
                    {
                        return null;
                    }
                    else if (callExpr.Method.Name == "Where" ||
                        fromUnary && (callExpr.Method.Name == "Any"))
                    {
                        String path = this.ExtractPath(callExpr.Arguments[0], false, fromOperand); // get the chain if required
                        var guardExpression = callExpr.Arguments[1] as LambdaExpression;
                        // Where should be a guard so we just grab the unary equals only!
                        // Is the expression the guard?
                        string guardString = String.Empty;
                        // TODO: The building of a guard expression may not be required - it may save some computation downstream to use full expressions in the guard
                        if (guardExpression.Body is BinaryExpression binaryExpression && !this.TryBuildGuardExpression(binaryExpression, out guardString) ||
                            String.IsNullOrEmpty(guardString) && guardExpression.Body.Type == typeof(bool))
                        {
                            // Attempt to build a complex guard
                            var guardParameter = guardExpression.Parameters[0];
                            var subQuery = new NameValueCollection();
                            var subVisitor = new HttpQueryExpressionVisitor(subQuery, guardParameter.Type);
                            subVisitor.Visit(guardExpression);
                            guardString = Uri.EscapeDataString(subQuery.ToHttpString());
                        }
                        if(String.IsNullOrEmpty(guardString))
                        {
                            throw new InvalidOperationException(String.Format(ErrorMessages.HDSI_GUARD_INVALID, guardExpression));
                        }
                        return String.Format("{0}[{1}]", path, guardString);
                    }
                    else if (callExpr.Method.Name == "First" ||
                        callExpr.Method.Name == "FirstOrDefault")
                    {
                        String path = this.ExtractPath(callExpr.Arguments[0], false, fromOperand); // get the chain if required
                        return path;
                    }
                    else
                    {
                        var extendedFilter = QueryFilterExtensions.GetExtendedFilterByMethod(callExpr.Method);
                        if (extendedFilter != null)
                        {
                            if (callExpr.Arguments.Count > 0)
                            {
                                return this.ExtractPath(callExpr.Arguments[0], false, fromOperand); // get the chain if required
                            }
                            else
                            {
                                return this.ExtractPath(callExpr.Object, false, fromOperand); // get the chain if required
                            }
                        }
                        else if (!s_reservedNames.Contains(callExpr.Method.Name))
                        {
                            throw new InvalidOperationException($"Can't find extended method handler for {callExpr.Method.Name}");
                        }
                    }
                }
                else if (access.NodeType == ExpressionType.TypeAs)
                {
                    UnaryExpression ua = (UnaryExpression)access;
                    return String.Format("{0}@{1}", this.ExtractPath(ua.Operand, false, fromOperand), ua.Type.GetCustomAttribute<XmlTypeAttribute>().TypeName);
                }
                else if (access.NodeType == ExpressionType.Parameter && fromOperand)
                {
                    return "$_";
                }
                else if (access.NodeType == ExpressionType.Coalesce)
                {
                    BinaryExpression ba = (BinaryExpression)access;
                    return this.ExtractPath(ba.Left, fromUnary, fromOperand) ?? this.ExtractPath(ba.Right, fromUnary, fromOperand);
                }
                return null;
            }

            /// <summary>
            /// Build a guard expression
            /// </summary>
            private bool TryBuildGuardExpression(BinaryExpression binaryExpression, out string simpleGuardExpression)
            {
                switch (binaryExpression.NodeType)
                {
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        var retVal = this.TryBuildGuardExpression(binaryExpression.Right as BinaryExpression, out var rightExpression) &
                            this.TryBuildGuardExpression(binaryExpression.Left as BinaryExpression, out var leftExpression);
                        simpleGuardExpression = $"{leftExpression}|{rightExpression}";
                        return retVal;

                    case ExpressionType.Equal:
                        var expressionMember = this.StripConvert(binaryExpression.Left) as MemberExpression;
                        var valueExpression = this.ExtractValue(binaryExpression.Right);
                        var classifierProperty = ExtractValue(binaryExpression.Right) is Guid ?
                            expressionMember.Member.DeclaringType.GetClassifierKeyProperty() : expressionMember.Member.DeclaringType.GetClassifierProperty();
                        if (classifierProperty != expressionMember.Member || valueExpression == null) // Complex processing of the value
                        {
                            simpleGuardExpression = String.Empty;
                            return false;
                        }
                        else if (expressionMember.Type.IsEnum && valueExpression is int valueInt)
                        {
                            valueExpression = Enum.GetName(expressionMember.Type, valueInt);
                        }

                        simpleGuardExpression = valueExpression.ToString();
                        return true;

                    default:
                        simpleGuardExpression = String.Empty;
                        return false;
                }
            }
        }


        /// <summary>
        /// Builds the query dictionary .
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="model">Model.</param>
        /// <param name="stripNullChecks">True if null checks should not be included in the output</param>
        /// <param name="stripControl">True if the <see cref="QueryFilterExtensions.WithControl(object, string, object)"/> should be excluded from the output</param>
        /// <typeparam name="TModel">The 1st type parameter.</typeparam>
        public static NameValueCollection BuildQuery<TModel>(Expression<Func<TModel, bool>> model, bool stripNullChecks = false, bool stripControl = false) => BuildQuery(typeof(TModel), model, stripNullChecks, stripControl);

        /// <summary>
        /// Build query non-generic version
        /// </summary>
        public static NameValueCollection BuildQuery(Type tmodel, LambdaExpression model, bool stripNullChecks = false, bool stripControl = false)
        {
            var retVal = new NameValueCollection();
            var visitor = new HttpQueryExpressionVisitor(retVal, tmodel)
            {
                StripNullChecks = stripNullChecks,
                StripControl = stripControl
            };
            visitor.Visit(model);
            return retVal;
        }

        /// <summary>
        /// Builds the query dictionary .
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="model">Model.</param>
        public static String BuildPropertySelector(LambdaExpression model)
        {
            var visitor = new HttpQueryExpressionVisitor(new NameValueCollection(), model.Parameters[0].Type);
            return visitor.ExtractPropertySelector(model);
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
            {
                memberExpression = (memberExpression as UnaryExpression)?.Operand;
            }

            if (memberExpression == null)
            {
                throw new InvalidOperationException("Cannot convert sort expression");
            }
            else if (memberExpression is MemberExpression mexp &&
                mexp.Member is PropertyInfo propertyInfo)
            {
                var serializationName = propertyInfo.GetSerializationName();
                if (String.IsNullOrEmpty(serializationName))
                {
                    throw new ArgumentException(String.Format(ErrorMessages.FIELD_NOT_FOUND, propertyInfo.Name));
                }
                return $"{serializationName}:{(sort.SortOrder == Map.SortOrderType.OrderBy ? "asc" : "desc")}";
            }
            else
            {
                throw new InvalidOperationException(String.Format(ErrorMessages.ARGUMENT_INCOMPATIBLE_TYPE, typeof(MemberExpression), memberExpression.GetType()));
            }
        }
    }
}