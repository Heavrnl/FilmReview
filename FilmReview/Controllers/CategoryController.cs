using AutoMapper;
using FilmReview.Dto;
using FilmReview.Interfaces;
using FilmReview.Models;
using FilmReview.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace FilmReview.Controllers
{

    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var categories =  _mapper.Map<List<CategoryDto>>(await _categoryRepository.GetAll());

            return ModelState.IsValid ? Ok(categories) : BadRequest(ModelState);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId).Result)
                return NotFound();
            var category = _mapper.Map <CategoryDto> (await _categoryRepository.GetById(categoryId));
            return ModelState.IsValid ? Ok(category) : BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody]CategoryDto categoryDto)
        {
            var exist = _categoryRepository.CategoryExists(categoryDto.Name).Result;
            if (!exist)
            {
                var category = _mapper.Map<Category>(categoryDto);
                var res = await _categoryRepository.CreateCategory(category);
                return res ?  Ok(res):BadRequest(res) ;
            }
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(422, ModelState);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if(categoryId != categoryDto.CategoryId || categoryDto.Name == null)
            {
                return BadRequest(ModelState);
            }
            var exist = _categoryRepository.CategoryExists(categoryId).Result;
            if (exist)
            {
                var category = _mapper.Map<Category>(categoryDto) ;
                var res = await _categoryRepository.UpdateCategory(category);
                return res ? Ok(res) : BadRequest(res);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var exist = _categoryRepository.CategoryExists(categoryId).Result;
            if (exist)
            {
                var res = await _categoryRepository.DeleteCategory(new Category() { CategoryId = categoryId });
                return res ? Ok(res) : BadRequest(res);
            }
            return BadRequest();
        }
    }
}
