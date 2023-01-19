using System.Threading;
using System.Threading.Tasks;
using Account.Business.Mappers.CreateAccount;
using Account.Business.Services.Interfaces;
using Account.Data.Repositories.Interfaces;
using Account.Dto.WebDtos;

namespace Account.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
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

        public async Task LoginAsync()
        {

        }
    }
}
