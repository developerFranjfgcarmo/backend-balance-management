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
        Task<bool> DeleteAsync(int id);
        Task<AccountDto> GetByUserIdAsync(int id);
        Task<PagedCollection<AccountDetailsDto>> GetBalanceByAccountAsync(AccountFilter accountFilter);
        Task<bool> ModifyBalanceAsync( ModifyBalanceDto modifyBalance);
        Task<bool> BalanceTransferToUser(BalanceTransferDto balanceTransfer);
        Task<bool> IsOwnerAccount(int userId, int accountId);
    }
}
