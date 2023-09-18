using FilmReview.Interfaces;
using FilmReview.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FilmReview.Repository
{
    public class RatingRepository: IRatingRepository 
    {
        private readonly DataContext _dataContext;

        public RatingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateRating(Rating rating)
        {

            await _dataContext.ratings.AddAsync(rating);
            ComputeAvgScore(rating);
            return await Save();
        }

        public async Task<bool> DeleteRating(Rating rating)
        {
            _dataContext.ratings.Remove(rating);
            ComputeAvgScore(rating);
            return await Save();
        }

        public async Task<IEnumerable<Rating>> GetAll()
        {
            return await _dataContext.ratings
                            .Include(r=>r.Film)
                            .Include(r=>r.User)
                            .ToArrayAsync();
        }

        public async Task<IEnumerable<Rating>> GetByFilmId(long filmId)
        {
            return await _dataContext.ratings
                    .Where(r=>r.Film.FilmId == filmId)
                    .Include(r => r.Film)
                    .Include(r => r.User)
                    .ToListAsync();
        }

        public async Task<Rating> GetById(long id)
        {
            return await _dataContext.ratings.Include(r => r.Film)
                            .Include(r => r.User).FirstOrDefaultAsync(r => r.RatingId == id);
        }

        public async Task<IEnumerable<Rating>> GetByUserId(long userId)
        {
            return await _dataContext.ratings
                            .Where(r=>r.User!.Id == userId)
                            .Include(r => r.Film)
                            .Include(r => r.User)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetByUserName(string userName)
        {
            return await _dataContext.ratings
                            .Where(r => r.User!.UserName == userName)
                            .Include(r => r.Film)
                            .Include(r => r.User)
                            .ToListAsync();
        }

        public async Task<bool> RatingExists(int ratingId)
        {
            return await _dataContext.ratings.AnyAsync(r=>r.RatingId == ratingId);
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateRating(Rating rating)
        {
            _dataContext.ratings.Update(rating);
            ComputeAvgScore(rating);
            return await Save();
        }

        public async void ComputeAvgScore(Rating rating)
        {
            // 计算电影的新平均评分
            var film = await _dataContext.films
                .FirstOrDefaultAsync(f => f.FilmId == rating.Film.FilmId);

            if (film != null)
            {
                // 更新电影的平均评分
                double newAverageRating = film.Ratings.Average(r => r.Score);
                int roundedAverageRating = (int)Math.Round(newAverageRating);
                film.Score = roundedAverageRating;
            }
        }

    }
}
