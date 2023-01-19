using System.Threading;
using System.Threading.Tasks;
using Account.Dto.WebDtos;

namespace Account.Business.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        ///     Create account service.
        /// </summary>
        /// <param name="accountDto">Create account Dto</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created account dto</returns>
        Task<CreatedAccountDto> CreateAccountAsync(CreateAccountDto accountDto, CancellationToken cancellationToken);


        Task<CreatedAccountDto> LoginAsync(CreateAccountDto accountDto, CancellationToken cancellationToken);
    }
}
