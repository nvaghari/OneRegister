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
    public class ClassRoomService
    {
        private readonly IOrganizedRepository<ClassRoom> _classRoomRepository;
        private readonly SchoolService _schoolService;

        public ClassRoomService(
            IOrganizedRepository<ClassRoom> classRepository,
            SchoolService schoolService)
        {
            _classRoomRepository = classRepository;
            _schoolService = schoolService;
        }

        public PersistResult Add(Guid schoolId, int year, string name, string label)
        {
            if (year < 2000 || year > 3000)
            {
                return new PersistResult("Year you selected is not valid");
            }
            var school = _schoolService.GetSchool(schoolId);
            if (school == null)
            {
                return new PersistResult("School you selected is not valid");
            }
            if (string.IsNullOrEmpty(name))
            {
                return new PersistResult("Class name is required");
            }
            if (AnyByNameAndYearAndSchool(name, year, schoolId))
            {
                return new PersistResult("Class name does exist");
            }
            var newClass = new ClassRoom
            {
                Name = name,
                Label = label,
                Year = year,
                OrganizationId = schoolId
            };
            _classRoomRepository.Add(newClass);
            return PersistResult.SuccessWithId(newClass.Id);
        }

        public PersistResult Update(Guid classId, string name, string label)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new PersistResult("Class name is required");
            }
            ClassRoom classRoom = _classRoomRepository.GetById(classId);
            if (classRoom == null)
            {
                return new PersistResult("the Class doesn't exist or you don't have permission");
            }
            if (AnyByNameAndYearAndSchool(name, classRoom.Year.Value, classRoom.OrganizationId, classRoom.Id))
            {
                return new PersistResult("Class name does exist");
            }
            classRoom.Name = name;
            classRoom.Label = label;
            _classRoomRepository.Update(classRoom);
            return PersistResult.SuccessWithId(classRoom.Id);
        }

        internal ClassRoom FindById(Guid id)
        {
            return _classRoomRepository.GetById(id);
        }

        public ClassRoom RetrieveByNameAndYear(string className, int year)
        {
            return _classRoomRepository.Entities.FirstOrDefault(c => c.Name == className && c.Year == year);
        }
        private bool AnyByNameAndYearAndSchool(string name, int year, Guid schoolId, Guid classId)
        {
            return _classRoomRepository.Entities.Any(c => c.Id != classId && c.Organization.Id == schoolId && c.Year == year && c.Name == name);
        }
        private bool AnyByNameAndYearAndSchool(string name, int year, Guid schoolId)
        {
            return _classRoomRepository.Entities.Any(c => c.Organization.Id == schoolId && c.Year == year && c.Name == name);
        }
        public ICollection<ClassRoom> RetrieveBySchoolYear(Guid schoolId, int year)
        {
            return _classRoomRepository.Entities
                 .Where(c => c.Organization.Id == schoolId && c.Year == year)
                 .AsNoTracking()
                 .ToList();
        }
    }
}
