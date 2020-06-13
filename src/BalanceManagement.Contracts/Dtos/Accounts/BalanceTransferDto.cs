namespace BalanceManagement.Contracts.Dtos.Accounts
{
    public class BalanceTransferDto
    {
        public int AccountId { get; set; }
        public int Amount { get; set; }
        public string UserTarget { get; set; }
        public int AccountIdTarget { get; set; }
    }
}
