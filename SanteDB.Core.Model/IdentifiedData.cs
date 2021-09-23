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
    /// Identifies the state of loading of the object from persistence
    /// </summary>
    public enum LoadState
    {
        /// <summary>
        /// Newly created, not persisted, no data loaded
        /// </summary>
        New = 0,
        /// <summary>
        /// Object was partially loaded meaning some properties are not populated
        /// </summary>
        PartialLoad = 1,
        /// <summary>
        /// The object was fully loaded
        /// </summary>
        FullLoad = 2
    }

    /// <summary>
    /// Represents data that is identified by a key
    /// </summary>
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

        // True when the data class is locked for storage
        private bool m_delayLoad = false;

        // Type id
        private string m_typeId = String.Empty;

        // Load state
        private LoadState m_loadState = LoadState.New;

        /// <summary>
        /// True if the class is currently loading associations when accessed
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public bool IsDelayLoadEnabled
        {
            get
            {
                return this.m_delayLoad;
            }
        }

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
        [DataIgnore, XmlIgnore, JsonProperty("$type")]
        public virtual String Type
        {
            get
            {
                if (String.IsNullOrEmpty(this.m_typeId))
                    this.m_typeId = this.GetType().GetCustomAttribute<JsonObjectAttribute>().Id;
                return this.m_typeId;
            }
            set { }
        }

        /// <summary>
        /// Get associated entity
        /// </summary>
        protected TEntity DelayLoad<TEntity>(Guid? keyReference, TEntity currentInstance) where TEntity : IdentifiedData, new()
        {
            //if (currentInstance == null &&
            //    this.m_delayLoad &&
            //    keyReference.HasValue)
            //{
            //    //Debug.WriteLine("Delay loading key reference: {0}>{1}", this.Key, keyReference);
            //    currentInstance = EntitySource.Current.Get<TEntity>(keyReference.Value);
            //}
            //currentInstance?.SetDelayLoad(this.IsDelayLoadEnabled);
            return currentInstance;
        }

        /// <summary>
        /// Gets or sets date/time that this object was last created or modified
        /// </summary>
        [XmlElement("modifiedOn"), JsonProperty("modifiedOn"), DataIgnore]
        public abstract DateTimeOffset ModifiedOn { get; }

        /// <summary>
        /// Never serialize modified on
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeModifiedOn()
        {
            return false;
        }

        /// <summary>
        /// Gets a tag which changes whenever the object is updated
        /// </summary>
        [XmlIgnore, JsonIgnore, DataIgnore]
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
        /// <returns></returns>
        public virtual bool IsEmpty() => false;

        /// <summary>
        /// Gets or sets whether the object was partial loaded
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public LoadState LoadState
        {
            get
            {
                return this.m_loadState;
            }
            set
            {
                if (value >= this.m_loadState)
                    this.m_loadState = value;
            }
        }

        /// <summary>
        /// Clone the specified data
        /// </summary>
        public virtual IdentifiedData Clone()
        {
            var retVal = this.MemberwiseClone() as IdentifiedData;
            retVal.m_delayLoad = true;
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
        /// Clone the specified data
        /// </summary>
        public virtual IdentifiedData GetLocked()
        {
            var retVal = this.MemberwiseClone() as IdentifiedData;
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
        public void RemoveAnnotation(Object annotation)
        {
            lock (this.m_lock)
            {
                this.m_annotations.Remove(annotation);
            }
        }

        /// <summary>
        /// Get annotations of specified <typeparamref name="T"/>
        /// </summary>
        public IEnumerable<T> GetAnnotations<T>()
        {
            lock (this.m_lock)
            {
                return this.m_annotations.ToArray().OfType<T>();
            }
        }

        /// <summary>
        /// Add an annotated object
        /// </summary>
        public void AddAnnotation(Object annotation)
        {
            lock (this.m_lock)
            {
                this.m_annotations.Add(annotation);
            }
        }
    }
}
