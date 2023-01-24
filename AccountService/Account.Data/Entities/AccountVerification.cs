namespace Account.Data.Entities
{
    public class AccountVerification
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
        ///     Application code the account is using.
        /// </summary>
        public string ApplicationCode { get; set; }

        /// <summary>
        ///     Is reset code?.
        /// </summary>
        public bool IsReset { get; set; }

        /// <summary>
        ///     Token or reset code.
        /// </summary>
        public string Token { get; set; }
    }
}
