using System.Collections.Generic;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;

namespace BalanceManagement.Service.IService
{
    public interface IAccountService
    {
        Task<AccountDto> AddAsync(AccountDto accountDto);
        Task<AccountDto> UpdateAsync(AccountDto accountDto);
        Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(int userId);
        Task<bool> DeleteAsync(int id);
        Task<PagedCollection<AccountBalanceDto>> GetBalanceByAccountAsync(AccountFilter accountFilter);
        Task<bool> ModifyBalanceAsync( ModifyBalanceDto modifyBalance);
        Task<bool> BalanceTransferToUserAsync(BalanceTransferDto balanceTransfer);
        Task<bool> IsOwnerAccountAsync(int userId, int accountId);
        Task<PagedCollection<AccountDto>> GetListAsync(int? userId,PagedFilter pagedFilter);
    }
}
