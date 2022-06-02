using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npoi.Mapper;
using OneRegister.Data.Entities.EDuit;
using OneRegister.Data.Entities.StudentRegistration;
using OneRegister.Data.Model.StudentRegistration;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Extentions;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OneRegister.Core.Model;
using OneRegister.Domain.Services.Dms;
using OneRegister.Data.Identication;
using OneRegister.Domain.Model.ExportPhoto;
using Microsoft.EntityFrameworkCore;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Data.Contract;

namespace OneRegister.Domain.Services.StudentRegistration
{
    public class StudentService
    {
        private readonly IOrganizedRepository<Student> _studentRepository;
        private readonly DmsService _dmsService;
        private readonly SchoolService _schoolService;
        private readonly ClassRoomService _classRoomService;
        private readonly HomeRoomService _homeRoomService;
        private readonly EDuitRepository _eDuitRepository;

        public StudentService(
            IOrganizedRepository<Student> studentRepository,
            DmsService dmsService,
            SchoolService schoolService,
            ClassRoomService classRoomService,
            HomeRoomService homeRoomService,
            EDuitRepository eDuitRepository)
        {
            _studentRepository = studentRepository;
            _dmsService = dmsService;
            _schoolService = schoolService;
            _classRoomService = classRoomService;
            _homeRoomService = homeRoomService;
            _eDuitRepository = eDuitRepository;
        }
        internal bool IsIdentityNumberExist(string school, string identityType, string identityNumber)
        {
            return _studentRepository.Entities.Any(s => s.Organization.Name == school && s.IdentityType == identityType && s.IdentityNumber == identityNumber);
        }

        internal bool IsStudentNameExist(string schoolName, string name)
        {
            return _studentRepository.Entities.Any(s => s.Organization.Name == schoolName && s.Name == name);
        }

        public List<SelectListItem> GetClassListItem(Guid schoolId, int year)
        {
            var classList = new List<SelectListItem>();
            var classes = _classRoomService.RetrieveBySchoolYear(schoolId, year);
            classList.Add(new SelectListItem { Selected = true, Text = "-Select Class-", Value = "" });
            classList.AddRange(classes.OrderBy(c => c.Name).Select(c => new SelectListItem
            {
                Text = string.IsNullOrEmpty(c.Label) ? c.Name : $"{c.Name} [{c.Label}]",
                Value = c.Id.ToString()
            }).ToList());
            return classList;
        }

        public Dictionary<string, string> GetClassList(Guid schoolId, int year)
        {
            return _classRoomService.RetrieveBySchoolYear(schoolId, year).OrderBy(c => c.Name).ToDictionary(c => c.Id.ToString(), c => c.Name);
        }

        public Student FindStudentByIdentityNumber(string identityNumber)
        {
            return _studentRepository.Entities
                .Include(s => s.MemberFiles)
                .Include(s => s.MemberAddresses)
                .Include(s => s.ClassRoom)
                .Include(s => s.School)
                .Include(s => s.HomeRoom)
                .FirstOrDefault(s => s.IdentityNumber == identityNumber);
        }

        public StudentRegisterModel GetStudentForEdit(Guid id)
        {
            var student = _studentRepository.GetById(id,false,
                s=>s.MemberFiles,
                s=>s.MemberAddresses,
                s => s.School,
                s => s.HomeRoom,
                s => s.ClassRoom);
            if (student == null)
            {
                throw new ApplicationException("student doesn't exist");
            }

            var model = new StudentRegisterModel()
            {
                Id = student.Id,
                Gender = student.Gender,
                Name = student.Name,
                Nationality = student.Nationality,
                StudentNumber = student.StudentNumber,
                IdentityType = student.IdentityType,
                SchoolId = student.School.Id,
                ClassId = student.ClassRoom?.Id,
                Year = student.ClassRoom?.Year,
                HomeRoomId = student.HomeRoom?.Id,
                IdentityNumber = student.IdentityNumber,
                Birthday = student.BirthDay,
                ParentName = student.ParentName,
                ParentPhone = student.ParentPhone,
                State = student.State,
                PhotoId = student.MemberFiles?.SingleOrDefault(f => f.FileType == MemberFileType.Photo)?.DmsId,
                PhotoUrl = _dmsService.GetFileUrl(student.MemberFiles?.FirstOrDefault(f => f.FileType == MemberFileType.Photo)),
                Address = student.MemberAddresses?.SingleOrDefault(a => a.AddressType == AddressType.ResidentialAddress)?.Address
            };
            return model;
        }

        public List<ExportStudentModel> GetStudentsForExportPhoto(ExportStudentListParams model)
        {
            var query = _studentRepository.Entities
                .Include(s => s.School)
                .Include(s => s.ClassRoom)
                .Include(s=> s.MemberFiles)
                .Where(s=> s.MemberFiles.Any(f=> f.FileType == MemberFileType.Photo && f.DmsUrl.HasValue))
                .Where(s => model.SchoolId == model.SchoolId)
                .Where(s => model.ClassId == null || s.ClassRoomId == model.ClassId)
                .Where(s => model.HomeRoomId == null || s.HomeRoomId == model.HomeRoomId);

            return query.Select(q => new ExportStudentModel { 
                Id = q.Id,
                Ic = q.IdentityNumber,
                PhotoId = q.MemberFiles.First(f => f.FileType == MemberFileType.Photo).DmsUrl.Value
            }).ToList();

        }

        public ExportStudentModel GetStudentForExportPhoto(string icNumber,List<Guid> authorizedOrgs)
        {
            var student = _studentRepository.Entities
                .Include(s => s.MemberFiles)
                .Where(s => s.MemberFiles.Any(f => f.FileType == MemberFileType.Photo))
                .FirstOrDefault(s => s.IdentityNumber == icNumber);

            if (student == null) return null;
            if (!authorizedOrgs.Contains(student.SchoolId.Value)) return null;

            return new ExportStudentModel
            {
                Id = student.Id,
                Ic = student.IdentityNumber,
                PhotoId = student.MemberFiles.First(f => f.FileType == MemberFileType.Photo).DmsUrl.Value
            };
        }

        public FullResponse Register(StudentRegisterModel model)
        {
            try
            {
                Student student = new()
                {
                    StudentNumber = model.StudentNumber,
                    Name = model.Name,
                    IdentityNumber = model.IdentityNumber,
                    IdentityType = model.IdentityType,
                    Gender = model.Gender.Value,
                    Nationality = model.Nationality,
                    BirthDay = model.Birthday,
                    SchoolId = model.SchoolId,
                    ClassRoomId = model.ClassId,
                    ParentName = model.ParentName,
                    ParentPhone = model.ParentPhone,
                    HomeRoomId = model.HomeRoomId
                };
                if (model.Photo != null)
                {
                    AddFile(student, model.Photo, MemberFileType.Photo, model.Thumbnail.ToByte());
                }
                if (!string.IsNullOrEmpty(model.Address))
                {
                    AddAddress(student, model.Address, AddressType.ResidentialAddress);
                }

                _studentRepository.Add(student);
                return FullResponse.SuccessWithId(student.Id.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        private Student AddAddress(Student student, string address, AddressType addressType)
        {
            student.MemberAddresses.Add(new MemberAddress
            {
                Name = student.Name,
                Address = address,
                AddressType = addressType
            });

            return student;
        }

        private Student AddFile(Student student, IFormFile file, MemberFileType fileType, byte[] thumbnail)
        {
            var (docId, url) = _dmsService.InsertFile(file);
            student.MemberFiles.Add(new MemberFile
            {
                Name = file.Name,
                DmsId = docId,
                Thumbnail = thumbnail,
                DmsUrl = url,
                FileType = fileType
            });

            return student;
        }


        public FullResponse Update(StudentRegisterModel model)
        {
            try
            {
                var student = _studentRepository.GetById(model.Id);
                if (student.State == StateOfEntity.Complete)
                {
                    throw new ApplicationException("Student status is completed.");
                }
                student.StudentNumber = model.StudentNumber;
                student.Name = model.Name;
                student.IdentityNumber = model.IdentityNumber;
                student.IdentityType = model.IdentityType;
                student.Gender = model.Gender.Value;
                student.Nationality = model.Nationality;
                student.BirthDay = model.Birthday;
                student.SchoolId = model.SchoolId;
                student.ClassRoomId = model.ClassId;
                student.HomeRoomId = model.HomeRoomId;
                student.ParentName = model.ParentName;
                student.ParentPhone = model.ParentPhone;

                UpdateFile(student, model.Photo, MemberFileType.Photo, model.Thumbnail.ToByte());
                UpdateAddress(student, model.Address, AddressType.ResidentialAddress);
                _studentRepository.Update(student);
                return FullResponse.SuccessWithId(student.Id.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        private Student UpdateAddress(Student student, string address, AddressType addressType)
        {
            if (!string.IsNullOrEmpty(address))
            {
                var studentAddress = student.MemberAddresses.SingleOrDefault(a => a.AddressType == addressType);
                if (studentAddress == null)
                {
                    student.MemberAddresses.Add(new MemberAddress
                    {
                        Name = student.Name,
                        Address = address,
                        AddressType = addressType
                    });
                }
                else
                {
                    studentAddress.Name = student.Name;
                    studentAddress.Address = address;
                }
            }

            return student;
        }

        private Student UpdateFile(Student student, IFormFile file, MemberFileType fileType, byte[] thumbnail)
        {
            if (file != null)
            {
                var studentFile = student.MemberFiles.SingleOrDefault(f => f.FileType == fileType);
                if (studentFile == null)
                {
                    var (docId, url) = _dmsService.InsertFile(file);
                    student.MemberFiles.Add(new MemberFile
                    {
                        Name = file.Name,
                        DmsId = docId,
                        Thumbnail = thumbnail,
                        DmsUrl = url,
                        FileType = fileType
                    });
                }
                else
                {
                    var (editedDocId, editedUrl) = _dmsService.UpdateFile(studentFile.DmsId.Value, file);
                    studentFile.Name = file.Name;
                    studentFile.Thumbnail = thumbnail;
                    studentFile.DmsId = editedDocId;
                    studentFile.DmsUrl = editedUrl;
                }
            }
            return student;
        }

        public List<StudentExportModel> SearchStudentForExport(StudentExportSearchParam filterParams)
        {
            var result = SearchByParams(filterParams);
            return result.Select(r => new StudentExportModel
            {
                School = r.School.Name,
                Year = r.ClassRoom.Year.Value.ToString(),

                Name = r.Name.ToUpper(),
                StudentNumber = r.StudentNumber,
                @Class = r.ClassRoom.Name,
                Gender = r.Gender.ToGenderString(),
                IdentityNumber = r.IdentityNumber,
                IdentityType = r.IdentityType,
                Nationality = r.Nationality,
                HasPicture = r.MemberFiles.FirstOrDefault(f => f.FileType == MemberFileType.Photo) == null ? "No" : "Yes",
                ClassLabel = r.ClassRoom.Label,
                HomeRoom = r.HomeRoom?.Name,
                Birthday = r.BirthDay?.ToString("yyyyMMdd"),
                ParentName = r.ParentName,
                ParentNumber = r.ParentPhone,
                ParentAddress = r.MemberAddresses.SingleOrDefault(m => m.AddressType == AddressType.ResidentialAddress)?.Address
            }).ToList();
        }

        public byte[] CreateExcelFile(List<StudentExportModel> result)
        {
            var mapper = new Mapper();
            var mStream = new MemoryStream();
            mapper.Save(mStream, result, "students", overwrite: true, xlsx: false);
            return mStream.ToArray();
        }

        public DtReturn<StudentListModel> RetrieveStudentsForList(DtReceive dtReceive)
        {
            var result = new DtReturn<StudentListModel>();
            List<Student> fetchedStudents = RetrieveForList(dtReceive.Search.Value, dtReceive.Start, dtReceive.Length, out int total, out int filteredTotal);
            result.RecordsTotal = total;
            result.RecordsFiltered = filteredTotal;
            result.Draw = dtReceive.Draw;
            result.Data = new List<StudentListModel>(fetchedStudents.Count);
            result.Data.AddRange(fetchedStudents.Select(s => new StudentListModel
            {
                Gender = s.Gender.ToGenderString(),
                Id = s.Id,
                IdentityNumber = s.IdentityNumber,
                Name = s.Name,
                Nationality = s.Nationality,
                IdentityType = s.IdentityType,
                StudentNumber = s.StudentNumber,
                HasPicture = s.MemberFiles.Any(f => f.FileType == MemberFileType.Photo),
                School = s.School.Name,
                Status = s.State.ToString()
            }));
            return result;
        }

        public static string GenerateName()
        {
            var t = DateTime.Now;
            return t.ToString("yyyyMMdd_Hmmss");
        }

        public byte[] GetThumbnail(Guid studentId)
        {
            const string unknownPicBase64Str = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAYEBQYFBAYGBQYHBwYIChAKCgkJChQODwwQFxQYGBcUFhYaHSUfGhsjHBYWICwgIyYnKSopGR8tMC0oMCUoKSj/2wBDAQcHBwoIChMKChMoGhYaKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCj/wAARCABCADIDASIAAhEBAxEB/8QAGgABAAIDAQAAAAAAAAAAAAAAAAUGAQIEA//EACoQAAICAQMCBQMFAAAAAAAAAAABAgMEBRESIWETIjFBgQZCUTRicXKx/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/ALAd2mYEsufKW6pT6v8APZHCWrTKlVg1JbdVyfyB60UVUR2qrjFdvV/Jm+iq+HG6Cku/segAhdR0qEKpW42649XFvfp2IUuVkVOuUH6STRTpLaTXcDAAAFvxP0tP9I/4VAn9ByfEpdMn5odV/AEqAABTrePiz4b8d3tuWTVsl4+I+L2nPyrt3KwAAAA7NJslXn1cfufF/JtRpeTa/NDw1v1cicwcKvEhtHzTfrJoDqAAED9RN+PUt3tx329vUiS3ZWPXk1OFkU/XZv2f5K7k6dkUNtwcor7o9QOMB9Hs+jAF0AAAAAAABo6q223XBt/tQNwAAAAAAAAAAAH/2Q==";
            try
            {
                byte[] thumbnail = GetThumbnailFromDatabase(studentId);
                if (thumbnail == null)
                {
                    return Convert.FromBase64String(unknownPicBase64Str);
                }

                return thumbnail;
            }
            catch (Exception ex)
            {
                //log ex
                return Convert.FromBase64String(unknownPicBase64Str);
            }
        }

        public void ConfirmStudent(Guid studentId)
        {
            if (!_eDuitRepository.IsConnected())
            {
                throw new Exception("Can not connect to eDuit database, please check connection string");
            }
            var student = _studentRepository.GetById(studentId);
            if (student == null)
            {
                throw new Exception("Student doesn't exist");
            }
            if (student.State == StateOfEntity.Complete)
            {
                throw new ApplicationException("Student status is Completed and you can not Approve it again!");
            }
            var model = new StudentConfirmDataModel()
            {
                Id = student.Id,
                Name = student.Name,
                StudentNumber = student.StudentNumber,
                SchoolName = student.School?.Name,
                Nationality = student.Nationality,
                Gender = student.Gender == Gender.Male ? 'M' : 'F',
                Address = student.MemberAddresses?.SingleOrDefault(a => a.AddressType == AddressType.ResidentialAddress)?.Address,
                Birthday = student.BirthDay?.ToString("yyyyMMdd"),
                ClassName = student.ClassRoom?.Name,
                ClassLabel = student.ClassRoom?.Label,
                HomeRoomName = student.HomeRoom?.Name,
                IdentityNumber = student.IdentityNumber,
                IdentityType = student.IdentityType,
                ParentName = student.ParentName,
                ParentPhone = student.ParentPhone,
                Year = student.ClassRoom == null ? 0 : student.ClassRoom.Year.Value,
                DmsRef = student.MemberFiles?.FirstOrDefault(f => f.FileType == MemberFileType.Photo)?.DmsId
            };
            _eDuitRepository.ConfirmStudent(model);

            student.State = StateOfEntity.Complete;
            _studentRepository.Update(student);
        }

        public List<StudentListModel> RetrieveStudents()
        {
            var students = _studentRepository.Entities.Include(s => s.MemberFiles).ToList();
            return students.Select(s => new StudentListModel
            {
                Gender = s.Gender.ToGenderString(),
                Id = s.Id,
                IdentityNumber = s.IdentityNumber,
                Name = s.Name,
                IdentityType = s.IdentityType,
                Nationality = s.Nationality,
                HasPicture = s.MemberFiles.Any(f => f.FileType == MemberFileType.Photo),
                StudentNumber = s.StudentNumber
            }).ToList();
        }

        public ClassessJsonResultModel GetClassesAndHomeRooms(Guid schoolid, int year)
        {
            return new ClassessJsonResultModel
            {
                Classes = _classRoomService.RetrieveBySchoolYear(schoolid, year)
                .Select(c =>
                    new TextValueModel
                    {
                        Text = string.IsNullOrEmpty(c.Label) ? c.Name : $"{c.Name} [{c.Label}]",
                        Value = c.Id.ToString()
                    }).ToList(),
                HomeRooms = _homeRoomService.RetrieveBySchoolYear(schoolid, year)
                .Select(c =>
                    new TextValueModel
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList()
            };
        }

        public List<SelectListItem> GetHomeRoomListItem(Guid schoolId, int year)
        {
            var homeRoomList = new List<SelectListItem>();
            var homeRooms = _homeRoomService.RetrieveBySchoolYear(schoolId, year);
            homeRoomList.Add(new SelectListItem { Selected = true, Text = "-Select Homeroom-", Value = "" });
            homeRoomList.AddRange(homeRooms.OrderBy(h => h.Name).Select(h => new SelectListItem
            {
                Text = h.Name,
                Value = h.Id.ToString()
            }).ToList());
            return homeRoomList;
        }

        public Dictionary<string, string> GetHomeRoomList(Guid schoolId, int year)
        {
            return _homeRoomService.RetrieveBySchoolYear(schoolId, year).OrderBy(h => h.Name).ToDictionary(h => h.Id.ToString(), h => h.Name);
        }

        public List<SelectListItem> GetYearListItem()
        {
            var startYear = 2015;
            var currentYear = DateTime.Now.Year;
            var years = Enumerable.Range(startYear, currentYear - startYear + 1).Reverse();
            var yearsList = new List<SelectListItem>();
            yearsList.Add(new SelectListItem { Selected = true, Text = "-Select Year-", Value = "" });
            yearsList.AddRange(years.Select(y => new SelectListItem
            {
                Text = y.ToString(),
                Value = y.ToString()
            }).ToList());

            return yearsList;
        }

        public static Dictionary<string, string> GetYearList()
        {
            var startYear = 2015;
            var currentYear = DateTime.Now.Year;
            var years = Enumerable.Range(startYear, currentYear - startYear + 1).Reverse();
            return years.ToDictionary(y => y.ToString(), y => y.ToString());
        }

        public List<SelectListItem> GetSchoolListItem()
        {
            List<School> schools = _schoolService.GetAll().ToList();
            var schoolList = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "-Select School-", Value = "" }
            };
            schoolList.AddRange(schools.OrderBy(s => s.Name).Select(s =>
              new SelectListItem
              {
                  Text = s.Name,
                  Value = s.Id.ToString()
              }));

            return schoolList.ToList();
        }

        public Dictionary<string, string> GetSchoolList()
        {
            return _schoolService.GetAll(IsNoTracking: true).OrderBy(s => s.Name).ToDictionary(s => s.Id.ToString(), s => s.Name);
        }

        public bool IsDuplicate(StudentRegisterModel model)
        {
            if (model.Id == Guid.Empty)
            {
                return IsDuplicate(model.Name, model.SchoolId.Value);
            }
            else
            {
                return IsDuplicate(model.Name, model.SchoolId.Value, model.Id);
            }
        }

        internal bool IsStudentNumberExist(string schoolName, string studentNumber)
        {
            return _studentRepository.Entities.Any(s => s.Organization.Name == schoolName && s.StudentNumber == studentNumber);
        }

        internal bool IsSchoolExist(string schoolName)
        {
            return _schoolService.Any(schoolName);
        }

        internal bool IsLabelValid(string label, string className, int year)
        {
            var fetchClass = _classRoomService.RetrieveByNameAndYear(className, year);
            return !(fetchClass != null && fetchClass.Label != label);
        }
        private List<Student> SearchByParams(StudentExportSearchParam filterParams)
        {
            return _studentRepository.FilteredEntities
                .Include(s => s.School)
                .Include(s => s.ClassRoom)
                .Include(s => s.MemberFiles)
                .Include(s => s.HomeRoom)
                .Include(s => s.MemberAddresses)
                .Where(s => filterParams.Year == 0 || s.ClassRoom.Year == filterParams.Year || s.HomeRoom.Year == filterParams.Year)
                .Where(s => filterParams.School == Guid.Empty || s.School.Id == filterParams.School)
                .Where(s => filterParams.Class == Guid.Empty || s.ClassRoom.Id == filterParams.Class)
                .Where(s => filterParams.HomeRoom == Guid.Empty || s.HomeRoom.Id == filterParams.HomeRoom)
                .AsNoTracking()
                .ToList();
        }
        private List<Student> RetrieveForList(string searchValue, int start, int take, out int total, out int count)
        {
            total = _studentRepository.Entities.Count();
            var result = _studentRepository.FilteredEntities
                                .AsNoTracking()
                .Where(s =>
                string.IsNullOrEmpty(searchValue)
                || s.Name.Contains(searchValue)
                || s.StudentNumber.Contains(searchValue)
                || s.IdentityNumber.Contains(searchValue))
                .Include(s => s.MemberFiles)
                .Include(s => s.School)
                .OrderBy(s => s.Name)
                .Skip(start)
                .Take(take)
                .ToList();
            count = _studentRepository.FilteredEntities
            .Where(s =>
                string.IsNullOrEmpty(searchValue)
                || s.Name.Contains(searchValue)
                || s.StudentNumber.Contains(searchValue)
                || s.IdentityNumber.Contains(searchValue))
            .Count();
            return result;
        }
        private byte[] GetThumbnailFromDatabase(Guid studentId)
        {
            var file = _studentRepository.Context.MemberFiles.SingleOrDefault(f => f.MemberId == studentId && f.FileType == MemberFileType.Photo);
            return file?.Thumbnail;
        }
        private bool IsDuplicate(string name, Guid schoolId, Guid studentId)
        {
            return _studentRepository.Entities.Any(s =>
            s.Id != studentId &&
            s.Organization.Id == schoolId &&
            string.Compare(s.Name.ToLower(), name.ToLower().Trim()) == 0);
        }

        private bool IsDuplicate(string name, Guid schoolId)
        {
            return _studentRepository.Entities.Any(s =>
            s.Organization.Id == schoolId &&
            string.Compare(s.Name.ToLower(), name.ToLower().Trim()) == 0);
        }
    }
}