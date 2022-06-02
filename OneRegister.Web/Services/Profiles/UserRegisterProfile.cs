using OneRegister.Domain.Model.Account;
using OneRegister.Web.Models.Account;

namespace OneRegister.Web.Services.Profiles
{
    public class UserRegisterProfile : WebMapperProfile
    {
        public UserRegisterProfile()
        {
            CreateMap<UserRegisterViewModel, UserRegisterModel>();
        }
    }
}
