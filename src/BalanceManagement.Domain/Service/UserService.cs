using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Contracts.Dtos.Users;
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

        public async Task<UserDto> Authenticate(string userName, string password)
        {
            //todo: dencrypt  password
            var user = await BalanceManagementDbContext.Users.FirstOrDefaultAsync(w => w.UserName == userName && w.Password == password && !w.IsDeleted);
            return user.MapTo<UserDto>();
        }

        public async Task<UserDto> AddAsync(UserDto user)
        {
            //todo: encrypt  password
            Debug.Assert(user == null);
            var entity = await BalanceManagementDbContext.Users.AddAsync(user.MapTo<User>());
            await SaveChangesAsync();
            return entity.Entity.MapTo<UserDto>();

        }

        public async Task<UserDto> UpdateAsync(UserDto user)
        {
            //todo: encrypt  password
            Debug.Assert(user == null);
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

        public async Task<PagedCollection<UserDto>> GetAllAsync(PagedFilter pagedFilter)
        {
            Debug.Assert(pagedFilter == null);
            var result = new PagedCollection<UserDto>();
            var owners = await BalanceManagementDbContext.Users.AsNoTracking().Where(w => !w.IsDeleted).Skip(pagedFilter.Page * pagedFilter.Take).Take(pagedFilter.Take).ToListAsync();
            result.Items = owners.MapTo<IEnumerable<UserDto>>().ToList();
            result.Total = await BalanceManagementDbContext.Users.CountAsync(w => !w.IsDeleted);
            return result;
        }

        public async Task<bool> ExistsUser(UserDto user)
        {
            return await BalanceManagementDbContext.Users.CountAsync(w =>
                w.Id != user.Id && w.UserName == user.UserName && !w.IsDeleted) > 0;
        }

        private async Task<User> GetEntityByIdAsync(int id)
        {
            return await BalanceManagementDbContext.Users.FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }


    }
}
