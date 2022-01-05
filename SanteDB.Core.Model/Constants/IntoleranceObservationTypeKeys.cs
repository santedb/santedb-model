using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Intolerance observation types
    /// </summary>
    public static class IntoleranceObservationTypeKeys
    {
        /// <summary>
        /// Unclassified intolerance type
        /// </summary>
        public static readonly Guid OtherIntolerance = Guid.Parse("D0962D26-2230-41FD-A67C-02CE905C5D1F");

        /// <summary>
        /// Intolerance to a Drug or Medication
        /// </summary>
        public static readonly Guid DrugIntolerance = Guid.Parse("0124FDE0-7857-4815-B257-74ACAA0DD92D");

        /// <summary>
        /// Intolerance to an environmental substance
        /// </summary>
        public static readonly Guid EnvironmentalIntolerance = Guid.Parse("9CAFB9EC-CD0B-4003-B30C-677FA39B2E16");

        /// <summary>
        /// Intolerance to a food substance
        /// </summary>
        public static readonly Guid FoodIntolerance = Guid.Parse("298AC8E5-84BA-4992-9EEC-892C636C8E73");

        /// <summary>
        /// An intolerance to a druge which is not a allergy
        /// </summary>
        public static readonly Guid DrugNonAllergyIntolerance = Guid.Parse("62FC0BE4-F75F-460B-AB98-4215F4573748");

        /// <summary>
        /// An intolerance to an environmental substance
        /// </summary>
        public static readonly Guid EnvironmentalNonAllergyIntolerance = Guid.Parse("0577D51F-8FF5-4CF7-A2DD-BFC52841FFF2");

        /// <summary>
        /// An intolerance to a food which is not an allergy
        /// </summary>
        public static readonly Guid FoodNonAllergyIntolerance = Guid.Parse("594F0A36-34C1-48B4-B870-883173450FF4");
    }
}
