using OneRegister.Domain.Model.Notification;
using OneRegister.Domain.Model.Settings;
using OneRegister.Web.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Services.Profiles
{
    public class MailSettingProfile : WebMapperProfile
    {
        public MailSettingProfile()
        {
            CreateMap<MailSettingModel, MailSettingViewModel>();
            CreateMap<MailSettingViewModel, MailSettingModel>();
            CreateMap<MailSettingViewModel, SendEmailModel>()
                .ForMember(d => d.From, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.To, o => o.MapFrom(s => s.EmailTo))
                .ForMember(d => d.Body, o => o.MapFrom(s => s.TestText));
        }
    }
}
