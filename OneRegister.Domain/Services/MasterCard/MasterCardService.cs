using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Data.Context.MasterCard;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Services.Dms;
using OneRegister.Domain.Services.MasterCard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OneRegister.Domain.Services.MasterCard
{
    public class MasterCardService
    {
        private readonly MasterCardRepository _masterCardRepository;
        private readonly ILogger<MasterCardService> _logger;
        private readonly DmsService _dmsService;
        private readonly MasterCardTasksRepository _masterCardTasksRepository;
        private readonly IAuthorizedRepository<InquiryTask> _inquiryTaskRepository;

        public MasterCardService(
            MasterCardRepository masterCardRepository,
            ILogger<MasterCardService> logger,
            DmsService dmsService,
            MasterCardTasksRepository masterCardTasksRepository,
            IAuthorizedRepository<InquiryTask> inquiryTaskRepository)
        {
            _masterCardRepository = masterCardRepository;
            _logger = logger;
            _dmsService = dmsService;
            _masterCardTasksRepository = masterCardTasksRepository;
            _inquiryTaskRepository = inquiryTaskRepository;
        }
        public Dictionary<string, string> GetChannelList()
        {
            return _masterCardRepository.Context.ClChannel.AsNoTracking().OrderBy(e => e.ShortName).ToDictionary(e => e.Code, e => e.LongName);
        }

        public Dictionary<string, string> GetAddressTypeList()
        {
            return _masterCardRepository.Context.ClAddressType.AsNoTracking().OrderBy(e => e.ShortName).ToDictionary(e => e.Code, e => e.ShortName);
        }

        public Dictionary<string, string> GetCountryStateList()
        {
            return _masterCardRepository.Context.ClCountryState.AsNoTracking().Where(e => e.CountryCode == "MY").OrderBy(e => e.StateName).ToDictionary(e => e.StateIsocode, e => e.StateName);
        }

        public Dictionary<string, string> GetIcSourceList()
        {
            return _masterCardRepository.Context.ClCddactionIcsource.AsNoTracking().OrderBy(e => e.LongName).ToDictionary(e => e.Code, e => e.LongName);
        }

        public FullResponse Register(MasterCardRegisterModel model)
        {
            var regCaseID = new OutputParameter<string>();
            var entityCIF = new OutputParameter<string>();
            var formStatus = new OutputParameter<string>();
            var returnValue = new OutputParameter<int>();

            SerializeAddress(model);

            model.FaceDmsId = AddFile(model.FaceDmsFile);
            model.DocPage1DmsId = AddFile(model.DocPage1DmsFile);
            model.DocPage2DmsId = AddFile(model.DocPage2DmsFile);

            var result = _masterCardRepository.Context.Procedures.RegCust_Init3Async(
                ICSource: model.ICSource,
                Channel: model.Channel,
                ListPackages: model.ListPackages.ArrayToString(),
                CustAuthMode: model.CustAuthMode,
                OrgID: model.OrgID.ToString(),
                Title: model.Title,
                FullName: model.FullName,
                FirstName: model.FirstName,
                MiddleName1: model.MiddleName1,
                MiddleName2: model.MiddleName2,
                LastName: model.LastName,
                Suffix: null,
                Patronym: null,
                Nationality: model.Nationality,
                NID: model.NID,
                PassportNo: model.PassportNo,
                PassportExpiry: model.PassportExpiry,
                PassportIssuingDate: model.PassportIssuingDate,
                PassportIssuingPlace: model.PassportIssuingPlace,
                PassportIssuingAuthority: model.PassportIssuingAuthority,
                BirthDate: model.BirthDate,
                Gender: model.Gender.HasValue ? model.Gender.Value.ToCharacter() : null,
                MobileNo: model.MobileNo,
                Email: model.Email,
                jsonAddrHome: model.HomeAddressJson,
                jsonAddrPost: model.PostAddressJson,
                BankBIC: model.BankBIC,
                BankAcctNo: model.BankAcctNo,
                BankAcctName: model.BankAcctName,
                OccType: model.OccType,
                OccIndustry: model.OccIndustry,
                OccPosition: model.OccPosition,
                OccCompany: model.OccCompany,
                MotherName: model.MotherName,
                FaceDmsId: model.FaceDmsId,
                DocPage1DmsId: model.DocPage1DmsId,
                DocPage2DmsId: model.DocPage2DmsId,
                xUser: "test",
                RegCaseID: regCaseID,
                EntityCIF: entityCIF,
                FormStatus: formStatus,
                emCardDesign: "EM_CARD_1",
                mcCardDesign: "MIPAY_CARD_A1",
                fpxBIC: "Test",
                fpxAcctName: "Test",
                returnValue: returnValue
                ).Result;

            if (returnValue.Value == 0)
            {
                return FullResponse.Success;
            }
            else
            {
                _logger.LogError($"RegCust_InitAsync -> returned with error. Error Ticket {returnValue.Value}");

                string userMessage = GetError(returnValue.Value);
                return new FullResponse { IsSuccessful = false, Message = userMessage };
            }
        }

        public Guid ChangeInquiryTaskState(Guid taskId, StateOfEntity newState)
        {
            var task = _inquiryTaskRepository.GetById(taskId);
            if (task == null) throw new ApplicationException("task doesn't exist");
            task.State = newState;
            task.Result = null;
            task.ErrorSource = null;
            task.ErrorCode = null;
            _inquiryTaskRepository.Update(task);
            _logger.LogInformation($"task id {task.Id} re-initiated by user {_inquiryTaskRepository.CurrentUserId}");
            return task.Id;
        }

        public Dictionary<string, string> GetListPackagesList()
        {
            return _masterCardRepository.Context.ClServicePackage.AsNoTracking().OrderBy(e => e.ServicePackage).ToDictionary(e => e.ServicePackage, e => e.ServicePackage);
        }

        public DtReturn<MCTaskListGridModel> RetrieveTasksForList(DtReceive dtReceive)
        {
            IEnumerable<InquiryTask> queryResult = _masterCardTasksRepository.RetrieveTasksForList(dtReceive.Search.Value, dtReceive.Start, dtReceive.Length, out int total, out int filteredTotal);
            var dataResult = queryResult.Select(s => new MCTaskListGridModel
            {
                Id = s.Id.ToString(),
                Name = s.Name,
                RefId = s.RefId,
                RefId2 = s.RefId2,
                CreatedAt = s.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                ModifiedAt = s.ModifiedAt.ToString("yyyy-MM-dd HH:mm"),
                ErrorSource = s.ErrorSource,
                InquiryName = s.InquiryName,
                Result = s.Result,
                Source = s.Source,
                State = s.State.ToString(),
                ErrorCode = s.ErrorCode
            }).ToList();

            return new DtReturn<MCTaskListGridModel>
            {
                Data = dataResult,
                RecordsTotal = total,
                RecordsFiltered = filteredTotal,
                Draw = dtReceive.Draw
            };
        }

        private string AddFile(IFormFile faceDmsFile)
        {
            if (faceDmsFile == null) return string.Empty;
            var result = _dmsService.InsertFile(faceDmsFile);
            return result.url.ToString();
        }

        private static void SerializeAddress(MasterCardRegisterModel model)
        {
            JsonSerializerOptions addressSerializeOption = new()
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            model.HomeAddressJson = JsonSerializer.Serialize(model.HomeAddress, addressSerializeOption);
            model.PostAddressJson = model.IsAddressSame ? model.HomeAddressJson : JsonSerializer.Serialize(model.PostAddress);
        }

        public DtReturn<CustomerListModel> RetrieveForList(DtReceive dtReceive)
        {
            OutputParameter<int?> rowCount = new();
            OutputParameter<int> returnValue = new();
            var fetchedCustomer = _masterCardRepository.Context.Procedures.WFForms_ListAsync(null, null, null, null, null, null, null, null, null, null, null, null,null, rowCount, returnValue).Result;



            var filteredData = fetchedCustomer.Select(r => new CustomerListModel {
                FormNo = r.FormNo,
                FormType = r.FormType,
                FormStatus = r.FormStatus,
                Channel = r.Channel,
                FullName = r.FullName,
                MobileNo = r.MobileNo,
                IdNo = r.IdNo,
                EntityCIF = r.EntityCIF,
                ListServicePackages = r.ListServicePackages
            })
            .Where(c => string.IsNullOrEmpty(dtReceive.Search.Value) || c.FormNo.Contains(dtReceive.Search.Value));
            var result = new DtReturn<CustomerListModel>
            {
                Draw = dtReceive.Draw,
                RecordsTotal = rowCount.Value.Value,
                RecordsFiltered = filteredData.Count(),
                Data = filteredData.Skip(dtReceive.Start).Take(dtReceive.Length).ToList()
            };

            return result;
        }

        private string GetError(int ticketID)
        {
            var returnValue = new OutputParameter<int>();
            var error = _masterCardRepository.Context.Procedures.ErrorLog_GetAsync(ticketID, returnValue).Result;
            if (returnValue.Value == 0)
            {
                return error.FirstOrDefault().UserMsg;
            }

            _logger.LogError($"ErrorLog_GetAsync -> returned with error. Error Ticket {returnValue.Value}");
            return "Error on getting ErrorTicket";
        }

        public Dictionary<string, string> GetNationalityList()
        {
            return _masterCardRepository.Context.ClCountry.AsNoTracking().OrderBy(e => e.CountryName).ToDictionary(e => e.CountryCode, e => e.CountryName);
        }

        public Dictionary<string, string> GetBankBICList()
        {
            return _masterCardRepository.Context.ClBank.AsNoTracking().OrderBy(e => e.BankName).ToDictionary(e => e.BankCode, e => e.BankName);
        }

        public Dictionary<string, string> GetOccIndustryList()
        {
            return _masterCardRepository.Context.ClBusinessNature.AsNoTracking().OrderBy(e => e.ShortName).ToDictionary(e => e.Code, e => e.ShortName);
        }

        public Dictionary<string, string> GetOccTypeList()
        {
            return _masterCardRepository.Context.ClOccupation.AsNoTracking().OrderBy(e => e.ShortName).ToDictionary(e => e.Code, e => e.ShortName);
        }
    }
}
