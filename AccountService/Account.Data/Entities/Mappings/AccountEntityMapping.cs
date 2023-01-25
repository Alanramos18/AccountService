using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Data.Entities.Mappings
{
    internal class AccountEntityMapping : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.Property(t => t.ApplicationCode)
                .HasColumnName("ApplicationCode");
        }
    }
}
