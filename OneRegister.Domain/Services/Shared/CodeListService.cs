using OneRegister.Domain.Repository;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Domain.Services.Shared
{
    public class CodeListService
    {
        private readonly GemsRepository _gemsRepository;

        public CodeListService(GemsRepository gemsRepository)
        {
            _gemsRepository = gemsRepository;
        }
        public Dictionary<string, string> GetCountryList()
        {
            return _gemsRepository.GetCountries().OrderBy(i => i.CountryName).ToDictionary(i => i.CountryCode, i => i.CountryName);
        }
        public Dictionary<string, string> GetCountryStateList()
        {
            return _gemsRepository.GetCountryStates().OrderBy(i => i.StateName).ToDictionary(i => i.StateISOCode, i => i.StateName);
        }
        public Dictionary<string, string> GetOccupationList()
        {
            return _gemsRepository.GetOccupations().OrderBy(i => i.ShortName).ToDictionary(i => i._Code, i => i.ShortName);
        }
        public Dictionary<string, string> GetIndustryList()
        {
            return _gemsRepository.GetIndustries().OrderBy(i => i.ShortName).ToDictionary(i => i._Code, i => i.ShortName);
        }
        public Dictionary<string, string> GetRemitPurposeList()
        {
            return _gemsRepository.GetRemitPurposes().OrderBy(i => i.ShortName).ToDictionary(i => i._Code, i => i.ShortName);
        }
        public Dictionary<string, string> GetBankList()
        {
            return _gemsRepository.GetBanks().OrderBy(i => i.BankName).ToDictionary(i => i.BankCode, i => i.BankName);
        }

        public Dictionary<string, string> GetIdentityTypeList()
        {
            return _gemsRepository.GetIdentityTypes().OrderBy(i => i.ShortName).ToDictionary(i => i._Code, i => i.ShortName);
        }
    }
}
