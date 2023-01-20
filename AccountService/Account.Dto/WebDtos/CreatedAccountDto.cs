using System;

namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Response create account dto
    /// </summary>
    public class CreatedAccountDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
