using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;
using BalanceManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BalanceManagement.Data.Context
{
    public interface IBalanceManagementDbContext :IDisposable
    {
        DbSet<User> Users  { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<AccountTransaction> AccountTransactions { get; set; }
        DatabaseFacade Database { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken());

        EntityEntry Entry(object entity);
    }
}
