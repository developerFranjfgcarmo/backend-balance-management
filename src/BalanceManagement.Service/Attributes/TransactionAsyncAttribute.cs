using System;
using System.Threading.Tasks;
using ArxOne.MrAdvice.Advice;

namespace BalanceManagement.Service.Attributes
{
    /// <summary>
    ///  Aspect as attribute that initiates a transaction, perform commit when ends succeful, or call rollback on exception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionAsyncAttribute : Attribute, IMethodAsyncAdvice
    {
        public async Task Advise(MethodAsyncAdviceContext context)
        {
            var service = (ServiceBase)context.Target;

            if (service.BalanceManagementDbContext.Database.CurrentTransaction == null)
            {
                await using var transaction = await service.BalanceManagementDbContext.Database.BeginTransactionAsync();
                try
                {
                    await context.ProceedAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await context.ProceedAsync();
            }
        }
    }
}
