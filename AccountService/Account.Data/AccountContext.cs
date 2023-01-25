using Account.Data.Entities;
using Account.Data.Entities.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Account.Data
{
    /// <summary>
    ///     Account context that made DB connection.
    /// </summary>
    public class AccountContext : IdentityDbContext<AccountEntity>, IAccountContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }

        /// <inheritdoc />  
        public DbSet<AccountEntity> Accounts { get; set; }
        //public DbSet<AccountVerification> AccountsVerification { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);

            builder.ApplyConfiguration(new AccountEntityMapping());
            //builder.ApplyConfiguration(new AccountVerificationMapping());
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole { Name = "Unlam", NormalizedName = "Unlam", ConcurrencyStamp = "1" }
                );
        }
    }
}
