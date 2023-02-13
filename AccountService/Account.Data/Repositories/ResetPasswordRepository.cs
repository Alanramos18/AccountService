using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Account.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Account.Data.Entities;

namespace Account.Data.Repositories
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly IAccountContext _context;

        public ResetPasswordRepository(IAccountContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<ResetPasswordEntity> GetAsync(string email, string appSource, string code, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<ResetPasswordEntity>().FirstOrDefaultAsync(x => x.Email.Equals(email) 
                && x.ApplicationCode.Equals(appSource) && x.DigitCode.Equals(code), cancellationToken);

            return entity;
        }

        /// <inheritdoc />
        public async Task AddAsync(ResetPasswordEntity entity, CancellationToken cancellationToken)
        {
            await _context.Set<ResetPasswordEntity>().AddAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Delete(ResetPasswordEntity entity)
        {
            _context.Set<ResetPasswordEntity>().Remove(entity);
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
