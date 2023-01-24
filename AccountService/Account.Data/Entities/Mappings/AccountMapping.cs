﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Data.Entities.Mappings
{
    internal class AccountMapping : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
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

            builder.Property(t => t.Hash)
                .HasColumnName("HASH")
                .IsRequired();

            builder.Property(t => t.ApplicationCode)
                .HasColumnName("APPLICATION_CODE");

            builder.Property(t => t.Verification)
                .HasColumnName("VERIFICATION")
                .IsRequired();
        }
    }
}
