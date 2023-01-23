using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Account.Data.Entities;
using Account.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Account.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        protected readonly IAccountContext _context;

        public AccountRepository(IAccountContext accountServiceContext)
        {
            _context = accountServiceContext ?? throw new ArgumentNullException(nameof(accountServiceContext));
        }

        /// <inheritdoc />
        public IQueryable<AccountEntity> Get()
        {
            return _context.Set<AccountEntity>();
        }

        /// <inheritdoc />
        public virtual async Task<AccountEntity> GetByIdAsync(int entityId, CancellationToken cancellationToken)
        {
            return await _context.Set<AccountEntity>().FindAsync(new object[] { entityId }, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<AccountEntity> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await Get().FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<bool> CheckRegisteredEmailAsync(string email, string source, CancellationToken cancellationToken)
        {
            return await Get().AnyAsync(x => x.Email.Equals(email) && x.ApplicationCode.Equals(source), cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task AddAsync(AccountEntity entity, CancellationToken cancellationToken)
        {
            await _context.Set<AccountEntity>().AddAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task AddAsync(IEnumerable<AccountEntity> entities, CancellationToken cancellationToken)
        {
            await _context.Set<AccountEntity>().AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Delete(AccountEntity entity)
        {
            _context.Set<AccountEntity>().Remove(entity);
        }

        /// <inheritdoc />
        public virtual void Delete(IEnumerable<AccountEntity> entities)
        {
            _context.Set<AccountEntity>().RemoveRange(entities);
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await (_context as DbContext).SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
