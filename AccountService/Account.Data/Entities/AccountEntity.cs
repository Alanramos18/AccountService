using System;

namespace Account.Data.Entities
{
    public class AccountEntity
    {
        /// <summary>
        ///     Id of the account.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Email of the account.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Hash of the account.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        ///     Application code the account is using.
        /// </summary>
        public string ApplicationCode { get; set; }

        /// <summary>
        ///     Verification of the account
        /// </summary>
        public int Verification { get; set; }
    }
}
