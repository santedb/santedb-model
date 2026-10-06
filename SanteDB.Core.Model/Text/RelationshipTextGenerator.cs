using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.EntityLoader;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Generic relationship text generator
    /// </summary>
    public abstract class RelationshipTextGenerator<TModel> : ComplexTextGeneratorBase<TModel>
        where TModel : IdentifiedData, ITargetedAssociation
    {
        public override TextGeneratorRenderStyle RenderStyle => TextGeneratorRenderStyle.DefinitionList;

        protected XElement GetTargetSummary(TModel target) =>
            new XElement("span", (target.LoadProperty(o => o.TargetEntity) as IdentifiedData)?.ToDisplay() ??
                target.TargetEntity.ToString());
    }

    public class EntityRelationshipTextGenerator : RelationshipTextGenerator<EntityRelationship>
    {
        public override IEnumerable<Expression<Func<EntityRelationship, object>>> GetRenderFields() => new Expression<Func<EntityRelationship, object>>[]
        {
            o=> o.RelationshipType,
            o=> o.Quantity,
            o=> this.GetTargetSummary(o)
        };

    }

    public class ActRelationshipTextGenerator : RelationshipTextGenerator<ActRelationship>
    {
        public override IEnumerable<Expression<Func<ActRelationship, object>>> GetRenderFields() => new Expression<Func<ActRelationship, object>>[]
         {
            o=> o.RelationshipType,
            o=> this.GetTargetSummary(o)
         };
    }

    public class ActParticipationTextGenerator : RelationshipTextGenerator<ActParticipation>
    {
        public override IEnumerable<Expression<Func<ActParticipation, object>>> GetRenderFields() => new Expression<Func<ActParticipation, object>>[]
        {
            o=> o.ParticipationRole,
            o=> o.Quantity,
            o=> this.GetTargetSummary(o)
        };
    }
}
