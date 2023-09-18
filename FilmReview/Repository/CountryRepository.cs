using FilmReview.Interfaces;
using FilmReview.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmReview.Repository
{
    public class CountryRepository: ICountryRepository
    {
        private readonly DataContext _dataContext;


        public CountryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CountryExists(string countryName)
        {
            return await _dataContext.countries.AnyAsync(c => c.Name == countryName);
        }

        public async Task<bool> CountryExists(int countryId)
        {
            return await _dataContext.countries.AnyAsync(c => c.CountryId == countryId);
        }

        public  async Task<bool> CreateCountry(Country country)
        {
            await  _dataContext.countries.AddAsync(country);
            return await  Save();
        }

        public async Task<bool> DeleteCountry(Country country)
        {
            _dataContext.countries.Remove(country);
            return await Save();
        }

        public async Task<IEnumerable<Country>> GetAll()
        {
            return await _dataContext.countries.ToListAsync();
        }

        public async Task<Country?> GetById(int id)
        {
           return await _dataContext.countries.FirstOrDefaultAsync(c => c.CountryId == id);
        }

        public Task<Country?> GetByName(string name)
        {
            return _dataContext.countries.FirstOrDefaultAsync(c=>c.Name == name);
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public Task<bool> UpdateCountry(Country country)
        {
            _dataContext.countries.Update(country);
            return Save();
        }
    }
}
