using Microsoft.AspNetCore.Mvc;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Domain.Model.ExportPhoto;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.StudentRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class ExportController : ControllerBase
    {
        private readonly AuthorizationService _authorizationService;
        private readonly StudentService _studentService;
        private readonly UserService _userService;
        private readonly SchoolService _schoolService;

        public ExportController(
            AuthorizationService authorizationService,
            StudentService studentService,
            UserService userService,
            SchoolService schoolService)
        {
            _authorizationService = authorizationService;
            _studentService = studentService;
            _userService = userService;
            _schoolService = schoolService;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken(ExportLoginModel model)
        {
            if (model is null)
            {
                return BadRequest();
            }
            var loginResult = await _authorizationService.ExportLoginAsync(model.UserName,model.Password);
            if (loginResult.IsSuccessful)
            {
                string token = _authorizationService.GetToken(model.UserName);
                return Ok(new { token });
            }

            return Unauthorized(loginResult);
        }

        [HttpPost]
        public IActionResult GetSchoolYears(ExportTokenModel model)
        {
            if (model is null)
            {
                return BadRequest();
            }
            string userName; 
            try
            {
                userName = _authorizationService.ValidateToken(model.Token);

            }
            catch (Exception ex)
            {

                return Unauthorized(SimpleResponse.FailBecause(ex.Message));
            }

            try
            {
                var outModel = new ExportSchoolYears
                {
                    Schools = _studentService.GetSchoolList(),
                    Years = StudentService.GetYearList()
                };
                return new JsonResult(outModel);
            }
            catch (Exception ex)
            {

                return StatusCode(500, SimpleResponse.FailBecause(ex.Message));
            }
        }

        [HttpPost]
        public IActionResult GetClassHomerRooms(ExportClassesModel model)
        {
            if(model is null)
            {
                return BadRequest();
            }
            string userName;
            try
            {
                userName = _authorizationService.ValidateToken(model.Token);

            }
            catch (Exception ex)
            {

                return Unauthorized(SimpleResponse.FailBecause(ex.Message));
            }
            try
            {
                var outModel = _studentService.GetClassesAndHomeRooms(model.SchoolId, model.YearId);
                return new JsonResult(outModel);
            }
            catch (Exception ex)
            {

                return StatusCode(500, SimpleResponse.FailBecause(ex.Message));
            }

        }

        [HttpPost]
        public IActionResult GetStudentList(ExportStudentListParams model)
        {
            if (model is null)
            {
                return BadRequest();
            }
            string userName;
            try
            {
                userName = _authorizationService.ValidateToken(model.Token);

            }
            catch (Exception ex)
            {

                return Unauthorized(SimpleResponse.FailBecause(ex.Message));
            }
            try
            {
                var authorizedOrgs = _schoolService.GetAll(true);
                if (model.SchoolId == Guid.Empty || !authorizedOrgs.Select(o => o.Id).Contains(model.SchoolId))
                {
                    return new JsonResult(new List<ExportStudentModel>());
                }

                List<ExportStudentModel> outModel = _studentService.GetStudentsForExportPhoto(model);
                return new JsonResult(outModel);
            }
            catch (Exception ex)
            {

                return StatusCode(500, SimpleResponse.FailBecause(ex.Message));
            }

        }


        [HttpPost]
        public IActionResult GetStudent(ExportStudentParams model)
        {
            if (model is null)
            {
                return BadRequest();
            }
            string userName;
            try
            {
                userName = _authorizationService.ValidateToken(model.Token);

            }
            catch (Exception ex)
            {

                return Unauthorized(SimpleResponse.FailBecause(ex.Message));
            }
            try
            {
                var authorizedOrgs = _schoolService.GetAll(true);

                ExportStudentModel outModel = _studentService.GetStudentForExportPhoto(model.IcNumber, authorizedOrgs.Select(o => o.Id).ToList());
                return new JsonResult(outModel);
            }
            catch (Exception ex)
            {

                return StatusCode(500, SimpleResponse.FailBecause(ex.Message));
            }
        }
    }
}
