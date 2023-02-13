using System.ComponentModel.DataAnnotations;

namespace Account.Dto.WebDtos
{
    /// <summary>
    ///     Request create account dto
    /// </summary>
    public class ChangePasswordRequest
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? NewPassword { get; set; }

        public string? Token { get; set; }
    }
}
