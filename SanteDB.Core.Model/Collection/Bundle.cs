/*
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors (See NOTICE.md)
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
 * Date: 2021-2-9
 */
using Newtonsoft.Json;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Roles;
using SanteDB.Core.Model.Security;
using SanteDB.Core.Model.Serialization;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Collection
{
    /// <summary>
    /// A bundle represents a batch of objects which are included within the bundle
    /// </summary>
    [ResourceCollection]
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
    [XmlInclude(typeof(ControlAct))]
    [XmlInclude(typeof(Account))]
    [XmlInclude(typeof(InvoiceElement))]
    [XmlInclude(typeof(FinancialContract))]
    [XmlInclude(typeof(FinancialTransaction))]
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
    [XmlInclude(typeof(ConceptRelationship))]
    [XmlInclude(typeof(ConceptRelationshipType))]
    [XmlInclude(typeof(SecurityUser))]
    [XmlInclude(typeof(SecurityProvenance))]
    [XmlInclude(typeof(SecurityRole))]
    [XmlInclude(typeof(SecurityChallenge))]
    [XmlInclude(typeof(CodeSystem))]
    public class Bundle : IdentifiedData
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
        /// Create new bundle
        /// </summary>
        public Bundle(IEnumerable<IdentifiedData> objects)
        {
            this.Item = new List<IdentifiedData>(objects);
            this.FocalObjects = new List<Guid>(objects.Select(o => o.Key).OfType<Guid>());
        }

        // Property cache
        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> m_propertyCache = new ConcurrentDictionary<System.Type, IEnumerable<PropertyInfo>>();

        // Bundle contents
        private HashSet<String> m_bundleTags = new HashSet<string>(); // hashset of all tags

        // Modified now
        private DateTimeOffset m_modifiedOn = DateTime.Now;

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
            if (this.FocalObjects.Count == 1)
            {
                return this.Item.FirstOrDefault(o => o.Key == this.FocalObjects.FirstOrDefault());
            }
            return null;
        }

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
        public int TotalResults { get; set; }

        /// <summary>
        /// Add item to the bundle
        /// </summary>
        public void Add(IdentifiedData data)
        {
            if (data == null) return;
            this.Item.Add(data);
            if (!String.IsNullOrEmpty(data.Tag))
                this.m_bundleTags.Add(data.Tag);
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
            if (data == null) return;
            this.Item.Insert(index, data);
            if (!String.IsNullOrEmpty(data.Tag))
                this.m_bundleTags.Add(data.Tag);
        }

        /// <summary>
        /// True if the bundle has a tag
        /// </summary>
        public bool HasTag(String tag) => this.m_bundleTags.Contains(tag);

        /// <summary>
        /// Create a bundle
        /// </summary>
        public static Bundle CreateBundle(IdentifiedData resourceRoot, bool followList = true)
        {
            if (resourceRoot is Bundle) return resourceRoot as Bundle;
            Bundle retVal = new Bundle();
            retVal.Key = Guid.NewGuid();
            retVal.Count = retVal.TotalResults = 1;
            if (resourceRoot == null)
            {
                return retVal;
            }
            retVal.FocalObjects.Add(resourceRoot.Key.Value);
            retVal.Add(resourceRoot);
            ProcessModel(resourceRoot, retVal, followList);
            return retVal;
        }

        /// <summary>
        /// Create a bundle
        /// </summary>
        public static Bundle CreateBundle(IEnumerable<IdentifiedData> resourceRoot, int totalResults, int offset)
        {
            Bundle retVal = new Bundle();
            retVal.Key = Guid.NewGuid();
            retVal.Count = resourceRoot.Count();
            retVal.Offset = offset;
            retVal.TotalResults = totalResults;
            if (resourceRoot == null)
                return retVal;

            // Resource root
            foreach (var itm in resourceRoot)
            {
                if (itm == null)
                    continue;
                if (!retVal.HasTag(itm.Tag) && itm.Key.HasValue)
                {
                    retVal.Add(itm);
                    Bundle.ProcessModel(itm as IdentifiedData, retVal);
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
                return;
            context.Add(data);

            // Iterate over properties
            foreach (var pi in data.GetType().GetRuntimeProperties().Where(o => o.GetCustomAttribute<DataIgnoreAttribute>() == null))
            {

                // Is this property not null? If so, we want to iterate
                object value = pi.GetValue(data);
                if (value is IList listValue)
                {
                    foreach (var itm in listValue)
                    {
                        if (itm is IdentifiedData identifiedData)
                        {
                            this.Reconstitute(identifiedData, context);
                        }
                    }
                }
                else if (value is IdentifiedData identifiedData1)
                {
                    this.Reconstitute(identifiedData1, context);
                }

                // Is the pi a delay load? if so then get the key property
                var keyPi = pi.GetSerializationRedirectProperty();
                if (keyPi == null || (keyPi.PropertyType != typeof(Guid) &&
                    keyPi.PropertyType != typeof(Guid?)))
                    continue; // Invalid key link name

                // Get the key and find a match
                var key = (Guid?)keyPi.GetValue(data);
                if (key == null)
                    continue;
                var bundleItem = this.Item.Find(o => o.Key == key);
                if (bundleItem != null)
                {
                    pi.SetValue(data, bundleItem);
                }

            }

            context.Remove(data);

        }

        /// <summary>
        /// Packages the objects into a bundle
        /// </summary>
        public static void ProcessModel(IdentifiedData model, Bundle currentBundle, bool followList = true)
        {
            try
            {

                // Iterate over properties
                IEnumerable<PropertyInfo> properties = null;
                if (!m_propertyCache.TryGetValue(model.GetType(), out properties))
                {
                    properties = model.GetType().GetRuntimeProperties().Where(p => p.GetCustomAttribute<SerializationReferenceAttribute>() != null ||
                        typeof(IList).IsAssignableFrom(p.PropertyType) && p.GetCustomAttributes<XmlElementAttribute>().Count() > 0 && followList).ToList();

                    m_propertyCache.TryAdd(model.GetType(), properties);
                }

                currentBundle.m_modifiedOn = DateTimeOffset.Now;
                foreach (var pi in properties)
                {
                    try
                    {
                        object rawValue = pi.GetValue(model);
                        if (rawValue == null) continue;


                        if (rawValue is IList && followList)
                        {
                            foreach (var itm in rawValue as IList)
                            {

                                var iValue = itm as IdentifiedData;
                                if (iValue != null)
                                {
                                    if (currentBundle.Item.Exists(o => o?.Tag == iValue?.Tag))
                                        continue;

                                    if (pi.GetCustomAttribute<XmlIgnoreAttribute>() != null)
                                    {
                                        if (!currentBundle.HasTag(iValue.Tag) && iValue.Key.HasValue)
                                        {
                                            currentBundle.Insert(0, iValue);
                                        }
                                    }
                                    ProcessModel(iValue, currentBundle, false);
                                }
                            }
                        }
                        else if (rawValue is IdentifiedData)
                        {
                            var iValue = rawValue as IdentifiedData;

                            // Check for existing item
                            if (iValue != null && !currentBundle.HasTag(iValue.Tag))
                            {
                                if (pi.GetCustomAttribute<XmlIgnoreAttribute>() != null && iValue != null)
                                {
                                    if (!currentBundle.HasTag(iValue.Tag) && iValue.Key.HasValue)
                                    {
                                        currentBundle.Insert(0, iValue);
                                    }
                                }
                                ProcessModel(iValue, currentBundle, followList);
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

    }
}