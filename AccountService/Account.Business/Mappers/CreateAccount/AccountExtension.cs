using Account.Dto.WebDtos;
using Account.Data.Entities;

namespace Account.Business.Mappers.CreateAccount
{
    public static class AccountExtension
    {
        public static RegisterResponsetDto Convert(this AccountEntity account)
        {
            if (account == null)
                return null;

            var dto = new RegisterResponsetDto
            {
                Id = account.Id,
                Email = account.Email
            };

            return dto;
        }
    }
}
