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
        ///// <summary>
        /////     Get All.
        ///// </summary>
        //IQueryable<AccountEntity> Get();

        ///// <summary>
        /////     Gets the specific entity.
        ///// </summary>
        ///// <param name="entityId">Id of the entity</param>
        ///// <param name="cancellationToken">Cancellation Transaction Token</param>
        ///// <returns></returns>
        //Task<AccountEntity> GetByIdAsync(int entityId, CancellationToken cancellationToken);

        ///// <summary>
        /////     Get email by username.
        ///// </summary>
        ///// <param name="email">Email</param>
        ///// <param name="cancellationToken">Cancellation Transaction Token</param>
        ///// <returns></returns>
        //Task<AccountEntity> GetByEmailAsync(string email, CancellationToken cancellationToken);

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

        ///// <summary>
        /////     Deletes the specified entity.
        ///// </summary>
        ///// <param name="entity">The entity.</param>
        //void Delete(AccountEntity entity);

        ///// <summary>
        /////     Deletes the specified entities.
        ///// </summary>
        ///// <param name="entities">The entities.</param>
        //void Delete(IEnumerable<AccountEntity> entities);

        ///// <summary>
        /////     Saves whatever entities have been added to the unit of work.
        ///// </summary>
        ///// <param name="identifier"></param>
        ///// <param name="cancellationToken">Cancellation Transaction Token</param>
        //Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
