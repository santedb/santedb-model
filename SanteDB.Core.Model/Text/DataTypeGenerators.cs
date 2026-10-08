using SanteDB.Core.Extensions;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Extensions;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Entity name text generator
    /// </summary>
    public class EntityNameTextGenerator : SimpleTextGeneratorBase<EntityName>
    {
        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, EntityName data)
        {
            htmlWriter.WriteElementString("span", SanteDBModelConstants.NS_XHTML, data.ToDisplay());
            htmlWriter.WriteStartElement("strong", SanteDBModelConstants.NS_XHTML);
            htmlWriter.WriteString("(");
            data.LoadProperty(o => o.NameUse).WriteText(htmlWriter);
            htmlWriter.WriteString(")");
            htmlWriter.WriteEndElement();
        }
    }

    /// <summary>
    /// Entity address text generator
    /// </summary>
    public class EntityAddressTextGenerator : SimpleTextGeneratorBase<EntityAddress>
    {
        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, EntityAddress data)
        {
            htmlWriter.WriteElementString("span", SanteDBModelConstants.NS_XHTML, data.ToDisplay());
            htmlWriter.WriteStartElement("strong", SanteDBModelConstants.NS_XHTML);
            htmlWriter.WriteString("(");
            data.LoadProperty(o => o.AddressUse).WriteText(htmlWriter);
            htmlWriter.WriteString(")");
            htmlWriter.WriteEndElement();
        }
    }

    /// <summary>
    /// Identifier text generator
    /// </summary>
    public abstract class IdentifierTextGenerator<TModel> : SimpleTextGeneratorBase<TModel>
        where TModel : IdentifiedData, IExternalIdentifier
    {
        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, TModel data)
        {
            htmlWriter.WriteElementString("span", SanteDBModelConstants.NS_XHTML, data.Value);
            htmlWriter.WriteElementString("strong", SanteDBModelConstants.NS_XHTML, $"[{data.LoadProperty(o => o.IdentityDomain).Name}]");
        }
    }

    /// <summary>
    /// Entity identifier text generator
    /// </summary>
    public class EntityIdentifierTextGenerator : IdentifierTextGenerator<EntityIdentifier> { }

    /// <summary>
    /// Entity identifier text generator
    /// </summary>
    public class ActIdentifierTextGenerator : IdentifierTextGenerator<ActIdentifier> { }

    /// <summary>
    /// Extension renderer
    /// </summary>
    public abstract class ExtensionTextGenerator<TModel> : SimpleTextGeneratorBase<TModel>
        where TModel : IdentifiedData, IModelExtension
    {
        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, TModel data)
        {
            if (data.ExtensionTypeKey == ExtensionTypeKeys.JpegPhotoExtension)
            {
                htmlWriter.WriteStartElement("img", SanteDBModelConstants.NS_XHTML);
                htmlWriter.WriteAttributeString("alt", "JPEG Photo of Object");
                htmlWriter.WriteAttributeString("src", $"data:image/png;base64,{Convert.ToBase64String(data.Data)}");
                htmlWriter.WriteEndElement();
            }
            else {
                var extensionType = EntitySource.Current.Get<ExtensionType>(data.ExtensionTypeKey);
                htmlWriter.WriteElementString("strong", SanteDBModelConstants.NS_XHTML, extensionType.Name);
                htmlWriter.WriteElementString("span", SanteDBModelConstants.NS_XHTML, data.Display);
            }
        }
    }

    /// <summary>
    /// text handler for entity extension
    /// </summary>
    public class EntityExtensionTextGenerator : ExtensionTextGenerator<EntityExtension> { }

    /// <summary>
    /// text handler for act extension
    /// </summary>
    public class ActExtensionTextGenerator : ExtensionTextGenerator<ActExtension> { }

    /// <summary>
    /// Text generator for telecome
    /// </summary>
    public class EntityTelecomTextGenerator : SimpleTextGeneratorBase<EntityTelecomAddress>
    {
        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, EntityTelecomAddress data)
        {
            htmlWriter.WriteStartElement("strong", SanteDBModelConstants.NS_XHTML);
            data.LoadProperty(o => o.AddressUse).WriteText(htmlWriter);
            htmlWriter.WriteEndElement();
            htmlWriter.WriteElementString("span", SanteDBModelConstants.NS_XHTML, data.Value);
            
            if(data.TypeConceptKey.HasValue)
            {
                htmlWriter.WriteStartElement("em", SanteDBModelConstants.NS_XHTML);
                data.LoadProperty(o => o.TypeConcept).WriteText(htmlWriter);
                htmlWriter.WriteEndElement();
            }
        }
    }

    /// <summary>
    /// Security policy instance
    /// </summary>
    public class SecurityPolicyInstanceTextGenerator : SimpleTextGeneratorBase<SecurityPolicyInstance>
    {
        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, SecurityPolicyInstance data)
        {
            htmlWriter.WriteElementString("span", SanteDBModelConstants.NS_XHTML, data.LoadProperty(o => o.Policy).Name);

            htmlWriter.WriteString("(");
            htmlWriter.WriteElementString("em", SanteDBModelConstants.NS_XHTML, data.LoadProperty(o => o.Policy).Oid);
            htmlWriter.WriteString(")");

            if (data.Policy.ClassConceptKey.HasValue)
            {
                htmlWriter.WriteStartElement("strong", SanteDBModelConstants.NS_XHTML);
                data.Policy.LoadProperty(o => o.ClassConcept).WriteText(htmlWriter);
                htmlWriter.WriteEndElement();
            }
        }
    }

    /// <summary>
    /// Entity note text generator
    /// </summary>
    public class EntityNoteTextGenerator : SimpleTextGeneratorBase<EntityNote>
    {
        /// <inheritdoc/>
        protected override void WriteSummary(XmlWriter htmlWriter, EntityNote data)
        {
            htmlWriter.WriteElementString("strong", SanteDBModelConstants.NS_XHTML, $"Author: {data.LoadProperty(c=>c.Author).ToDisplay()}");
            htmlWriter.WriteElementString("pre", SanteDBModelConstants.NS_XHTML, data.Text);
        }
    }
}
