﻿namespace BalanceManagement.Contracts.Dtos.Filter
{
    public class AccountFilter:PagedFilter
    {
        public int? UserId { get; set; }
        public int AccountId { get; set; }
    }
}
