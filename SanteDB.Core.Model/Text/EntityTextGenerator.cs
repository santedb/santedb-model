using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Generator for generic entities
    /// </summary>
    public abstract class EntityTextGeneratorBase<TEntity> : ComplexTextGeneratorBase<TEntity>
        where TEntity : Entity
    {
        /// <inheritdoc/>
        public override TextGeneratorRenderStyle RenderStyle => TextGeneratorRenderStyle.Table;

        /// <inheritdoc/>
        public override IEnumerable<Expression<Func<TEntity, object>>> GetRenderFields() => new Expression<Func<TEntity, object>>[] {
                o => o.ClassConcept,
                o => o.DeterminerConcept,
                o => o.TypeConcept,
                o => o.Identifiers,
                o => o.Names,
                o => o.Addresses,
                o => o.Telecoms,
                o => o.Extensions,
                o => o.Relationships
            };
    }

    /// <summary>
    /// Concrete class
    /// </summary>
    public class EntityTextGenerator : EntityTextGeneratorBase<Entity> { }
}
