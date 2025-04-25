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
 * Date: 2024-6-21
 */
using System;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents a check digit algorithm
    /// </summary>
    public interface ICheckDigitAlgorithm
    {

        /// <summary>
        /// Gets the name of the check digit algorithm
        /// </summary>
        string Name { get; }

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
