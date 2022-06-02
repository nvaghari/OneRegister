using Microsoft.Extensions.Logging;
using OneRegister.Data.Context.MasterCard;
using OneRegister.Data.Entities.MasterCard;
using OneRegister.Data.Entities.MasterCardGems;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Exceptions;
using OneRegister.Domain.Services.KYCApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.MasterCard
{
    public class AMLService
    {
        private readonly MasterCardRepository _repository;

        public AMLService(MasterCardRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CDDActionIV_ListBankAcctInfoResult> GetCDDActionIV_ListBankAcctInfoResults(GemStatus? status)
        {

            OutputParameter<int?> rowCount = new();
            OutputParameter<int> returnValue = new();
            var result = _repository.Context.Procedures.CDDActionIV_ListBankAcctInfoAsync(null, null, status?.ToString(), null, null, null,1, rowCount,returnValue).Result;

            return result;
        }
        public IEnumerable<CDDActionID_ListRequestsResult> GetCDDActionIV_ListRequests()
        {
            OutputParameter<int?> rowCount = new();
            var result = _repository.Context.Procedures.CDDActionID_ListRequestsAsync(null,rowCount).Result;
            return result;
        }
        public void SetBankAccountStatus(int? cddActionIV, string iVBankBIC, string iVBankAccountNo, string iVBankAccountName, string processorTxnID, string errMessage)
        {
            OutputParameter<int> returnValue = new();
            _ = _repository.Context.Procedures.CDDActionIV_SetBankAcctStatusAsync(cddActionIV, iVBankBIC, iVBankAccountNo, iVBankAccountName, null, "RPP", processorTxnID, "RPP", errMessage, returnValue).Result;

            if (returnValue.Value != 0)
            {
                throw new GemsException(returnValue.Value, nameof(_repository.Context.Procedures.CDDActionIV_SetBankAcctStatusAsync));
            }
        }

        internal void SetIVVeriStatusL1(IVResultModel model, int cDDActionIV,string userKey)
        {
            var returnValue = new OutputParameter<int>();
            var resultStr = JsonSerializer.Serialize(model);
            _ = _repository.Context.Procedures.CDDActionID_EKyc_SetVeriStatusL1Async(
                CDDActionID: cDDActionIV,
                KycFaceRecogStatus: EvaluateFaceRecognitionResult(model.LivenessResult,model.FaceRecognitionResult),
                ErrorMsg: null,
                MatchScore: null,
                ProcessorID: "OneKyc",
                ProcessorTxnID: userKey,
                FaceDmsId: model.ImageUrl1,
                RefDocPage1DmsId: null,
                FullName: null,
                FirstName: model.FirstName,
                MiddleName1: null,
                MiddleName2: null,
                LastName: model.LastName,
                Nationality: GetCountryCodeFromIsoAlpha3(model.Nationality),
                IdType: model.DocumentType,
                IdValue: model.IdNumber,
                BirthDate: GetDateTime(model.BirthDate),
                Gender: GetGender(model.Gender),
                jsonAddr: null,
                DocExpiry: GetDateTime(model.ExpiryDate),
                DocIssuingDate: null,
                DocIssuingPlace: null,
                DocIssuingCountry: GetCountryCodeFromIsoAlpha3(model.IssuingCountry),
                DocIssuingAuthority: null,
                VeriRemarks: null,
                VeriBy: "OneKyc",
                jsonResultsRaw: resultStr,
                returnValue: returnValue).Result;
            if (returnValue.Value != 0)
            {
                throw new GemsException(returnValue.Value, nameof(_repository.Context.Procedures.CDDActionID_EKyc_SetVeriStatusL1Async));
            }
        }

        private static string EvaluateFaceRecognitionResult(string livenessResult, string faceRecognitionResult)
        {
            if (string.IsNullOrEmpty(faceRecognitionResult) || string.IsNullOrEmpty(livenessResult))
            {
                return "U";
            }else
            if (faceRecognitionResult == "UNDEFINED")
            {
                return "I";
            }else
            if ( livenessResult == "SUCCEED" && faceRecognitionResult == "CLEAR")
            {
                return "P";
            }
            else{
                return "U";
            }
        }

        private static string GetGender(string gender)
        {
            return gender switch
            {
                "MALE" => "M",
                "FEMALE" => "F",
                _ => null
            };
        }

        private static DateTime? GetDateTime(string birthDate)
        {
            if (DateTime.TryParse(birthDate, out DateTime result))
            {
                return result;
            }
            else {
                return null;
            };
        }

        private string GetCountryCodeFromIsoAlpha3(string nationality)
        {
            var countryRow = _repository.Context.ClCountry.Where(c => c.IsoAlpha3 == nationality).FirstOrDefault();
            return countryRow?.CountryCode;
        }

        internal void SetIVVeriStatusL1(int cDDActionIV, string errorMessage)
        {
            var returnValue = new OutputParameter<int>();
            _ = _repository.Context.Procedures.CDDActionID_EKyc_SetVeriStatusL1Async(cDDActionIV, null, errorMessage,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,returnValue).Result;
            if (returnValue.Value != 0)
            {
                throw new GemsException(returnValue.Value, nameof(_repository.Context.Procedures.CDDActionID_EKyc_SetVeriStatusL1Async));
            }
        }

        public IEnumerable<CDDActionDV_ListResult> GetCDDActionDV_ListResults(GemStatus? status)
        {
            OutputParameter<int?> rowCount = new();
            var result = _repository.Context.Procedures.CDDActionDV_ListAsync(null, null, status?.ToString(), null, null, null, rowCount).Result;
            return result;
        }
        public void SetDVVeriStatusL1(int? cDDActionDV, string dVStatusL1, string processorTxnID, string jsonResult)
        {
            var returnValue = new OutputParameter<int>();
            var dVStatusAgg = new OutputParameter<string>();
            _ = _repository.Context.Procedures.CDDActionDV_SetVeriStatusL1Async("OneRegister", cDDActionDV, dVStatusL1,"auto.AI",null,null,"OneKyc", processorTxnID,null, jsonResult,null, dVStatusAgg,returnValue).Result;
            if (returnValue.Value != 0)
            {
                throw new GemsException(returnValue.Value, nameof(_repository.Context.Procedures.CDDActionDV_SetVeriStatusL1Async));
            }
        }
        public void SetVeriStatusL1(int cDDActionDV, string errorMessage)
        {
            var returnValue = new OutputParameter<int>();
            var dVStatusAgg = new OutputParameter<string>();
            _ = _repository.Context.Procedures.CDDActionDV_SetVeriStatusL1Async("OneRegister", cDDActionDV, null, "auto.AI", null, null, "OneKyc", null, null, null, errorMessage, dVStatusAgg, returnValue).Result;
            if (returnValue.Value != 0)
            {
                throw new GemsException(returnValue.Value, nameof(_repository.Context.Procedures.CDDActionDV_SetVeriStatusL1Async));
            }
        }

        public List<SSTxn_ListRequestsResult> GetSSTxn_ListRequestResult()
        {
            OutputParameter<int?> rowCount = new();
            OutputParameter<int> returnValue = new();
            return _repository.Context.Procedures.SSTxn_ListRequestsAsync(null, rowCount, returnValue).Result;
        }
        public void SetSSResultV2(int cDDActionSS, string jsonResult)
        {
            OutputParameter<int> returnValue = new();
            _ = _repository.Context.Procedures.SSTxn_SetResultV2Async(cDDActionSS, jsonResult, returnValue).Result;
            if (returnValue.Value != 0)
            {
                throw new GemsException(returnValue.Value, nameof(_repository.Context.Procedures.SSTxn_SetResultV2Async));
            }
        }
    }
}
