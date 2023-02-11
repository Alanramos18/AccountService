using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Account.Business.Exceptions;
using Account.Business.Helpers.Interfaces;
using Account.Business.Mappers.CreateAccount;
using Account.Business.Services.Interfaces;
using Account.Data.Entities;
using Account.Data.Repositories.Interfaces;
using Account.Dto.Shared;
using Account.Dto.WebDtos;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Account.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IResetPasswordRepository _resetPasswordRepository;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, IResetPasswordRepository resetPasswordRepository,
            IEmailService emailService, IOptions<JwtSettings> jwtSettings, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _resetPasswordRepository = resetPasswordRepository;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        #region Register

        /// <inheritdoc />
        public async Task<RegisterResponsetDto> RegisterAsync(RegisterRequestDto accountDto, string confirmationLink, string source, CancellationToken cancellationToken)
        {
            try
            {
                var email = await _accountRepository.CheckRegisteredEmailAsync(accountDto.Email, source, cancellationToken);

                if (email)
                    return null;

                var account = accountDto.Convert(source);

                var result = await _accountRepository.CreateAsync(account, accountDto.Password);

                if (!result.Succeeded)
                {
                    throw new ApplicationException("Error creating user");
                }

                await SendVerificationEmailAsync(account, confirmationLink, cancellationToken);

                var response = account.Convert();

                return response;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Send Verification email.
        /// </summary>
        /// <param name="account">Account to create token for</param>
        /// <param name="confirmationLink">Link to send in the html</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        private async Task SendVerificationEmailAsync(AccountEntity account, string confirmationLink, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _accountRepository.GenerateEmailConfirmationTokenAsync(account);

                confirmationLink = string.Concat(confirmationLink, "?email=", account.Email, "&token=", HttpUtility.UrlEncode(token));

                await _emailService.SendVerificationAsync(account.Email, confirmationLink, cancellationToken);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IdentityResult> ConfirmEmailAsync(string email, string token, string appSource, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.FindByEmailAsync(email, appSource, cancellationToken);

            if (user == null)
                return null;

            var result = await _accountRepository.ConfirmEmailAsync(user, token);

            return result;
        }

        #endregion

        #region Login

        /// <inheritdoc />
        public async Task<string> LoginAsync(LoginDto loginDto, string appSource, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.FindByEmailAsync(loginDto.Email, appSource, cancellationToken);

            if (user == null || !await _accountRepository.CheckPasswordAsync(user, loginDto.Password))
            {
                _logger.LogInformation($"The email: { loginDto.Email } has try to log in with password: { loginDto.Password }");
                throw new ApplicationException("Email or password is invalid");
            }

            if (user.EmailConfirmed == false) { throw new ApplicationException("Please validate your account to log in"); }

            // We need to update claims in the future.
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwt = CreateToken(authClaims);

            return jwt;
        }

        #endregion

        #region Forgot Password

        /// <inheritdoc />
        public async Task ForgotPasswordAsync(string email, string source, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _accountRepository.FindByEmailAsync(email, source, cancellationToken);

                if (user == null)
                    return;

                Random generator = new Random();
                String code = generator.Next(0, 1000000).ToString("D6");

                var token = await _accountRepository.GeneratePasswordResetTokenAsync(user);

                var savePassword = new ResetPasswordEntity
                {
                    Email = email,
                    ApplicationCode = source,
                    DigitCode = code,
                    Token = token
                };

                await _resetPasswordRepository.AddAsync(savePassword, cancellationToken);
                await _resetPasswordRepository.SaveChangesAsync(cancellationToken);

                await _emailService.SendResetCodeAsync(email, code, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> VerifyResetCodeAsync(string email, string appSource, string code, CancellationToken cancellationToken)
        {
            var entity = await _resetPasswordRepository.GetAsync(email, appSource, code, cancellationToken);

            if (entity == null)
                throw new ApplicationException("There no entity with that parameters");
            
            _resetPasswordRepository.Delete(entity);
            await _resetPasswordRepository.SaveChangesAsync(cancellationToken);

            return entity.Token;
        }

        public async Task ChangePasswordAsync(string email, string appSource, string newPassword, string token, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _accountRepository.FindByEmailAsync(email, appSource, cancellationToken);

                if (user == null)
                    return;

                var result = await _accountRepository.ResetPasswordAsync(user, token, newPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private string CreateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
