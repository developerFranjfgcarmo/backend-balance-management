using System;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Data.Types;
using BalanceManagement.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManagement.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Add a new account
        /// </summary>
        /// <param name="account">>Account to add</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AddAsync(AccountDto account)
        {
            if (account.Id != GetUser() && GetRol() == Roles.User) return Forbid();
            var result = await _accountService.AddAsync(account);
            return result != null ? (IActionResult)Ok(result) : Conflict();
        }

        /// <summary>
        /// Update account data 
        /// </summary>
        /// <param name="account">Account to update</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> UpdateAsync(AccountDto account)
        {
            if (account.Id != GetUser() && GetRol() == Roles.User) return Forbid();
            var result = await _accountService.UpdateAsync(account);
            return result != null ? (IActionResult)Ok(result) : Conflict();
        }

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _accountService.DeleteAsync(id);
            return result ? (IActionResult)Ok() : NotFound();
        }

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="id">account id</param>
        /// <param name="modifyBalance">Data to update the balance of the account</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/add-balance")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AddBalanceAsync(int id,ModifyBalanceDto modifyBalance)
        {
            var userId = GetRol() == Roles.User ? GetUser() : modifyBalance.userId;
            var isOwnner = !await _accountService.IsOwnerAccount(userId, modifyBalance.AccountId);
            if ((GetRol() == Roles.User && !isOwnner) || (GetRol() == Roles.Admin && !isOwnner))
            {
                return Conflict("The user doesn`t own of this account");
            }

            modifyBalance.AccountId = id;
            modifyBalance.Amount = Math.Abs(modifyBalance.Amount);
            var result =await _accountService.ModifyBalanceAsync(modifyBalance);
            return result ? (IActionResult)Ok() : NotFound();
        }

    }
}
