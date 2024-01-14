using DAO.Models;
namespace Repository
{
    public interface ICountryCodeRepository
    {
        string GetCountryProfileDTO(string countryId);
        string GetCountryCodes();
        string GetCountryCodeJson(string CountryId);

        int GetCountryPopulation(string countryCode);
        string GetCountryName(string countryId);
        CountryCode GetCountryCode(string countryId);
        int GetCountryPopulationRank(string countryCode);
        string GetCountryRankingProfile(string countryId);

    }
}
