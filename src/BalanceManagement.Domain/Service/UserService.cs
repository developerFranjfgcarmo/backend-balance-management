using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Mapper;
using BalanceManagement.Data.Context;
using BalanceManagement.Data.Entities;
using BalanceManagement.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BalanceManagement.Service.Service
{
    public class UserService:ServiceBase, IUserService
    {
        public UserService(IBalanceManagementDbContext balanceManagementDbContext) : base(balanceManagementDbContext)
        {
        }

        public async Task<UserDto> AddAsync(UserDto user)
        {
            var entity = await BalanceManagementDbContext.Users.AddAsync(user.MapTo<User>());
            await SaveChangesAsync();
            return entity.Entity.MapTo<UserDto>();

        }

        public async Task<UserDto> UpdateAsync(UserDto user)
        {
            var currentUser = await GetEntityByIdAsync(user.Id);
            var newUser = user.MapTo<UserDto>();
            BalanceManagementDbContext.Entry(currentUser).CurrentValues.SetValues(newUser);
            await SaveChangesAsync();
            return newUser.MapTo<UserDto>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetEntityByIdAsync(id);
            user.IsDeleted = true;
            return await SaveChangesAsync();
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var owner = await GetEntityByIdAsync(id);
            return owner.MapTo<UserDto>();
        }
        private async Task<User> GetEntityByIdAsync(int id)
        {
            return await BalanceManagementDbContext.Users.FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }
    }
}
