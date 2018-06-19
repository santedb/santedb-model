﻿/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
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
 * Date: 2017-9-1
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SanteDB.Core.Model.Security;
using SanteDB.Core.Model.Map;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SanteDB.Core.Model.Map
{
    /// <summary>
    /// Model conversion visitor is used to convert a lambda expression based on the business model 
    /// into a domain model lamda expression
    /// </summary>
    public class ModelExpressionVisitor : ExpressionVisitor
    {


        /// <summary>
        /// A small visitor which corrects lambda expressions to skip over associative
        /// classes
        /// </summary>
        private class LambdaCorrectionVisitor : ExpressionVisitor
        {

            // Original Parameter
            private readonly ParameterExpression m_originalParameter;
            // Member access
            private readonly Expression m_memberAccess;

            private readonly ModelMapper m_sourceMapper;

            /// <summary>
            /// Creates a new instance of the lambda correction visitor
            /// </summary>
            public LambdaCorrectionVisitor(Expression correctedMemberAccess, ParameterExpression lambdaExpressionParameter, ModelMapper sourceMapper)
            {
                this.m_originalParameter = lambdaExpressionParameter;
                this.m_memberAccess = correctedMemberAccess;
                this.m_sourceMapper = sourceMapper;
            }

            /// <summary>
            /// Visit the node
            /// </summary>
            public override Expression Visit(Expression node)
            {

                if (node == null)
                    return node;

                switch (node.NodeType)
                {
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Equal:
                        return this.VisitBinary((BinaryExpression)node);
                    case ExpressionType.MemberAccess:
                        {
                            MemberExpression memberExpression = node as MemberExpression;
                            if ((memberExpression.Expression as ParameterExpression)?.Name == this.m_originalParameter.Name)
                            {

                                var memInfo = this.m_memberAccess.Type.GetRuntimeProperty(memberExpression.Member.Name);
                                if(memInfo == null)
                                    return memberExpression;
                                else
                                    return Expression.MakeMemberAccess(this.m_memberAccess, memInfo ?? memberExpression.Member);
                            }
                            else
                                return base.Visit(node);
                        }
                    default:
                        return base.Visit(node);
                }

            }

            /// <summary>
            /// Visit a binary method
            /// </summary>
            protected override Expression VisitBinary(BinaryExpression node)
            {
                Expression right = this.Visit(node.Right),
                    left = this.Visit(node.Left);
                if (right != node.Right || left != node.Left)
                    return Expression.MakeBinary(node.NodeType, left, right);
                return node;
            }
        }

        // The mapper to be used
        private readonly ModelMapper m_mapper;

        // Parameters
        private readonly ParameterExpression[] m_parameters;

        // Scope stack
        private Stack<ParameterExpression> m_scope = new Stack<ParameterExpression>();

        /// <summary>
        /// Attempt to get constant value
        /// </summary>
        private Object GetConstantValue(Expression expression)
        {
            if (expression == null)
                return null;
            else if (expression is ConstantExpression)
                return (expression as ConstantExpression).Value;
            else if (expression is UnaryExpression)
            {
                var un = expression as UnaryExpression;
                switch (expression.NodeType)
                {
                    case ExpressionType.TypeAs:
                        return this.GetConstantValue(un.Operand);
                    case ExpressionType.Convert:
                        return this.GetConstantValue(un.Operand);
                    default:
                        throw new InvalidOperationException($"Expression {expression} not supported for constant extraction");
                }
            }
            else if (expression is MemberExpression)
            {
                var mem = expression as MemberExpression;
                var obj = this.GetConstantValue(mem.Expression);
                if (mem.Member is PropertyInfo)
                    return (mem.Member as PropertyInfo).GetValue(obj);
                else if (mem.Member is FieldInfo)
                    return (mem.Member as FieldInfo).GetValue(obj);
                else
                    throw new NotSupportedException();
            }
            else
                throw new InvalidOperationException($"Expression {expression} not supported for constant extraction");

        }


        /// <summary>
        /// Model conversion visitor 
        /// </summary>
        public ModelExpressionVisitor(ModelMapper mapData, params ParameterExpression[] parameters)
        {
            this.m_mapper = mapData;
            this.m_parameters = parameters;
        }

        /// <summary>
        /// Visit an expression
        /// </summary>
        public override Expression Visit(Expression node)
        {

            if (node == null)
                return node;

            switch (node.NodeType)
            {
                // TODO: Unary
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    return this.VisitBinary((BinaryExpression)node);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)node);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)node);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)node);
                case ExpressionType.Lambda:
                    return this.VisitLambdaGeneric((LambdaExpression)node);
                case ExpressionType.Convert:
                    return this.VisitConvert((UnaryExpression)node);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)node);
                case ExpressionType.TypeIs:
                    return this.VisitTypeBinary((TypeBinaryExpression)node);
                case ExpressionType.TypeAs:
                    return this.VisitUnary((UnaryExpression)node);
                default:
                    return base.Visit(node);
            }

        }

        /// <summary>
        /// Map type binary
        /// </summary>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            Expression newExpr = this.Visit(node.Expression);
            if (newExpr == null)
                return null;
            var newType = this.m_mapper.MapModelType(node.TypeOperand);
            if (newExpr != node.Expression || newType != node.TypeOperand)
                return Expression.TypeIs(newExpr, newType);
            return node;
        }

        /// <summary>
        /// Visit unary expression
        /// </summary>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            var newOp = this.Visit(node.Operand);
            if (newOp == null)
                return null;
            if (newOp != node.Operand && node.NodeType == ExpressionType.TypeAs)
                return this.m_mapper.MapTypeCast(node, newOp);
            else if (newOp != node.Operand)
                return Expression.MakeUnary(node.NodeType, newOp, this.m_mapper.MapModelType(node.Type));
            return node;
        }

        /// <summary>
        /// Remove unnecessary convert statement
        /// </summary>
        public virtual Expression VisitConvert(UnaryExpression convert)
        {
            Expression newOperand = this.Visit(convert.Operand);
            if (newOperand == null)
                return null;

            if (newOperand != convert.Operand)
            {
                Type targetType = m_mapper.MapModelType(convert.Type);
                if (targetType == convert.Type) // No map
                    return newOperand;
                
                return Expression.Convert(newOperand, targetType);
            }
            return convert;
        }

        /// <summary>
        /// Visit a lambda expression
        /// </summary>
        protected virtual Expression VisitLambdaGeneric(LambdaExpression node)
        {
            var parameters = this.VisitExpressionList(node.Parameters.OfType<Expression>().ToList()).OfType<ParameterExpression>().ToArray();
            if (parameters == null)
                return null;
            var parmExpr = this.Visit(parameters[0]);
            if (parmExpr == null)
                return null;
            this.m_scope.Push(parmExpr as ParameterExpression);
            Expression newBody = this.Visit(node.Body);
            if (newBody == null)
                return null;

            if (newBody != node.Body)
            {
                var lambdaType = node.Type;
                if (lambdaType.GetGenericTypeDefinition() == typeof(Func<,>))
                    lambdaType = typeof(Func<,>).MakeGenericType(parameters.Select(p => p.Type).Union(new Type[] { newBody.Type }).ToArray());
                return Expression.Lambda(lambdaType, newBody, this.m_scope.Pop());
            }
            else this.m_scope.Pop();
            return node;

        }

        /// <summary>
        /// Visit method call
        /// </summary>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Expression newExpression = this.Visit(node.Object);
            if (newExpression == null)
                return null;
            IEnumerable<Expression> args = this.VisitExpressionList(node.Arguments);
            if (args == null)
                return null;

            if (newExpression != node.Object || args != node.Arguments)
            {
                // Re-bind the parameter types
                MethodInfo methodInfo = node.Method;
                if (methodInfo.IsGenericMethod) // Generic re-bind
                {
                    // HACK: Find a more appropriate way of doing this
                    Type bindType = this.m_mapper.ExtractDomainType(args.First().Type);
                    methodInfo = methodInfo.GetGenericMethodDefinition().MakeGenericMethod(new Type[] { bindType });
                }

                return Expression.Call(newExpression, methodInfo, args);
            }
            return base.VisitMethodCall(node);
        }

        /// <summary>
        /// Visit each expression in the args
        /// </summary>
        protected virtual ICollection<Expression> VisitExpressionList(ICollection<Expression> args)
        {
            List<Expression> retVal = new List<Expression>();
            bool isDifferent = false;
            foreach (var exp in args)
            {
                Expression argExpression = this.Visit(exp);
                if (argExpression == null) // couldn't map // invalid
                    return null;

                // Is there a VIA expression to be corrected?
                if (argExpression is LambdaExpression)
                {
                    var lambdaExpression = argExpression as LambdaExpression;
					// Ok, we need to find the traversal expression
					//this.m_mapper.
					

					var newParameter = Expression.Parameter(this.m_mapper.ExtractDomainType(retVal[0].Type), lambdaExpression.Parameters[0].Name);
					//Expression accessExpression = this.m_mapper.CreateLambdaMemberAdjustmentExpression(retVal.First(), newParameter);

					var accessExpression = this.m_mapper.CreateLambdaMemberAdjustmentExpression(args.First() as MemberExpression, newParameter);

					var newBody = new LambdaCorrectionVisitor(accessExpression, lambdaExpression.Parameters[0], this.m_mapper).Visit(lambdaExpression.Body);
                    if (newBody == null)
                        return null;

                    var lambdaType = typeof(Func<,>).MakeGenericType(new Type[] { newParameter.Type, newBody.Type });

                    argExpression = Expression.Lambda(lambdaType, newBody, newParameter);

                }
                // Add the expression
                if (argExpression != exp)
                {
                    isDifferent = true;
                    retVal.Add(argExpression);
                }
                else
                    retVal.Add(exp);
            }
            if (isDifferent)
                return retVal;
            else
                return args;
        }

        /// <summary>
        /// Visit a binary method
        /// </summary>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression right = this.Visit(node.Right),
                left = this.Visit(node.Left);

            if (right == null || left == null)
                return null;

            // Are the types compatible?
            if (!right.Type.GetTypeInfo().IsAssignableFrom(left.Type.GetTypeInfo()))
            {
                if(right.NodeType == ExpressionType.Convert)
                    right = ((UnaryExpression)right).Operand;

                // Convert byte[] <= Guid
                if ((right.Type == typeof(Guid) || right.Type == typeof(Guid?)) && left.Type == typeof(Byte[]))
                {
                    switch(right.NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            var memberExpr = (MemberExpression)right;
                            Object scope = null;
                            if (memberExpr.Expression != null)
                            {
                                Stack<Expression> accessStack = new Stack<Expression>();
                                Expression accessExpr = memberExpr.Expression;
                                while (!(accessExpr is ConstantExpression))
                                {
                                    accessStack.Push(accessExpr);
                                    if (accessExpr is MemberExpression)
                                        accessExpr = (accessExpr as MemberExpression).Expression;
                                    else break;
                                }

                                scope = ((ConstantExpression)accessExpr).Value;
                                while (accessStack.Count > 0)
                                {
                                    var member = (accessStack.Pop() as MemberExpression).Member;
                                    if (member is PropertyInfo)
                                        scope = (member as PropertyInfo).GetValue(scope);
                                    else if (member is FieldInfo)
                                        scope = (member as FieldInfo).GetValue(scope);
                                }
                            }
                            if (memberExpr.Member is FieldInfo)
                                right = Expression.Constant(((Guid)(memberExpr.Member as FieldInfo).GetValue(scope)).ToByteArray());
                            else if(memberExpr.Member is MethodInfo)
                                right = Expression.Constant(((Guid)(memberExpr.Member as MethodInfo).Invoke(scope, null)).ToByteArray());
                            else if(memberExpr.Member is PropertyInfo)
                                right = Expression.Constant(((Guid)(memberExpr.Member as PropertyInfo).GetValue(scope)).ToByteArray());

                            break;
                        case ExpressionType.Constant:
                            right = Expression.Constant(((Guid)((ConstantExpression)right).Value).ToByteArray());
                            break;
                    }
                }
				else if ((right.Type == typeof(DateTimeOffset) || right.Type == typeof(DateTimeOffset?)) && (left.Type == typeof(DateTime?) || left.Type == typeof(DateTime)))
				{
					DateTime dateTime;
                    var cvalue = this.GetConstantValue(right );

                    if (cvalue == null)
                    {
                        return Expression.MakeBinary(node.NodeType, left, Expression.Constant(null));
                    }
                    else if (!DateTime.TryParse(cvalue.ToString(), out dateTime))
                    {
                        throw new InvalidOperationException($"Unable to convert { (right as ConstantExpression)?.Value } to a valid date time");
                    }
                    else
                    {
                        right = Expression.Constant(dateTime, left.Type);
                        return Expression.MakeBinary(node.NodeType, left, Expression.Convert(right, left.Type));
                    }
				}

				return Expression.MakeBinary(node.NodeType, left, Expression.Convert(right, left.Type));
            }
            else if (right != node.Right || left != node.Left)
            {
                if (right.Type != left.Type)
                {

                    if (right.Type.GetTypeInfo().IsGenericType &&
                        right.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (right.NodeType == ExpressionType.Convert &&
                            (right as UnaryExpression).Operand.Type == left.Type)
                            right = (right as UnaryExpression).Operand;
                        else
                            right = Expression.Coalesce(right, Expression.Constant(Activator.CreateInstance(right.Type.GetTypeInfo().GenericTypeArguments[0])));
                    }
                    if (left.Type.GetTypeInfo().IsGenericType &&
                        left.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        if (left.NodeType == ExpressionType.Convert &&
                            (left as UnaryExpression).Operand.Type == right.Type)
                            left = (left as UnaryExpression).Operand;
                        else
                            left = Expression.Coalesce(left, Expression.Constant(Activator.CreateInstance(left.Type.GetTypeInfo().GenericTypeArguments[0])));
                    // Handle nullable <> null to always be true
                    if ((right is ConstantExpression && (right as ConstantExpression).Value == null ||
                        left is ConstantExpression && (left as ConstantExpression).Value == null) &&
                        (!right.Type.GetTypeInfo().IsClass || !left.Type.GetTypeInfo().IsClass))
                        return Expression.Constant(true);
                }
                return Expression.MakeBinary(node.NodeType, left, right);
            }
            return node;
        }

        /// <summary>
        /// Visit parameter
        /// </summary>
        protected override Expression VisitParameter(ParameterExpression node)
        {

            Type mappedType = this.m_mapper.MapModelType(node.Type);
            var parameterRef = this.m_parameters.FirstOrDefault(p => p.Name == node.Name && p.Type == mappedType);
            
            if (parameterRef != null)
                return parameterRef;

            parameterRef = this.m_scope.FirstOrDefault(p => p.Name == node.Name && p.Type == mappedType);
            if (parameterRef != null)
                return parameterRef;

            if (mappedType != null && mappedType != node.Type)
                return Expression.Parameter(mappedType, node.Name);

            return node;
        }

        /// <summary>
        /// Visit member access, converts member expression type and name
        /// </summary>
        /// <param name="node">The node to be converted</param>
        protected virtual Expression VisitMemberAccess(MemberExpression node)
        {
            // Convert the expression
            Expression newExpression = this.Visit(node.Expression);
            if (newExpression == null)
                return null;
            if (newExpression != node.Expression)
            {
                // Is the node member access a useless convert function?
                if (node.Expression.NodeType == ExpressionType.Convert)
                {
                    UnaryExpression convertExpression = node.Expression as UnaryExpression;
                    if (convertExpression.Type.GetTypeInfo().IsAssignableFrom(convertExpression.Operand.Type.GetTypeInfo()))
                        node = Expression.MakeMemberAccess(convertExpression.Operand, node.Member);
                }
                return this.m_mapper.MapModelMember(node, newExpression);
            }
            return node;
        }


    }
}