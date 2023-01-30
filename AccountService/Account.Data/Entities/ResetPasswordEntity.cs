namespace Account.Data.Entities
{
    public class ResetPasswordEntity
    {
        /// <summary>
        ///     Id of the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Email of the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        ///     Application code the account is using.
        /// </summary>
        public string? ApplicationCode { get; set; }

        /// <summary>
        ///     6 Digit reset code.
        /// </summary>
        public string? DigitCode { get; set; }

        /// <summary>
        ///     Reset password token.
        /// </summary>
        public string? Token { get; set;}
    }
}
