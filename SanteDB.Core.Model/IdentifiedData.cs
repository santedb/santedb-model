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
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// A class which represent data which has a keyed identifier
    /// </summary>
    /// <remarks><para>The IdentifiedData class is the root of the SanteDB business object model, 
    /// and is the class from which all other business object model instances are derived.
    /// </para><para>This class contains </para></remarks>
    [XmlType("IdentifiedData", Namespace = "http://santedb.org/model"), JsonObject("IdentifiedData")]
    public abstract class IdentifiedData : IIdentifiedEntity
    {
        // Annotations
        /// <summary>
        /// A list of custom tags which were added to this object
        /// </summary>
        protected List<Object> m_annotations = new List<object>();

        // Lock
        private object m_lock = new object();

        // Type id
        private string m_typeId = String.Empty;

        /// <summary>
        /// Gets or sets the primary identifying UUID of this object
        /// </summary>
        [XmlElement("id"), JsonProperty("id")]
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the operation
        /// </summary>
        [XmlAttribute("operation"), JsonProperty("operation")]
        public BatchOperationType BatchOperation { get; set; }

        /// <summary>
        /// Should serialize batch operation
        /// </summary>
        public bool ShouldSerializeBatchOperation() => this.BatchOperation != BatchOperationType.Auto;

        /// <summary>
        /// True if key should be serialized
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeKey()
        {
            return this.Key.HasValue;
        }

        /// <summary>
        /// Gets the type registration of this object
        /// </summary>
        /// <remarks>The type is used in JSON serialization to provide a concise $type value on the serializer which 
        /// does not worry about the namespacing</remarks>
        [SerializationMetadata, XmlIgnore, JsonProperty("$type")]
        public virtual String Type
        {
            get
            {
                if (String.IsNullOrEmpty(this.m_typeId))
                    this.m_typeId = this.GetType().GetSerializationName();
                return this.m_typeId;
            }
            set { }
        }

        /// <summary>
        /// Gets or sets date/time that this object was last created or modified
        /// </summary>
        /// <remarks>Classes which extend this should expose the last modified date (for example: CreationTime or UpdatedTime) </remarks>
        [XmlElement("modifiedOn"), JsonProperty("modifiedOn"), SerializationMetadata]
        public abstract DateTimeOffset ModifiedOn { get; }

        /// <summary>
        /// Never serialize modified on
        /// </summary>
        public virtual bool ShouldSerializeModifiedOn() => false;

        /// <summary>
        /// Gets a tag which changes whenever the object is updated
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationMetadata]
        public virtual String Tag
        {
            get
            {
                return this.Key?.ToString("N");
            }
        }

        /// <summary>
        /// True if the object is empty
        /// </summary>
        public virtual bool IsEmpty() => false;

        /// <summary>
        /// Clone the specified data
        /// </summary>
        public virtual IdentifiedData Clone()
        {
            var retVal = this.MemberwiseClone() as IdentifiedData;
            retVal.m_annotations = new List<object>();

            // Re-initialize all arrays
            foreach (var pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var thisValue = pi.GetValue(this);
                if (thisValue is IList listValue && listValue.Count > 0 &&
                    pi.PropertyType.GetConstructor(new Type[] { thisValue.GetType() }) != null)
                {
                    pi.SetValue(retVal, Activator.CreateInstance(pi.PropertyType, pi.GetValue(this)));
                }
            }
            return retVal;
        }

        /// <summary>
        /// Determines the semantic equality of this object an <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object to which the semantic equality should be evaluated</param>
        /// <returns>True if this object is semantically the same as <paramref name="obj"/></returns>
        /// <remarks>
        /// In SanteDB's data model, an object is semantically equal when the two objects clinically mean the
        /// same thing. This differs from reference equality (when two objects are the same instance) and value equality
        /// (when two objects carry all the same values). For example, two <see cref="ActParticipation"/> instances may
        /// be semantically equal when they both represent the same entity playing the same role in the same act as one another,
        /// even though their keys and effective / obsolete version properties may be different.
        /// </remarks>
        public virtual bool SemanticEquals(object obj)
        {
            var other = obj as IdentifiedData;
            if (other == null)
                return false;
            return this.Type == other.Type;
        }

        /// <summary>
        /// To display value
        /// </summary>
        public virtual String ToDisplay()
        {
            return this.Key.ToString();
        }

        /// <summary>
        /// Quality Comparer
        /// </summary>
        public class SemanticEqualityComparer<T> : IEqualityComparer<T>
            where T : IdentifiedData
        {
            /// <summary>
            /// Equality
            /// </summary>
            public bool Equals(T x, T y)
            {
                return x.SemanticEquals(y);
            }

            /// <summary>
            /// Get comparison hash code
            /// </summary>
            public int GetHashCode(T obj)
            {
                return obj.Key.GetHashCode();
            }
        }

        /// <summary>
        /// Remove annotation
        /// </summary>
        public virtual void RemoveAnnotation(Object annotation)
        {
            lock (this.m_lock)
            {
                this.m_annotations.Remove(annotation);
            }
        }

        /// <summary>
        /// Get annotations of specified <typeparamref name="T"/>
        /// </summary>
        public virtual IEnumerable<T> GetAnnotations<T>()
        {
            lock (this.m_lock)
            {
                return this.m_annotations.ToArray().OfType<T>();
            }
        }

        /// <summary>
        /// Add an annotated object
        /// </summary>
        public virtual void AddAnnotation(Object annotation)
        {
            lock (this.m_lock)
            {
                this.m_annotations.Add(annotation);
            }
        }

        /// <summary>
        /// Copy annotations from another resource
        /// </summary>
        public virtual IIdentifiedEntity CopyAnnotations(IIdentifiedEntity other)
        {
            if (other is IdentifiedData id)
            {
                this.m_annotations.AddRange(id.m_annotations);
            }
            return this;
        }
    }
}