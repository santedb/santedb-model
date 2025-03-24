using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Instructs <see cref="ExtensionMethods.LoadProperty(Interfaces.IAnnotatedResource, string, bool, IEnumerable{IdentifiedData})"/> to load this property in an inverse way
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class DelayLoadInverseAttribute : Attribute
    {
    }
}
