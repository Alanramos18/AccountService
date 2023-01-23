using System.Text.RegularExpressions;
using Account.Business.Exceptions;
using Account.Dto.WebDtos;
using Account.Web.Validations.Interfaces;

namespace Account.Web.Validations
{
    public class AccountValidation : IAccountValidation
    {
        /// <inheritdoc/>
        public void Validate(RegisterRequestDto createAccountDto)
        {
            ValidateEmail(createAccountDto.Email);
            ValidatePassword(createAccountDto.Password);
            //ValidateApplication(createAccountDto.Application);
        }

        /// <inheritdoc/>
        public void ValidateLogin(LoginDto loginDto)
        {
            ValidateEmail(loginDto.Email);
            ValidatePassword(loginDto.Password);
            //ValidateApplication(createAccountDto.Application);
        }

        /// <summary>
        ///     Validate the email.
        /// </summary>
        /// <param name="email">Email</param>
        public void ValidateEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            var result = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            
            if (!result)
            {
                throw new AccountException("The email is invalid");
            }
        }

        /// <summary>
        ///     Validate the user password.
        /// </summary>
        /// <param name="password">User Password</param>
        private void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new AccountException("The password cannot be empty");
            }
        }

        private void ValidateApplication(string app)
        {
            if (string.IsNullOrEmpty(app))
            {
                throw new AccountException("La aplicacion de origen no puede estar vacia");
            }
        }
    }
}
