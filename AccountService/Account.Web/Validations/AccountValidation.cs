using Account.Business.Exceptions;
using Account.Dto.WebDtos;
using Account.Web.Validations.Interfaces;

namespace Account.Web.Validations
{
    public class AccountValidation : IAccountValidation
    {
        /// <inheritdoc/>
        public void Validate(CreateAccountDto createAccountDto)
        {
            ValidateUserName(createAccountDto.UserName);
            ValidatePassword(createAccountDto.Password);
            ValidateApplication(createAccountDto.Application);
        }

        /// <inheritdoc/>
        public void ValidateLogin(LoginDto loginDto)
        {
            ValidateUserName(loginDto.Email);
            ValidatePassword(loginDto.Password);
            //ValidateApplication(createAccountDto.Application);
        }

        /// <summary>
        ///     Validate the user name.
        /// </summary>
        /// <param name="username">User Name</param>
        private void ValidateUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new AccountException("El usuario no puede estar vacio");
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
                throw new AccountException("La contraseña no puede estar vacia");
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
