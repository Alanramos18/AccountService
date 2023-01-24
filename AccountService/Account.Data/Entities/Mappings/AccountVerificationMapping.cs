using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Data.Entities.Mappings
{
    internal class AccountVerificationMapping : IEntityTypeConfiguration<AccountVerification>
    {
        public void Configure(EntityTypeBuilder<AccountVerification> builder)
        {
            builder.ToTable("ACCOUNTS");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("ID")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Email)
                .HasColumnName("EMAIL")
                .IsRequired();

            builder.Property(t => t.ApplicationCode)
                .HasColumnName("APPLICATION_CODE")
                .IsRequired();

            builder.Property(t => t.IsReset)
                .HasColumnName("IS_REST")
                .IsRequired();

            builder.Property(t => t.Token)
                .HasColumnName("TOKEN")
                .IsRequired();
        }
    }
}
