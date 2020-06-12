using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Data.Context;
using BalanceManagement.Service.IService;

namespace BalanceManagement.Service.Service
{
    public class UserService:ServiceBase, IUserService
    {
        public UserService(IBalanceManagementDbContext balanceManagementDbContext) : base(balanceManagementDbContext)
        {
        }

        public Task<UserDto> AddAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> UpdateAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
