using System.Threading;
using System.Threading.Tasks;

namespace Account.Business.Helpers.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        ///     Send email.
        /// </summary>
        /// <param name="to">Email that will received the mail</param>
        /// <param name="link">Verification link</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="from">Email Sender</param>
        /// <returns></returns>
        Task SendVerificationAsync(string to, string link, CancellationToken cancellationToken, string from = null);

        /// <summary>
        ///     Send reset code email
        /// </summary>
        /// <param name="to">Destination email</param>
        /// <param name="code">Reset code</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="from">Origin email sender</param>
        Task SendResetCodeAsync(string to, string code, CancellationToken cancellationToken, string from = null);
    }
}
