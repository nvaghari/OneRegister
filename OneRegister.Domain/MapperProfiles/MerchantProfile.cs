using OneRegister.Data.Entities.MerchantRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class MerchantProfile : DomainMapperProfile
    {
        public MerchantProfile()
        {
            CreateMap<Merchant, Merchant>()
                .ForMember(m => m.Id, op => op.Ignore())
                .ForMember(m=> m.CreatedAt, op => op.Ignore());
        }
    }
}
