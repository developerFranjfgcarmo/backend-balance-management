using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Service.Service;
using BalanceManagement.Test.Mocks;
using Xunit;

namespace BalanceManagement.Test.Services
{
    public class AccountServiceTest : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        public AccountServiceTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            AccountServiceMock.Instance().AddUserWithAccount(_fixture);
        }

        [Fact]
        public async void User_Modify_TotalBalance()
        {
            var userId = 1;
            var userService = new UserService(_fixture.GetDbContext(), null);
            var accountTransactionService = new AccountTransactionService(_fixture.GetDbContext(), userService);

            await accountTransactionService.ModifyBalanceAsync(new ModifyBalanceDto
                {AccountId = 1, Amount = 100, Description = "Movimiento 1", UserId = userId });
            await accountTransactionService.ModifyBalanceAsync(new ModifyBalanceDto
                {AccountId = 1, Amount = -5, Description = "Movimiento 2", UserId = userId});
            var user = await userService.GetByIdAsync(userId);

            Assert.True(user.TotalBalance.Equals(95.0));
        }

        [Fact]
        public async void User_Transfer_TotalBalance()
        {
            var sourceUserId = 1;
            var targetUserId = 2;
            var UserName = "Pepe";
            var accountId = 1;
            var targetAccountId = 2;

            var userService = new UserService(_fixture.GetDbContext(), null);
            var accountTransactionService = new AccountTransactionService(_fixture.GetDbContext(), userService);

            await accountTransactionService.ModifyBalanceAsync(new ModifyBalanceDto
                { AccountId = accountId, Amount = 100, Description = "Movimiento 1", UserId = sourceUserId });
            await accountTransactionService.BalanceTransferToUserAsync(new BalanceTransferDto
                { AccountId = targetAccountId, Amount = 10, AccountIdTarget = targetAccountId, UserTarget = UserName });
            
            var sourceUser = await userService.GetByIdAsync(sourceUserId);
            var targetUsert = await userService.GetByIdAsync(targetUserId);

            
            var sourceLastTransaction =  _fixture.GetDbContext().AccountTransactions.Where(w => w.AccountId == accountId)
                .OrderByDescending(o => o.Id).FirstOrDefault();

            var targetLastTransaction = _fixture.GetDbContext().AccountTransactions.Where(w => w.AccountId == targetAccountId)
                .OrderByDescending(o => o.Id).FirstOrDefault();

            Assert.True(sourceUser.TotalBalance.Equals(sourceLastTransaction.Total));
            Assert.True(targetUsert.TotalBalance.Equals(targetLastTransaction.Total));
        }


    }
}
