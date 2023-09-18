using AutoMapper;
using FilmReview.Dto;
using FilmReview.Interfaces;
using FilmReview.Models;
using FilmReview.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace FilmReview.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IFilmRepository _filmRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public FilmController(IFilmRepository filmRepository, IMapper mapper, ICategoryRepository categoryRepository, IDistributedCache distributedCache)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Film>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFilmsWithFilters(int? categoryId, long? filmId, string? filmName, string? categoryName, string? directorName, int? rating)
        {
            var films = _mapper.Map<List<FilmDto>>(await _filmRepository.GetFilmsWithFilters(categoryId,filmId,filmName,categoryName,directorName,rating));

            return ModelState.IsValid ? Ok(films) : BadRequest(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Film>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var films_redis = await _distributedCache.GetAsync("Allfilm");
            if(films_redis == null)
            {

                var f = _mapper.Map<List<FilmDto>>(await _filmRepository.GetAll());
                var json = JsonConvert.SerializeObject(f);
                var bytes = Encoding.UTF8.GetBytes(json);
                var opt = new DistributedCacheEntryOptions();
                opt.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1800);
                await _distributedCache.SetAsync("Allfilm", bytes, opt);
                return ModelState.IsValid ? Ok(f) : BadRequest(ModelState);
            }
            var json1 = Encoding.UTF8.GetString(films_redis);
            var films = JsonConvert.DeserializeObject<IEnumerable<FilmDto>>(json1);
            return ModelState.IsValid ? Ok(films) : BadRequest(ModelState);
        }

        [HttpGet("{filmId}")]
        [ProducesResponseType(200, Type = typeof(Film))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int filmId)
        {
            if (!_filmRepository.FilmExists(filmId).Result)
                return NotFound();
            var film = _mapper.Map<FilmDto>(await _filmRepository.GetById(filmId));
            return ModelState.IsValid ? Ok(film) : BadRequest(ModelState);
        }

        [HttpGet("{filmName}")]
        [ProducesResponseType(200, Type = typeof(Film))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName(string filmName)
        {
            if (!_filmRepository.FilmExists(filmName).Result)
                return NotFound();
            var film = _mapper.Map<FilmDto>(await _filmRepository.GetByName(filmName));
            return ModelState.IsValid ? Ok(film) : BadRequest(ModelState);
        }

        [HttpGet("{categoryName}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Film>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByCategoryName(string categoryName)
        {
            var films = _mapper.Map<List<FilmDto>>(await _filmRepository.GetByCategoryName(categoryName));

            return ModelState.IsValid ? Ok(films) : BadRequest(ModelState);
        }

        [HttpGet("{score}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Film>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByRating(int score)
        {
            var films = _mapper.Map<List<FilmDto>>(await _filmRepository.GetByRating(score));

            return ModelState.IsValid ? Ok(films) : BadRequest(ModelState);
        }

        [HttpGet("{directorName}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Film>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByDirector(string directorName)
        {
            var films = _mapper.Map<List<FilmDto>>(await _filmRepository.GetByDirector(directorName));

            return ModelState.IsValid ? Ok(films) : BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFilm([FromBody] FilmDto filmDto)
        {
            var exist = _filmRepository.FilmExists(filmDto.Name).Result;
            if (!exist)
            {
                var film = _mapper.Map<Film>(filmDto);
                if (filmDto.CategoryName != null)
                {
                    film.Category = await _categoryRepository.GetByName(filmDto.CategoryName);
                }  
                var res = await _filmRepository.CreateFilm(film);
                return res ? Ok(res) : BadRequest(res);
            }
            ModelState.AddModelError("", "Film already exists");
            return StatusCode(422, ModelState);
        }

        [HttpPut("{filmId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateFilm(int filmId,[FromBody] FilmDto filmDto)
        {
            if (filmId != filmDto.FilmId)
            {
                return BadRequest(ModelState);
            }
            var exist = _filmRepository.FilmExists(filmId).Result;
            if (exist)
            {

               Film film = _mapper.Map<Film>(filmDto);

                var res = await _filmRepository.UpdateFilm(film);
                return res ? Ok(res) : BadRequest(res);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{filmId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteFilm(int filmId)
        {
            var exist = _filmRepository.FilmExists(filmId).Result;
            if (exist)
            {
                var res = await _filmRepository.DeleteFilm(new Film() { FilmId = filmId });
                return res ? Ok(res) : BadRequest(res);
            }
            return BadRequest();
        }
    }
}
