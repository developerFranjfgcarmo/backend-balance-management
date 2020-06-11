using BalanceManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BalanceManagement.Data.Configurations
{
    public class AccountBalanceConfiguration: IEntityTypeConfiguration<AccountBalance>
    {
        public void Configure(EntityTypeBuilder<AccountBalance> builder)
        {
            builder.ToTable("AccountBalance");
            builder.Property(p => p.Description).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Amount).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(p => p.Total).HasColumnType("decimal(18,2)").IsRequired();
            builder.HasOne(ho => ho.Account).WithMany(wm => wm.AccountBalances).HasForeignKey(fk => fk.AccountId);
            builder.HasOne(ho => ho.User).WithMany(wm => wm.AccountBalances).HasForeignKey(fk => fk.TransferredByUser);
        }
    }
}
