using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Exceptions
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
            foreach(CompilerError itm in errors)
            {
                errorList.Add($"{(itm.IsWarning ? "W" : "E")}: {itm.ErrorText} @ {itm.FileName}:{itm.Line}:{itm.Column}");
            }
            this.Errors = errorList;
        }
    }
}
