using System.Collections.Generic;

namespace BalanceManagement.Data.Entities
{
    public class Role
    {
        //todo: modify to Roles
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
