using OneRegister.Domain.Model.StudentRegistration;
using OneRegister.Web.Models.StudentRegistration;

namespace OneRegister.Web.Services.Profiles
{
    public class StudentRegisterProfile : WebMapperProfile
    {
        public StudentRegisterProfile()
        {
            CreateMap<StudentRegisterViewModel, StudentRegisterModel>();
            CreateMap<StudentRegisterModel, StudentRegisterViewModel>();
        }
    }
}
