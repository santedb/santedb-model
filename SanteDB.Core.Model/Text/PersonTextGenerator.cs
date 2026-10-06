using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace SanteDB.Core.Model.Text
{

    /// <summary>
    /// Person text generator
    /// </summary>
    public abstract class PersonTextGeneratorBase<TPerson> : EntityTextGeneratorBase<TPerson>
        where TPerson : Person
    {
        /// <summary>
        /// Get the rendering fields
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Expression<Func<TPerson, object>>> GetRenderFields() => new Expression<Func<TPerson, object>>[] {
                o => o.TypeConcept,
                o => o.Policies,
                o => o.DateOfBirth,
                o => o.DeceasedDate,
                o => o.GenderConcept,
                o => o.Occupation,
                o => o.VipStatus,
                o => o.MaritalStatus,
                o => o.Identifiers,
                o => o.Names,
                o => o.Addresses,
                o => o.Telecoms,
                o => o.Extensions,
                o => o.Relationships
            };
    }

    /// <summary>
    /// Person text generator
    /// </summary>
    public class PersonTextGenerator : PersonTextGeneratorBase<Person> { }

}
