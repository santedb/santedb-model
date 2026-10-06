using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Material text generator
    /// </summary>
    public abstract class MaterialTextGeneratorBase<TMaterial> : EntityTextGeneratorBase<TMaterial>
        where TMaterial : Material
    {
        /// <inheritdoc/>
        public override IEnumerable<Expression<Func<TMaterial, object>>> GetRenderFields()
        {
            var renderFields = new List<Expression<Func<TMaterial, object>>>(base.GetRenderFields());
            // Insert Multiple Birth Order
            renderFields.Insert(3, o => o.ExpiryDate);
            renderFields.Insert(4, o => o.Quantity);
            renderFields.Insert(4, o => o.FormConcept);
            return renderFields;
        }
    }

    public class MaterialTextGenerator : MaterialTextGeneratorBase<Material> { }

    public class ManufacturedMaterialTextGenerator : MaterialTextGeneratorBase<ManufacturedMaterial>
    {
        /// <inheritdoc/>
        public override IEnumerable<Expression<Func<ManufacturedMaterial, object>>> GetRenderFields()
        {
            var renderFields = new List<Expression<Func<ManufacturedMaterial, object>>>(base.GetRenderFields());
            // Insert Multiple Birth Order
            renderFields.Insert(5, o => o.LotNumber);
            renderFields.Insert(5, o => o.Notes);
            renderFields.Insert(5, o => o.IsAdministrable);
            return renderFields;
        }
    }
}
