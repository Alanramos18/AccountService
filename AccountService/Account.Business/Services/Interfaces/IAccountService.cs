using System.Threading;
using System.Threading.Tasks;
using Account.Dto.WebDtos;

namespace Account.Business.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        ///     Create account service.
        /// </summary>
        /// <param name="accountDto">Create account Dto</param>
        /// <param name="source">App source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created account dto</returns>
        Task<RegisterResponsetDto> RegisterAsync(RegisterRequestDto accountDto, string source, CancellationToken cancellationToken);

        /// <summary>
        ///     Login user to get token.
        /// </summary>
        /// <param name="dto">Login dto</param>
        /// <param name="source">App source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JWT string</returns>
        Task<string> LoginAsync(LoginDto dto, string source, CancellationToken cancellationToken);

        /// <summary>
        ///     Send email with code
        /// </summary>
        /// <param name="email">User Email</param>
        /// <param name="source">App source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task ForgotPasswordAsync(string email, string source, CancellationToken cancellationToken);
    }
}
