using System.ComponentModel.DataAnnotations;

namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Request create account dto
    /// </summary>
    public class VerifyResetRequest
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? Code { get; set; }
    }
}
