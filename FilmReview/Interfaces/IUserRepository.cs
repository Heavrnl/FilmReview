using FilmReview.Models;

namespace FilmReview.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetByCountryId(int id);
        Task<IEnumerable<User>> GetByCountryName(string name);


    }
}
