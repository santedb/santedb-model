/*
 * Copyright (C) 2021 - 2021, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2021-8-5
 */

using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.DataTypes;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Acts
{
    /// <summary>
    /// Represents a act (<see cref="Act"/>) which is an observation
    /// </summary>
    /// <remarks>
    /// <para>
    /// The observation class itself is an abstract class which is generically used to represent something that is observed about a patient.
    /// </para>
    /// <para>
    /// It is not recommended to use this class directly, rather one of its sub classes based on the type of observation being made such as:
    /// </para>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Observation</term>
    ///         <term>Type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>Coded</term>
    ///         <term><see cref="CodedObservation"/></term>
    ///         <term>Observations whose values are codified (example: blood type, presentation, etc.)</term>
    ///     </item>
    ///     <item>
    ///         <term>Quantity</term>
    ///         <term><see cref="QuantityObservation"/></term>
    ///         <term>Observations whose values are codified (example: blood type, presentation, etc.)</term>
    ///     </item>
    ///     <item>
    ///         <term>Text</term>
    ///         <term><see cref="TextObservation"/></term>
    ///         <term>Observations whose values are codified (example: blood type, presentation, etc.)</term>
    ///     </item>
    /// </list>
    /// <para>
    /// No matter what type of value an observation carries (coded, quantity, text) it is always classified by the type concept (<see cref="Act.TypeConceptKey"/>).
    /// </para>
    /// </remarks>
    [XmlType("Observation", Namespace = "http://santedb.org/model"), JsonObject("Observation")]
    [XmlRoot("Observation", Namespace = "http://santedb.org/model")]
    [ClassConceptKey(ActClassKeyStrings.Observation)]
    public class Observation : Act
    {
        /// <summary>
        /// Observation ctor
        /// </summary>
        public Observation()
        {
            this.ClassConceptKey = ActClassKeys.Observation;
        }

        /// <summary>
        /// Gets or sets the interpretation concept
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [XmlElement("interpretationConcept"), JsonProperty("interpretationConcept")]
        public Guid? InterpretationConceptKey { get; set; }

        /// <summary>
        /// Value type
        /// </summary>
        [XmlElement("valueType"), JsonProperty("valueType")]
        public virtual String ValueType
        { get { return "NA"; } set { } }

        /// <summary>
        /// Gets or sets the concept which indicates the interpretation of the observtion
        /// </summary>
        [SerializationReference(nameof(InterpretationConceptKey))]
        [XmlIgnore, JsonIgnore]
        public Concept InterpretationConcept { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as Observation;
            if (other == null) return false;
            return base.SemanticEquals(obj) && this.InterpretationConceptKey == other.InterpretationConceptKey;
        }

        /// <summary>
        /// Should serialize value type?
        /// </summary>
        public bool ShouldSerializeValueType() => false;
    }

    /// <summary>
    /// Represents an observation that contains a quantity
    /// </summary>
    /// <remarks>
    /// The quantity observation class should be used whenever you wish to store an observation which carries a numerical value
    /// and an optional unit of measure (example: length = 3.2 ft, weight = 1.2 kg, etc.)
    /// </remarks>
    [XmlType("QuantityObservation", Namespace = "http://santedb.org/model"), JsonObject("QuantityObservation")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "QuantityObservation")]
    public class QuantityObservation : Observation
    {
        /// <summary>
        /// Gets or sets the observed quantity
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public Decimal Value { get; set; }

        /// <summary>
        /// Value type
        /// </summary>
        [XmlElement("valueType"), JsonProperty("valueType")]
        public override string ValueType
        {
            get
            {
                return "PQ";
            }
            set { }
        }

        /// <summary>
        /// Gets or sets the key of the uom concept
        /// </summary>
        [XmlElement("unitOfMeasure"), JsonProperty("unitOfMeasure")]
        public Guid? UnitOfMeasureKey { get; set; }

        /// <summary>
        /// Gets or sets the unit of measure
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(UnitOfMeasureKey))]
        public Concept UnitOfMeasure { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as QuantityObservation;
            if (other == null) return false;
            return base.SemanticEquals(obj) && this.Value == other.Value && this.UnitOfMeasureKey == other.UnitOfMeasureKey;
        }
    }

    /// <summary>
    /// Represents an observation with a text value
    /// </summary>
    /// <remarks>
    /// The text observation type represents an observation made with a textual value. This is done whenever an observation type
    /// cannot be quantified or classified using either a coded or observed value. Please note that this type should not be used
    /// for taking notes, rather it is a specific type of thing observed about a patient. For example: Interpretation of patient's mood
    /// </remarks>
    [XmlType("TextObservation", Namespace = "http://santedb.org/model"), JsonObject("TextObservation")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "TextObservation")]
    public class TextObservation : Observation
    {
        /// <summary>
        /// Value type
        /// </summary>
        [XmlElement("valueType"), JsonProperty("valueType")]
        public override string ValueType
        {
            get
            {
                return "ED";
            }
            set { }
        }

        /// <summary>
        /// Gets or sets the textual value
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        public String Value { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as TextObservation;
            if (other == null) return false;
            return base.SemanticEquals(obj) && this.Value == other.Value;
        }
    }

    /// <summary>
    /// Represents an observation with a concept value
    /// </summary>
    /// <remarks>
    /// A coded observation represents an observation whose value is classified using a coded concept. For example: fetal presentation,
    /// stage of pregnancy, etc.
    /// </remarks>
    [XmlType("CodedObservation", Namespace = "http://santedb.org/model"), JsonObject("CodedObservation")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "CodedObservation")]
    public class CodedObservation : Observation
    {
        /// <summary>
        /// Value type
        /// </summary>
        [XmlElement("valueType"), JsonProperty("valueType")]
        public override string ValueType
        {
            get
            {
                return "CD";
            }
            set { }
        }

        /// <summary>
        /// Gets or sets the key of the uom concept
        /// </summary>
        [XmlElement("value"), JsonProperty("value")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Guid? ValueKey { get; set; }

        /// <summary>
        /// Gets or sets the coded value of the observation
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [SerializationReference(nameof(ValueKey))]
        public Concept Value { get; set; }

        /// <summary>
        /// Semantic equality function
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as CodedObservation;
            if (other == null) return false;
            return base.SemanticEquals(obj) && other.ValueKey == this.ValueKey;
        }
    }
}