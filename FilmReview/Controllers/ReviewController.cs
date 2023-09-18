using AutoMapper;
using FilmReview.Dto;
using FilmReview.Filters;
using FilmReview.Interfaces;
using FilmReview.Models;
using FilmReview.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FilmReview.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<User> _userManager;
        private readonly IFilmRepository _filmRepository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, UserManager<User> userManager, IFilmRepository filmRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userManager = userManager;
            _filmRepository = filmRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetAll());

            return ModelState.IsValid ? Ok(reviews) : BadRequest(ModelState);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId).Result)
                return NotFound();
            var review = _mapper.Map<ReviewDto>(await _reviewRepository.GetById(reviewId));
            return ModelState.IsValid ? Ok(review) : BadRequest(ModelState);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            User u = await _userManager.FindByIdAsync(userId.ToString());
            if (u == null)
                return NotFound();
            var review = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetByUserId(userId));
            return ModelState.IsValid ? Ok(review) : BadRequest(ModelState);
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            User u = await _userManager.FindByNameAsync(userName);
            if (u == null)
                return NotFound();
            var review = _mapper.Map<ReviewDto>(await _reviewRepository.GetByUserName(userName));
            return ModelState.IsValid ? Ok(review) : BadRequest(ModelState);
        }

        [HttpGet("{filmId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByFilmId(long filmId)
        {
            var review = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetByFilmId(filmId));
            return ModelState.IsValid ? Ok(review) : BadRequest(ModelState);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ServiceFilter(typeof(SensitiveWordFilterAttribute))] // 使用过滤器
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
        {
            var film = await _filmRepository.GetById(reviewDto.FilmId);
            var user = await _userManager.FindByIdAsync(reviewDto.UserId.ToString());
            if (film != null && user != null)
            {
                var review = _mapper.Map<Review>(reviewDto);
                review.Film = film;
                review.User = user;
                var res = await _reviewRepository.CreateReview(review);
                return res ? Ok("评论成功") : BadRequest(res);
            }
            ModelState.AddModelError("", "评论已存在");
            return StatusCode(422, ModelState);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(SensitiveWordFilterAttribute))] // 使用过滤器
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto reviewDto)
        {
            if (reviewId != reviewDto.ReviewId)
            {
                return BadRequest(ModelState);
            }
            var exist = _reviewRepository.ReviewExists(reviewId).Result;
            if (exist)
            {
                var film = await _filmRepository.GetById(reviewDto.FilmId);
                var user = await _userManager.FindByIdAsync(reviewDto.UserId.ToString());
                Review review = _mapper.Map<Review>(reviewDto);
                review.Film = film;
                review.User = user;

                var res = await _reviewRepository.UpdateReview(review);
                return res ? Ok("更新成功") : BadRequest(res);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var exist = _reviewRepository.ReviewExists(reviewId).Result;
            if (exist)
            {
                var res = await _reviewRepository.DeleteReview(new Review() { ReviewId = reviewId });
                return res ? Ok("删除成功") : BadRequest(res);
            }
            return BadRequest("评论不存在");
        }
    }
}
