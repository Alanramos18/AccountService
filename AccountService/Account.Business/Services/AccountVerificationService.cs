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
using Account.Data.Repositories.Interfaces;
using Account.Dto.WebDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Account.Business.Services
{
    public class AccountVerificationService : IAccountVerificationService
    {
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IEmailService _emailService;
        private readonly IEncryption _encryption;
        private readonly IConfiguration _configuration;

        public AccountVerificationService(IAccountVerificationRepository accountVerificationRepository,
            IEmailService emailService, IEncryption encryption, IConfiguration configuration)
        {
            _accountVerificationRepository = accountVerificationRepository;
            _emailService = emailService;
            _encryption = encryption;
            _configuration = configuration;
        }

        #region Register

        /// <inheritdoc />
        public async Task SendValidationLinkAsync(string email, string link, string source, CancellationToken cancellationToken)
        {
            try
            {
                var token = CreateToken();

                await _emailService.SendVerificationAsync(email, link, cancellationToken);

                await _accountVerificationRepository.AddAsync(new Data.Entities.AccountVerification
                {
                    Email = email,
                    ApplicationCode = source,
                    IsReset = false,
                    Token = token
                }, cancellationToken);

                await _accountVerificationRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc />
        public async Task ConfirmEmailAsync(string email, string token, string source, CancellationToken cancellationToken)
        {
            var emailExist = await _accountVerificationRepository.VerifyEmailCodeAsync(email, token, source, false, cancellationToken);

            if (!emailExist)
                throw new ApplicationException("Code or email does not matched");
        }

        #endregion

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
