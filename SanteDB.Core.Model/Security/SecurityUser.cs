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
 * Date: 2021-8-27
 */

using Newtonsoft.Json;
using SanteDB.Core.Model.Attributes;
using SanteDB.Core.Model.Constants;
using SanteDB.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace SanteDB.Core.Model.Security
{
    /// <summary>
    /// Security user represents a user for the purpose of security
    /// </summary>
    [XmlType("SecurityUser", Namespace = "http://santedb.org/model"), JsonObject("SecurityUser")]
    [XmlRoot(Namespace = "http://santedb.org/model", ElementName = "SecurityUser")]
    [KeyLookup(nameof(UserName))]
    public class SecurityUser : SecurityEntity
    {
        /// <summary>
        /// Roles belonging to the user
        /// </summary>
        public SecurityUser()
        {
            this.Roles = new List<SecurityRole>();
        }

        /// <summary>
        /// Gets or sets the email address of the user
        /// </summary>
        [XmlElement("email"), JsonProperty("email"), NoCase]
        public String Email { get; set; }

        /// <summary>
        /// Gets or sets whether the email address is confirmed
        /// </summary>
        [XmlElement("emailConfirmed"), JsonProperty("emailConfirmed")]
        public Boolean EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the number of invalid login attempts by the user
        /// </summary>
        [XmlElement("invalidLoginAttempts"), JsonProperty("invalidLoginAttempts")]
        public Int32 InvalidLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets whether the account is locked out
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(LockoutXml))]
        public DateTimeOffset? Lockout { get; set; }

        /// <summary>
        /// Gets or sets the creation time in XML format
        /// </summary>
        [XmlElement("lockout"), JsonProperty("lockout")]
        public String LockoutXml
        {
            get { return this.Lockout?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                if (value != null)
                    this.Lockout = DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture);
                else
                    this.Lockout = default(DateTime);
            }
        }

        /// <summary>
        /// Gets or sets whether the password hash is enabled
        /// </summary>
        [XmlElement("password"), JsonProperty("password")]
        public String Password { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether the security data for the user has changed
        /// </summary>
        [XmlElement("securityStamp"), JsonProperty("securityStamp")]
        public String SecurityHash { get; set; }

        /// <summary>
        /// Gets or sets whether two factor authentication is required
        /// </summary>
        [XmlElement("twoFactorEnabled"), JsonProperty("twoFactorEnabled")]
        public Boolean TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the logical user name ofthe user
        /// </summary>
        [XmlElement("userName"), JsonProperty("userName"), NoCase]
        public String UserName { get; set; }

        /// <summary>
        /// Gets or sets the binary representation of the user's photo
        /// </summary>
        [XmlElement("photo"), JsonProperty("photo")]
        public byte[] UserPhoto { get; set; }

        /// <summary>
        /// The last login time
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(LastLoginTimeXml))]
        public DateTimeOffset? LastLoginTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time in XML format
        /// </summary>
        [XmlElement("lastLoginTime"), JsonProperty("lastLoginTime")]
        public String LastLoginTimeXml
        {
            get { return this.LastLoginTime?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.LastLoginTime = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else this.LastLoginTime = default(DateTimeOffset);
            }
        }

        /// <summary>
        /// The last login time
        /// </summary>
        [XmlIgnore, JsonIgnore, SerializationReference(nameof(PasswordExpirationXml))]
        public DateTimeOffset? PasswordExpiration { get; set; }

        /// <summary>
        /// Gets or sets the creation time in XML format
        /// </summary>
        [XmlElement("passwordExpiry"), JsonProperty("passwordExpiry")]
        public String PasswordExpirationXml
        {
            get { return this.PasswordExpiration?.ToString("o", CultureInfo.InvariantCulture); }
            set
            {
                DateTimeOffset val = default(DateTimeOffset);
                if (value != null)
                {
                    if (DateTimeOffset.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out val) ||
                        DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out val))
                        this.PasswordExpiration = val;
                    else
                        throw new FormatException($"Date {value} was not recognized as a valid date format");
                }
                else this.PasswordExpiration = null;
            }
        }

        /// <summary>
        /// Represents roles
        /// </summary>
        [XmlIgnore, JsonIgnore, QueryParameter("roles")]
        public List<SecurityRole> Roles { get; set; }

        /// <summary>
        /// Gets or sets the patient's phone number
        /// </summary>
        [XmlElement("phoneNumber"), JsonProperty("phoneNumber")]
        public String PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets whether the phone number was confirmed
        /// </summary>
        [XmlElement("phoneNumberConfirmed"), JsonProperty("phoneNumberConfirmed")]
        public Boolean PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the preferred tfa mechanism
        /// </summary>
        [XmlElement("twoFactorMechanism"), JsonProperty("twoFactorMechanism")]
        public Guid TwoFactorMechnaismKey { get; set; }

        /// <summary>
        /// Gets or sets the user class key
        /// </summary>
        [XmlElement("userClass"), JsonProperty("userClass")]
        [Binding(typeof(ActorTypeKeys))]
        public Guid UserClass { get; set; }

        /// <summary>
        /// Gets the etag
        /// </summary>
        public override string Tag
        {
            get
            {
                return this.SecurityHash;
            }
        }

        /// <summary>
        /// Link from this security resource to an entity resource
        /// </summary>
        [XmlIgnore, QueryParameter("userEntity"), JsonIgnore]
        public UserEntity UserEntity { get; set; }

        /// <summary>
        /// Determine semantic equality of user
        /// </summary>
        public override bool SemanticEquals(object obj)
        {
            var other = obj as SecurityUser;
            if (other == null) return false;
            return base.SemanticEquals(obj) &&
                this.Email == other.Email &&
                this.EmailConfirmed == other.EmailConfirmed &&
                this.Password == other.Password &&
                this.PhoneNumber == other.PhoneNumber &&
                this.PhoneNumberConfirmed == other.PhoneNumberConfirmed &&
                this.Roles?.SemanticEquals(other.Roles) == true &&
                this.SecurityHash == other.SecurityHash &&
                this.TwoFactorEnabled == other.TwoFactorEnabled &&
                this.UserClass == other.UserClass &&
                this.UserName == other.UserName &&
                (this.UserPhoto ?? new byte[0]).HashCode().Equals((other.UserPhoto ?? new byte[0]).HashCode()) == true;
        }

        /// <summary>
        /// Get the name of the object as a display string
        /// </summary>
        public override string ToDisplay() => $"{this.UserName} [{this.Key}]";
    }
}