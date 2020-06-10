using BalanceManagement.Data.Context;
using System;
using System.Threading.Tasks;

namespace BalanceManagement.Domain
{
    public class DomainBase : IDisposable
    {
        public DomainBase(IBalanceManagementDbContext balanceManagementDbContext)
        {
            BalanceManagementDbContext = balanceManagementDbContext;
        }
        public IBalanceManagementDbContext BalanceManagementDbContext { get; set; }

        protected bool SaveChanges()
        {
            return BalanceManagementDbContext.SaveChanges() > 0;
        }

        protected async Task<bool> SaveChangesAsync()
        {
            return await BalanceManagementDbContext.SaveChangesAsync() > 0;
        }

        #region [Disposable]
        ~DomainBase()
        {
            Dispose(false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
                BalanceManagementDbContext.Dispose();
        }

        public void Dispose()
        {
            Dispose(false);
        }
        #endregion

    }
}
