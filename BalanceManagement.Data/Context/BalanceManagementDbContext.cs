using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BalanceManagement.Data.Context
{
    public class BalanceManagementDbContext: DbContext, IBalanceManagementDbContext
    {
        public BalanceManagementDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
