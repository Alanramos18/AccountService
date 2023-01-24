using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Account.Data.Entities;

namespace Account.Data.Repositories.Interfaces
{
    public interface IAccountVerificationRepository
    {
        /// <summary>
        ///     Get All.
        /// </summary>
        IQueryable<AccountVerification> Get();

        /// <summary>
        ///     Gets the specific entity.
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        /// <returns></returns>
        Task<AccountVerification> GetByIdAsync(int entityId, CancellationToken cancellationToken);

        /// <summary>
        ///     Get email by username.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        /// <returns></returns>
        Task<AccountVerification> GetByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        ///     Verify the token in a given email matches
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="token">Token code</param>
        /// <param name="source">Application Source</param>
        /// <param name="isReset">Is reset code?</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Boolean</returns>
        Task<bool> VerifyEmailCodeAsync(string email, string token, string source, bool isReset, CancellationToken cancellationToken);

        /// <summary>
        ///     Adds the specific entity.
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task AddAsync(AccountVerification entity, CancellationToken cancellationToken);

        /// <summary>
        ///     Adds the specifics entities.
        /// </summary>
        /// <param name="entities">Entities to be added.</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task AddAsync(IEnumerable<AccountVerification> entities, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(AccountVerification entity);

        /// <summary>
        ///     Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<AccountVerification> entities);

        /// <summary>
        ///     Saves whatever entities have been added to the unit of work.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
