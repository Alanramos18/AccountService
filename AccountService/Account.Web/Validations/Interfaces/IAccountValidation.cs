﻿using Account.Dto.WebDtos;

namespace Account.Web.Validations.Interfaces
{
    public interface IAccountValidation
    {
        /// <summary>
        ///     Validate properties of the account
        /// </summary>
        /// <param name="createAccountDto">Create account Dto</param>
        void Validate(CreateAccountDto createAccountDto);
    }
}
