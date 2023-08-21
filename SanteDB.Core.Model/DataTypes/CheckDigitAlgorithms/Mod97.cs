using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanteDB.Core.Model.DataTypes.CheckDigitAlgorithms
{
    /// <summary>
    /// Check digit algorithm for validating MOD-97 check digits
    /// </summary>
    public class Mod97CheckDigitAlgorithm : ICheckDigitAlgorithm
    {
        /// <inheritdoc/>
        public string Name => "MOD-97 Based Algorithm";

        /// <inheritdoc/>
        public string GenerateCheckDigit(string identifierValue)
        {
            var seed = ("0" + identifierValue)
                .Select(i => int.Parse(i.ToString()))
                .Aggregate((a, b) => ((a + b) * 10) % 97);
            seed *= 10;
            seed %= 97;
            var checkDigit = (97 - seed + 1) % 97;
            return checkDigit.ToString().PadLeft(2, '0');
        }

        /// <inheritdoc/>
        public bool ValidateCheckDigit(string identifierValue, string checkDigit)
        {
            return this.GenerateCheckDigit(identifierValue).Equals(checkDigit);
        }
    }

    /// <summary>
    /// Identifier validator for identifiers which are suffixed with a MOD-97 as the last two digits in the identifier
    /// </summary>
    public class InlineMod97Validator : IIdentifierValidator
    {
        private static readonly ICheckDigitAlgorithm m_mod97Algorithm = new Mod97CheckDigitAlgorithm();

        /// <inheritdoc/>
        public string Name => "Inline MOD-97 Check Digit Validator";

        /// <inheritdoc/>
        public bool IsValid(IExternalIdentifier id)
        {
            if(id.Value.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(id.Value));
            }

            string checkDigit = id.Value.Substring(id.Value.Length - 2),
                identifierSeed = id.Value.Substring(0, id.Value.Length - 2);
            return !String.IsNullOrEmpty(id.CheckDigit) && id.CheckDigit.Equals(checkDigit) // If the identifier provides the check digit separately it needs to match the last two digits of the identifier
                && m_mod97Algorithm.ValidateCheckDigit(identifierSeed, checkDigit); // The last two digits should match the identifier seed
        }
    }
}
