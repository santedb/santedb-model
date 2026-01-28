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
