using Microsoft.EntityFrameworkCore;
using BalanceManagement.Data.Entities;
using BalanceManagement.Data.Extensions;

namespace BalanceManagement.Data.Context
{
    public class BalanceManagementDbContext: DbContext, IBalanceManagementDbContext
    {
        public BalanceManagementDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigurationBuilder();
            modelBuilder.SeedRoles();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
    }
}
