using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Data.Model.StudentRegistration;
using OneRegister.Domain.Model.Shared;
using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Services.Shared;
using OneRegister.Domain.Services.StudentRegistration;
using OneRegister.Framework.Extensions;
using OneRegister.Security.Attributes;
using OneRegister.Web.Models.StudentRegistration;
using OneRegister.Web.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    [Menu(Id: "2C416E30-50B6-4A5A-BF32-9952FBB6B0D3", Name: "Student", OrganizationId: BasicOrganizations.School_ID, Order = 2, FasIcon = "fa-user-graduate")]
    public class StudentController : Controller
    {
        private readonly CodeListService _cLService;
        private readonly IMapper _mapper;
        private readonly StudentImportService _studentImportService;
        private readonly StudentService _studentService;
        public StudentController(StudentService studentService,
            StudentImportService studentImportService,
            CodeListService cLService,
            IMapper mapper)
        {
            _studentService = studentService;
            _studentImportService = studentImportService;
            _cLService = cLService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var model = _studentService.GetStudentForEdit(id);
            var viewModel = _mapper.Map<StudentRegisterViewModel>(model);
            FillUpLists(viewModel);
            return View(viewModel);
        }

        [HttpGet]
        [Menu(Id: "6361C047-B4C5-4867-AA4B-411326738683", Name: "Export", OrganizationId: BasicOrganizations.School_ID, Order = 4, Parent = "2C416E30-50B6-4A5A-BF32-9952FBB6B0D3")]
        public ActionResult Export()
        {
            var model = new ExportViewModel
            {
                Schools = _studentService.GetSchoolListItem(),
                Years = _studentService.GetYearListItem()
            };
            return View(model);
        }

        [HttpGet]
        [Menu(Id: "75E4271A-B3DF-45D7-8487-279EAD41CA5C", Name: "Import", OrganizationId: BasicOrganizations.School_ID, Order = 3, Parent = "2C416E30-50B6-4A5A-BF32-9952FBB6B0D3")]
        public IActionResult Import()
        {
            return View();
        }

        [HttpGet]
        [Menu(Id: "3A0F7E7F-2FEF-4AED-A8DB-9EEC2E1603B2", Name: "List", OrganizationId: BasicOrganizations.School_ID, Order = 1,Parent = "2C416E30-50B6-4A5A-BF32-9952FBB6B0D3")]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        [Menu(Id: "68B47253-10F0-4288-ADF3-CFF0ECA82CC7", Name: "Register", OrganizationId: BasicOrganizations.School_ID, Order = 2, Parent = "2C416E30-50B6-4A5A-BF32-9952FBB6B0D3")]
        public IActionResult Register(Guid? schoolId, string studentNumber)
        {
            var viewModel = new StudentRegisterViewModel
            {
                SchoolId = schoolId,
                StudentNumber = studentNumber
            };
            FillUpLists(viewModel);
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }
        private void FillUpLists(StudentRegisterViewModel model)
        {
            int year = model.Year ?? DateTime.Now.Year;
            model.Schools = _studentService.GetSchoolList();
            model.Years = StudentService.GetYearList();
            model.ClassNames = _studentService.GetClassList(model.SchoolId.GetValueOrDefault(), year);
            model.HomeRooms = _studentService.GetHomeRoomList(model.SchoolId.GetValueOrDefault(), year);
            model.Nationalities = _cLService.GetCountryList();
            model.IdentityTypes = _cLService.GetIdentityTypeList();
        }



        #region API
        [HttpPost]
        public JsonResult Edit(StudentRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_studentService.Update(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }

        [HttpPost]
        public IActionResult ExportFile(StudentExportSearchParam filterParams)
        {
            try
            {
                List<StudentExportModel> result = _studentService.SearchStudentForExport(filterParams);
                var file = _studentService.CreateExcelFile(result);
                var fileName = StudentService.GenerateName();
                return File(file, "application/vnd.ms-excel", fileName + ".xls");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Billboard", new { t = "Failure" });
            }
        }

        [HttpPost]
        public JsonResult ExportSearch(StudentExportSearchParam filterParams)
        {
            var students = new StudentExportListResponseModel();
            try
            {
                List<StudentExportModel> result = _studentService.SearchStudentForExport(filterParams);
                students.IsSuccessful = true;
                if (result.Count > 0)
                {
                    students.Students.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                students.IsSuccessful = false;
                students.Description = ex.Message;
            }
            return Json(students);
        }
        [HttpPost]
        public JsonResult GetClasses(Guid schoolid, int year)
        {
            return new JsonResult(_studentService.GetClassesAndHomeRooms(schoolid, year));
        }

        [HttpPost]
        public JsonResult GetClassHomeRooms(int year, Guid schoolId)
        {
            var model = new ClassHomeRooms();
            try
            {
                model.Classes = _studentService.GetClassListItem(schoolId, year).ToTextValue();
                model.HomeRooms = _studentService.GetHomeRoomListItem(schoolId, year).ToTextValue();
                model.IsSuccessful = true;
                return Json(model);
            }
            catch (Exception ex)
            {
                model.IsSuccessful = false;
                model.Message = ex.Message;
            }
            return Json(model);
        }

        [HttpGet]
        public IActionResult GetThumbnail(Guid studentId)
        {
            byte[] photo = _studentService.GetThumbnail(studentId);
            return File(photo, "image/jpeg");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ImportFile(ImportFile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<StudentImportModel> records = _studentImportService.ReadFile(model.File);
                    foreach (var student in records)
                    {
                        ModelState.Clear();
                        if (TryValidateModel(student))
                        {
                            student.IsAcceptable = true;
                        }
                        else
                        {
                            student.IsAcceptable = false;
                            student.Description = ModelState.ConcatToString();
                        }
                    }

                    var count = _studentImportService.ImportAcceptedRecords(records.Where(s => s.IsAcceptable == true).ToList());
                    return Json(StudentImportListResponseModel.Success($"{count} records was imported successfully"));
                }
                else
                {
                    return Json(StudentImportListResponseModel.Failure(ModelState.ConcatToString()));
                }
            }
            catch (Exception ex)
            {
                return Json(StudentImportListResponseModel.Failure(ex.Message));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PreviewFile(ImportFile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<StudentImportModel> records = _studentImportService.ReadFile(model.File);
                    foreach (var student in records)
                    {
                        ModelState.Clear();
                        if (TryValidateModel(student))
                        {
                            student.IsAcceptable = true;
                        }
                        else
                        {
                            student.IsAcceptable = false;
                            student.Description = ModelState.ConcatToString();
                        }
                    }
                    return Json(StudentImportListResponseModel.Success(records));
                }
                else
                {
                    return Json(StudentImportListResponseModel.Failure(ModelState.ConcatToString()));
                }
            }
            catch (Exception ex)
            {
                return Json(StudentImportListResponseModel.Failure(ex.Message));
            }

        }

        [HttpPost]
        public JsonResult Register(StudentRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_studentService.Register(model));
            }
            else
            {
                return Json(ModelState.FullResponse());
            }
        }
        [HttpPost]
        public JsonResult Retrieve()
        {
            List<StudentListModel> model = _studentService.RetrieveStudents();
            return new JsonResult(model);
        }
        [HttpPost]
        public JsonResult StudentList(DtReceive dtReceive)
        {
            var model = _studentService.RetrieveStudentsForList(dtReceive);
            return new JsonResult(model);
        }
        [HttpPost]
        public JsonResult ConfirmStudent(Guid studentId)
        {
            try
            {
                _studentService.ConfirmStudent(studentId);
                return Json(SimpleResponse.Success());
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }
        #endregion

    }
}