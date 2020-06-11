using System;
using System.Threading.Tasks;
using BalanceManagement.Data.Context;

namespace BalanceManagement.Service
{
    public class ServiceBase : IDisposable
    {
        public ServiceBase(IBalanceManagementDbContext balanceManagementDbContext)
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
        ~ServiceBase()
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
