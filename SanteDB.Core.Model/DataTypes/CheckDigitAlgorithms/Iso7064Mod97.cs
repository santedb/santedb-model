﻿using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SanteDB.Core.Model.DataTypes.CheckDigitAlgorithms
{
    /// <summary>
    /// ISO/IEC-7064 MOD-97,10 Algorithm
    /// </summary>
    public class Iso7064Mod97CheckDigitAlgorithm : ICheckDigitAlgorithm
    {
        private readonly Regex m_identifierExtractor = new Regex(@"[^0-9]", RegexOptions.Compiled);
        /// <inheritdoc/>
        public string Name => "ISO/IEC-7064 MOD-97,10";


        /// <inheritdoc/>
        public string GenerateCheckDigit(string identifierValue)
        {
            var value = this.m_identifierExtractor.Replace(identifierValue, "");
            if(ulong.TryParse(value, out var lng))
            {
                var check = (98 - (lng * 100) % 97) % 97;
                return check.ToString().PadLeft(2, '0');
            }
            else
            {
                throw new InvalidOperationException("Value exceeds valid range for MOD-97 check digit validation");
            }
        }

        /// <inheritdoc/>
        public bool ValidateCheckDigit(string identifierValue, string checkDigit)
        {
            var value = this.m_identifierExtractor.Replace(identifierValue, "") + checkDigit;
            if(ulong.TryParse(value, out var lng))
            {
                return lng % 97 == 1;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// ISO/IEC-7064 MOD-97,10 Algorithm
    /// </summary>
    public class InlineIso7064Mod97Validator : IIdentifierValidator
    {
        private static readonly ICheckDigitAlgorithm m_mod97Algorithm = new Iso7064Mod97CheckDigitAlgorithm();

        /// <inheritdoc/>
        public string Name => "ISO/IEC MOD-97,10 Check Digit Validator";

        /// <inheritdoc/>
        public bool IsValid(IExternalIdentifier id)
        {
            if (id.Value.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(id.Value));
            }

            string checkDigit = id.Value.Substring(id.Value.Length - 2),
                identifierSeed = id.Value.Substring(0, id.Value.Length - 2);
            return (!String.IsNullOrEmpty(id.CheckDigit) && id.CheckDigit.Equals(checkDigit) || String.IsNullOrEmpty(id.CheckDigit)) // If the identifier provides the check digit separately it needs to match the last two digits of the identifier
                && m_mod97Algorithm.ValidateCheckDigit(identifierSeed, checkDigit); // The last two digits should match the identifier seed
        }
    }
}