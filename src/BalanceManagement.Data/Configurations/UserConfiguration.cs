using System;
using BalanceManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BalanceManagement.Data.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasIndex(i=>i.UserName);
            builder.Property(p => p.Password).IsRequired().HasMaxLength(250);
            builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Surname).HasMaxLength(50);
            builder.Property(p => p.Nick).HasMaxLength(50);
            builder.Property(p => p.PhoneNumber).HasMaxLength(15);
            builder.Property(p => p.Street).HasMaxLength(250);
            builder.Property(p => p.PostalCode).HasMaxLength(6);
            builder.Property(p => p.City).HasMaxLength(50);
            builder.HasOne(ho => ho.Role).WithMany(wm => wm.User).HasForeignKey(fk => fk.RoleId);
        }
    }
}
