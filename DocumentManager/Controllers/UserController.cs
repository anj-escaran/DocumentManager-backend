using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DocumentManager.Models;
using DocumentManager.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DocumentManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<Account> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRoleRepository _userroleRepo;

        public UserController(UserManager<Account> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("UserList")]

        public async Task<IActionResult> UserList()
        {
            //fetch all users
            var users = await userManager.Users
            .Select(u => new { User = u, Roles = new List<string>() })
            .ToListAsync();
            //fetch all roles
            var roleNames = await roleManager.Roles.Select(r => r.Name).ToListAsync();

            foreach (var roleName in roleNames)
            {
                //For each role, fetch the users
                var usersInRole = await userManager.GetUsersInRoleAsync(roleName);

                //Populate the roles for each user in memory
                var toUpdate = users.Where(u => usersInRole.Any(ur => ur.Id == u.User.Id));
                foreach (var user in toUpdate)
                {
                    user.Roles.Add(roleName);
                }
            }
            return Ok(new
            {
                data = users
            });
        }

    }
}
