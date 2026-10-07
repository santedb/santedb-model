using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Roles;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Render style
    /// </summary>
    public enum TextGeneratorRenderStyle
    {
        /// <summary>
        /// Represent as a table
        /// </summary>
        Table,
        /// <summary>
        /// Represent as a list
        /// </summary>
        List,
        /// <summary>
        /// represents as a DL/DD relationship
        /// </summary>
        DefinitionList,
        /// <summary>
        /// Represent inline
        /// </summary>
        Inline
    };

    /// <summary>
    /// Simplified text generator
    /// </summary>
    public abstract class SimpleTextGeneratorBase<TEntity> : IResourceTextGenerator
        where TEntity : IdentifiedData
    {
        /// <inheritdoc/>
        public Type ResourceType => typeof(TEntity);

        /// <inhertidoc/>
        public void WriteSummary(XmlWriter htmlWriter, IAnnotatedResource resource)
        {
            if (resource is TEntity ent)
            {
                this.WriteSummary(htmlWriter, ent);
            }
            else
            {
                htmlWriter.WriteComment($"Expected {typeof(TEntity)} but received {resource.GetType()}");
            }
        }

        /// <summary>
        /// Write summary 
        /// </summary>
        protected abstract void WriteSummary(XmlWriter htmlWriter, TEntity data);

    }

    /// <summary>
    /// Entity text generator
    /// </summary>
    public abstract class ComplexTextGeneratorBase<TEntity> : SimpleTextGeneratorBase<TEntity>
        where TEntity : IdentifiedData
    {

        /// <summary>
        /// True if this is a simple type
        /// </summary>
        public abstract TextGeneratorRenderStyle RenderStyle { get; }

        /// <summary>
        /// Get the list of fields from this object which are to be rendered
        /// </summary>
        /// <returns>The ordered list of field expressions to render</returns>
        public abstract IEnumerable<Expression<Func<TEntity, object>>> GetRenderFields();

        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, TEntity idType)
        {
            switch (this.RenderStyle)
            {
                case TextGeneratorRenderStyle.Table:
                    {
                        htmlWriter.WriteStartElement("table", SanteDBModelConstants.NS_XHTML);
                        htmlWriter.WriteAttributeString("border", "1");
                        htmlWriter.WriteElementString("caption", SanteDBModelConstants.NS_XHTML, $"{idType.ToDisplay()} ({idType.Type})");
                        this.GetRenderFields().ForEach(r => idType.WriteSummaryRow(htmlWriter, r));
                        htmlWriter.WriteEndElement();
                        break;

                    }
                case TextGeneratorRenderStyle.List:
                    {
                        htmlWriter.WriteStartElement("ul", SanteDBModelConstants.NS_XHTML);
                        this.GetRenderFields().ForEach(r => idType.WriteSummaryListItem(htmlWriter, r));
                        htmlWriter.WriteEndElement();
                        break;
                    }
                case TextGeneratorRenderStyle.DefinitionList:
                    {
                        htmlWriter.WriteStartElement("dl", SanteDBModelConstants.NS_XHTML);
                        this.GetRenderFields().ForEach(r => idType.WriteDataDefinitionTerm(htmlWriter, r));
                        htmlWriter.WriteEndElement(); // dl
                        break;
                    }
                case TextGeneratorRenderStyle.Inline:
                    {
                        htmlWriter.WriteStartElement("div", SanteDBModelConstants.NS_XHTML);
                        this.GetRenderFields().ForEach(r => idType.WriteDataDefinitionInline(htmlWriter, r));
                        htmlWriter.WriteEndElement();
                        break;
                    }
            }
        }
    }
}
