using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;
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
        
        public async Task<PagedCollection<AccountBalanceDto>> GetBalanceByAccountAsync(AccountFilter accountFilter)
        {
            var pagedCollection = new PagedCollection<AccountBalanceDto>
            {
               Items = await (from a in BalanceManagementDbContext.Accounts
                   join ab in BalanceManagementDbContext.AccountBalances on a.Id equals ab.AccountId
                   join u in BalanceManagementDbContext.Users on ab.TransferredByUser equals u.Id into uab
                   from x in uab.DefaultIfEmpty()
                              orderby ab.Id descending 
                   where a.UserId == accountFilter.UserId && !a.IsDeleted && a.Id == accountFilter.AccountId
                   select new AccountBalanceDto
                   {
                       Id = ab.Id,
                       TransferredByUser = $"{x.FirstName} {x.Surname}",
                       Total = ab.Total,
                       Description = ab.Description,
                       TransferDate = ab.TransferDate,
                       Amount = ab.Amount

                   }).Skip(accountFilter.Page * accountFilter.Take).Take(accountFilter.Take).ToListAsync(),

                    Total = await BalanceManagementDbContext.AccountBalances.CountAsync(c =>
                    c.AccountId == accountFilter.AccountId)
            };

            return pagedCollection;
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

        public async Task<PagedCollection<AccountDto>> GetListAsync(int? userId, PagedFilter pagedFilter)
        {
            var pagedCollection = new PagedCollection<AccountDto>
            {
                Items = (await BalanceManagementDbContext.Accounts
                    .Where(w => !w.IsDeleted && w.UserId == (userId ?? w.UserId))
                    .Skip(pagedFilter.Page * pagedFilter.Take)
                    .Take(pagedFilter.Take).AsNoTracking().ToListAsync()).MapTo<List<AccountDto>>(),
                Total = await BalanceManagementDbContext.Accounts.CountAsync(w => !w.IsDeleted)
            };
            return pagedCollection;
        }

        private async Task<Account> GetEntityByIdAsync(int id)
        {
            return await BalanceManagementDbContext.Accounts.FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }

    }
}
