using AutoMapper;
using FilmReview.Dto;
using FilmReview.Interfaces;
using FilmReview.Models;
using FilmReview.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FilmReview.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICountryRepository _countryRepository;

        private readonly IMapper _mapper;

        public UserController(IMapper mapper, UserManager<User> userManager, ICountryRepository countryRepository)
        {

            _mapper = mapper;
            _userManager = userManager;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var users = _mapper.Map<List<UserDto>>(await _userManager.Users.ToListAsync());

            return ModelState.IsValid ? Ok(users) : BadRequest(ModelState);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(long userId)
        {
            User u = await _userManager.FindByIdAsync(userId.ToString());
            if (u == null)
                return NotFound();
            var user = _mapper.Map<UserDto>(u);
            return ModelState.IsValid ? Ok(user) : BadRequest(ModelState);
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName(string userName)
        {
            User u = await _userManager.FindByNameAsync(userName);
            if (u == null)
                return NotFound();
            var user = _mapper.Map<UserDto>(u);
            return ModelState.IsValid ? Ok(user) : BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateUser(string password, [FromBody] UserDto userDto)
        {
            User u = await _userManager.FindByNameAsync(userDto.UserName);
            if (u == null)
            {
                var user = _mapper.Map<User>(userDto);
                Country c = await _countryRepository.GetById(userDto.CountryId);
                user.Country = c;
                var res = await _userManager.CreateAsync(user, password);
                return res.Succeeded ? Ok("用户创建成功") : BadRequest($"创建用户失败: {string.Join(", ", res.Errors.Select(e => e.Description))}");
            }
            ModelState.AddModelError("", "用户已存在");
            return StatusCode(422, ModelState);
        }


        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(long userId, [FromBody] UserDto userDto)
        {
            if (userId != userDto.UserId)
            {
                return BadRequest(ModelState);
            }
            User u = await _userManager.FindByNameAsync(userDto.UserName);
            if (u != null)
            {

                User user = _mapper.Map<User>(userDto);

                var res = await _userManager.UpdateAsync(user);
                return res.Succeeded ? Ok("用户更新成功") : BadRequest(res);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(long userId)
        {
            User u = await _userManager.FindByIdAsync(userId.ToString());
            if (u != null)
            {
                var res = await _userManager.DeleteAsync(u);
                
                return res.Succeeded ? Ok("删除成功") : BadRequest(res);
            }
            return BadRequest("用户不存在");
        }

       
    }
}
