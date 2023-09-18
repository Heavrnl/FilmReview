using FilmReview.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FilmReview.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IOptionsSnapshot<JWTOptions> _optionsSnapshot;
        public LoginController(UserManager<User> userManager, IOptionsSnapshot<JWTOptions> optionsSnapshot)
        {
            _userManager = userManager;
            _optionsSnapshot = optionsSnapshot;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            User? user = await _userManager.FindByNameAsync(req.username);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, req.password))
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    string jwt = JWTOptions.BuildToken(claims, _optionsSnapshot);



                    //return Ok("asd");
                    return Ok(jwt);

                }

                return BadRequest("用户名或密码错误");
            }
            return BadRequest("用户名或密码错误");

        }
    }
}
