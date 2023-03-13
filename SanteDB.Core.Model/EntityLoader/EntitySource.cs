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
using SanteDB.Core.Model.Interfaces;
using SanteDB.Core.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SanteDB.Core.Model.EntityLoader
{
    /// <summary>
    /// Delay loader class
    /// </summary>
    public sealed class EntitySource
    {
        /// <summary>
        /// Dummy entity source
        /// </summary>
        public class DummyEntitySource : IEntitySourceProvider
        {
            /// <summary>
            /// Gets the specified object
            /// </summary>
            public TObject Get<TObject>(Guid? key) where TObject : IdentifiedData, new()
            {
                return new TObject() { Key = key };
            }

            /// <summary>
            /// Gets the specified object
            /// </summary>
            public TObject Get<TObject>(Guid? key, Guid? versionKey) where TObject : IdentifiedData, new()
            {
                return new TObject() { Key = key };
            }

            /// <summary>
            /// Gets the specified relations
            /// </summary>
            public IEnumerable<TObject> GetRelations<TObject>(Guid? sourceKey, int? sourceVersionSequence) where TObject : IdentifiedData, IVersionedAssociation, new()
            {
                return new List<TObject>();
            }

            /// <summary>
            /// Gets the specified relations
            /// </summary>
            public IQueryResultSet<TObject> GetRelations<TObject>(params Guid?[] sourceKey) where TObject : IdentifiedData, ISimpleAssociation, new()
            {
                return new MemoryQueryResultSet<TObject>(new List<TObject>());
            }

            /// <inheritdoc/>
            public IQueryResultSet GetRelations(Type tobject, params Guid?[] sourceKey)
            {
                return new MemoryQueryResultSet(new Object[0]);

            }

            /// <summary>
            /// Query
            /// </summary>
            public IQueryResultSet<TObject> Query<TObject>(Expression<Func<TObject, bool>> query) where TObject : IdentifiedData, new()
            {
                return new MemoryQueryResultSet<TObject>(new TObject[0]);
            }
        }

        // Load object
        private static Object s_lockObject = new object();

        // Current instance
        private static EntitySource s_instance = new EntitySource(new DummyEntitySource());

        /// <summary>
        /// Delay load provider
        /// </summary>
        private IEntitySourceProvider m_provider;

        /// <summary>
        /// Delay loader ctor
        /// </summary>
        public EntitySource(IEntitySourceProvider provider)
        {
            m_provider = provider;
        }

        /// <summary>
        /// Gets the current delay loader
        /// </summary>
        public static EntitySource Current
        {
            get
            {
                return s_instance;
            }
            set
            {
                lock (s_lockObject)
                {
                    s_instance = value;
                }
            }
        }

        /// <summary>
        /// Get the specified object / version
        /// </summary>
        public TObject Get<TObject>(Guid? key, Guid? version) where TObject : IdentifiedData, IVersionedData, new()
        {
            if (key == null)
            {
                return null;
            }

            return this.m_provider.Get<TObject>(key, version);
        }

        /// <summary>
        /// Get the current version of the specified object
        /// </summary>
        public TObject Get<TObject>(Guid? key) where TObject : IdentifiedData, new()
        {
            if (key == null)
            {
                return null;
            }
            else
            {
                return this.m_provider.Get<TObject>(key);
            }
        }

        /// <summary>
        /// Gets the current entity source provider
        /// </summary>
        public IEntitySourceProvider Provider { get { return this.m_provider; } }
    }
}