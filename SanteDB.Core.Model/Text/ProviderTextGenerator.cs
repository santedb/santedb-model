using SanteDB.Core.Model.Roles;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SanteDB.Core.Model.Text
{
    /// <summary>
    /// Provider text generator
    /// </summary>
    public class ProviderTextGenerator : PersonTextGeneratorBase<Provider>
    {

        /// <inheritdoc/>
        public override IEnumerable<Expression<Func<Provider, object>>> GetRenderFields()
        {
            var renderFields = new List<Expression<Func<Provider, object>>>(base.GetRenderFields());
            // Insert Multiple Birth Order
            renderFields.Insert(3, o => o.Specialty);
            return renderFields;
        }
    }
}
