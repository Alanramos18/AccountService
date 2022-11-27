using System;

namespace Account.Data.Entities
{
    public class AccountEntity
    {
        /// <summary>
        ///     Id of the account.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     User name of the account.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Password of the account.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Email of the account.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Application the account is using.
        /// </summary>
        public string Application { get; set; }
    }
}
