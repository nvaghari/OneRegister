using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Domain.Model.MerchantRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class MerchantInfoRegisterModel : DomainMapperProfile
    {
        public MerchantInfoRegisterModel()
        {
            CreateMap<MerchantRegisterModel_Info, MerchantInfo>()
                .ForMember(d => d.Name, op => op.MapFrom(s => s.RegisteredBusiness))
                .ForMember(d => d.MerchantId, op => op.MapFrom(s => s.Mid));
        }
    }
}
