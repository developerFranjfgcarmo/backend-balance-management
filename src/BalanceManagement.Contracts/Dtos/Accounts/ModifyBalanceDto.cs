namespace BalanceManagement.Contracts.Dtos.Accounts
{
    public class ModifyBalanceDto
    {
        public int AccountId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
    }
}
