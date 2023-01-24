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
    public class AccountVerificationRepository : IAccountVerificationRepository
    {
        protected readonly IAccountContext _context;

        public AccountVerificationRepository(IAccountContext accountServiceContext)
        {
            _context = accountServiceContext ?? throw new ArgumentNullException(nameof(accountServiceContext));
        }

        /// <inheritdoc />
        public IQueryable<AccountVerification> Get()
        {
            return _context.Set<AccountVerification>();
        }

        /// <inheritdoc />
        public virtual async Task<AccountVerification> GetByIdAsync(int entityId, CancellationToken cancellationToken)
        {
            return await _context.Set<AccountVerification>().FindAsync(new object[] { entityId }, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<AccountVerification> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await Get().FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<bool> VerifyEmailCodeAsync(string email, string token, string source, bool isReset, CancellationToken cancellationToken)
        {
            return await Get().AnyAsync(x => x.Email.Equals(email) && x.ApplicationCode.Equals(source) && x.IsReset == isReset && x.Token.Equals(token), cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task AddAsync(AccountVerification entity, CancellationToken cancellationToken)
        {
            await _context.Set<AccountVerification>().AddAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task AddAsync(IEnumerable<AccountVerification> entities, CancellationToken cancellationToken)
        {
            await _context.Set<AccountVerification>().AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Delete(AccountVerification entity)
        {
            _context.Set<AccountVerification>().Remove(entity);
        }

        /// <inheritdoc />
        public virtual void Delete(IEnumerable<AccountVerification> entities)
        {
            _context.Set<AccountVerification>().RemoveRange(entities);
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
