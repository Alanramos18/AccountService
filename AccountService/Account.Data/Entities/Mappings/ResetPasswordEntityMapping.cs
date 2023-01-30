using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Data.Entities.Mappings
{
    internal class ResetPasswordEntityMapping : IEntityTypeConfiguration<ResetPasswordEntity>
    {
        public void Configure(EntityTypeBuilder<ResetPasswordEntity> builder)
        {
            builder.ToTable("ResetPassword");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Email)
                .HasColumnName("Email")
                .IsRequired();

            builder.Property(t => t.ApplicationCode)
                .HasColumnName("ApplicationCode")
                .IsRequired();

            builder.Property(t => t.DigitCode)
                .HasColumnName("DigitCode")
                .IsRequired();

            builder.Property(t => t.Token)
                .HasColumnName("Token")
                .IsRequired();
        }
    }
}
