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
