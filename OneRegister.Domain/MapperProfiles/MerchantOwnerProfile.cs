using OneRegister.Data.Entities.MerchantRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class MerchantOwnerProfile : DomainMapperProfile
    {
        public MerchantOwnerProfile()
        {
            CreateMap<MerchantOwner,MerchantOwner>()
                .ForMember(m => m.Id, op => op.Ignore())
                .ForMember(m => m.CreatedAt, op => op.Ignore());
        }
    }
}
