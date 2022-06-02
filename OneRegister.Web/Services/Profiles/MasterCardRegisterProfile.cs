using OneRegister.Domain.Services.MasterCard.Model;
using OneRegister.Web.Models.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Services.Profiles
{
    public class MasterCardRegisterProfile : WebMapperProfile
    {
        public MasterCardRegisterProfile()
        {
            CreateMap<RegisterViewModel, MasterCardRegisterModel>();
            CreateMap<MasterCardRegisterModel, RegisterViewModel>();
            CreateMap<AddressViewModel, MasterCardAddressModel>();
            CreateMap<MasterCardAddressModel, AddressViewModel>();
            CreateMap<CheckAccountViewModel, CheckBankAccountModel>();
            CreateMap<CheckBankAccountModel, CheckAccountViewModel>();
        }
    }
}
