using Account.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Data
{
    public interface IAccountContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        ///     Accounts entities.
        /// </summary>
        DbSet<AccountEntity> Accounts { get; set; }

        /// <summary>
        ///     Reset password entities.
        /// </summary>
        DbSet<ResetPasswordEntity> ResetPasswords { get; set; }
    }
}