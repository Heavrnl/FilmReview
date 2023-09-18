using FilmReview.Models;

namespace FilmReview.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();

        Task<Category?> GetById(int id);

        Task<Category?> GetByName(string name);

        Task<bool> CategoryExists(int categoryId);
        Task<bool> CategoryExists(string categoryName);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(Category category);
        Task<bool> Save();
    }
}
