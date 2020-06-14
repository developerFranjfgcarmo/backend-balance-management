using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Accounts;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Data.Types;
using BalanceManagement.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        /// <response code="200">Account created successfully</response>
        /// <response code="409">The account exists</response>
        /// <response code="403">You don’t have permission to access this resource</response>
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> AddAsync(AccountDto account)
        {
            if (account.UserId != GetUser() && GetRole() == Roles.User) return Forbid();
            var result = await _accountService.AddAsync(account);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        /// <summary>
        ///     Update account data
        /// </summary>
        /// <param name="account">Account to update</param>
        /// <returns></returns>
        /// <response code="200">Account Updated successfully</response>
        /// <response code="409">The account exists</response>
        /// <response code="403">You don’t have permission to access this resource</response>
        [ProducesResponseType(typeof(AccountDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateAsync(AccountDto account)
        {
            if (account.UserId != GetUser() && GetRole() == Roles.User) return Forbid();
            var result = await _accountService.UpdateAsync(account);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        /// <summary>
        ///     Delete a account
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns></returns>
        /// <response code="200">Account deleted successfully</response>
        /// <response code="404">The account does not exists</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _accountService.DeleteAsync(id);
            return result ? (IActionResult) Ok() : NotFound();
        }

        /// <summary>
        /// Get All user accounts and all accounts if your are an admin user
        /// </summary>
        /// <param name="filter">paging filter</param>
        /// <returns></returns>
        /// <response code="200">Get All user accounts </response>
        /// <response code="404">There are not accounts</response>
        [ProducesDefaultResponseType]
        [ProducesResponseType(typeof(PagedCollection<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetListAsync([FromQuery] PagedFilter filter)
        {
            var userId = (int?) null;
            if (GetRole() == Roles.User)
            {
                userId = GetUser();
            }
            var result = await _accountService.GetListAsync(userId,filter);
            return result.Items.Any() ? (IActionResult)Ok(result) : NotFound();
        }
        /// <summary>
        /// Get the accounts of a user
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        /// <response code="200">Get All accounts of a user</response>
        /// <response code="404">There are not accounts</response>
        /// <response code="403">You don’t have permission to access this resource</response>
        [ProducesResponseType(typeof(IEnumerable<AccountDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet]
        [Route("user/{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAccountsByUserIdAsync(int id)
        {
            if (GetRole() == Roles.User && GetUser()!=id)
                return Forbid();
            var result = await _accountService.GetAccountsByUserIdAsync(id);
            return result.Any() ? (IActionResult)Ok(result) : NotFound();
        }
        #endregion

        #region [Balance]
        /// <summary>
        /// Transfer balance to other user
        /// </summary>
        /// <param name="id">Id of the account</param>
        /// <param name="balanceTransfer">Information about the transfer</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/transfer-to-user")]
        [Authorize(Roles = nameof(Roles.User))]
        public async Task<IActionResult> BalanceTransferToUserAsync(int id, BalanceTransferDto balanceTransfer)
        {
            if (GetRole() == Roles.User && !await _accountService.IsOwnerAccountAsync(GetUser(), id))
                return Forbid();
            if (id == balanceTransfer.AccountIdTarget)
                return Conflict("The destination account must be different");
            balanceTransfer.AccountId = id;
            var result = await _accountService.BalanceTransferToUserAsync(balanceTransfer);
            return result ? (IActionResult)Ok() : NotFound();
        }

        /// <summary>
        /// Get the list of transactions for an account
        /// </summary>
        /// <param name="id">Id of the account</param>
        /// <param name="filter">paging filter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/transactions")]
        [Authorize(Roles ="User,Admin")]
        public async Task<IActionResult> GetBalanceByAccountAsync(int id, [FromQuery] PagedFilter filter)
        {
            if(GetRole()==Roles.User && !await _accountService.IsOwnerAccountAsync(GetUser(), id))
                return Forbid();
            var userId = GetRole() == Roles.User ? GetUser() : (int?)null;
            var accountFilter = new AccountFilter{ AccountId = id, Page = filter.Page, Take = filter.Take, UserId = userId };
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
            if (!await IsOwnerAccount(modifyBalance)) return Forbid();
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
            if (!await IsOwnerAccount(modifyBalance)) return Forbid();
            modifyBalance.AccountId = id;
            modifyBalance.Amount = -Math.Abs(modifyBalance.Amount);
            var result = await _accountService.ModifyBalanceAsync(modifyBalance);
            return result ? (IActionResult) Ok() : NotFound();
        }

        private async Task<bool> IsOwnerAccount(ModifyBalanceDto modifyBalance)
        {
            var userId = GetRole() == Roles.User ? GetUser() : modifyBalance.UserId;
            var isOwnner = await _accountService.IsOwnerAccountAsync(userId, modifyBalance.AccountId);
            return !(GetRole() == Roles.User && !isOwnner || GetRole() == Roles.Admin && !isOwnner);
        }

        #endregion
    }
}