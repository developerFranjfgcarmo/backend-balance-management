using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BalanceManagement.Contracts.Dtos.Users;
using BalanceManagement.Data.Types;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BalanceManagement.Api.Auth
{
    public class JwtTokenService: IJwtTokenService
    {
        private readonly IConfiguration _appSettings;

        public JwtTokenService(IConfiguration appSettings)
        {
            _appSettings = appSettings;
        }

        public UserAuthenticatedDto GenerateToken(UserDto user)
        {
            var userAuthenticated = new UserAuthenticatedDto();
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.GetSection("AppSettings").GetSection("Secret").Value);
            var role = (Roles)user.RoleId;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userAuthenticated.Id = user.Id;
            userAuthenticated.Username = user.UserName;
            userAuthenticated.Token = tokenHandler.WriteToken(token);

            return userAuthenticated;
        }
    }
}
