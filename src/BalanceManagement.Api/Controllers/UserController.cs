using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using BalanceManagement.Contracts.Dtos.Filter;
using BalanceManagement.Contracts.Dtos.Users;
using BalanceManagement.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManagement.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

         [HttpPost]
        public async Task<IActionResult> AddAsync(UserDto owner)
        {
            if (await _userService.ExistsUser(owner)) return Conflict("username exists");
            var result = await _userService.AddAsync(owner);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UserDto owner)
        {
            if (await _userService.ExistsUser(owner)) return Conflict("username exists");
            var result = await _userService.UpdateAsync(owner);
            return result != null ? (IActionResult) Ok(result) : Conflict();
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] PagedFilter filter)
        {
            var result = await _userService.GetAllAsync(filter);
            return result.Items.Any() ? (IActionResult) Ok(result) : NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return result != null ? (IActionResult) Ok(result) : NotFound();
        }
    }
}
