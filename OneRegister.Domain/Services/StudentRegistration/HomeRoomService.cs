using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.StudentRegistration;
using OneRegister.Domain.Model;
using OneRegister.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRegister.Domain.Services.StudentRegistration
{
    public class HomeRoomService
    {
        private readonly IOrganizedRepository<HomeRoom> _homeRoomRepository;
        private readonly SchoolService _schoolService;

        public HomeRoomService(IOrganizedRepository<HomeRoom> homeRoomRepository,
            SchoolService schoolService)
        {
            _homeRoomRepository = homeRoomRepository;
            _schoolService = schoolService;
        }

        public PersistResult Add(Guid schoolId, int year, string name)
        {
            if (year < 2000 || year > 3000)
            {
                return new PersistResult("Year you selected is not valid");
            }
            if (string.IsNullOrEmpty(name))
            {
                return new PersistResult("HomeRoom name is required");
            }
            var school = _schoolService.GetSchool(schoolId);
            if (school == null)
            {
                return new PersistResult("School you selected is not valid");
            }
            if (AnyNameBySchoolYear(name, year, schoolId))
            {
                return new PersistResult("HomeRoom name does exist");
            }
            var homeRoom = new HomeRoom
            {
                Name = name,
                Year = year,
                OrganizationId = schoolId
            };
            _homeRoomRepository.Add(homeRoom);
            return PersistResult.SuccessWithId(homeRoom.Id);
        }

        public PersistResult Update(Guid homeId, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new PersistResult("HomeRoom name is required");
            }
            HomeRoom homeRoom = _homeRoomRepository.GetById(homeId);
            if (homeRoom == null)
            {
                return new PersistResult("the HomeRoom doesn't exist or you don't have permission");
            }
            if (AnyNameBySchoolYear(name, homeRoom.Year.Value, homeRoom.OrganizationId))
            {
                return new PersistResult("HomeRoom name does exist");
            }
            homeRoom.Name = name;
            _homeRoomRepository.Update(homeRoom);
            return PersistResult.SuccessWithId(homeRoom.Id);
        }

        internal HomeRoom FindById(Guid id)
        {
            return _homeRoomRepository.GetById(id);
        }
        private bool AnyNameBySchoolYear(string name, int year, Guid schoolId)
        {
            return _homeRoomRepository.Entities.Any(h => h.Organization.Id == schoolId && h.Year == year && h.Name == name);
        }
        public ICollection<HomeRoom> RetrieveBySchoolYear(Guid schoolId, int year)
        {
            return _homeRoomRepository.Entities
                    .Where(c => c.Organization.Id == schoolId && c.Year == year)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
