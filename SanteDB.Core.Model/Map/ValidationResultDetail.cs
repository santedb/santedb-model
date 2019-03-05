/*
 * Copyright 2015-2019 Mohawk College of Applied Arts and Technology
 *
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
 * User: JustinFyfe
 * Date: 2019-1-22
 */
using System;

namespace SanteDB.Core.Model.Map
{

    /// <summary>
    /// Result detail types
    /// </summary>
    public enum ResultDetailType
    {
        /// <summary>
        /// Represents an error.
        /// </summary>
        Error,

        /// <summary>
        /// Represents a warning.
        /// </summary>
        Warning,

        /// <summary>
        /// Represents information.
        /// </summary>
        Information
    }

    /// <summary>
    /// Represents a result detail which is a validation result
    /// </summary>
    public class ValidationResultDetail
    {

        /// <summary>
        /// Creates a new validation result detail
        /// </summary>
        /// <param name="level">The type of detail</param>
        /// <param name="message">The message of the result detail</param>
        /// <param name="causedBy">The exception that cuased this detail to be creatd</param>
        /// <param name="location">The location in the original configuration that caused the detail</param>
        public ValidationResultDetail(ResultDetailType level, string message, Exception causedBy, String location)
        {
            this.Level = level;
            this.Message = message;
            this.CausedBy = causedBy;
            this.Location = location;
        }

        /// <summary>
        /// Gets or sets the message which caused the error
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// Gets or sets the exception that caused this error
        /// </summary>
        public Exception CausedBy { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        public String Location { get; set; }

        /// <summary>
        /// The level of the warning
        /// </summary>
        public ResultDetailType Level { get; set; }

    }
}
