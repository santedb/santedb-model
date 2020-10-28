using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
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
        /// Gets or loads addresses attached to an entity
        /// </summary>
        public static IEnumerable<EntityAddress> GetAddresses(this Entity me) => me.LoadCollection<EntityAddress>(nameof(Entity.Addresses));

        /// <summary>
        /// Gets or loads names attached to an entity
        /// </summary>
        public static IEnumerable<EntityName> GetNames(this Entity me) => me.LoadCollection<EntityName>(nameof(Entity.Names));

        /// <summary>
        /// Gets or loads telecoms attached to an entity
        /// </summary>
        public static IEnumerable<EntityTelecomAddress> GetTelecoms(this Entity me) => me.LoadCollection<EntityTelecomAddress>(nameof(Entity.Telecoms));

        /// <summary>
        /// Gets or loads relationships
        /// </summary>
        public static IEnumerable<EntityRelationship> GetRelationships(this Entity me) => me.LoadCollection<EntityRelationship>(nameof(Entity.Relationships));

        /// <summary>
        /// Gets or loads identifiers
        /// </summary>
        public static IEnumerable<EntityIdentifier> GetIdentifiers(this Entity me) => me.LoadCollection<EntityIdentifier>(nameof(Entity.Identifiers));
        
        /// <summary>
        /// Gets or loads notes
        /// </summary>
        public static IEnumerable<EntityNote> GetNotes(this Entity me) => me.LoadCollection<EntityNote>(nameof(Entity.Notes));

        /// <summary>
        /// Gets or loads tags
        /// </summary>
        public static IEnumerable<EntityTag> GetTags(this Entity me) => me.LoadCollection<EntityTag>(nameof(Entity.Tags));

        /// <summary>
        /// Gets or loads extensions
        /// </summary>
        public static IEnumerable<EntityExtension> GetExtensions(this Entity me) => me.LoadCollection<EntityExtension>(nameof(Entity.Extensions));

        /// <summary>
        /// Gets or loads identifiers
        /// </summary>
        public static IEnumerable<ActIdentifier> GetIdentifiers(this Act me) => me.LoadCollection<ActIdentifier>(nameof(Act.Identifiers));

        /// <summary>
        /// Gets or loads relationships
        /// </summary>
        public static IEnumerable<ActRelationship> GetRelationships(this Act me) => me.LoadCollection<ActRelationship>(nameof(Act.Relationships));
        
        /// <summary>
        /// Gets or loads participations
        /// </summary>
        public static IEnumerable<ActParticipation> GetParticipations(this Act me) => me.LoadCollection<ActParticipation>(nameof(Act.Participations));

    }
}
