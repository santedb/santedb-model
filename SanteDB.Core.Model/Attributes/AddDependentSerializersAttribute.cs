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
 * Date: 2023-6-21
 */
using SanteDB.Core.Model.Serialization;
using System;

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