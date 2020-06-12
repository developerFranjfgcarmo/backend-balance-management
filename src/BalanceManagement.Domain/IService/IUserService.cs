using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;

namespace BalanceManagement.Service.IService
{
    public interface IUserService
    {
        Task<UserDto> AddAsync(UserDto user);
        Task<UserDto> UpdateAsync(UserDto user);
        Task<bool> DeleteAsync(int id);
        Task<UserDto> GetByIdAsync(int id);
    }
}
