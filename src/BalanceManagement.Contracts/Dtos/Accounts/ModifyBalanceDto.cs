namespace BalanceManagement.Contracts.Dtos.Accounts
{
    public class ModifyBalanceDto
    {
        public int AccountId { get; set; }
        public string Description { get; set; }
        public int userId { get; set; }
        public int Amount { get; set; }
    }
}
