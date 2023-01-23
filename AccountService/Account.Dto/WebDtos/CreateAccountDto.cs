namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Request create account dto
    /// </summary>
    public class RegisterRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
