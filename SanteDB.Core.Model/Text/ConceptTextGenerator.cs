using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Renders a concept in current language
    /// </summary>
    public class ConceptTextGenerator : SimpleTextGeneratorBase<Concept>
    {
        protected override void WriteSummary(XmlWriter htmlWriter, Concept data)
        {
            var currentLanguageString = data.LoadProperty(o => o.ConceptNames).FirstOrDefault(o => o.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) ??
                data.LoadProperty(o => o.ConceptNames).FirstOrDefault();

            //var referenceTerms = data.LoadProperty(o => o.ReferenceTerms).Where(r => r.RelationshipTypeKey == ConceptRelationshipTypeKeys.SameAs).Select(o => o.LoadProperty(t => t.ReferenceTerm));

            if(currentLanguageString == null)
            {
                htmlWriter.WriteString(data.Mnemonic);
            }
            else
            {
                htmlWriter.WriteString($"{currentLanguageString.Name} [{currentLanguageString.Language}]");
            }

            // Terms
            //if(referenceTerms.Any())
            //{
            //    foreach(var rtg in referenceTerms.GroupBy(o => o.LoadProperty(r => r.CodeSystem)))
            //    {
            //        htmlWriter.WriteString("[");
            //        htmlWriter.WriteStartElement("em", SanteDBModelConstants.NS_XHTML);
            //        htmlWriter.WriteString(rtg.Key.Name ?? rtg.Key.Domain);
            //        htmlWriter.WriteString($": {String.Join(", ", rtg.Select(o=>o.Mnemonic))}");
            //        htmlWriter.WriteEndElement();
            //        htmlWriter.WriteString("] ");

            //    }
            //}

        }
    }

}
