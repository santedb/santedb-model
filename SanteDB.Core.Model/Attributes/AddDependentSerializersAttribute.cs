using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Attribute which instructs the XmlSerializerFactory to append all dependent serializers
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AddDependentSerializersAttribute : Attribute
    {
    }
}