using OneRegister.Data.Entities.MerchantRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class MerchantOutletProfile : DomainMapperProfile
    {
        public MerchantOutletProfile()
        {
            CreateMap<MerchantOutlet, MerchantOutlet>()
                .ForMember(m => m.Id, op => op.Ignore())
                .ForMember(m => m.CreatedAt, op => op.Ignore());
        }
    }
}
