using System.Threading;
using System.Threading.Tasks;
using Account.Data.Entities;

namespace Account.Data.Repositories.Interfaces
{
    public interface IResetPasswordRepository
    {
        /// <summary>
        ///     Gets the specific entity.
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="appSource">Application source</param>
        /// <param name="code">Reset code</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        /// <returns></returns>
        Task<ResetPasswordEntity> GetAsync(string email, string appSource, string code, CancellationToken cancellationToken);

        /// <summary>
        ///     Adds the specific entity.
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task AddAsync(ResetPasswordEntity entity, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(ResetPasswordEntity entity);

        /// <summary>
        ///     Saves whatever entities have been added to the unit of work.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken">Cancellation Transaction Token</param>
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
