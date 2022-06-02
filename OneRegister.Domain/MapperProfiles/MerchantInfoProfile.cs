using OneRegister.Data.Entities.MerchantRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class MerchantInfoProfile : DomainMapperProfile
    {
        public MerchantInfoProfile()
        {
            CreateMap<MerchantInfo,MerchantInfo>()
                .ForMember(m=> m.Id, op=> op.Ignore())
                .ForMember(m => m.CreatedAt, op => op.Ignore());
        }
    }
}
