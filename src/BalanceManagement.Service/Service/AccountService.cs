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
using Microsoft.EntityFrameworkCore.Storage;

namespace BalanceManagement.Service.Service
{
    public class AccountService : ServiceBase, IAccountService
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
    
        public async Task<bool> IsOwnerAccountAsync(int userId, int accountId)
        {
            return await BalanceManagementDbContext.Accounts
                       .CountAsync(c => c.UserId == userId && c.Id == accountId) >0;
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