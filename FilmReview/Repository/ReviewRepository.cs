using FilmReview.Interfaces;
using FilmReview.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmReview.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateReview(Review review)
        {
            await _dataContext.reviews.AddAsync(review);
            return await Save();
        }

        public async Task<bool> DeleteReview(Review review)
        {
            _dataContext.reviews.Remove(review);
            return await Save();
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            return await _dataContext.reviews
                            .Include(r => r.Film)
                            .Include(r => r.User)
                            .ToArrayAsync();
        }

        public async Task<IEnumerable<Review>> GetByFilmId(long filmId)
        {
            return await _dataContext.reviews
                    .Where(r => r.Film.FilmId == filmId)
                    .Include(r => r.Film)
                    .Include(r => r.User)
                    .ToListAsync();
        }

        public async Task<Review> GetById(int id)
        {
            return await _dataContext.reviews
                            .Include(r => r.Film)
                            .Include(r => r.User)
                            .FirstOrDefaultAsync(f => f.ReviewId == id);
        }

        public async Task<IEnumerable<Review>> GetByUserId(long userId)
        {
            return await _dataContext.reviews      
                            .Where(r => r.User!.Id == userId)
                            .Include(r => r.Film)
                            .Include(r => r.User)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserName(string userName)
        {
            return await _dataContext.reviews
                            .Where(r => r.User!.UserName == userName)
                            .Include(r => r.Film)
                            .Include(r => r.User)
                            .ToListAsync();
        }

        public async Task<bool> ReviewExists(int reviewId)
        {
            return await _dataContext.reviews.AnyAsync(r => r.ReviewId == reviewId);
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateReview(Review review)
        {
            _dataContext.reviews.Update(review);
            return await Save();
        }

    }
}
