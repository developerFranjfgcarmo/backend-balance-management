using BalanceManagement.Data.Configurations;
using BalanceManagement.Data.Entities;
using BalanceManagement.Data.Types;
using Microsoft.EntityFrameworkCore;

namespace BalanceManagement.Data.Extensions
{
    /// <summary>
    /// Loads the configuration of the entities and run the seed.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        public static void ConfigurationBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AccountTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
        }

        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = (int)Roles.Admin, Name = Roles.Admin.ToString() },
                new Role { Id = (int)Roles.User, Name = Roles.User.ToString() }
            );
        }
    }
}
