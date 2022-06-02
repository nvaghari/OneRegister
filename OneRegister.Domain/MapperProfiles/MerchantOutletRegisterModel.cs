using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Domain.Model.MerchantRegistration;

namespace OneRegister.Domain.MapperProfiles
{
    public class MerchantOutletRegisterModel : DomainMapperProfile
    {
        public MerchantOutletRegisterModel()
        {
            CreateMap<MerchantRegisterModel_Outlet, MerchantOutlet>()
                .ForMember(d => d.MerchantId, op => op.MapFrom(s => s.Mid))
                .ForMember(d => d.Address, op => op.MapFrom(s => s.OAddress))
                .ForMember(d => d.ContactPerson, op => op.MapFrom(s => s.OContactPerson))
                .ForMember(d => d.Fax, op => op.MapFrom(s => s.OFaxNo))
                .ForMember(d => d.Lat, op => op.MapFrom(s => s.OLat))
                .ForMember(d => d.Lng, op => op.MapFrom(s => s.OLng))
                .ForMember(d => d.Name, op => op.MapFrom(s => s.OName))
                .ForMember(d => d.OperatingDaysHours, op => op.MapFrom(s => s.OOperatingDaysHours))
                .ForMember(d => d.ContactEmail, op => op.MapFrom(s => s.OPEmail))
                .ForMember(d => d.ContactMobile, op => op.MapFrom(s => s.OPMobileNo))
                .ForMember(d => d.PostCode, op => op.MapFrom(s => s.OPostCode))
                .ForMember(d => d.ContactTel, op => op.MapFrom(s => s.OPTelNo))
                .ForMember(d => d.Remark, op => op.MapFrom(s => s.ORemarks))
                .ForMember(d => d.RegionState, op => op.MapFrom(s => s.OState))
                .ForMember(d => d.Tel, op => op.MapFrom(s => s.OTelNo))
                .ForMember(d => d.Town, op => op.MapFrom(s => s.OTown))
                .ForMember(d => d.OutletType, op => op.MapFrom(s => s.OType))
                ;
        }
    }
}
