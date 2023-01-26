using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Account.Business.Exceptions;
using Account.Business.Helpers.Interfaces;
using Account.Business.Mappers.CreateAccount;
using Account.Business.Services.Interfaces;
using Account.Data.Entities;
using Account.Data.Repositories.Interfaces;
using Account.Dto.WebDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Account.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IEmailService _emailService;
        private readonly IEncryption _encryption;
        private readonly IConfiguration _configuration;

        public AccountService(IAccountRepository accountRepository, IAccountVerificationRepository accountVerificationRepository,
            IEmailService emailService, IEncryption encryption, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _accountVerificationRepository = accountVerificationRepository;
            _emailService = emailService;
            _encryption = encryption;
            _configuration = configuration;
        }

        #region Register

        /// <inheritdoc />
        public async Task<AccountEntity> RegisterAsync(RegisterRequestDto accountDto, string source, CancellationToken cancellationToken)
        {
            try
            {
                var email = await _accountRepository.CheckRegisteredEmailAsync(accountDto.Email, source, cancellationToken);

                if (email)
                    throw new ApplicationException("That email is already registered");

                var account = accountDto.Convert(source);

                var result = await _accountRepository.CreateAsync(account, accountDto.Password);

                if (!result.Succeeded)
                {
                    throw new ApplicationException("Error creating user");
                }

                //var response = account.Convert();

                return account;
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public async Task SendVerificationEmailAsync(AccountEntity account, string hostLink, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _accountRepository.GenerateEmailConfirmationTokenAsync(account);

                var confirmationLink = string.Concat(hostLink, "&token=", token);

                await _emailService.SendVerificationAsync(account.Email, confirmationLink, cancellationToken);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string email, string token, string appSource, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.FindByEmailAsync(email, appSource, cancellationToken);

            if (user == null)
                throw new ApplicationException("That email is not register");

            var result = await _accountRepository.ConfirmEmailAsync(user, token);

            return result;
        }

        #endregion

        #region Login

        /// <inheritdoc />
        public async Task<string> LoginAsync(LoginDto loginDto, string code, CancellationToken cancellationToken)
        {
            //var user = await _accountRepository.GetByEmailAsync(loginDto.Email, cancellationToken);

            //if (user == null || !_encryption.Verify(loginDto.Password, user.Hash))
            //{
            //    throw new AccountException("The email or the password are incorrect");
            //}

            var jwt = CreateToken();

            return jwt;
        }

        #endregion

        #region Forgot Password

        /// <inheritdoc />
        public async Task ForgotPasswordAsync(string email, string source, CancellationToken cancellationToken)
        {
            var emailExist = await _accountRepository.CheckRegisteredEmailAsync(email, source, cancellationToken);

            if (!emailExist)
                return;

            // generate random 6 number code
            var code = "123456";

            await _emailService.SendResetCodeAsync(email, code, cancellationToken);
        }

        public async Task VerifyResetCodeAsync(string code, CancellationToken cancellationToken)
        {

        }

        #endregion



        private string CreateToken()
        {
            List<Claim> claims = new List<Claim>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // ADD CLAIMS

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
