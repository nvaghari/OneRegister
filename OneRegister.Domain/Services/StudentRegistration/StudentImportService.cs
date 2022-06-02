using CsvHelper;
using Microsoft.AspNetCore.Http;
using Npoi.Mapper;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.StudentRegistration;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Model;
using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OneRegister.Domain.Services.StudentRegistration
{
    public class StudentImportService
    {
        private readonly IOrganizationRepository<School> _schoolRepository;
        private readonly IOrganizedRepository<ClassRoom> _classRepository;
        private readonly IOrganizedRepository<HomeRoom> _homeRoomRepository;
        private readonly IOrganizedRepository<Student> _studentRepository;

        public StudentImportService(
            IOrganizationRepository<School> schoolRepository,
            IOrganizedRepository<ClassRoom> classRepository,
            IOrganizedRepository<HomeRoom> homeRoomRepository,
            IOrganizedRepository<Student> studentRepository)
        {
            _schoolRepository = schoolRepository;
            _classRepository = classRepository;
            _homeRoomRepository = homeRoomRepository;
            _studentRepository = studentRepository;
        }

        public List<StudentImportModel> ReadFile(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) == ".csv")
            {
                return ReadCsvFile(file);
            }
            else if (Path.GetExtension(file.FileName) == ".xls")
            {
                return ReadXlsFile(file);
            }
            else if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                return ReadXlsxFile(file);
            }
            else
            {
                throw new ApplicationException($"{Path.GetExtension(file.FileName)} extension is not supported");
            }
        }

        private List<StudentImportModel> ReadXlsFile(IFormFile file)
        {
            try
            {
                var workbook = new HSSFWorkbook(file.OpenReadStream());
                var sheet = workbook.GetSheetAt(0);
                var header = sheet.GetRow(0);
                var importer = new Mapper(workbook);
                var models = importer.Take<StudentImportModel>(0).ToList();
                return models.Where(m => m.Value.School != null).Select(m => m.Value).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private List<StudentImportModel> ReadXlsxFile(IFormFile file)
        {
            try
            {
                var workbook = new XSSFWorkbook(file.OpenReadStream());
                var importer = new Mapper(workbook);
                var models = importer.Take<StudentImportModel>().ToList();
                return models.Where(m => m.Value.School != null).Select(m => m.Value).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private List<StudentImportModel> ReadCsvFile(IFormFile file)
        {
            try
            {

                using var reader = new StreamReader(file.OpenReadStream());
                using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                //csvReader.Configuration.MissingFieldFound = null;
                //csvReader.Configuration.IgnoreBlankLines = true;
                //csvReader.Configuration.RegisterClassMap<StudentCsvFileMapper>();
                return csvReader.GetRecords<StudentImportModel>().Where(r => !string.IsNullOrEmpty(r.School)).ToList();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Only CSV file is acceptable. you can use template file linked into page to fill and convert to CSV");
            }
        }
        public int ImportAcceptedRecords(List<StudentImportModel> records)
        {
            int count = 0;
            foreach (var record in records)
            {
                try
                {
                    var newStudent = new Student();
                    var schoolResult = _schoolRepository.GetByName(record.School, true);
                    if (schoolResult == null) continue;
                    newStudent.SchoolId = schoolResult.Id;

                    if (!string.IsNullOrEmpty(record.Class))
                    {
                        var classResult = InsertClassRoom(record.Class, record.ClassLabel, Convert.ToInt32(record.Year), schoolResult.Id);
                        if (!classResult.IsSuccessful) continue;
                        newStudent.ClassRoomId = classResult.Id;
                    }

                    if (!string.IsNullOrEmpty(record.HomeRoom))
                    {
                        var homeRoomResult = InsertHomeRoom(record.HomeRoom, Convert.ToInt32(record.Year), schoolResult.Id);
                        if (!homeRoomResult.IsSuccessful) continue;
                        newStudent.HomeRoomId = homeRoomResult.Id;
                    }

                    newStudent = MapStudentFromRecord(newStudent, record);
                    _studentRepository.Add(newStudent);
                    count++;
                }
                catch (Exception ex)
                {

                }
            }
            return count;
        }
        private Student MapStudentFromRecord(Student student, StudentImportModel record)
        {
            student.StudentNumber = record.StudentNumber;
            student.Name = record.Name;
            student.Gender = record.Gender == "F" ? Gender.Female : Gender.Male;
            student.Nationality = record.Nationality;
            student.IdentityType = record.IdentityType;
            student.IdentityNumber = record.IdentityNumber;
            student.BirthDay = record.Birthday;
            student.ParentName = record.ParentName;
            student.ParentPhone = record.ParentPhone;
            if (!string.IsNullOrEmpty(record.Address))
            {
                student.MemberAddresses.Add(new MemberAddress
                {
                    Name = record.Name,
                    Address = record.Address,
                    AddressType = AddressType.ResidentialAddress,
                });
            }
            return student;
        }
        private PersistResult InsertClassRoom(string className, string label, int year, Guid schoolId)
        {
            var result = new PersistResult();
            var oClass = _classRepository.Entities.FirstOrDefault(c =>
            c.Name == className.Trim() &&
            c.Organization.Id == schoolId &&
            c.Year == year);
            if (oClass != null)
            {
                result.Id = oClass.Id;
                return result;
            }
            var newClass = new ClassRoom
            {
                Name = className.Trim(),
                Year = year,
                Label = label,
                OrganizationId=schoolId,
                CreatedBy = _classRepository.CurrentUserId
            };
            _classRepository.Add(newClass);
            result.Id = newClass.Id;
            return result;
        }

        private PersistResult InsertHomeRoom(string homeRoomName, int year, Guid schoolId)
        {
            var result = new PersistResult();
            var homeRoom = _homeRoomRepository.Entities.FirstOrDefault(h =>
            h.Name == homeRoomName.Trim()
            && h.Organization.Id == schoolId
            && h.Year == year);
            if (homeRoom != null)
            {
                result.Id = homeRoom.Id;
                return result;
            }
            var newHomeRoom = new HomeRoom
            {
                Name = homeRoomName.Trim(),
                Year = year,
                OrganizationId = schoolId,
                CreatedBy = _homeRoomRepository.CurrentUserId
            };
            _homeRoomRepository.Add(newHomeRoom);
            result.Id = newHomeRoom.Id;
            return result;
        }

    }
}
