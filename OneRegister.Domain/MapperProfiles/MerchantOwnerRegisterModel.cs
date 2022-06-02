using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Domain.Model.MerchantRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class MerchantOwnerRegisterModel : DomainMapperProfile
    {
        public MerchantOwnerRegisterModel()
        {
            CreateMap<MerchantRegisterModel_Owner, MerchantOwner>()
                .ForMember(d => d.MerchantId, op => op.MapFrom(s => s.Mid))
                .ForMember(d => d.Designation, op => op.MapFrom(s => s.OwnerDesignation))
                .ForMember(d => d.IdentityNumber, op => op.MapFrom(s => s.OwnerIdentityNo))
                .ForMember(d => d.Mobile, op => op.MapFrom(s => s.OwnerMobile))
                .ForMember(d => d.Name, op => op.MapFrom(s => s.OwnerName))
                ;
        }
    }
}
