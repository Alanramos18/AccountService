using Account.Dto.WebDtos;
using Account.Data.Entities;

namespace Account.Business.Mappers.CreateAccount
{
    public static class AccountExtension
    {
        public static CreatedAccountDto Convert(this AccountEntity account)
        {
            if (account == null)
                return null;

            var dto = new CreatedAccountDto
            {
                Id = account.Id,
                UserName = account.UserName
            };

            return dto;
        }
    }
}
