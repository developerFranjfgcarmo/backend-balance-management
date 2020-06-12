using System;

namespace BalanceManagement.Contracts.Dtos.Accounts
{
    public class AccountBalanceDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string User { get; set; }
        public double Total { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
