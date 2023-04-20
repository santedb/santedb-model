/*
 * Copyright (C) 2021 - 2023, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2023-3-10
 */
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Exceptions
{
    /// <summary>
    /// An exception of model map compilation errors
    /// </summary>
    public class ModelMapCompileException : Exception
    {

        /// <summary>
        /// Gets the errors from the compiler
        /// </summary>
        public IEnumerable<String> Errors { get; private set; }

        /// <summary>
        /// Creates a new model map compilation exception
        /// </summary>
        internal ModelMapCompileException(CompilerErrorCollection errors)
        {
            var errorList = new List<String>(errors.Count);
            foreach (CompilerError itm in errors)
            {
                errorList.Add($"{(itm.IsWarning ? "W" : "E")}: {itm.ErrorText} @ {itm.FileName}:{itm.Line}:{itm.Column}");
            }
            this.Errors = errorList;
        }

        /// <summary>
        /// Represent as a string
        /// </summary>
        public override string ToString()
        {
            StringBuilder retVal = new StringBuilder(this.Message);
            foreach(var itm in this.Errors)
            {
                retVal.AppendFormat("\r\n\t{0}", itm);
            }
            return retVal.ToString();
        }
    }
}
