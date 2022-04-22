using SanteDB.Core.Model.Acts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Keys for concepts which represent an interpretation of a <see cref="Observation"/>
    /// </summary>
    public static class ActInterpretationKeys
    {
        /// <summary>
        /// The observation represents a value which is higher than expected
        /// </summary>
        public static readonly Guid AbnormalHigh = Guid.Parse("3C4D6579-7496-4B44-AAC1-18A714FF7A05");
        /// <summary>
        /// The observation represents a value which is much higher than expected
        /// </summary>
        public static readonly Guid AbnormalVeryHigh = Guid.Parse("8B553D58-6C8C-4D01-A534-83BA5780B41A");
        /// <summary>
        /// The observation is much lower than expected 
        /// </summary>
        public static readonly Guid AbnormalVeryLow = Guid.Parse("A7159BA0-A9EC-4565-95B8-ED364794C0B8");
        /// <summary>
        /// The observation is abnormally low
        /// </summary>
        public static readonly Guid AbnormalLow = Guid.Parse("6188F821-261F-420C-9520-0DE240A05661");
        /// <summary>
        /// The observation is normal
        /// </summary>
        public static readonly Guid Normal = Guid.Parse("41D42ABF-17AD-4144-BF97-EC3FD907F57D");
    }
}
