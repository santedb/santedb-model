using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Represents built in assigning authorities
    /// </summary>
    public static class AssigningAuthorityKeys
    {

        /// <summary>
        /// ISO 3166 Country Code Identifier Authority
        /// </summary>
        public static readonly Guid Iso3166CountryCode = Guid.Parse("ff6e7402-8545-48e1-8a70-ebf06f3ee4b8");
    }
}
