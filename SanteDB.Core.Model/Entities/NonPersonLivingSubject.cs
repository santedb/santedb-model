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
    public class NonPersonLivingSubject : Entity
    {
        // Strain
        private Guid? m_strainKey;

        // The strain of the non-person living subject
        private Concept m_strain;

        /// <inheritdoc/>
        [XmlElement("classConcept"), JsonProperty("classConcept")]
        public override Guid? ClassConceptKey
        {
            get => base.ClassConceptKey;
            set
            {
                if (value == EntityClassKeys.Animal || value == EntityClassKeys.LivingSubject)
                {
                    base.ClassConceptKey = value;
                }
                else
                {
                    throw new InvalidOperationException($"Class concept {value} is no permitted in this context");
                }
            }
        }

        /// <summary>
        /// Gets the description of the strain
        /// </summary>
        [XmlElement("strain"), JsonProperty("strain")]
        public Guid? StrainKey
        {
            get => this.m_strainKey;
            set
            {
                this.m_strain = null;
                this.m_strainKey = value;
            }
        }

        /// <summary>
        /// Strain
        /// </summary>
        [SerializationReference(nameof(StrainKey))]
        [XmlIgnore, JsonIgnore, AutoLoad]
        public Concept Strain
        {
            get
            {
                this.m_strain = base.DelayLoad(this.m_strainKey, this.m_strain);
                return this.m_strain;
            }
            set
            {
                this.m_strain = value;
                this.m_strainKey = value?.Key;
            }
        }
    }
}