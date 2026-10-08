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
 * Date: 2024-12-12
 */
using SanteDB.Core.i18n;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Roles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Text generator utilities
    /// </summary>
    public static class TextGeneratorUtil
    {

        private static readonly IDictionary<Type, IResourceTextGenerator> m_textGenerators;

        /// <summary>
        /// Initialize the text generator
        /// </summary>
        static TextGeneratorUtil()
        {
            m_textGenerators = AppDomain.CurrentDomain.GetAllTypes()
                .Where(t => typeof(IResourceTextGenerator).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .Select(t => Activator.CreateInstance(t) as IResourceTextGenerator)
                .ToDictionaryIgnoringDuplicates(o => o.ResourceType, o => o);
        }

        /// <summary>
        /// Try to get the text generator for <paramref name="me"/>
        /// </summary>
        /// <param name="me">The annotated resource to get the text generator</param>
        /// <param name="textGenerator">The text generator</param>
        /// <returns>True if a text generator exists</returns>
        public static bool TryGetTextGenerator(this IAnnotatedResource me, out IResourceTextGenerator textGenerator) => m_textGenerators.TryGetValue(me.GetType(), out textGenerator);

        /// <summary>
        /// Write text to <paramref name="htmlWriter"/>
        /// </summary>
        /// <param name="me">The resource to be written</param>
        /// <param name="htmlWriter">The writer to write to</param>
        /// <returns></returns>
        public static void WriteText(this IAnnotatedResource me, XmlWriter htmlWriter)
        {
            if (me.TryGetTextGenerator(out var generator))
            {
                generator.WriteSummary(htmlWriter, me);
            }
            else
            {
                htmlWriter.WriteElementString("em", SanteDBModelConstants.NS_XHTML, $"No Renderer for {me.GetType().FullName}");
            }
        }

        /// <summary>
        /// Write summary list item
        /// </summary>
        public static void WriteSummaryListItem<TResource>(this TResource resource, XmlWriter htmlWriter, Expression<Func<TResource, object>> selector)
            where TResource : IAnnotatedResource
        {
            if (selector is LambdaExpression lambda)
            {
                htmlWriter.WriteStartElement("li", SanteDBModelConstants.NS_XHTML);
                resource.WriteDataDefinitionInline(htmlWriter, selector);
                htmlWriter.WriteEndElement();
            }
            else
            {
                throw new ArgumentOutOfRangeException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(Expression<Func<TResource, Object>>), selector.GetType()));
            }
        }

        /// <summary>
        /// Write a dt/dd pair
        /// </summary>
        public static void WriteDataDefinitionTerm<TResource>(this TResource resource, XmlWriter htmlWriter, Expression<Func<TResource, object>> selector)
            where TResource : IAnnotatedResource
        {
            if (selector is LambdaExpression lambda)
            {
                switch (lambda.Body)
                {
                    case MemberExpression me:

                        htmlWriter.WriteElementString("dt", SanteDBModelConstants.NS_XHTML, me.Member.Name);
                        htmlWriter.WriteStartElement("dd", SanteDBModelConstants.NS_XHTML);

                        var data = resource.LoadProperty(selector);
                        WriteDataObject(htmlWriter, data);
                        htmlWriter.WriteEndElement();
                        break;
                    case ConstantExpression ce:

                        htmlWriter.WriteStartElement("dd", SanteDBModelConstants.NS_XHTML);
                        WriteDataObject(htmlWriter, ce.Value);
                        htmlWriter.WriteEndElement();
                        break;
                    case MethodCallExpression mce:
                        WriteDataObject(htmlWriter, mce.GetValue(resource));
                        break;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(Expression<Func<TResource, Object>>), selector.GetType()));
            }
        }

        /// <summary>
        /// Write data inline
        /// </summary>
        public static void WriteDataDefinitionInline<TResource>(this TResource resource, XmlWriter htmlWriter, Expression<Func<TResource, object>> selector)
            where TResource : IAnnotatedResource
        {
            if (selector is LambdaExpression lambda)
            {
                switch (lambda.Body)
                {
                    case MemberExpression memberExpression:
                        htmlWriter.WriteStartElement("strong", SanteDBModelConstants.NS_XHTML);
                        htmlWriter.WriteAttributeString("style", "display:block");
                        htmlWriter.WriteString(memberExpression.Member.Name);
                        htmlWriter.WriteEndElement();

                        var data = resource.LoadProperty(selector);
                        WriteDataObject(htmlWriter, data);

                        break;
                    case ConstantExpression ce:
                        WriteDataObject(htmlWriter, ce.Value);
                        break;
                    case MethodCallExpression mce:
                        WriteDataObject(htmlWriter, mce.GetValue(resource));
                        break;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(Expression<Func<TResource, Object>>), selector.GetType()));
            }
        }

        /// <summary>
        /// Write a summary row to a table
        /// </summary>
        public static void WriteSummaryRow<TResource>(this TResource resource, XmlWriter htmlWriter, Expression<Func<TResource, object>> selector)
            where TResource : IAnnotatedResource
        {

            if (selector is LambdaExpression lambda)
            {
                htmlWriter.WriteStartElement("tr", SanteDBModelConstants.NS_XHTML);
                try
                {
                    switch (lambda.Body)
                    {
                        case MemberExpression memberExpression:
                            {
                                var data = resource.LoadProperty(selector);
                                if (data == null) return;

                                htmlWriter.WriteElementString("th", SanteDBModelConstants.NS_XHTML, memberExpression.Member.Name);
                                htmlWriter.WriteStartElement("td", SanteDBModelConstants.NS_XHTML);

                                WriteDataObject(htmlWriter, data);

                                htmlWriter.WriteEndElement(); // td
                                break;
                            }
                        case ConstantExpression ce: // Constant like a string
                            {
                                htmlWriter.WriteStartElement("th", SanteDBModelConstants.NS_XHTML);
                                htmlWriter.WriteAttributeString("colspan", "2");
                                WriteDataObject(htmlWriter, ce.Value);
                                htmlWriter.WriteEndElement();
                                break;
                            }
                        case MethodCallExpression mce:
                            WriteDataObject(htmlWriter, mce.GetValue(resource));
                            break;
                    }

                }
                finally
                {
                    htmlWriter.WriteEndElement(); // tr
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(String.Format(ErrorMessages.INVALID_EXPRESSION_TYPE, typeof(Expression<Func<TResource, Object>>), selector.GetType()));
            }

        }

        private static object GetValue<TResource>(this MethodCallExpression mce, TResource resource)
        {
            if (mce.Object is ConstantExpression boundObject)
            {
                return mce.Method.Invoke(boundObject.Value, mce.Arguments.Select(arg =>
                {
                    switch (arg)
                    {
                        case ParameterExpression pe: return pe.Type == typeof(TResource) ? (object)resource : null;
                        case ConstantExpression ce: return ce.Value;
                        default: return null;
                    }
                }).ToArray());
            }
            else
            {
                return "INVALID CONDITION";
            }
        }

        /// <summary>
        /// Write data object 
        /// </summary>
        private static void WriteDataObject(XmlWriter htmlWriter, object data)
        {
            switch (data)
            {
                case IList il:
                    if (!il.IsNullOrEmpty())
                    {
                        htmlWriter.WriteStartElement("ul", SanteDBModelConstants.NS_XHTML);
                        foreach (var n in il.OfType<IAnnotatedResource>())
                        {
                            htmlWriter.WriteStartElement("li", SanteDBModelConstants.NS_XHTML);
                            n.WriteText(htmlWriter);
                            htmlWriter.WriteEndElement();
                        }
                        htmlWriter.WriteEndElement(); // ul
                    }
                    else
                    {
                        htmlWriter.WriteElementString("em", SanteDBModelConstants.NS_XHTML, "Empty");
                    }
                    break;
                case IAnnotatedResource iar:
                    iar.WriteText(htmlWriter);
                    break;
                case XElement xe:
                    xe.WriteTo(htmlWriter);
                    break;
                default:
                    htmlWriter.WriteElementString("span", SanteDBModelConstants.NS_XHTML, data.ToString());
                    break;
            }
        }
    }
}
