using OneRegister.Domain.Model.Account;
using OneRegister.Web.Models.Account;

namespace OneRegister.Web.Services.Profiles
{
    public class LoginProfile : WebMapperProfile
    {
        public LoginProfile()
        {
            CreateMap<LoginViewModel, LoginModel>();
        }
    }
}
