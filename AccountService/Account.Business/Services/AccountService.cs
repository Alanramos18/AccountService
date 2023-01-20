using Account.Business.Enums;
using Account.Business.Exceptions;
using Account.Business.Helpers;
using Account.Business.Mappers.CreateAccount;
using Account.Business.Services.Interfaces;
using Account.Data.Entities;
using Account.Data.Repositories.Interfaces;
using Account.Dto.WebDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEncryption _encryption;
        private readonly IConfiguration _configuration;

        public AccountService(IAccountRepository accountRepository, IEncryption encryption, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _encryption = encryption;
            _configuration = configuration;
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
        public async Task<string> LoginAsync(LoginDto loginDto, ApplicationCode code, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.GetByEmailAsync(loginDto.Email, cancellationToken);

            if (user == null || _encryption.Verify(loginDto.Password, user.Hash))
            {
                throw new AccountException("El usuario o la contraseña son incorrectos");
            }

            var jwt = CreateToken();

            // map response

            return jwt;
        }

        private string CreateToken()
        {
            List<Claim> claims = new List<Claim>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
