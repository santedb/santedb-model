using SanteDB.Core.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Attributes
{
    /// <summary>
    /// Attribute which instructs the XmlSerializerFactory to append all dependent serializers
    /// </summary>
    /// <remarks>
    /// In SanteDB plugins are permitted to register and expose new types of <see cref="IdentifiedData"/> for their specific use cases.
    /// This does not bode well for the .NET <see cref="System.Xml.Serialization.XmlSerializer"/> which requires knowledge of types prior to 
    /// calling the constructor. This attribute instructs the <see cref="XmlModelSerializerFactory"/> to reflect all implementations of
    /// <see cref="IdentifiedData"/> from the current application context and append them as <see cref="System.Xml.Serialization.XmlIncludeAttribute"/>
    /// instructions
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class AddDependentSerializersAttribute : Attribute
    {
    }
}