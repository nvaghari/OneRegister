using OneRegister.Data.SuperEntities;

namespace OneRegister.Domain.MapperProfiles
{
    public class OrganizationFileProfile : DomainMapperProfile
    {
        public OrganizationFileProfile()
        {
            CreateMap<OrganizationFile,OrganizationFile>()
                .ForMember(m => m.Id, op => op.Ignore())
                .ForMember(m => m.CreatedAt, op => op.Ignore());
        }
    }
}
