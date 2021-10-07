using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentManager.Models;
using DocumentManager.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DocumentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {  
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration _configuration;
        public RoleController(RoleManager<Role> roleManager, IConfiguration configuration)
        {
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Role model)
        {
            var roleExist = await roleManager.FindByNameAsync(model.Name);
            if (roleExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role already exist!" });
            Role role = new()
            {
                Name = model.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString(),

            };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role creation failed!" });
            }

            return Ok(new Response { Status = "Success", Message = "Role Created Successfully" });

        }
    }
}
