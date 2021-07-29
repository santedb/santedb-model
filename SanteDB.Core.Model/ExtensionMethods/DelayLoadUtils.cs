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
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model
{

    /// <summary>
    /// Delay load utilities
    /// </summary>
    public static class DelayLoadUtils
    {

        /// <summary>
        /// Load the person's languages
        /// </summary>
        public static IEnumerable<PersonLanguageCommunication> GetPersonLanguages(this Person me) => me.LoadCollection<PersonLanguageCommunication>(nameof(Person.LanguageCommunication)).AsResultSet();

        /// <summary>
        /// Gets or loads addresses attached to an entity
        /// </summary>
        public static IEnumerable<EntityAddress> GetAddresses(this Entity me) => me.LoadCollection<EntityAddress>(nameof(Entity.Addresses)).AsResultSet();

        /// <summary>
        /// Gets or loads names attached to an entity
        /// </summary>
        public static IEnumerable<EntityName> GetNames(this Entity me) => me.LoadCollection<EntityName>(nameof(Entity.Names)).AsResultSet();

        /// <summary>
        /// Gets or loads telecoms attached to an entity
        /// </summary>
        public static IEnumerable<EntityTelecomAddress> GetTelecoms(this Entity me) => me.LoadCollection<EntityTelecomAddress>(nameof(Entity.Telecoms)).AsResultSet();

        /// <summary>
        /// Gets or loads relationships
        /// </summary>
        public static IEnumerable<EntityRelationship> GetRelationships(this Entity me) => me.LoadCollection<EntityRelationship>(nameof(Entity.Relationships)).AsResultSet();

        /// <summary>
        /// Gets or loads identifiers
        /// </summary>
        public static IEnumerable<EntityIdentifier> GetIdentifiers(this Entity me) => me.LoadCollection<EntityIdentifier>(nameof(Entity.Identifiers)).AsResultSet();
        
        /// <summary>
        /// Gets or loads notes
        /// </summary>
        public static IEnumerable<EntityNote> GetNotes(this Entity me) => me.LoadCollection<EntityNote>(nameof(Entity.Notes)).AsResultSet();

        /// <summary>
        /// Gets or loads tags
        /// </summary>
        public static IEnumerable<EntityTag> GetTags(this Entity me) => me.LoadCollection<EntityTag>(nameof(Entity.Tags)).AsResultSet();

        /// <summary>
        /// Gets or loads extensions
        /// </summary>
        public static IEnumerable<EntityExtension> GetExtensions(this Entity me) => me.LoadCollection<EntityExtension>(nameof(Entity.Extensions)).AsResultSet();

        /// <summary>
        /// Gets or loads identifiers
        /// </summary>
        public static IEnumerable<ActIdentifier> GetIdentifiers(this Act me) => me.LoadCollection<ActIdentifier>(nameof(Act.Identifiers)).AsResultSet();

        /// <summary>
        /// Gets or loads relationships
        /// </summary>
        public static IEnumerable<ActRelationship> GetRelationships(this Act me) => me.LoadCollection<ActRelationship>(nameof(Act.Relationships)).AsResultSet();
        
        /// <summary>
        /// Gets or loads participations
        /// </summary>
        public static IEnumerable<ActParticipation> GetParticipations(this Act me) => me.LoadCollection<ActParticipation>(nameof(Act.Participations)).AsResultSet();

        /// <summary>
        /// Get the target of the relationship
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        public static T GetTargetAs<T>(this EntityRelationship me) where T : Entity, new()
        {
            var existing= me.LoadProperty(o=>o.TargetEntity);
            if(existing is T retVal)
            {
                return retVal;
            }
            else
            {
                retVal = EntitySource.Current.Get<T>(me.TargetEntityKey);
                me.TargetEntity = retVal ?? me.TargetEntity;
                return retVal;
            }
        }
    }
}
