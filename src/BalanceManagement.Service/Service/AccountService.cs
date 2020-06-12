using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Contracts.Dtos.Users;
using BalanceManagement.Contracts.Mapper;
using BalanceManagement.Data.Context;
using BalanceManagement.Data.Entities;
using BalanceManagement.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BalanceManagement.Service.Service
{
    public class AccountService:ServiceBase, IAccountService
    {
        public AccountService(IBalanceManagementDbContext balanceManagementDbContext) : base(balanceManagementDbContext)
        {
        }

        public async Task<AccountDto> AddAsync(AccountDto accountDto)
        {
            Debug.Assert(accountDto == null);
            var entity = await BalanceManagementDbContext.Accounts.AddAsync(accountDto.MapTo<Account>());
            await SaveChangesAsync();
            return entity.Entity.MapTo<AccountDto>();
        }

        public async Task<AccountDto> UpdateAsync(AccountDto accountDto)
        {
            Debug.Assert(accountDto == null);
            var currentAccount = await GetEntityByIdAsync(accountDto.Id);
            var newAccount = accountDto.MapTo<Account>();
            BalanceManagementDbContext.Entry(currentAccount).CurrentValues.SetValues(newAccount);
            await SaveChangesAsync();
            return newAccount.MapTo<AccountDto>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetEntityByIdAsync(id);
            user.IsDeleted = true;
            return await SaveChangesAsync();
        }

        public Task<AccountDto> GetByUserIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedCollection<AccountDetailsDto>> GetBalanceByAccountAsync(AccountFilter accountFilter)
        {
            var result = await BalanceManagementDbContext.Accounts.AsNoTracking().Where(w =>
                    !w.IsDeleted && w.UserId == accountFilter.UserId && w.Id == accountFilter.AccountId)
                .Include(i => i.AccountBalances).Skip(accountFilter.Page * accountFilter.Take).Take(accountFilter.Take)
                .OrderBy(o => o.AccountBalances.OrderBy(oc => oc.Id)).ToListAsync();
            var total = await BalanceManagementDbContext.AccountBalances.CountAsync(c =>
                c.AccountId == accountFilter.AccountId);
            throw new NotImplementedException();
        }

        public async Task<bool> ModifyBalanceAsync(ModifyBalanceDto modifyBalance)
        {
            var lastTotal = await BalanceManagementDbContext.AccountBalances.OrderByDescending(m => m.Id).Select(s=>s.Total).FirstOrDefaultAsync();
            var balance = modifyBalance.MapTo<AccountBalance>();
            balance.Total = lastTotal + modifyBalance.Amount;
            balance.TransferDate = DateTime.Now;
            await BalanceManagementDbContext.AccountBalances.AddAsync(balance);
            return await SaveChangesAsync();
        }

        public Task<bool> BalanceTransferToUser(BalanceTransferDto balanceTransfer)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsOwnerAccount(int userId, int accountId)
        {
            return await BalanceManagementDbContext.Accounts.CountAsync(c => c.UserId == userId && c.Id== accountId) > 0;
        }

        private async Task<Account> GetEntityByIdAsync(int id)
        {
            return await BalanceManagementDbContext.Accounts.FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }

    }
}
