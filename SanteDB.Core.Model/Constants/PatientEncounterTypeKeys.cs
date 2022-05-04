using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Types of patient encounters
    /// </summary>
    public static class PatientEncounterTypeKeys
    {
        /// <summary>
        /// Ambulatory care setting
        /// </summary>
        public static Guid Ambulatory = Guid.Parse("42765002-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Emergency encounter (in an ER)
        /// </summary>
        public static Guid Emergency = Guid.Parse("427650FC-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Field hospital encounter
        /// </summary>
        public static Guid Field = Guid.Parse("427652C8-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Home care encounter
        /// </summary>
        public static Guid Home = Guid.Parse("427653EA-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Inpatient encounter
        /// </summary>
        public static Guid Inpatient = Guid.Parse("427654B2-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Pre-admission encounter
        /// </summary>
        public static Guid PreAdmission = Guid.Parse("4276573C-17BE-11EB-ADC1-0242AC120002");
        /// <summary>
        /// Virtual care encounter
        /// </summary>
        public static Guid Virtual = Guid.Parse("42765804-17BE-11EB-ADC1-0242AC120002");
    }
}
