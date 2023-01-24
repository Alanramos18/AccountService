using System.Threading;
using System.Threading.Tasks;

namespace Account.Business.Services.Interfaces
{
    public interface IAccountVerificationService
    {
        /// <summary>
        ///     Send validation email.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="link">Validation link</param>
        /// <param name="source">App source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created account dto</returns>
        Task SendValidationLinkAsync(string email, string link, string source, CancellationToken cancellationToken);

        /// <summary>
        ///     Verify and confirm the email code.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="token">Activation token/code</param>
        /// <param name="source">App origin</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task ConfirmEmailAsync(string email, string token, string source, CancellationToken cancellationToken);
    }
}
