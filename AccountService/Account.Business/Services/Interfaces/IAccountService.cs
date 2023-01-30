﻿using System.Threading;
using System.Threading.Tasks;
using Account.Data.Entities;
using Account.Dto.WebDtos;
using Microsoft.AspNetCore.Identity;

namespace Account.Business.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        ///     Create account service.
        /// </summary>
        /// <param name="accountDto">Create account Dto</param>
        /// <param name="source">App source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created account dto</returns>
        Task<AccountEntity> RegisterAsync(RegisterRequestDto accountDto, string source, CancellationToken cancellationToken);

        Task SendVerificationEmailAsync(AccountEntity account, string hostLink, CancellationToken cancellationToken);
        Task<IdentityResult> ConfirmEmailAsync(string email, string token, string AppSource, CancellationToken cancellationToken);
        Task<string> VerifyResetCodeAsync(string email, string appSource, string code, CancellationToken cancellationToken);
        Task ChangePasswordAsync(string email, string appSource, string newPassword, string token, CancellationToken cancellationToken);

        /// <summary>
        ///     Login user to get token.
        /// </summary>
        /// <param name="dto">Login dto</param>
        /// <param name="source">App source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JWT string</returns>
        Task<string> LoginAsync(LoginDto dto, string source, CancellationToken cancellationToken);

        /// <summary>
        ///     Send email with code
        /// </summary>
        /// <param name="email">User Email</param>
        /// <param name="source">App source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task ForgotPasswordAsync(string email, string source, CancellationToken cancellationToken);
    }
}
