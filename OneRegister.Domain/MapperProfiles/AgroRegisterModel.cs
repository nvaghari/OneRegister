using OneRegister.Data.Entities.AgroRegistration;
using OneRegister.Domain.Model.AgropreneurRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class AgroRegisterModel : DomainMapperProfile
    {
        public AgroRegisterModel()
        {
            CreateMap<AGPRegisterModel, Agropreneur>();
            CreateMap<Agropreneur, AGPRegisterModel>();
        }
    }
}
