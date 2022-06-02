using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Web.Models.MerchantRegistration;

namespace OneRegister.Web.Services.Profiles
{
    public class MerchantRegisterProfile : WebMapperProfile
    {
        public MerchantRegisterProfile()
        {
            CreateMap<MerchantRegisterModel, MerchantRegisterViewModel>();
        }
    }
}
