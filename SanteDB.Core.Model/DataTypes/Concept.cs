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
using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.DataTypes
{
    /// <summary>
    /// A class representing a generic concept used in the SanteDB datamodel
    /// </summary>

    [XmlType("Concept", Namespace = "http://santedb.org/model"), JsonObject("Concept")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "Concept")]
    [Classifier(nameof(Mnemonic)), KeyLookup(nameof(Mnemonic))]
    public class Concept : VersionedEntityData<Concept>, IHasState
    {
        /// <summary>
        /// Creates a new concept
        /// </summary>
        public Concept()
        {

        }


        /// <summary>
        /// Gets or sets the unchanging mnemonic for the concept
        /// </summary>
        [XmlElement("mnemonic"), JsonProperty("mnemonic")]
        public String Mnemonic { get; set; }

        /// <summary>
        /// Gets or sets the status concept key
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("statusConcept"), JsonProperty("statusConcept")]
        [Binding(typeof(StatusKeys))]
        public Guid? StatusConceptKey { get; set; }

        /// <summary>
        /// Gets or sets the status of the concept
        /// </summary>
        [SerializationReference(nameof(StatusConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept StatusConcept { get; set; }

        /// <summary>
        /// Gets a list of concept relationships
        /// </summary>
        [XmlElement("relationship"), JsonProperty("relationship")]
        public List<ConceptRelationship> Relationships { get; set; }

        /// <summary>
        /// True if concept is empty
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return String.IsNullOrEmpty(this.Mnemonic);
        }

        /// <summary>
        /// Gets or sets the class identifier
        /// </summary>

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("conceptClass"), JsonProperty("conceptClass")]
        [Binding(typeof(ConceptClassKeys))]
        public Guid? ClassKey { get; set; }

        /// <summary>
        /// Gets or sets the classification of the concept
        /// </summary>
        [SerializationReference(nameof(ClassKey))]
        [XmlIgnore, JsonIgnore]
        public ConceptClass Class { get; set; }

        /// <summary>
        /// Gets a list of concept reference terms
        /// </summary>
        [XmlElement("referenceTerm"), JsonProperty("referenceTerm")]
        public List<ConceptReferenceTerm> ReferenceTerms { get; set; }

        /// <summary>
        /// Gets the concept names
        /// </summary>
        [XmlElement("name"), JsonProperty("name")]
        public List<ConceptName> ConceptNames { get; set; }

        /// <summary>
        /// Concept sets as identifiers for XML purposes only
        /// </summary>
        [XmlElement("conceptSet"), JsonProperty("conceptSet")]
        public List<Guid> ConceptSetsXml { get; set; }

        /// <summary>
        /// Gets concept sets to which this concept is a member
        /// </summary>
        [SerializationMetadata, XmlIgnore, JsonIgnore, SerializationReference(nameof(ConceptSetsXml))]
        public List<ConceptSet> ConceptSets
        {
            get
            {
                return this.ConceptSetsXml?.Select(o => EntitySource.Current.Get<ConceptSet>(o)).ToList();
            }
            set
            {
                this.ConceptSetsXml = value?.Where(o => o.Key.HasValue).Select(o => o.Key.Value).ToList();
            }
        }

        /// <summary>
        /// Override string
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} [M: {1}]", base.ToString(), this.Mnemonic);
        }

        /// <summary>
        /// Determine equality
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Concept;
            if (other == null)
            {
                return false;
            }

            return base.SemanticEquals(obj) && other.Mnemonic == this.Mnemonic &&
                this.ClassKey == other.ClassKey &&
                this.ConceptNames?.SemanticEquals(other.ConceptNames) != false &&
                this.ConceptSets?.SemanticEquals(other.ConceptSets) != false &&
                this.Relationships?.SemanticEquals(other.Relationships) != false;
        }

        /// <summary>
        /// Represent as a display string
        /// </summary>
        public override string ToDisplay()
        {
            return this.LoadCollection<ConceptName>("ConceptNames")?.FirstOrDefault().Name;
        }
    }
}