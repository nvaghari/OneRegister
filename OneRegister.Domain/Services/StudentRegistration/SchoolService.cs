using OneRegister.Data.Contract;
using OneRegister.Data.Entities.EDuit;
using OneRegister.Data.Entities.StudentRegistration;
using OneRegister.Domain.Model;
using OneRegister.Domain.Repository;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.StudentRegistration
{
    public class SchoolService
    {
        private readonly AuthorizationService _authorizationService;
        private readonly EDuitRepository _eDuitRepository;
        private readonly OrganizationService _organizationService;
        private readonly IOrganizationRepository<School> _schoolRepository;
        public SchoolService(IOrganizationRepository<School> schoolRepository,
            OrganizationService organizationService,
            EDuitRepository eDuitRepository,
            AuthorizationService authorizationService)
        {
            _schoolRepository = schoolRepository;
            _organizationService = organizationService;
            _eDuitRepository = eDuitRepository;
            _authorizationService = authorizationService;
        }

        public PersistResult AddByName(string name)
        {
            if (_authorizationService.GetCurrentUserId() != BasicUser.AdminId)
            {
                return new PersistResult("You Don't Have Permission to add new school");
            }
            if (string.IsNullOrEmpty(name) || !name.Replace(" ", string.Empty).All(char.IsLetterOrDigit))
            {
                return new PersistResult("please enter proper name for school");
            }
            if (_schoolRepository.AnyByNameAsAdmin(name))
            {
                return new PersistResult("The school name does exist");
            }

            var school = new School
            {
                Name = name,
                ParentId = BasicOrganizations.School,
            };
            _schoolRepository.Add(school);
            return PersistResult.SuccessWithId(school.Id);
        }

        public bool Any(string name)
        {
            return _schoolRepository.AnyByNameAsAdmin(name);
        }

        public IEnumerable<School> GetAll(bool IsNoTracking = false)
        {
            return _schoolRepository.GetList(IsNoTracking);
        }
        public IEnumerable<School> GetAllAsAdmin(bool IsNoTracking = false)
        {
            return _schoolRepository.GetListAsAdmin(IsNoTracking);
        }

        public School GetSchool(Guid schoolId)
        {
            return _schoolRepository.GetById(schoolId);
        }

        public PersistResult Update(Guid schoolId, string name)
        {
            if (_authorizationService.GetCurrentUserId() != BasicUser.AdminId)
            {
                return new PersistResult("You Don't Have Permission to add new school");
            }
            if (string.IsNullOrEmpty(name) || !name.Replace(" ", string.Empty).All(char.IsLetterOrDigit))
            {
                return new PersistResult("please enter proper name for school");
            }
            if (_schoolRepository.AnyByName(name))
            {
                return new PersistResult("The school name does exist");
            }
            var school = _schoolRepository.GetById(schoolId);
            if (school == null)
            {
                return new PersistResult("School doesn't exist");
            }
            school.Name = name;
            _schoolRepository.Update(school);
            return PersistResult.SuccessWithId(school.Id);
        }

        private static string MakeCode(string name, List<EDuitSchool> schools)
        {
            var words = name.Split(' ');
            var code = string.Concat(words.Select(c => c[0]));
            int indexer = 1;
            string newCode = code;
            while (schools.Any(s => s.Code == newCode))
            {
                newCode = code + indexer.ToString();
                indexer++;
            }

            return newCode.ToUpper();
        }

        private void CreateSchoolInEDuitDatabase(string name)
        {
            List<EDuitSchool> schools = _eDuitRepository.GetSchools();
            if (!schools.Any(s => s.ShortName == name))
            {
                _eDuitRepository.AddSchool(new EDuitSchool
                {
                    Code = MakeCode(name, schools),
                    LongName = name,
                    ShortName = name
                });
            }
        }
    }
}
