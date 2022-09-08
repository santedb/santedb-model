/*
 * Copyright (C) 2021 - 2022, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
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
 * Date: 2022-5-30
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Class keys for vital sign
    /// </summary>
    public static class VitalSignObservationTypeKeys
    {
        /// <summary>
        /// Respiration rate
        /// </summary>
        public static readonly Guid Respiration = Guid.Parse("4E3DE4EC-17B8-4816-83A4-0792C4FB2A29");
        /// <summary>
        /// Heartbeat
        /// </summary>
        public static readonly Guid HeartBeat = Guid.Parse("5B4661EA-EECB-47DA-8E25-71E1C1DB04D8");
        /// <summary>
        /// O2 saturation
        /// </summary>
        public static readonly Guid OxygenSaturation = Guid.Parse("6FAE313D-FB69-400A-BC2C-31CF4C542251");
        /// <summary>
        /// Systolic BP
        /// </summary>
        public static readonly Guid SystolicBp = Guid.Parse("C0E4E81E-AF1F-4A81-80A6-C59A32F57CFA");
        /// <summary>
        /// Diastolic BP
        /// </summary>
        public static readonly Guid DiastolicBp = Guid.Parse("8E105155-32D4-4BBE-BC86-60154706B3CA");
        /// <summary>
        /// Body tempurature
        /// </summary>
        public static readonly Guid BodyTemperature = Guid.Parse("174EF16C-5779-4713-A5E0-4E838B60FE81");
        /// <summary>
        /// Height while standing
        /// </summary>
        public static readonly Guid Height = Guid.Parse("850CA898-C656-4BA2-A7C1-FF74E3331396");
        /// <summary>
        /// Height while laying
        /// </summary>
        public static readonly Guid HeightLying = Guid.Parse("8D2E144B-D7BF-4C17-8916-25AFF60BCB2F");
        /// <summary>
        /// Circumference
        /// </summary>
        public static readonly Guid HeadCircumference = Guid.Parse("B971ED37-0EC9-43E6-9DEA-E8399417EA44");
        /// <summary>
        /// Weight
        /// </summary>
        public static readonly Guid Weight = Guid.Parse("A261F8CD-69B0-49AA-91F4-E6D3E5C612ED");
    }
}
