using FilmReview.Interfaces;
using FilmReview.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FilmReview.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;


        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
          
        }

        public async Task<IEnumerable<User>> GetByCountryId(int id)
        {
            return await _dataContext.Users
                            .Where(u => u.Country.CountryId == id)
                            .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByCountryName(string name)
        {
            return await _dataContext.Users
                            .Where(u => u.Country.Name == name)
                            .ToListAsync();
        }

  
    }
}
