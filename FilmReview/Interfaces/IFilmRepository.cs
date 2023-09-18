using FilmReview.Models;
using System.Collections.ObjectModel;

namespace FilmReview.Interfaces
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAll();
        Task<Film> GetById(long id);
        Task<Film> GetByName(string name);

        Task<IEnumerable<Film>> GetFilmsWithFilters(int? categoryId, long? filmId, string? filmName, string? categoryName, string? directorName, int? rating);

        Task<IEnumerable<Film>> GetByCategoryId(int id);

        Task<IEnumerable<Film>> GetByCategoryName(string name);

        Task<IEnumerable<Film>> GetByRating(int score);

        Task<IEnumerable<Film>> GetByDirector(string directorName);
        Task<bool> FilmExists(long filmId);

        Task<bool> FilmExists(string filmName);
        Task<bool> CreateFilm(Film film);
        Task<bool> UpdateFilm(Film film);
        Task<bool> DeleteFilm(Film film);
        Task<bool> Save();

    }
}
