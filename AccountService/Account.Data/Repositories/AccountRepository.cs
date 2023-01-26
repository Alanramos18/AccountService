using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Account.Data.Entities;
using Account.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Account.Data.Repositories
{
    public class AccountRepository : UserManager<AccountEntity>, IAccountRepository
    {
        private readonly UserStore<AccountEntity, IdentityRole, AccountContext, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>
        _store;

        public AccountRepository(IUserStore<AccountEntity> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<AccountEntity> passwordHasher,
        IEnumerable<IUserValidator<AccountEntity>> userValidators,
        IEnumerable<IPasswordValidator<AccountEntity>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<AccountEntity>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _store = (UserStore<AccountEntity, IdentityRole, AccountContext, string, IdentityUserClaim<string>,
            IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)store;
        }

        public async Task<bool> CheckRegisteredEmailAsync(string email, string appSource, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(appSource))
                throw new ArgumentNullException(nameof(appSource));

            var emailExist = await _store.Context.Set<AccountEntity>().AnyAsync(x => x.Email.Equals(email) && x.ApplicationCode.Equals(appSource));

            return emailExist;
        }

        public override Task<IdentityResult> CreateAsync(AccountEntity entity, string password)
        {
            return base.CreateAsync(entity, password);
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(AccountEntity entity)
        {
            return base.GenerateEmailConfirmationTokenAsync(entity);
        }

        public override Task<IdentityResult> ConfirmEmailAsync(AccountEntity entity, string token)
        {
            return base.ConfirmEmailAsync(entity, token);
        }

        public async Task<AccountEntity> FindByEmailAsync(string email, string appSource, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(appSource))
                throw new ArgumentNullException(nameof(appSource));

            var user = await _store.Context.Set<AccountEntity>().FirstOrDefaultAsync(x => x.Email.Equals(email) && x.ApplicationCode.Equals(appSource));
         
            return user;
        }
    }
}
