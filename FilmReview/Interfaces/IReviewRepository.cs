using FilmReview.Models;

namespace FilmReview.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAll();
        Task<Review> GetById(int id);
        Task<IEnumerable<Review>> GetByUserId(long userId);
        Task<IEnumerable<Review>> GetByUserName(string userName);
        Task<IEnumerable<Review>> GetByFilmId(long filmId);
        Task<bool> ReviewExists(int reviewId);
        Task<bool> CreateReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(Review review);
        Task<bool> Save();
    }
}
