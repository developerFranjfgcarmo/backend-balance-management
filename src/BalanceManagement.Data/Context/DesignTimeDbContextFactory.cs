using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;

namespace BalanceManagement.Data.Context
{
    /// <summary>
    ///     Implement this interface to enable design-time services for context types that do not have a public
    ///     default constructor. Design-time services will automatically discover implementations of this interface
    ///     that are in the same assembly as the derived context.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BalanceManagementDbContext>
    {
        public BalanceManagementDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BalanceManagementDbContext>();
            builder.UseSqlServer("Server=.\\;Database=BalanceManagement;Trusted_Connection=True;");
            return new BalanceManagementDbContext(builder.Options);
        }
    }
}
