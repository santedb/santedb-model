using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Administrative gender concept reference keys
    /// </summary>
    public static class AdministrativeGenderConceptKeys
    {
        /// <summary>
        /// Gender for administrative reference is Male
        /// </summary>
        public static readonly Guid Male = Guid.Parse("f4e3a6bb-612e-46b2-9f77-ff844d971198");

        /// <summary>
        /// Gender for administrative reference is Female
        /// </summary>
        public static readonly Guid Female = Guid.Parse("094941e9-a3db-48b5-862c-bc289bd7f86c");

        /// <summary>
        /// Gender for administrative purposes is not differentiated
        /// </summary>
        public static readonly Guid Undifferentiated = Guid.Parse("ae94a782-1485-4241-9bca-5b09db2156bf");


    }
}
