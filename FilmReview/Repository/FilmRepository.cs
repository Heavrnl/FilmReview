using FilmReview.Interfaces;
using FilmReview.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmReview.Repository
{
    public class FilmRepository : IFilmRepository
    {
        private readonly DataContext _dataContext;

        public FilmRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Film>> GetFilmsWithFilters(int? categoryId, long? filmId,string? filmName, string? categoryName, string? directorName, int? rating)
        {
            var query = _dataContext.films
                            .Include(f => f.Category)
                            .AsQueryable();

            if (filmId.HasValue)
            {
                query = query.Where(f => f.FilmId == filmId);
            }

            if (!string.IsNullOrEmpty(filmName))
            {
                query = query.Where(f => f.Name.Contains(filmName));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(f => f.Category.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(f => f.Category.Name == categoryName);
            }

            if (!string.IsNullOrEmpty(directorName))
            {
                query = query.Where(f => f.Director.Contains(directorName));
            }

            if (rating.HasValue)
            {
                query = query.Where(f => f.Ratings.Average(r => r.Score) == rating);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CreateFilm(Film film)
        {
            await _dataContext.films.AddAsync(film);
            return await Save();
        }

        public async Task<bool> DeleteFilm(Film film)
        {
            _dataContext.films.Remove(film);
            return await Save();
        }

        public async Task<bool> FilmExists(long filmId)
        {
            return await _dataContext.films.AnyAsync(f => f.FilmId == filmId);
        }

        public async Task<bool> FilmExists(string filmName)
        {
            return await _dataContext.films.AnyAsync(f => f.Name == filmName);
        }

        public async Task<IEnumerable<Film>> GetAll()
        {
            return await _dataContext.films
                .Include(f => f.Category)
                .OrderBy(o => o.FilmId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Film>> GetByCategoryId(int id)
        {
            return await _dataContext.films
                .Include(f => f.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Film>> GetByCategoryName(string name)
        {
            return await _dataContext.films
                        .Where(f => f.Category!.Name!.Equals(name))
                        .ToListAsync();
        }

        public async Task<IEnumerable<Film>> GetByDirector(string directorName)
        {
            return await _dataContext.films
                            .Where(f => f.Director!.Equals(directorName))
                            .ToListAsync();
        }

        public async Task<Film> GetById(long id)
        {
            return await _dataContext.films.Include(f => f.Category).FirstOrDefaultAsync(f => f.FilmId == id);

        }

        public async Task<Film> GetByName(string name)
        {
            return await _dataContext.films.Include(f => f.Category).FirstOrDefaultAsync(f => f.Name!.Equals(name));
        }

        public async Task<IEnumerable<Film>> GetByRating(int score)
        {
            var films = await _dataContext.films
                            .Where(film => film.Score ==  score)
                            .Include(f => f.Category)
                            .ToListAsync();

            return films;
        }


        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateFilm(Film film)
        {
            _dataContext.films.Update(film);
            return await Save();
        }
    }
}
