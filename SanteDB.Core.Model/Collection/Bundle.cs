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
using Newtonsoft.Json;
using SanteDB.Core.i18n;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Roles;
using SanteDB.Core.Model.Security;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Collection
{
    /// <summary>
    /// A bundle represents a batch of objects which are included within the bundle
    /// </summary>
    [AddDependentSerializers]
    [XmlType(nameof(Bundle), Namespace = "http://santedb.org/model")]
    [XmlRoot(nameof(Bundle), Namespace = "http://santedb.org/model")]
    [JsonObject("Bundle")]
    [XmlInclude(typeof(Concept))]
    [XmlInclude(typeof(ReferenceTerm))]
    [XmlInclude(typeof(Act))]
    [XmlInclude(typeof(TextObservation))]
    [XmlInclude(typeof(ConceptSet))]
    [XmlInclude(typeof(CodedObservation))]
    [XmlInclude(typeof(QuantityObservation))]
    [XmlInclude(typeof(PatientEncounter))]
    [XmlInclude(typeof(ExtensionType))]
    [XmlInclude(typeof(SubstanceAdministration))]
    [XmlInclude(typeof(UserEntity))]
    [XmlInclude(typeof(ApplicationEntity))]
    [XmlInclude(typeof(DeviceEntity))]
    [XmlInclude(typeof(Entity))]
    [XmlInclude(typeof(Patient))]
    [XmlInclude(typeof(AssigningAuthority))]
    [XmlInclude(typeof(ControlAct))]
    [XmlInclude(typeof(Account))]
    [XmlInclude(typeof(InvoiceElement))]
    [XmlInclude(typeof(DateObservation))]
    [XmlInclude(typeof(FinancialContract))]
    [XmlInclude(typeof(FinancialTransaction))]
    [XmlInclude(typeof(ConceptReferenceTerm))]
    [XmlInclude(typeof(Procedure))]
    [XmlInclude(typeof(Provider))]
    [XmlInclude(typeof(Organization))]
    [XmlInclude(typeof(TemplateDefinition))]
    [XmlInclude(typeof(Protocol))]
    [XmlInclude(typeof(Place))]
    [XmlInclude(typeof(Material))]
    [XmlInclude(typeof(ManufacturedMaterial))]
    [XmlInclude(typeof(CarePlan))]
    [XmlInclude(typeof(DeviceEntity))]
    [XmlInclude(typeof(ApplicationEntity))]
    [XmlInclude(typeof(DeviceEntity))]
    [XmlInclude(typeof(Bundle))]
    [XmlInclude(typeof(ConceptClass))]
    [XmlInclude(typeof(Container))]
    [XmlInclude(typeof(ConceptRelationship))]
    [XmlInclude(typeof(ConceptRelationshipType))]
    [XmlInclude(typeof(SecurityUser))]
    [XmlInclude(typeof(SecurityProvenance))]
    [XmlInclude(typeof(SecurityRole))]
    [XmlInclude(typeof(SecurityChallenge))]
    [XmlInclude(typeof(RelationshipValidationRule))]
    [XmlInclude(typeof(CodeSystem))]
    public class Bundle : IdentifiedData, IResourceCollection
    {
        /// <summary>
        /// Create new bundle
        /// </summary>
        public Bundle()
        {
            this.Item = new List<IdentifiedData>();
            this.FocalObjects = new List<Guid>();
        }

        /// <summary>
        /// Represent the bundle as
        /// </summary>
        public Bundle(IEnumerable<IdentifiedData> objects, int offset, int? total) : this(objects)
        {
            this.Offset = offset;
            this.TotalResults = total;
        }

        /// <summary>
        /// Create new bundle
        /// </summary>
        public Bundle(IEnumerable<IdentifiedData> objects)
        {
            this.Item = new List<IdentifiedData>(objects);
            this.Count = this.Item.Count;
            this.FocalObjects = new List<Guid>(this.Item.Select(o => o.Key).OfType<Guid>());
        }

        // Property cache
        private static ConcurrentDictionary<Type, PropertyInfo[]> m_propertyCache = new ConcurrentDictionary<System.Type, PropertyInfo[]>();

        // Bundle contents
        private HashSet<String> m_bundleTags = new HashSet<string>(); // hashset of all tags

        // Modified now
        private DateTimeOffset m_modifiedOn = DateTimeOffset.Now;
        private Guid? m_correlationKey = null;

        /// <summary>
        /// Gets the time the bundle was modified
        /// </summary>
        public override DateTimeOffset ModifiedOn
        {
            get
            {
                return this.m_modifiedOn;
            }
        }

        /// <summary>
        /// Gets the single focal object for this object
        /// </summary>
        public IdentifiedData GetFocalObject()
        {
            if (!this.FocalObjects.IsNullOrEmpty() && this.FocalObjects.Count == 1)
            {
                return this.Item.FirstOrDefault(o => o.Key == this.FocalObjects.FirstOrDefault());
            }
            return null;
        }

        /// <summary>
        /// Gets or sets the sequence control for this bundle
        /// </summary>
        [XmlElement("correlationSeq"), JsonProperty("correlationSeq")]
        public long? CorrelationSequence { get; set; }

        /// <inheritdoc/>
        public bool ShouldSerializeCorrelationSequence() => this.CorrelationSequence.HasValue;

        /// <summary>
        /// A unique identifier which correlates this bundle with other bundles that are about the same "subject"
        /// </summary>
        [XmlElement("correlationId"), JsonProperty("correlationId")]
        public Guid? CorrelationKey {
            get => this.m_correlationKey;
            set {
                if (!this.m_correlationKey.HasValue)
                {
                    this.m_correlationKey = value;
                    this.CorrelationSequence = this.CorrelationSequence ?? DateTime.Now.Ticks;
                }
                else if(!value.HasValue)
                {
                    this.CorrelationSequence = null;
                    this.m_correlationKey = null;
                }
                else if(!value.Equals(this.m_correlationKey))
                {
                    throw new InvalidOperationException(String.Format(ErrorMessages.WOULD_RESULT_INVALID_STATE, nameof(CorrelationKey)));
                }
            }
        }

        /// <inheritdoc/>
        public bool ShouldSerializeCorrelationKey() => this.CorrelationKey.HasValue;

        /// <summary>
        /// Gets or sets items in the bundle
        /// </summary>
        [XmlElement("resource"), JsonProperty("resource")]
        public List<IdentifiedData> Item { get; set; }

        /// <summary>
        /// Entry into the bundle
        /// </summary>
        [XmlElement("focal"), JsonProperty("focal")]
        public List<Guid> FocalObjects { get; set; }

        /// <summary>
        /// Gets or sets the count in this bundle
        /// </summary>
        [XmlElement("offset"), JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the count in this bundle
        /// </summary>
        [XmlElement("count"), JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the total results
        /// </summary>
        [XmlElement("totalResults"), JsonProperty("totalResults")]
        public int? TotalResults { get; set; }

        /// <summary>
        /// Generic resource entity
        /// </summary>
        [JsonIgnore, XmlIgnore]
        IEnumerable<IIdentifiedResource> IResourceCollection.Item => this.Item;

        /// <summary>
        /// Add item to the bundle
        /// </summary>
        public void Add(IdentifiedData data)
        {
            if (data == null)
            {
                return;
            }

            this.Item.Add(data);
            if (!String.IsNullOrEmpty(data.Tag))
            {
                this.m_bundleTags.Add(data.Tag);
            }
        }

        /// <summary>
        /// Add range of items
        /// </summary>
        public void AddRange(IEnumerable<IdentifiedData> items)
        {
            foreach (var itm in items)
            {
                if (!this.m_bundleTags.Contains(itm.Tag))
                {
                    this.Item.Add(itm);
                }
            }
        }

        /// <summary>
        /// Insert data at the specified index
        /// </summary>
        public void Insert(int index, IdentifiedData data)
        {
            if (data == null)
            {
                return;
            }

            this.Item.Insert(index, data);
            if (!String.IsNullOrEmpty(data.Tag))
            {
                this.m_bundleTags.Add(data.Tag);
            }
        }

        /// <summary>
        /// True if the bundle has a tag
        /// </summary>
        public bool HasTag(String tag) => this.m_bundleTags.Contains(tag);

        /// <summary>
        /// Create a bundle
        /// </summary>
        public static Bundle CreateBundle(IdentifiedData resourceRoot, bool followList = true) => CreateBundle(new IdentifiedData[] { resourceRoot }, 1, 1);

        /// <summary>
        /// Create a bundle
        /// </summary>
        public static Bundle CreateBundle(IEnumerable<IdentifiedData> resourceRoot, int totalResults, int offset, PropertyInfo[] propertiesToInclude = null, PropertyInfo[] propertiesToExclude = null)
        {
            Bundle retVal = new Bundle();
            retVal.Key = Guid.NewGuid();
            if (resourceRoot.Count() == 1)
            {
                retVal.CorrelationSequence = DateTime.Now.Ticks;
                retVal.CorrelationKey = resourceRoot.First()?.Key;
            }
            retVal.Count = resourceRoot.Count();
            retVal.Offset = offset;

            retVal.TotalResults = totalResults;
            if (resourceRoot == null)
            {
                return retVal;
            }

            // Resource root
            foreach (var itm in resourceRoot)
            {
                if (itm == null)
                {
                    continue;
                }

                if (!retVal.HasTag(itm.Tag) && itm.Key.HasValue)
                {
                    retVal.Add(itm);
                    retVal.FocalObjects.Add(itm.Key.Value);
                    Bundle.ProcessModel(itm as IdentifiedData, retVal, true, propertiesToInclude ?? new PropertyInfo[0], propertiesToExclude ?? new PropertyInfo[0]);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Remove the specified keyed object
        /// </summary>
        public void Remove(Guid key)
        {
            var itm = this.Item.Find(o => o.Key == key);
            if (itm != null)
            {
                this.Item.Remove(itm);
                this.m_bundleTags.Remove(itm.Tag);
            }
        }

        /// <summary>
        /// Reconstitutes the bundle (takes the flat reference structures and fills them out into proper object references)
        /// </summary>
        public void Reconstitute()
        {
            HashSet<IdentifiedData> context = new HashSet<IdentifiedData>();
            foreach (var itm in this.Item)
            {
                this.Reconstitute(itm, context);
            }
        }

        /// <summary>
        /// Re-constitute the data
        /// </summary>
        /// <remarks>Basically this will find any refs and fill them in</remarks>
        private void Reconstitute(IdentifiedData data, HashSet<IdentifiedData> context)
        {
            if (context.Contains(data))
            {
                return;
            }

            context.Add(data);

            // Iterate over properties
            foreach (var pi in data.GetType().GetRuntimeProperties().Where(o => o.GetCustomAttribute<SerializationMetadataAttribute>() == null))
            {
                // Is this property not null? If so, we want to iterate

                // Is the pi a delay load? if so then get the key property
                var keyPi = pi.GetSerializationRedirectProperty();
                if (keyPi == null || (keyPi.PropertyType != typeof(Guid) &&
                    keyPi.PropertyType != typeof(Guid?)))
                {
                    continue; // Invalid key link name
                }

                // Get the key and find a match
                var key = (Guid?)keyPi.GetValue(data);
                if (key == null)
                {
                    continue;
                }

                var bundleItem = this.Item.Find(o => o.Key == key);
                if (bundleItem != null)
                {
                    pi.SetValue(data, bundleItem);
                }
            }

            if(data is IHasRelationships ihr && ihr.Relationships != null)
            {
                foreach (var item in ihr.Relationships)
                {
                    if(item.TargetEntity == null && item.TargetEntityKey.HasValue && item.TargetEntityKey != data.Key)
                    {
                        item.TargetEntity = this.Item.Find(o => o.Key == item.TargetEntityKey);
                    }
                    if(item.SourceEntity == null && item.SourceEntityKey.HasValue && item.SourceEntityKey != data.Key)
                    {
                        item.SourceEntity = this.Item.Find(o => o.Key == item.SourceEntityKey);
                    }
                }
            }
            if(data is Act act && act.Participations != null)
            {
                foreach(var ptcpt in act.Participations)
                {
                    if(ptcpt.PlayerEntity == null && ptcpt.PlayerEntityKey.HasValue)
                    {
                        ptcpt.PlayerEntity = (Entity)this.Item.Find(o => o.Key == ptcpt.PlayerEntityKey);
                    }
                }
            }

            context.Remove(data);
        }

        /// <summary>
        /// Packages the objects into a bundle
        /// </summary>
        private static void ProcessModel(IdentifiedData model, Bundle currentBundle, bool followList, PropertyInfo[] propertiesToInclude, PropertyInfo[] propertiesToExclude)
        {
            try
            {
                // Iterate over properties
                PropertyInfo[] properties = propertiesToInclude.Union(propertiesToExclude).ToArray();
                if (properties.Length == 0 && !m_propertyCache.TryGetValue(model.GetType(), out properties))
                {
                    properties = model.GetType().GetRuntimeProperties().Where(p => p.GetCustomAttribute<SerializationReferenceAttribute>() != null && p.GetCustomAttribute<XmlIgnoreAttribute>() != null ||
                        typeof(IList).IsAssignableFrom(p.PropertyType) && p.HasCustomAttribute<XmlElementAttribute>() && followList).ToArray();
                    m_propertyCache.TryAdd(model.GetType(), properties);
                }

                currentBundle.m_modifiedOn = DateTimeOffset.Now;

                foreach (var pi in properties.Where(o => o.DeclaringType.IsAssignableFrom(model.GetType())))
                {
                    try
                    {
                        object rawValue = pi.GetValue(model);
                        if (propertiesToExclude.Contains(pi) || pi == null)
                        {
                            pi.SetValue(model, null);
                            continue;
                        }
                        else if (rawValue == null)
                        {
                            rawValue = model.LoadProperty(pi.Name);
                        }

                        if (rawValue is IList listValue && followList)
                        {
                            foreach (var itm in listValue.OfType<IdentifiedData>())
                            {
                                if (currentBundle.HasTag(itm.Tag)) // bundle already has this item
                                {
                                    continue;
                                }
                                else if (pi.GetCustomAttribute<XmlIgnoreAttribute>() != null) // won't be serialized so we need to add to bundle
                                {
                                    currentBundle.Insert(0, itm);
                                    ProcessModel(itm, currentBundle, false, propertiesToInclude, propertiesToExclude);
                                }
                            }
                        }
                        else if (rawValue is IdentifiedData ident)
                        {
                            // Check for existing item
                            if (!currentBundle.HasTag(ident.Tag) && pi.GetCustomAttribute<XmlIgnoreAttribute>() != null) // won't be serialized
                            {
                                currentBundle.Insert(0, ident);
                                ProcessModel(ident, currentBundle, followList, propertiesToInclude, propertiesToExclude);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Instance error: {0}", e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: {0}", e);
            }
        }

        /// <summary>
        /// Gets from the item set only those items which are results
        /// </summary>
        public IEnumerable<IdentifiedData> GetFocalItems()
        {
            return this.Item.Where(o => this.FocalObjects.Contains(o.Key.Value));
        }

        /// <summary>
        /// Add annotation
        /// </summary>
        public void AddAnnotationToAll(object annotation)
        {
            base.AddAnnotation(annotation);
            foreach (var itm in this.Item)
            {
                itm.AddAnnotation(annotation);
            }
        }

        /// <inheritdoc/>
        public override ICanDeepCopy DeepCopy() => this.CloneDeep();
    }
}