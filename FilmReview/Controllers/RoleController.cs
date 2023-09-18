using FilmReview.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FilmReview.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> createRole()
        {
            bool roleExist = await _roleManager.RoleExistsAsync("admin");
            if (!roleExist)
            {
                Role role = new Role { Name = "admin" };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            roleExist = await _roleManager.RoleExistsAsync("user");
            if (!roleExist)
            {
                Role role = new Role { Name = "user" };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(long userId,string role)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            var res = await _userManager.AddToRoleAsync(user, role);
            return Ok(res); 
        }
    }
}
