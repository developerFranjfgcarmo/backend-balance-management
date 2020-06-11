﻿using System.Collections.Generic;

namespace BalanceManagement.Data.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AccountBalance> AccountBalances { get; set; }
    }
}
