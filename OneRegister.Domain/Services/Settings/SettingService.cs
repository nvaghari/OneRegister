using OneRegister.Data.Contract;
using OneRegister.Data.Entities.Application;
using OneRegister.Domain.Model.Settings;
using System;
using System.Linq;
using System.Text.Json;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.Settings
{
    public class SettingService
    {
        private readonly IOrganizedRepository<Setting> _settingRepository;

        public SettingService(IOrganizedRepository<Setting> settingRepository)
        {
            _settingRepository = settingRepository;
        }
        public Guid Save(MailSettingModel model)
        {
            var emailSetting = GetsettingForOrganization(BasicOrganizations.Merchant,SettingType.Email);
            if(emailSetting == null)
            {
                var setting = new Setting
                {
                    Name = "Merchant Email",
                    OrganizationId = BasicOrganizations.Merchant,
                    SettingType = SettingType.Email,
                    Value = JsonSerializer.Serialize(model)
                };
                _settingRepository.Add(setting);
                return setting.Id;
            }
            else
            {
                emailSetting.Value = JsonSerializer.Serialize(model);
                _settingRepository.Update(emailSetting);
                return emailSetting.Id;
            }
        }

        public MailSettingModel GetEmail()
        {
            var emailSetting =  GetsettingForOrganization(BasicOrganizations.Merchant, SettingType.Email);
            if (emailSetting == null || string.IsNullOrEmpty(emailSetting.Value)) return null;

            return JsonSerializer.Deserialize<MailSettingModel>(emailSetting.Value);
        }
        private Setting GetsettingForOrganization(Guid orgId, SettingType settingType)
        {
            return _settingRepository.Entities.SingleOrDefault(s => s.SettingType == SettingType.Email && s.OrganizationId == orgId);
        }
    }
}
