using Microsoft.AspNetCore.Identity;

namespace Account.Data.Entities
{
    public class AccountEntity : IdentityUser
    {
        /// <summary>
        ///     Application code the account is using.
        /// </summary>
        public string? ApplicationCode { get; set; }
    }
}
