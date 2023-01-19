namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Request create account dto
    /// </summary>
    public class CreateAccountDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        //public string Email { get; set; }

        public string Application { get; set; }
    }
}
