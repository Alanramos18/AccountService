using Account.Data.Entities;
using Account.Data.Entities.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Account.Data
{
    /// <summary>
    ///     Account context that made DB connection.
    /// </summary>
    public class AccountContext : DbContext, IAccountContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }

        /// <inheritdoc />  
        public DbSet<AccountEntity> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AccountMapping());
        }
    }
}
