using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Contracts.Mapper;
using BalanceManagement.Data.Context;
using BalanceManagement.Data.Entities;
using BalanceManagement.Service.Attributes;
using BalanceManagement.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BalanceManagement.Service.Service
{
    public class AccountTransactionService : ServiceBase, IAccountTransactionService
    {
        private readonly IUserService _userService;
        public AccountTransactionService(IBalanceManagementDbContext balanceManagementDbContext, IUserService userService) : base(balanceManagementDbContext)
        {
            _userService = userService;
        }

        /// <summary>
        ///  Add or remove balance of an user
        /// </summary>
        /// <param name="modifyBalance"></param>
        /// <returns></returns>
        [TransactionAsync]
        public async Task<bool> ModifyBalanceAsync(ModifyBalanceDto modifyBalance)
        {

            var lastTotal = await BalanceManagementDbContext.AccountTransactions
                .Where(w => w.AccountId == modifyBalance.AccountId).OrderByDescending(m => m.Id)
                .Select(s => s.Total).FirstOrDefaultAsync();
            var balance = modifyBalance.MapTo<AccountTransaction>();
            balance.Total = lastTotal + modifyBalance.Amount;
            balance.TransferDate = DateTime.Now;
            await BalanceManagementDbContext.AccountTransactions.AddAsync(balance);
            await SaveChangesAsync();
            await UpdateBalanceOfUser(modifyBalance.UserId);
            return await Task.FromResult(true);
        }

        private async Task<bool> UpdateBalanceOfUser(int id)
        {
            var accounts = await GetAccountsWithBalanceByUserIdAsync(id);
            var totalBalance = accounts.Sum(s => s.Total);
            return await _userService.UpdateBalanceAsync(id, totalBalance);
        }
        /// <summary>
        /// This methods allow transfer balance to other account.
        /// The balance must be updated in the both account.
        /// </summary>
        /// <param name="balanceTransfer">transfer data</param>
        /// <returns></returns>
        [TransactionAsync]
        public async Task<bool> BalanceTransferToUserAsync(BalanceTransferDto balanceTransfer)
        {
            var targetUser = await BalanceManagementDbContext.Users.AsNoTracking().FirstOrDefaultAsync(f => f.UserName == balanceTransfer.UserTarget);
            var sourceUser = await BalanceManagementDbContext.Accounts.AsNoTracking()
                .Where(w => w.Id == balanceTransfer.AccountId).Select(s => s.User).FirstOrDefaultAsync();
            await ModifyBalanceAsync(new ModifyBalanceDto
            {
                AccountId = balanceTransfer.AccountId,
                Amount = -balanceTransfer.Amount,
                Description = $"Transfer to user: {balanceTransfer.UserTarget}",
                UserId = sourceUser.Id
            });
            await ModifyBalanceAsync(new ModifyBalanceDto
            {
                AccountId = balanceTransfer.AccountIdTarget,
                Amount = balanceTransfer.Amount,
                Description = $"Transfer from user: {sourceUser.UserName}",
                UserId = targetUser.Id
            });

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<AccountBalanceDto>> GetAccountsWithBalanceByUserIdAsync(int userId)
        {
            var result = await (from a in BalanceManagementDbContext.Accounts
                                where !a.IsDeleted && a.UserId == userId
                                select new AccountBalanceDto()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Total = a.AccountTransactions.OrderByDescending(o => o.Id).FirstOrDefault().Total
                                }).ToListAsync();

            return result;
        }

        public async Task<PagedCollection<AccountTransactionsDto>> GetTransactionByAccountAsync(AccountFilter accountFilter)
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
    }
}
