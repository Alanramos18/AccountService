namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Request Login account dto
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        ///     Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Password.
        /// </summary>
        public string Password { get; set; }
    }
}
