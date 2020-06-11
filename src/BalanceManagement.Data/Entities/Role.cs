namespace BalanceManagement.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public virtual User User { get; set; }
    }
}
