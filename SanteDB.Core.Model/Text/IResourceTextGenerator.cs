using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Represents a class that can generate a summary of an object 
    /// </summary>
    public interface IResourceTextGenerator
    {

        /// <summary>
        /// Gets the resource type that the text generator applies to
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// Write the summary for <paramref name="resource"/> onto <paramref name="htmlWriter"/>
        /// </summary>
        /// <param name="htmlWriter">The <see cref="XmlWriter"/> which is emitting the HTML for <paramref name="resource"/></param>
        /// <param name="resource">The resource on which the summation should be created</param>
        void WriteSummary(XmlWriter htmlWriter, IAnnotatedResource resource);

    }
}
