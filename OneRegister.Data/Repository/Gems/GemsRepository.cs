using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Context;
using OneRegister.Data.Entities.Gems;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Domain.Repository
{
    public class GemsRepository
    {
        private readonly GemsContext _context;

        public GemsRepository(GemsContext context)
        {
            _context = context;
        }

        public List<CL_Country> GetCountries()
        {
            return _context.Countries.FromSqlRaw("EXEC [Entity].[CL_Country_List]").AsNoTracking().ToList();
        }
        public List<CL_CountryState> GetCountryStates()
        {
            return _context.CountryStates.FromSqlRaw("EXEC [Entity].[CL_CountryState_List] @CountryCode = N'MY'").AsNoTracking().ToList();
        }
        public List<CL_Industry> GetIndustries()
        {
            return _context.Industries.FromSqlRaw("EXEC	[Entity].[CL_BusinessNature_List]").AsNoTracking().ToList();
        }
        public List<CL_Occupation> GetOccupations()
        {
            return _context.Occupations.FromSqlRaw("EXEC [Entity].[CL_Occupation_List]").AsNoTracking().ToList();
        }
        public List<CL_RemitPurpose> GetRemitPurposes()
        {
            return _context.RemitPurposes.FromSqlRaw("EXEC [Entity].[CL_TxnPurpose_List]").AsNoTracking().ToList();
        }
        public List<CL_Bank> GetBanks()
        {
            return _context.Banks.FromSqlRaw("EXEC [Entity].[CL_Bank_List]").AsNoTracking().ToList();
        }
        public List<CL_IdentityType> GetIdentityTypes()
        {
            return _context.IdentityTypes.FromSqlRaw("EXEC [Entity].[CL_IdType_List]").AsNoTracking().ToList();
        }
    }
}
