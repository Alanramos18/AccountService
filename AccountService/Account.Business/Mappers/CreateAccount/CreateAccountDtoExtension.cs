﻿using Account.Data.Entities;
using Account.Dto.WebDtos;

namespace Account.Business.Mappers.CreateAccount
{
    public static class CreateAccountDtoExtension
    {
        public static AccountEntity Convert(this CreateAccountDto dto)
        {
            if (dto == null)
                return null;

            var account = new AccountEntity
            {
                UserName = dto.UserName,
                Password = dto.Password,
                Application = dto.Application
            };

            return account;
        }
    }
}
