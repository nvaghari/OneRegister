using OneRegister.Domain.Model.Account;
using OneRegister.Web.Models.MerchantRegistration;

namespace OneRegister.Web.Services.Profiles
{
    public class MerchantUserRegisterProfile : WebMapperProfile
    {
        public MerchantUserRegisterProfile()
        {
            CreateMap<MerchantUserRegisterViewModel, UserRegisterModel>();
        }
    }
}
