namespace BalanceManagement.Contracts.Dtos.Users
{
    public class UserWithBalanceDto:UserDto
    {
        public double TotalBalance { get; set; }
    }
}
