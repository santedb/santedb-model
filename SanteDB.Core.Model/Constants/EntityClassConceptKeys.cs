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
 * Date: 2023-6-21
 */
using System;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
	/// Entity class concept keys
	/// </summary>
	public static class EntityClassKeyStrings
    {
        /// <summary>
        /// Animal
        /// </summary>
        public const string Animal = "61fcbf42-b5e0-4fb5-9392-108a5c6dbec7";

        /// <summary>
        /// Chemical Substance
        /// </summary>
        public const string ChemicalSubstance = "2e9fa332-9391-48c6-9fc8-920a750b25d3";

        /// <summary>
        /// City or town
        /// </summary>
        public const string CityOrTown = "79dd4f75-68e8-4722-a7f5-8bc2e08f5cd6";

        /// <summary>
        /// Container
        /// </summary>
        public const string Container = "b76ff324-b174-40b7-a6ac-d1fdf8e23967";

        /// <summary>
        /// Country or nation
        /// </summary>
        public const string Country = "48b2ffb3-07db-47ba-ad73-fc8fb8502471";

        /// <summary>
        /// County or parish
        /// </summary>
        public const string CountyOrParish = "6eefee7d-dff5-46d3-a6a7-171ef93879c7";

        /// <summary>
        /// Represents a precinct or sub-division of a city such as a burrogh
        /// </summary>
        public const string PrecinctOrBorough = "acafe0f2-e209-43bb-8633-3665fd7c90ba";

        /// <summary>
        /// Device
        /// </summary>
        public const string Device = "1373ff04-a6ef-420a-b1d0-4a07465fe8e8";

        /// <summary>
        /// Entity
        /// </summary>
        public const string Entity = "e29fcfad-ec1d-4c60-a055-039a494248ae";

        /// <summary>
        /// Food
        /// </summary>
        public const string Food = "e5a09cc2-5ae5-40c2-8e32-687dba06715d";

        /// <summary>
        /// Living Subject
        /// </summary>
        public const string LivingSubject = "8ba5e5c9-693b-49d4-973c-d7010f3a23ee";

        /// <summary>
        /// Manufactured material
        /// </summary>
        public const string ManufacturedMaterial = "fafec286-89d5-420b-9085-054aca9d1eef";

        /// <summary>
        /// Material
        /// </summary>
        public const string Material = "d39073be-0f8f-440e-b8c8-7034cc138a95";

        /// <summary>
        /// Non living subject
        /// </summary>
        public const string NonLivingSubject = "9025e5c9-693b-49d4-973c-d7010f3a23ee";

        /// <summary>
        /// Organization
        /// </summary>
        public const string Organization = "7c08bd55-4d42-49cd-92f8-6388d6c4183f";

        /// <summary>
        /// Patient
        /// </summary>
        public const string Patient = "bacd9c6f-3fa9-481e-9636-37457962804d";

        /// <summary>
        /// Person
        /// </summary>
        public const string Person = "9de2a846-ddf2-4ebc-902e-84508c5089ea";

        /// <summary>
        /// Place
        /// </summary>
        public const string Place = "21ab7873-8ef3-4d78-9c19-4582b3c40631";

        /// <summary>
        /// Service delivery location
        /// </summary>
        public const string Provider = "6b04fed8-c164-469c-910b-f824c2bda4f0";

        /// <summary>
        /// Service delivery location
        /// </summary>
        public const string ServiceDeliveryLocation = "ff34dfa7-c6d3-4f8b-bc9f-14bcdc13ba6c";

        /// <summary>
        /// State
        /// </summary>
        public const string StateOrProvince = "4d1a5c28-deb7-411e-b75f-d524f90dfa63";

        /// <summary>
        /// Person which is a user
        /// </summary>
        public const string UserEntity = "6a2b00ba-501b-4523-b57c-f96d8ae44684";

        /// <summary>
        /// Person which is a user
        /// </summary>
        public const string ZoneOrTerritory = "3F9B5FF2-97A6-40BE-8FCB-E71D18C8EE42";
    }


    /// <summary>
    /// Entity class concept keys
    /// </summary>
    // TODO: Refactor these
    public static class EntityClassKeys
    {
        /// <summary>
        /// Animal
        /// </summary>
        public static readonly Guid Animal = Guid.Parse(EntityClassKeyStrings.Animal);

        /// <summary>
        /// Chemical Substance
        /// </summary>
        public static readonly Guid ChemicalSubstance = Guid.Parse(EntityClassKeyStrings.ChemicalSubstance);

        /// <summary>
        /// City or town
        /// </summary>
        public static readonly Guid CityOrTown = Guid.Parse(EntityClassKeyStrings.CityOrTown);

        /// <summary>
        /// Container
        /// </summary>
        public static readonly Guid Container = Guid.Parse(EntityClassKeyStrings.Container);

        /// <summary>
        /// Country or nation
        /// </summary>
        public static readonly Guid Country = Guid.Parse(EntityClassKeyStrings.Country);

        /// <summary>
        /// County or parish
        /// </summary>
        public static readonly Guid CountyOrParish = Guid.Parse(EntityClassKeyStrings.CountyOrParish);

        /// <summary>
        /// Device
        /// </summary>
        public static readonly Guid Device = Guid.Parse(EntityClassKeyStrings.Device);

        /// <summary>
        /// Entity
        /// </summary>
        public static readonly Guid Entity = Guid.Parse(EntityClassKeyStrings.Entity);

        /// <summary>
        /// Food
        /// </summary>
        public static readonly Guid Food = Guid.Parse(EntityClassKeyStrings.Food);

        /// <summary>
        /// Living Subject
        /// </summary>
        public static readonly Guid LivingSubject = Guid.Parse(EntityClassKeyStrings.LivingSubject);

        /// <summary>
        /// Manufactured material
        /// </summary>
        public static readonly Guid ManufacturedMaterial = Guid.Parse(EntityClassKeyStrings.ManufacturedMaterial);

        /// <summary>
        /// Material
        /// </summary>
        public static readonly Guid Material = Guid.Parse(EntityClassKeyStrings.Material);

        /// <summary>
        /// Non living subject
        /// </summary>
        public static readonly Guid NonLivingSubject = Guid.Parse(EntityClassKeyStrings.NonLivingSubject);

        /// <summary>
        /// Organization
        /// </summary>
        public static readonly Guid Organization = Guid.Parse(EntityClassKeyStrings.Organization);

        /// <summary>
        /// Patient
        /// </summary>
        public static readonly Guid Patient = Guid.Parse(EntityClassKeyStrings.Patient);

        /// <summary>
        /// Person
        /// </summary>
        public static readonly Guid Person = Guid.Parse(EntityClassKeyStrings.Person);

        /// <summary>
        /// Place
        /// </summary>
        public static readonly Guid Place = Guid.Parse(EntityClassKeyStrings.Place);

        /// <summary>
        /// Service delivery location
        /// </summary>
        public static readonly Guid Provider = Guid.Parse(EntityClassKeyStrings.Provider);

        /// <summary>
        /// Service delivery location
        /// </summary>
        public static readonly Guid ServiceDeliveryLocation = Guid.Parse(EntityClassKeyStrings.ServiceDeliveryLocation);

        /// <summary>
        /// State
        /// </summary>
        public static readonly Guid StateOrProvince = Guid.Parse(EntityClassKeyStrings.StateOrProvince);

        /// <summary>
        /// Represents a precinct or sub-division of a city such as a burrogh
        /// </summary>
        public static readonly Guid PrecinctOrBorough = Guid.Parse(EntityClassKeyStrings.PrecinctOrBorough);

        /// <summary>
        /// Represents a person which is a user in the system
        /// </summary>
        public static readonly Guid UserEntity = Guid.Parse(EntityClassKeyStrings.UserEntity);

        /// <summary>
        /// Represents a zone or a territority
        /// </summary>
        public static readonly Guid ZoneOrTerritory = Guid.Parse(EntityClassKeyStrings.ZoneOrTerritory);
    }
}