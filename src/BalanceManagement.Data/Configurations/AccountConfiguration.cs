using System;
using System.Collections.Generic;
using System.Text;
using BalanceManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BalanceManagement.Data.Configurations
{
    public class AccountConfiguration:IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
            builder.HasOne(ho => ho.User).WithMany(wm => wm.Accounts).HasForeignKey(fk => fk.UserId);
        }
    }
}
