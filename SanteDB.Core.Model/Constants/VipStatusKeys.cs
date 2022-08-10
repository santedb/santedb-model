using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Status concept keys for very imporatnt person indicators
    /// </summary>
    public static class VipStatusKeys
    {
        /// <summary>
        /// Person is a board member
        /// </summary>
        public static readonly Guid BoardMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130002");
        /// <summary>
        /// Person is a family member of a physician
        /// </summary>
        public static readonly Guid PhysicianFamilyMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130003");
        /// <summary>
        /// Person is a staff physician
        /// </summary>
        public static readonly Guid StaffPhysician = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130004");
        /// <summary>
        /// Person is a financial donor
        /// </summary>
        public static readonly Guid FinancialDonor = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130005");
        /// <summary>
        /// Person is a foreign dignitary
        /// </summary>
        public static readonly Guid ForeignDignitary = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130006");
        /// <summary>
        /// Person is a government dignitary
        /// </summary>
        public static readonly Guid GovernmentDignitary = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130007");
        /// <summary>
        /// Person is a family member of a staff person
        /// </summary>
        public static readonly Guid StaffFamilyMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130008");
        /// <summary>
        /// Person is a staff member
        /// </summary>
        public static readonly Guid StaffMember = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC130009");
        /// <summary>
        /// Person is a very important person (celebrity, public figure, etc.) of an uclassified nature.
        /// </summary>
        public static readonly Guid VeryImportantPerson = Guid.Parse("2F5B021C-7AD1-11EB-9439-0242AC13000A");
    }
}
