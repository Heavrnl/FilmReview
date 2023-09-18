using AutoMapper;
using FilmReview.Dto;
using FilmReview.Interfaces;
using FilmReview.Models;
using FilmReview.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FilmReview.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var countries = _mapper.Map<List<CountryDto>>(await _countryRepository.GetAll());

            return ModelState.IsValid ? Ok(countries) : BadRequest(ModelState);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId).Result)
                return NotFound();
            var country = _mapper.Map<CountryDto>(await _countryRepository.GetById(countryId));
            return ModelState.IsValid ? Ok(country) : BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDto countryDto)
        {
            var exist = _countryRepository.CountryExists(countryDto.Name).Result;
            if (!exist)
            {
                var country = _mapper.Map<Country>(countryDto);
                var res = await _countryRepository.CreateCountry(country);
                return res ? Ok(res) : BadRequest(res);
            }
            ModelState.AddModelError("", "Country already exists");
            return StatusCode(422, ModelState);
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryDto countryDto)
        {
            if (countryId != countryDto.CountryId || countryDto.Name == null)
            {
                return BadRequest(ModelState);
            }
            var exist = _countryRepository.CountryExists(countryId).Result;
            if (exist)
            {
                var country = _mapper.Map<Country>(countryDto);
                var res = await _countryRepository.UpdateCountry(country);
                return res ? Ok(res) : BadRequest(res);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            var exist = _countryRepository.CountryExists(countryId).Result;
            if (exist)
            {
                var res = await _countryRepository.DeleteCountry(new Country() { CountryId = countryId });
                return res ? Ok(res) : BadRequest(res);
            }
            return BadRequest();
        }
    }
}
