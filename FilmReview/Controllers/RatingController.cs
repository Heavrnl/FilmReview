using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;
using FilmReview.Interfaces;
using FilmReview.Models;
using FilmReview.Dto;
using FilmReview.Repository;
using Microsoft.AspNetCore.Authorization;

namespace RatingReview.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RatingController(IRatingRepository ratingRepository, IMapper mapper,  UserManager<User> userManager, IFilmRepository filmRepository)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _userManager = userManager;
            _filmRepository = filmRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Rating>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var ratings = _mapper.Map<List<RatingDto>>(await _ratingRepository.GetAll());
            return ModelState.IsValid ? Ok(ratings) : BadRequest(ModelState);

        }

        [HttpGet("{ratingId}")]
        [ProducesResponseType(200, Type = typeof(Rating))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int ratingId)
        {
            if (!_ratingRepository.RatingExists(ratingId).Result)
                return NotFound();
            var rating = _mapper.Map<RatingDto>(await _ratingRepository.GetById(ratingId));
            return ModelState.IsValid ? Ok(rating) : BadRequest(ModelState);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(Rating))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            User u =await _userManager.FindByIdAsync(userId.ToString());
            if (u == null)
                return NotFound();
            var rating = _mapper.Map<List<RatingDto>>(await _ratingRepository.GetByUserId(userId));
            return ModelState.IsValid ? Ok(rating) : BadRequest(ModelState);
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(200, Type = typeof(Rating))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            User u = await _userManager.FindByNameAsync(userName);
            if (u == null)
                return NotFound();
            var rating = _mapper.Map<List<RatingDto>>(await _ratingRepository.GetByUserName(userName));
            return ModelState.IsValid ? Ok(rating) : BadRequest(ModelState);
        }


        [HttpGet("{filmId}")]
        [ProducesResponseType(200, Type = typeof(Rating))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByFilmId(long filmId)
        {
            var rating = _mapper.Map<List<RatingDto>>(await _ratingRepository.GetByFilmId(filmId));
            return ModelState.IsValid ? Ok(rating) : BadRequest(ModelState);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRating([FromBody] RatingDto ratingDto)
        {
            var film = await _filmRepository.GetById(ratingDto.FilmId);
            var user = await _userManager.FindByIdAsync(ratingDto.UserId.ToString());
            if (film != null && user != null)
            {
                var rating = _mapper.Map<Rating>(ratingDto);
                rating.Film = film;
                rating.User = user;
                var res = await _ratingRepository.CreateRating(rating);
                return res ? Ok("评分成功") : BadRequest(res);
            }
            ModelState.AddModelError("", "评分已存在");
            return StatusCode(422, ModelState);
        }

        [HttpPut("{ratingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRating(int ratingId, [FromBody] RatingDto ratingDto)
        {
            if (ratingId != ratingDto.RatingId)
            {
                return BadRequest(ModelState);
            }
            var exist = _ratingRepository.RatingExists(ratingId).Result;
            if (exist)
            {
                var film = await _filmRepository.GetById(ratingDto.FilmId);
                var user = await _userManager.FindByIdAsync(ratingDto.UserId.ToString());
                Rating rating = _mapper.Map<Rating>(ratingDto);
                rating.Film = film;
                rating.User = user;
                var res = await _ratingRepository.UpdateRating(rating);
                return res ? Ok("更新成功") : BadRequest(res);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{ratingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            var exist = _ratingRepository.RatingExists(ratingId).Result;
            if (exist)
            {
                var res = await _ratingRepository.DeleteRating(new Rating() { RatingId = ratingId });
                return res ? Ok("删除成功") : BadRequest(res);
            }
            return BadRequest("评分不存在");
        }
    }
}
