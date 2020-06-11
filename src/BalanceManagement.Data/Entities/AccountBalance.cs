﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManagement.Data.Entities
{
    public class AccountBalance
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int TransferredByUser { get; set; }
        public double Total { get; set; }
        public DateTime TransferDate { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
