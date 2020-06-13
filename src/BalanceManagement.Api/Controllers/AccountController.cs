using System;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Data.Types;
using BalanceManagement.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

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

        #region [Account]

        /// <summary>
        ///     Add a new account
        /// </summary>
        /// <param name="account">>Account to add</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> AddAsync(AccountDto account)
        {
            if (account.Id != GetUser() && GetRol() == Roles.User) return Forbid();
            var result = await _accountService.AddAsync(account);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        /// <summary>
        ///     Update account data
        /// </summary>
        /// <param name="account">Account to update</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateAsync(AccountDto account)
        {
            if (account.Id != GetUser() && GetRol() == Roles.User) return Forbid();
            var result = await _accountService.UpdateAsync(account);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        /// <summary>
        ///     Delete a account
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _accountService.DeleteAsync(id);
            return result ? (IActionResult) Ok() : NotFound();
        }

        /// <summary>
        /// Get All the accounts
        /// </summary>
        /// <param name="filter">paging filter</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetListAsync([FromQuery] PagedFilter filter)
        {
            var userId = (int?) null;
            if (GetRol() == Roles.User)
            {
                userId = GetUser();
            }
            var result = await _accountService.GetListAsync(userId,filter);
            return result.Items.Any() ? (IActionResult)Ok(result) : NotFound();
        }
        #endregion

        #region [Balance]

        /// <summary>
        /// Get All the accounts
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter">paging filter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/balance")]
        [Authorize(Roles = nameof(Roles.User))]
        public async Task<IActionResult> GetBalanceByAccountAsync(int id, [FromQuery] PagedFilter filter)
        {
            if(!await _accountService.IsOwnerAccount(GetUser(), id))
                return Conflict("The user doesn`t own of this account");
            var accountFilter = new AccountFilter{ AccountId = id, Page = filter.Page, Take = filter.Take, UserId = GetUser()};
            var result = await _accountService.GetBalanceByAccountAsync(accountFilter);
            return result.Items.Any() ? (IActionResult)Ok(result) : NotFound();
        }

        /// <summary>
        ///     Add a new balance to user's account
        /// </summary>
        /// <param name="id">account id</param>
        /// <param name="modifyBalance">Data to update the balance of the account</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/add-balance")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AddBalanceAsync(int id, ModifyBalanceDto modifyBalance)
        {
            if (!await IsOwnerAccount(modifyBalance)) return Conflict("The user doesn`t own of this account");

            modifyBalance.AccountId = id;
            modifyBalance.Amount = Math.Abs(modifyBalance.Amount);
            var result = await _accountService.ModifyBalanceAsync(modifyBalance);
            return result ? (IActionResult) Ok() : NotFound();
        }

        /// <summary>
        ///     Remove balance to user's account
        /// </summary>
        /// <param name="id">account id</param>
        /// <param name="modifyBalance">Data to update the balance of the account</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/remove-balance")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> RemoveBalanceAsync(int id, ModifyBalanceDto modifyBalance)
        {
            if (!await IsOwnerAccount(modifyBalance)) return Conflict("The user doesn`t own of this account");

            modifyBalance.AccountId = id;
            modifyBalance.Amount = -Math.Abs(modifyBalance.Amount);
            var result = await _accountService.ModifyBalanceAsync(modifyBalance);
            return result ? (IActionResult) Ok() : NotFound();
        }

        private async Task<bool> IsOwnerAccount(ModifyBalanceDto modifyBalance)
        {
            var userId = GetRol() == Roles.User ? GetUser() : modifyBalance.userId;
            var isOwnner = !await _accountService.IsOwnerAccount(userId, modifyBalance.AccountId);
            return !(GetRol() == Roles.User && !isOwnner || GetRol() == Roles.Admin && !isOwnner);
        }

        #endregion
    }
}