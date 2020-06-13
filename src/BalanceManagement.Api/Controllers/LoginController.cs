using System.Net;
using System.Threading.Tasks;
using BalanceManagement.Api.Auth;
using BalanceManagement.Contracts.Dtos.Users;
using BalanceManagement.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManagement.Api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Authenticate to user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Todo
        ///     {
        ///        "username": "user",
        ///        "password": "password"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">User and password</param>
        /// <returns>userAuthenticated</returns>
        /// <response code="200">User Authenticated</response>
        /// <response code="400">User or password incorrect</response>  
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var userAuthenticated = _jwtTokenService.GenerateToken(user);


            return Ok(userAuthenticated);
        }
    }
}
