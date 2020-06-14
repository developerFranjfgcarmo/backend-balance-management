using System;
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
using BalanceManagement.Service.Attributes;
using BalanceManagement.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BalanceManagement.Service.Service
{
    public class UserService:ServiceBase, IUserService
    {
        private readonly IAccountService _accountService;
        public UserService(IBalanceManagementDbContext balanceManagementDbContext, IAccountService accountService) : base(balanceManagementDbContext)
        {
            _accountService = accountService;
        }

        public async Task<UserDto> Authenticate(string userName, string password)
        {
            //todo: dencrypt  password
            var user = await BalanceManagementDbContext.Users.FirstOrDefaultAsync(w => w.UserName == userName && !w.IsDeleted);
            if (user != null && !VerifyPasswordHash(password, user.Password, user.PasswordSalt))
            {
                return null;
            }
            return user.MapTo<UserDto>();
        }

        public async Task<UserDto> AddAsync(UserDto user)
        {
            //todo: encrypt  password
            Debug.Assert(user == null);
            var entityUser = user.MapTo<User>();
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            entityUser.Password = passwordHash;
            entityUser.PasswordSalt = passwordSalt;
            var entity = await BalanceManagementDbContext.Users.AddAsync(entityUser);
            await SaveChangesAsync();
            return entity.Entity.MapTo<UserDto>();
        }

        public async Task<UserDto> UpdateAsync(UserDto user)
        {
            //todo: encrypt  password
            Debug.Assert(user == null);
            var currentUser = await GetEntityByIdAsync(user.Id);
            var newUser = user.MapTo<User>();
            BalanceManagementDbContext.Entry(currentUser).CurrentValues.SetValues(newUser);
            await SaveChangesAsync();
            return newUser.MapTo<UserDto>();
        }
        /// <summary>
        /// Delete a user and his accounts.
        /// This process is in a transaction to avoid corrupted data.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        [TransactionAsync]
        public async Task<bool> DeleteAsync(int id)
        {
       
            var user = await BalanceManagementDbContext.Users.Include(i=>i.Accounts).FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted); ;
            user.IsDeleted = true;
            await SaveChangesAsync();
            var accounts =user.Accounts;
            foreach (var account in accounts)
            {
                await _accountService.DeleteAsync(account.Id);
            }
           
            return await Task.FromResult(true); ;
        }

        public async Task<UserWithBalanceDto> GetByIdAsync(int id)
        {
            var user = await GetEntityByIdAsync(id);
            return user.MapTo<UserWithBalanceDto>();
        }

        public async Task<PagedCollection<UserWithBalanceDto>> GetListAsync(PagedFilter pagedFilter)
        {
            Debug.Assert(pagedFilter == null);
            var result = new PagedCollection<UserWithBalanceDto>();
            var users = await BalanceManagementDbContext.Users.AsNoTracking().Where(w => !w.IsDeleted).Skip(pagedFilter.Page * pagedFilter.Take).Take(pagedFilter.Take).ToListAsync();
            result.Items = users.MapTo<IEnumerable<UserWithBalanceDto>>().ToList();
            result.Total = await BalanceManagementDbContext.Users.CountAsync(w => !w.IsDeleted);
            return result;
        }

        public async Task<bool> ExistsUserAsync(UserDto user)
        {
            return await BalanceManagementDbContext.Users.CountAsync(w =>
                w.Id != user.Id && w.UserName == user.UserName && !w.IsDeleted) > 0;
        }

        public async Task<bool> UpdateBalanceAsync(int id, double totalBalance)
        {
            var currentUser = await GetEntityByIdAsync(id);
            currentUser.TotalBalance = totalBalance;
            return await SaveChangesAsync();
        }

        private async Task<User> GetEntityByIdAsync(int id)
        {
            return await BalanceManagementDbContext.Users.FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != passwordHash[i]).Any();
        }
    }
}
