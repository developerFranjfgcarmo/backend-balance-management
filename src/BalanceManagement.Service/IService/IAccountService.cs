﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;

namespace BalanceManagement.Service.IService
{
    public interface IAccountService
    {
        Task<AccountDto> AddAsync(AccountDto accountDto);
        Task<AccountDto> UpdateAsync(AccountDto accountDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsOwnerAccountAsync(int userId, int accountId);
        Task<PagedCollection<AccountDto>> GetListAsync(int? userId,PagedFilter pagedFilter);
    }
}
