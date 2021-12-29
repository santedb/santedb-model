using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Entities
{
    /// <summary>
    /// Represents an entity which is alive, but not a person
    /// </summary>
    /// <remarks>
    /// <para>In SanteDB a non-person living subject is used to represent entities such as parasites, viruses,
    /// bacteria, plants, etc. The primary use of this class is to capture information in relation
    /// to infections, protections or components of other entities.</para>
    /// </remarks>
    [XmlType(nameof(NonPersonLivingSubject), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(NonPersonLivingSubject), Namespace = "http://santedb.org/model")]
    [JsonObject(nameof(NonPersonLivingSubject))]
    [ClassConceptKey(EntityClassKeyStrings.LivingSubject)]
    [ClassConceptKey(EntityClassKeyStrings.Food)]
    [ClassConceptKey(EntityClassKeyStrings.Animal)]
    public class NonPersonLivingSubject : Entity
    {

        /// <summary>
        /// Living subject
        /// </summary>
        public NonPersonLivingSubject()
        {
            this.m_classConceptKey = EntityClassKeys.LivingSubject;
        }

        /// <inheritdoc/>
        protected override bool ValidateClassKey(Guid? classKey) => classKey == EntityClassKeys.LivingSubject || classKey == EntityClassKeys.Animal || classKey == EntityClassKeys.Food;


        /// <summary>
        /// Gets the description of the strain
        /// </summary>
        [XmlElement("strain"), JsonProperty("strain")]
        public Guid? StrainKey
        {
            get;set;
        }

        /// <summary>
        /// Strain
        /// </summary>
        [SerializationReference(nameof(StrainKey)), XmlIgnore, JsonIgnore]
        public Concept Strain
        {
            get;set;
        }
    }
}