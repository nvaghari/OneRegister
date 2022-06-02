using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model;
using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Domain.Services.StudentRegistration;
using OneRegister.Security.Attributes;
using OneRegister.Web.Services.Extensions;
using System;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Web.Controllers
{
    [Authorize]
    [Menu(Id: "B2AD6B26-D194-40D6-8170-E3E11CA0824A", Name: "School & Classes", OrganizationId: BasicOrganizations.School_ID, Order = 3, FasIcon = "fa-chalkboard-teacher")]
    public class SchoolController : Controller
    {
        private readonly ClassRoomService _classRoomService;
        private readonly HomeRoomService _homeRoomService;
        private readonly SchoolService _schoolService;
        private readonly StudentService _studentService;
        public SchoolController(SchoolService schoolService,
            StudentService studentService,
            ClassRoomService classRoomService,
            HomeRoomService homeRoomService)
        {
            _schoolService = schoolService;
            _studentService = studentService;
            _classRoomService = classRoomService;
            _homeRoomService = homeRoomService;
        }
        [HttpPost]
        public JsonResult AddClass(Guid schoolId, int year, string name, string label)
        {
            try
            {
                PersistResult result = _classRoomService.Add(schoolId, year, name, label);
                if (result.IsSuccessful)
                {
                    return Json(SimpleResponse.Success());
                }

                return Json(SimpleResponse.FailBecause(result.Errors.ToErrorMessage()));
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AddHomeRoom(Guid schoolId, int year, string name)
        {
            try
            {
                PersistResult result = _homeRoomService.Add(schoolId, year, name);
                if (result.IsSuccessful)
                {
                    return Json(SimpleResponse.Success());
                }

                return Json(SimpleResponse.FailBecause(result.Errors.ToErrorMessage()));
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AddSchool(string name)
        {
            try
            {
                PersistResult result = _schoolService.AddByName(name.Sanitize());
                if (result.IsSuccessful)
                {
                    return Json(SimpleResponse.Success());
                }
                return Json(SimpleResponse.FailBecause(result.Errors.ToErrorMessage()));
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
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

        [HttpPost]
        public JsonResult GetSchoolYears()
        {
            var model = new SchoolYears();
            try
            {
                model.Schools = _studentService.GetSchoolListItem().ToTextValue();
                model.Years = _studentService.GetYearListItem().ToTextValue();
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

        [Menu(Id: "56A166A4-05B5-4FFD-810C-C53145435A53", Name: "Manage", OrganizationId: BasicOrganizations.School_ID, Order = 1, Parent = "B2AD6B26-D194-40D6-8170-E3E11CA0824A")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult UpdateClass(Guid classId, string name, string label)
        {
            try
            {
                PersistResult result = _classRoomService.Update(classId, name, label);
                if (result.IsSuccessful)
                {
                    return Json(SimpleResponse.Success());
                }
                return Json(SimpleResponse.FailBecause(result.Errors.ToErrorMessage()));
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateHomeRoom(Guid homeId, string name)
        {
            try
            {
                PersistResult result = _homeRoomService.Update(homeId, name);
                if (result.IsSuccessful)
                {
                    return Json(SimpleResponse.Success());
                }
                return Json(SimpleResponse.FailBecause(result.Errors.ToErrorMessage()));
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateSchool(Guid schoolId, string name)
        {
            try
            {
                PersistResult result = _schoolService.Update(schoolId, name);
                if (result.IsSuccessful)
                {
                    return Json(SimpleResponse.Success());
                }
                return Json(SimpleResponse.FailBecause(result.Errors.ToErrorMessage()));
            }
            catch (Exception ex)
            {
                return Json(SimpleResponse.FailBecause(ex.Message));
            }
        }
    }
}