using Microsoft.EntityFrameworkCore;
using BalanceManagement.Data.Entities;

namespace BalanceManagement.Data.Context
{
    public class BalanceManagementDbContext: DbContext, IBalanceManagementDbContext
    {
        public BalanceManagementDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }
    }
}
