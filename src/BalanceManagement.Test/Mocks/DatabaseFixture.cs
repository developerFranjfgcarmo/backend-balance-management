using System;
using System.Collections.Generic;
using System.Text;
using BalanceManagement.Data.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BalanceManagement.Test.Mocks
{
    public class DatabaseFixture : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private SqliteConnection _connection;
        private BalanceManagementDbContext _dbContext;

        public DatabaseFixture()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<BalanceManagementDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new BalanceManagementDbContext(options);
            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
            // _dbContext = null;
            _dbContext.Dispose();
            // ... clean up test data from the database ...
        }
        public BalanceManagementDbContext GetDbContext()
        {
            return _dbContext;
        }
    }
}
