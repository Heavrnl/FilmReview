using FilmReview.Models;

namespace FilmReview.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAll();
        Task<IEnumerable<Rating>> GetByUserId(long userId);
        Task<IEnumerable<Rating>> GetByUserName(string userName);

        Task<IEnumerable<Rating>> GetByFilmId(long filmId);
        void ComputeAvgScore(Rating rating);
        Task<Rating> GetById(long id);
        Task<bool> RatingExists(int ratingId);
        Task<bool> CreateRating(Rating rating);
        Task<bool> UpdateRating(Rating rating);
        Task<bool> DeleteRating(Rating rating);
        Task<bool> Save();
    }
}
