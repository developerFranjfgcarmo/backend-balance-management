using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManagement.Contracts.Dtos
{
    public class AccountDetailsDto: AccountDto
    {
        public IEnumerable<AccountBalanceDto> AccountBalances;
    }
}
