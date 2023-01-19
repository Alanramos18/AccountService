using Account.Business.Exceptions;
using Account.Business.Helpers;
using Account.Business.Mappers.CreateAccount;
using Account.Business.Services.Interfaces;
using Account.Data.Repositories.Interfaces;
using Account.Dto.WebDtos;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Business.Services
{
    internal class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEncryption _encryption;

        public AccountService(IAccountRepository accountRepository, IEncryption encryption)
        {
            _accountRepository = accountRepository;
            _encryption = encryption;
        }

        /// <inheritdoc />
        public async Task<CreatedAccountDto> CreateAccountAsync(CreateAccountDto accountDto, CancellationToken cancellationToken)
        {
            try
            {
                var account = accountDto.Convert();

                await _accountRepository.AddAsync(account, cancellationToken);

                var createdAccount = account.Convert();

                return createdAccount;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <inheritdoc />
        public async Task LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.GetByUsernameAsync(username, cancellationToken);

            if (user == null || _encryption.Verify(password, user.Hash))
            {
                throw new AccountException("El usuario o la contraseña son incorrectos");
            }

            // Generate JWT

            // map response

            // return response
        }
    }
}
