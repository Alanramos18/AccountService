using Account.Business.Utils;
using Account.Data.Entities;
using Account.Dto.WebDtos;

namespace Account.Business.Mappers.CreateAccount
{
    public static class CreateAccountDtoExtension
    {
        public static AccountEntity Convert(this RegisterRequestDto dto, string source)
        {
            if (dto == null)
                return null;

            var account = new AccountEntity
            {
                Email = dto.Email,
                ApplicationCode = source,
                Verification = (int)Constants.AccountVerified.NotVerified
            };

            return account;
        }
    }
}
