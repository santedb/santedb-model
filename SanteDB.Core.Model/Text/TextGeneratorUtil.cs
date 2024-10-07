using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

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

    }
}
