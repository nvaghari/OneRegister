using OneRegister.Domain.Services.MasterCard.Model;
using OneRegister.Domain.Services.RPPApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.MapperProfiles
{
    public class RppServiceProfile : DomainMapperProfile
    {
        public RppServiceProfile()
        {
            CreateMap<CheckBankAccountModel, BankAccountSendModel>();
        }
    }
}
