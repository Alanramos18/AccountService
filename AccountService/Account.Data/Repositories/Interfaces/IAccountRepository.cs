using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Account.Data.Entities;

namespace Account.Data.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        /// <summary>
        ///     Get All.
        /// </summary>
        IQueryable<AccountEntity> Get();

        /// <summary>
        ///     Gets the specific entity.
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        /// <returns></returns>
        Task<AccountEntity> GetByIdAsync(int entityId, CancellationToken cancellationToken);

        /// <summary>
        ///     Get email by username.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        /// <returns></returns>
        Task<AccountEntity> GetByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        ///     Adds the specific entity.
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task AddAsync(AccountEntity entity, CancellationToken cancellationToken);

        /// <summary>
        ///     Adds the specifics entities.
        /// </summary>
        /// <param name="entities">Entities to be added.</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task AddAsync(IEnumerable<AccountEntity> entities, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(AccountEntity entity);

        /// <summary>
        ///     Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<AccountEntity> entities);

        /// <summary>
        ///     Saves whatever entities have been added to the unit of work.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
