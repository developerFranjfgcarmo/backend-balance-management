using System;
using System.Linq;
using System.Security.Claims;
using BalanceManagement.Data.Types;
using Microsoft.AspNetCore.Mvc;

namespace BalanceManagement.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected  int GetUser()
        {
            int.TryParse(HttpContext.Request.HttpContext.User.Identity.Name, out var userId);
            return userId;
        }

        protected Roles GetRole()
        {
           var currentRole= HttpContext.Request.HttpContext.User.Claims.FirstOrDefault(f =>
                f.Type == ClaimsIdentity.DefaultRoleClaimType);
           var rol = (Roles)Enum.Parse(typeof(Roles), currentRole.Value, true);
           return rol;
        }
    }
}
