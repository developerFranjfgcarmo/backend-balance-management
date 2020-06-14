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
        public async Task<bool> ModifyBalanceAsync(ModifyBalanceDto modifyBalance)
        {
            var lastTotal = await BalanceManagementDbContext.AccountTransactions
                .Where(w => w.AccountId == modifyBalance.AccountId).OrderByDescending(m => m.Id)
                .Select(s => s.Total).FirstOrDefaultAsync();
            var balance = modifyBalance.MapTo<AccountTransaction>();
            balance.Total = lastTotal + modifyBalance.Amount;
            balance.TransferDate = DateTime.Now;
            await BalanceManagementDbContext.AccountTransactions.AddAsync(balance);
            return await SaveChangesAsync();
        }
        /// <summary>
        /// This methods allow transfer balance to other account.
        /// The balance must be updated in the both account.
        /// </summary>
        /// <param name="balanceTransfer">transfer data</param>
        /// <returns></returns>
        public async Task<bool> BalanceTransferToUserAsync(BalanceTransferDto balanceTransfer)
        {
            var targetUser = await BalanceManagementDbContext.Users.AsNoTracking().FirstOrDefaultAsync(f => f.UserName == balanceTransfer.UserTarget);
            var sourceUser = await BalanceManagementDbContext.Accounts.AsNoTracking()
                .Where(w => w.Id == balanceTransfer.AccountId).Select(s => s.User).FirstOrDefaultAsync();
            try
            {
                await using var transaction = await BalanceManagementDbContext.Database.BeginTransactionAsync();
                await ModifyBalanceAsync(new ModifyBalanceDto
                {
                    AccountId = balanceTransfer.AccountId,
                    Amount = -balanceTransfer.Amount,
                    Description = $"Transfer to user: {balanceTransfer.UserTarget}",
                    UserId = targetUser.Id
                });
                await ModifyBalanceAsync(new ModifyBalanceDto
                {
                    AccountId = balanceTransfer.AccountIdTarget,
                    Amount = balanceTransfer.Amount,
                    Description = $"Transfer from user: {sourceUser.UserName}",
                    UserId = sourceUser.Id
                });
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(int userId)
        {
            var result =await (from a in BalanceManagementDbContext.Accounts
                select new AccountBalanceDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Total = a.AccountTransactions.OrderByDescending(o=>o.Id).FirstOrDefault().Total
                }).ToListAsync();

            var accounts = await BalanceManagementDbContext.Accounts.AsNoTracking().Where(w => w.UserId == userId)
                .ToListAsync();
            return accounts.MapTo<IEnumerable<AccountDto>>();
        }

        public async Task<PagedCollection<AccountTransactionsDto>> GetBalanceByAccountAsync(AccountFilter accountFilter)
        {
            var pagedCollection = new PagedCollection<AccountTransactionsDto>
            {
                Items = await (from a in BalanceManagementDbContext.Accounts
                    join ab in BalanceManagementDbContext.AccountTransactions on a.Id equals ab.AccountId
                    join u in BalanceManagementDbContext.Users on ab.TransferredByUser equals u.Id into uab
                    from x in uab.DefaultIfEmpty()
                    orderby ab.Id descending
                    where a.UserId == (accountFilter.UserId ?? a.UserId) && !a.IsDeleted &&
                          a.Id == accountFilter.AccountId
                    select new AccountTransactionsDto
                    {
                        Id = ab.Id,
                        TransferredByUser = $"{x.FirstName} {x.Surname}",
                        Total = ab.Total,
                        Description = ab.Description,
                        TransferDate = ab.TransferDate,
                        Amount = ab.Amount
                    }).Skip(accountFilter.Page * accountFilter.Take).Take(accountFilter.Take).ToListAsync(),

                Total = await BalanceManagementDbContext.AccountTransactions.CountAsync(c =>
                    c.AccountId == accountFilter.AccountId)
            };

            return pagedCollection;
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