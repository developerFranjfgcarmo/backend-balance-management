using System.Collections.Generic;

namespace BalanceManagement.Contracts.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<AccountBalanceDto> AccountBalances;
    }
}
