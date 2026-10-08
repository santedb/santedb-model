using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Roles;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Organization text generator
    /// </summary>
    public class OrganizationTextGenerator : EntityTextGeneratorBase<Organization>
    {

        /// <inheritdoc/>
        public override IEnumerable<Expression<Func<Organization, object>>> GetRenderFields()
        {
            var renderFields = new List<Expression<Func<Organization, object>>>(base.GetRenderFields());
            // Insert Multiple Birth Order
            renderFields.Insert(3, o => o.IndustryConcept);
            return renderFields;
        }

    }
}
