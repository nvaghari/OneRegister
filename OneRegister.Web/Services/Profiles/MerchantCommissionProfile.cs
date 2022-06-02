using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Web.Models.MerchantRegistration;

namespace OneRegister.Web.Services.Profiles
{
    public class MerchantCommissionProfile : WebMapperProfile
    {
        public MerchantCommissionProfile()
        {
            CreateMap<MerchantCommissionViewModel, MerchantCommissionModel>();
            CreateMap<MerchantCommissionModel, MerchantCommissionViewModel>();
        }
    }
}
