using FilmReview.Models;

namespace FilmReview.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAll();
        Task<Country?> GetById(int id);
        Task<Country?> GetByName(string name);

        Task<bool> CountryExists(int countryId);

        Task<bool> CountryExists(string countryName);
        Task<bool> CreateCountry(Country country);
        Task<bool> UpdateCountry(Country country);
        Task<bool> DeleteCountry(Country country);
        Task<bool> Save();
    }
}
