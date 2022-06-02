using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Web.Models.MerchantRegistration;

namespace OneRegister.Web.Services.Profiles
{
    public class MerchantRegisterOutletProfile : WebMapperProfile
    {
        public MerchantRegisterOutletProfile()
        {
            CreateMap<MerchantOutletViewModel, MerchantRegisterModel_Outlet>()
                .ForMember(d => d.Id, op => op.MapFrom(s => s.OutletId));
            CreateMap<MerchantRegisterModel_Outlet, MerchantOutletViewModel>()
                .ForMember(d => d.OutletId, op => op.MapFrom(s => s.Id));
        }
    }
}
