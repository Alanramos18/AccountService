using System;

namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Response create account dto
    /// </summary>
    public class CreatedAccountDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
