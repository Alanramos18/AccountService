using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Account.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Account.Data.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        ///     Check whether an email in that source exists.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="appSource">Application Source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<bool> CheckRegisteredEmailAsync(string email, string appSource, CancellationToken cancellationToken);

        /// <summary>
        ///     Adds the specific entity.
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        /// <param name="password">Cancellation Transaction Token</param>
        Task<IdentityResult> CreateAsync(AccountEntity entity, string password);

        /// <summary>
        ///     Generate an email confirmation token.
        /// </summary>
        /// <param name="entity">Entity to confirm email.</param>
        Task<string> GenerateEmailConfirmationTokenAsync(AccountEntity entity);

        Task<IdentityResult> ConfirmEmailAsync(AccountEntity entity, string token);

        Task<AccountEntity> FindByEmailAsync(string email, string appSource, CancellationToken cancellationToken);

        Task<bool> CheckPasswordAsync(AccountEntity user, string password);

        Task<string> GeneratePasswordResetTokenAsync(AccountEntity user);

        Task<IdentityResult> ResetPasswordAsync(AccountEntity user, string token, string newPassword);
    }
}
