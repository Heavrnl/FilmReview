using FilmReview.Interfaces;
using FilmReview.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmReview.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;


        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await _dataContext.categories.AnyAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> CategoryExists(string categoryName)
        {
            return await _dataContext.categories.AnyAsync(c => c.Name == categoryName);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await _dataContext.categories.AddAsync(category);
            return await Save();
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            _dataContext.categories.Remove(category);
            return await Save();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _dataContext.categories.ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await _dataContext.categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category?> GetByName(string name)
        {
            return await _dataContext.categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            _dataContext.Update(category);
            return await Save();
        }
    }
}
