namespace BalanceManagement.Contracts.Dtos
{
    public class BalanceTransferDto
    {
        public int UserId { get; set; }
        public int AccountSource { get; set; }
        public string UserTarget { get; set; }
        public int AccountTarget { get; set; }
    }
}
