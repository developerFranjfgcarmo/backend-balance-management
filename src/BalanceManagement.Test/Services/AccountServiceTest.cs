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
        public async void Add_Balance_And_Check_balance()
        {
            //arrange
            var userId = 3;
            var userService = new UserService(_fixture.GetDbContext(), null);
            var accountTransactionService = new AccountTransactionService(_fixture.GetDbContext(), userService);
            //act
            await accountTransactionService.ModifyBalanceAsync(new ModifyBalanceDto
                {AccountId = 3, Amount = 100, Description = "Movimiento 1", UserId = userId });
            await accountTransactionService.ModifyBalanceAsync(new ModifyBalanceDto
                {AccountId = 3, Amount = -5, Description = "Movimiento 2", UserId = userId});
            var user = await userService.GetByIdAsync(userId);

            var targetTotalBalance = AccountServiceMock.Instance().GetTotalBalanceByAccountId(_fixture,3);
            //Assert
            Assert.True(user.TotalBalance.Equals(targetTotalBalance));
        }

        [Fact]
        public async void Transfer_Balance_To_Other_User_And_Check_both_balances()
        {
            //arrange
            var sourceUserId = 1;
            var targetUserId = 2;
            var UserName = "Pepe";
            var accountId = 1;
            var targetAccountId = 2;
            //act
            var userService = new UserService(_fixture.GetDbContext(), null);
            var accountTransactionService = new AccountTransactionService(_fixture.GetDbContext(), userService);

            await accountTransactionService.ModifyBalanceAsync(new ModifyBalanceDto
                { AccountId = accountId, Amount = 100, Description = "Movimiento 1", UserId = sourceUserId });
            await accountTransactionService.BalanceTransferToUserAsync(new BalanceTransferDto
                { AccountId = targetAccountId, Amount = 10, AccountIdTarget = targetAccountId, UserTarget = UserName });
            
            var sourceUser = await userService.GetByIdAsync(sourceUserId);
            var targetUsert = await userService.GetByIdAsync(targetUserId);

            var sourceTotalBalance = AccountServiceMock.Instance().GetTotalBalanceByAccountId(_fixture, accountId);
            var targetTotalBalance = AccountServiceMock.Instance().GetTotalBalanceByAccountId(_fixture, targetAccountId);

            //Assert
            Assert.True(sourceUser.TotalBalance.Equals(sourceTotalBalance));
            Assert.True(targetUsert.TotalBalance.Equals(targetTotalBalance));
        }


    }
}
