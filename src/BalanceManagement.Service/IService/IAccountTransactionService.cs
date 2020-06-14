using System.Collections.Generic;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;

namespace BalanceManagement.Service.IService
{
    public interface IAccountTransactionService
    {
        Task<bool> ModifyBalanceAsync(ModifyBalanceDto modifyBalance);
        Task<bool> BalanceTransferToUserAsync(BalanceTransferDto balanceTransfer);
        Task<IEnumerable<AccountBalanceDto>> GetAccountsWithBalanceByUserIdAsync(int userId);
        Task<PagedCollection<AccountTransactionsDto>> GetTransactionByAccountAsync(AccountFilter accountFilter);
    }
}
