/*
 * Copyright (C) 2021 - 2025, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors
 * Portions Copyright (C) 2015-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2024-12-12
 */
using SanteDB.Core.Model.Interfaces;
using System;
using System.Linq;

namespace SanteDB.Core.Model.DataTypes.CheckDigitAlgorithms
{
    /// <summary>
    /// Check digit algorithm for validating MOD-97 check digits
    /// </summary>
    public class Mod97CheckDigitAlgorithm : ICheckDigitAlgorithm
    {
        /// <inheritdoc/>
        public string Name => "Custom MOD-97 Based Algorithm";

        /// <inheritdoc/>
        public string GenerateCheckDigit(string identifierValue)
        {
            var seed = ("0" + identifierValue) 
                .Where(i=>int.TryParse(i.ToString(), out _))
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
        public string Name => "Inline Custom MOD-97 Check Digit Validator";

        /// <inheritdoc/>
        public bool IsValid(IExternalIdentifier id)
        {
            if (id.Value.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(id.Value));
            }

            string checkDigit = id.Value.Substring(id.Value.Length - 2),
                identifierSeed = id.Value.Substring(0, id.Value.Length - 2);
            return 
                (!String.IsNullOrEmpty(id.CheckDigit) && id.CheckDigit.Equals(checkDigit) || String.IsNullOrEmpty(id.CheckDigit)) // If the identifier provides the check digit separately it needs to match the last two digits of the identifier
                && m_mod97Algorithm.ValidateCheckDigit(identifierSeed, checkDigit); // The last two digits should match the identifier seed
        }
    }
}
