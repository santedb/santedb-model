using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents a check digit algorithm
    /// </summary>
    public interface ICheckDigitAlgorithm
    {

        /// <summary>
        /// Generates a check digit based on <paramref name="identifierValue"/>
        /// </summary>
        /// <param name="identifierValue">The value on which a check digit should be calculated</param>
        /// <returns>The generated check digit</returns>
        string GenerateCheckDigit(String identifierValue);

        /// <summary>
        /// Validate the check digit
        /// </summary>
        /// <param name="identifierValue">The identifier value to check</param>
        /// <param name="checkDigit">The check digit</param>
        /// <returns>True if the check digit is correct</returns>
        bool ValidateCheckDigit(String identifierValue, String checkDigit);

    }
}
