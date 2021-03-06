﻿using System;

namespace BalanceManagement.Contracts.Dtos.Accounts
{
    public class AccountTransactionsDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string TransferredByUser { get; set; }
        public double Total { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
