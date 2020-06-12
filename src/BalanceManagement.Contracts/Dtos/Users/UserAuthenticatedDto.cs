namespace BalanceManagement.Contracts.Dtos.Users
{
    public class UserAuthenticatedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
