using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BalanceManagement.Data.Entities;

namespace BalanceManagement.Test.Mocks
{
    public class AccountServiceMock
    {
        private static AccountServiceMock _accountServiceMock;

        private AccountServiceMock()
        {
        }

        public static AccountServiceMock Instance()
        {
            return _accountServiceMock ??= new AccountServiceMock();
        }

        public void AddUserWithAccount(DatabaseFixture fixture)
        {
            var context = fixture.GetDbContext();

            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Francisco", Surname = "Fernández", Nick="Fran", UserName ="Francisco",Password = "1234", RoleId = 1},
                new User { Id = 2, FirstName = "Pepe", Surname = "Rodriguez", Nick="", UserName = "Pepe",Password = "1234", RoleId = 1},
                new User { Id = 3, FirstName = "Juan", Surname = "Villena", Nick="Juan", UserName ="Juan",Password = "1234", RoleId = 1}
            };
            
          
            var accounts = new List<Account>
            {
                new Account { Id = 1, Name = "Cuenta de Francisco", UserId = 1},
                new Account { Id = 2, Name = "Cuenta de Pepe", UserId = 2},
                new Account { Id = 3, Name = "Cuenta de Juan", UserId = 3}
            };
            var userAny = context.Users.Any(w => users.Select(s => s.Id).ToList().Contains(w.Id));
            if (userAny) return;
            var accountAny = context.Accounts.Any(w => accounts.Select(s => s.Id).ToList().Contains(w.Id));
            if (accountAny) return;

            context.Users.AddRange(users);
            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }

        public double GetTotalBalanceByAccountId(DatabaseFixture fixture, int accountId)
        {
            var accountTransactions = fixture.GetDbContext().AccountTransactions.Where(w => w.AccountId == accountId)
                .OrderByDescending(o => o.Id).FirstOrDefault();
            return accountTransactions.Total;
        }
    }
}
