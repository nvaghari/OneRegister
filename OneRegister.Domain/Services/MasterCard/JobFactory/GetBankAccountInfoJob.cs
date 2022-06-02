using Microsoft.Extensions.Logging;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard.JobFactory
{
    public class GetBankAccountInfoJob : IMasterCardJob
    {
        private readonly AMLService _aMLService;
        private readonly MasterCardInquiryRepository _inquiryRepository;
        private readonly ILogger<GetBankAccountInfoJob> _logger;

        public GetBankAccountInfoJob(
            AMLService aMLService,
            MasterCardInquiryRepository inquiryRepository,
            ILogger<GetBankAccountInfoJob> logger)
        {
            _aMLService = aMLService;
            _inquiryRepository = inquiryRepository;
            _logger = logger;
        }

        public AMLService AMLService => _aMLService;

        public void Execute()
        {
            _logger.LogDebug("[RPP] "+"GetCDDActionIV_ListBankAcctInfoResults...");
            var bankAccounts = _aMLService.GetCDDActionIV_ListBankAcctInfoResults(GemStatus.I);
            if (!bankAccounts.Any())
            {
                _logger.LogDebug("[RPP] " + bankAccounts.Count() + " Bank Account(s) was fetched");
                return;
            }
            _logger.LogInformation("[RPP] " + bankAccounts.Count() +" Bank Account(s) was fetched");

            IEnumerable<InquiryTask> tasks = GetTasks(bankAccounts);

            _inquiryRepository.AddInquiries(tasks);
        }

        private static IEnumerable<InquiryTask> GetTasks(IEnumerable<CDDActionIV_ListBankAcctInfoResult> bankAccounts)
        {
            return bankAccounts.Select(b => new InquiryTask { 
                InquiryType = InquiryType.RPP,
                InquiryName = InquiryType.RPP.ToString(),
                Source = nameof(CDDActionIV_ListBankAcctInfoResult),
                RefId = b.CDDActionIV.ToString(),
                Name = b.ICBankAcctName,
                JsonValue = JsonSerializer.Serialize(b)
            });
        }
    }
}
