using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Contracts.Dtos.Users;
using BalanceManagement.Data.Types;
using BalanceManagement.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManagement.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">>User to add</param>
        /// <returns></returns>
        /// <response code="200">User created successfully</response>
        /// <response code="409">The user exists</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddAsync(UserDto user)
        {
            if (await _userService.ExistsUser(user)) return Conflict("username exists");
            var result = await _userService.AddAsync(user);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        /// <summary>
        /// Update user data 
        /// </summary>
        /// <param name="user">User to update</param>
        /// <returns></returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="409">The user exists</response>
        /// <response code="403">You don’t have permission to access this resource</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateAsync(UserDto user)
        {
            if (user.Id != GetUser() && GetRole()==Roles.User) return Forbid();
            if (await _userService.ExistsUser(user)) return Conflict("username exists");
            var result = await _userService.UpdateAsync(user);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        /// <summary>
        /// Get All the users
        /// </summary>
        /// <param name="filter">paging filter</param>
        /// <returns></returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="404">There are not users</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> GetListAsync([FromQuery] PagedFilter filter)
        {
            var result = await _userService.GetListAsync(filter);
            return result.Items.Any() ? (IActionResult) Ok(result) : NotFound();
        }

        /// <summary>
        /// Get The user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="404">The user does not exist</response>
        /// <response code="403">You don’t have permission to access this resource</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id != GetUser() && GetRole() == Roles.User) return Forbid();
            var result = await _userService.GetByIdAsync(id);
            return result != null ? (IActionResult) Ok(result) : NotFound();
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="404">The user does not exist</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return result? (IActionResult)Ok() : NotFound();
        }
    }
}
