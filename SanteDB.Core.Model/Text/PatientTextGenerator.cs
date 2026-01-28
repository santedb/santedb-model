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
