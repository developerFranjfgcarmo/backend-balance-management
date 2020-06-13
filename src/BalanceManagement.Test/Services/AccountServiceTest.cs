using System;
using System.Collections.Generic;
using System.Text;
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
        }
    }
}
