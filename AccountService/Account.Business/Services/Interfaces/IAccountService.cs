using System.Threading;
using System.Threading.Tasks;
using Account.Dto.WebDtos;
using Microsoft.AspNetCore.Identity;

namespace Account.Business.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        ///     Create account service.
        /// </summary>
        /// <param name="accountDto">Create account Dto</param>
        /// <param name="confirmationLink">Link to confirm email</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created account dto</returns>
        Task<RegisterResponsetDto> RegisterAsync(RegisterRequestDto accountDto, string confirmationLink, string applicationCode, CancellationToken cancellationToken);

        /// <summary>
        ///     Confirm email to validate user.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="token">Validation token</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<IdentityResult> ConfirmEmailAsync(string email, string token, string applicationCode, CancellationToken cancellationToken);

        /// <summary>
        ///     Login user to get token.
        /// </summary>
        /// <param name="dto">Login dto</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JWT string</returns>
        Task<string> LoginAsync(LoginDto dto, string applicationCode, CancellationToken cancellationToken);

        /// <summary>
        ///     Send email with code.
        /// </summary>
        /// <param name="email">User Email</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task ForgotPasswordAsync(string email, string applicationCode, CancellationToken cancellationToken);

        Task<string> VerifyResetCodeAsync(string email, string appSource, string code, CancellationToken cancellationToken);
        Task ChangePasswordAsync(string email, string appSource, string newPassword, string token, CancellationToken cancellationToken);
    }
}
