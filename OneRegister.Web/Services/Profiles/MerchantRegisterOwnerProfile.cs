using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Web.Models.MerchantRegistration;

namespace OneRegister.Web.Services.Profiles
{
    public class MerchantRegisterOwnerProfile : WebMapperProfile
    {
        public MerchantRegisterOwnerProfile()
        {
            CreateMap<MerchantRegisterModel_Owner, MerchantOwnerViewModel>()
                .ForMember(d => d.OwnerId, op => op.MapFrom(s => s.Id));
            CreateMap<MerchantOwnerViewModel, MerchantRegisterModel_Owner>()
                .ForMember(d => d.Id, op => op.MapFrom(s => s.OwnerId));
        }
    }
}
