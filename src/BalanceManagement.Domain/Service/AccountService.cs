using System;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Data.Context;
using BalanceManagement.Service.IService;

namespace BalanceManagement.Service.Service
{
    public class AccountService:ServiceBase, IAccountService
    {
        public AccountService(IBalanceManagementDbContext balanceManagementDbContext) : base(balanceManagementDbContext)
        {
        }

        public Task<AccountDto> AddAsync(AccountDto accountDto)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDto> UpdateAsync(AccountDto accountDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDetailsDto> GetBalanceByAccountAsync(AccountFilter accountFilter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ModifyBalanceAsync(ModifyBalanceDto modifyBalance)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BalanceTransferToUser(BalanceTransferDto balanceTransfer)
        {
            throw new NotImplementedException();
        }
    }
}
