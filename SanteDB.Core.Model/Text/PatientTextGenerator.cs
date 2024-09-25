using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Implementation of a <see cref="IResourceTextGenerator"/> for <see cref="Patient"/>
    /// </summary>
    public class PatientTextGenerator : IResourceTextGenerator
    {
        /// <inheritdoc/>
        public Type ResourceType => typeof(Patient);

        /// <inheritdoc/>
        public void WriteSummary(XmlWriter htmlWriter, IAnnotatedResource resource)
        {
            if (resource is Patient patient) {
                htmlWriter.WriteStartElement("table", SanteDBModelConstants.NS_XHTML);
                htmlWriter.WriteStartElement("tr", SanteDBModelConstants.NS_XHTML);

                htmlWriter.WriteElementString("th", SanteDBModelConstants.NS_XHTML, "Name");
                htmlWriter.WriteStartElement("td", SanteDBModelConstants.NS_XHTML);
                htmlWriter.WriteStartElement("ul", SanteDBModelConstants.NS_XHTML);
                foreach(var n in patient.LoadProperty(o=>o.Names))
                {
                    htmlWriter.WriteStartElement("li", SanteDBModelConstants.NS_XHTML);
                    htmlWriter.WriteElementString("strong", SanteDBModelConstants.NS_XHTML, n.LoadProperty(o => o.NameUse).ToDisplay());
                    htmlWriter.WriteString(n.ToDisplay());
                    htmlWriter.WriteEndElement(); // li
                }
                htmlWriter.WriteEndElement(); // ul
                htmlWriter.WriteEndElement(); // td


                htmlWriter.WriteEndElement();
                htmlWriter.WriteEndElement();
            }
        }
    }
}
