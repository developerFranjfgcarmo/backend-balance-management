using System.Collections.Generic;

namespace BalanceManagement.Contracts.Dtos.Accounts
{
    public class AccountDetailsDto: AccountDto
    {
        public IEnumerable<AccountBalanceDto> AccountBalances;
    }
}
