﻿using System.Threading;
using System.Threading.Tasks;
using Account.Business.Enums;
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

        /// <summary>
        ///     Login user to get token.
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JWT string</returns>
        Task<string> LoginAsync(LoginDto dto, ApplicationCode code, CancellationToken cancellationToken);
    }
}
