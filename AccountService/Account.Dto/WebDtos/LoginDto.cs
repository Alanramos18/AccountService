namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Request Login account dto
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        ///     User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Password.
        /// </summary>
        public string Password { get; set; }
    }
}
