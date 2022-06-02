using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Data.Contract;
using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Data.Entities.Notification;
using OneRegister.Data.Identication;
using OneRegister.Data.Model.MerchantRegistration;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.Account;
using OneRegister.Domain.Model.Enum.Merchant;
using OneRegister.Domain.Model.MerchantRegistration;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.Dms;
using OneRegister.Domain.Services.NotificationFactory;
using OneRegister.Domain.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.MerchantRegistration
{
    public class MerchantService
    {
        public readonly OrganizationService _organizationService;
        private readonly AuthorizationService _authorizationService;
        private readonly DmsService _dmsService;
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository<Merchant> _merchantRepository;
        private readonly IAuthorizedRepository<MerchantCommission> _merchantCommisionRepo;
        private readonly IAuthorizedRepository<MerchantInfo> _merchantInfoRepo;
        private readonly IOrganizedRepository<OrganizationFile> _merchantFileRepo;
        private readonly IOrganizedRepository<MerchantOutlet> _merchantOutletRepo;
        private readonly IOrganizedRepository<MerchantOwner> _merchantOwnerRepo;
        private readonly NotificationJobService _notificationService;
        private readonly UserService _userService;

        public FullResponse GetBusinessName(string businessNo)
        {
            var merchantInfo = _merchantInfoRepo.Entities.FirstOrDefault(m => m.BusinessNo == businessNo);
            if (merchantInfo == null) return FullResponse.FailBecause("not found");

            return FullResponse.SuccessWithId(merchantInfo.Name);
        }

        public MerchantService(
            IOrganizationRepository<Merchant> merchantRepository,
            IAuthorizedRepository<MerchantCommission> merchantCommisionRepo,
            IAuthorizedRepository<MerchantInfo> merchantInfoRepo,
            IOrganizedRepository<OrganizationFile> merchantFileRepo,
            IOrganizedRepository<MerchantOutlet> merchantOutletRepo,
            IOrganizedRepository<MerchantOwner> merchantOwnerRepo,
            OrganizationService organizationService,
            DmsService dmsService,
            IMapper mapper,
            AuthorizationService authorizationService,
            UserService userService,
            NotificationJobService notificationService)
        {
            _merchantRepository = merchantRepository;
            _merchantCommisionRepo = merchantCommisionRepo;
            _merchantInfoRepo = merchantInfoRepo;
            _merchantFileRepo = merchantFileRepo;
            _merchantOutletRepo = merchantOutletRepo;
            _merchantOwnerRepo = merchantOwnerRepo;
            _organizationService = organizationService;
            _dmsService = dmsService;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public FullResponse Accept(Guid mid)
        {
            try
            {
                var merchantInfo = GetMerchantInfoByMid(mid);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant Id doesn't exist.");
                }
                //HeadRisk Accepting
                if (merchantInfo.MerchantStatus == MerchantStatus.Risk)
                {
                    AcceptMerchant(merchantInfo);
                    _notificationService.Register(ActionType.RiskAccepted, mid, nameof(Merchant));
                    return FullResponse.Success;
                }
                else
                {
                    throw new ApplicationException("Merchant Status is not correct");
                }
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public MerchantCommissionModel GetCommissionByMerchantId(Guid mid)
        {
            var merchant = _merchantRepository.GetById(mid, true, m => m.MerchantCommission);
            if (merchant == null)
            {
                return null;
            }
            if (merchant.MerchantCommission == null)
            {
                //some defaults from business
                var currency = "MYR";
                return new MerchantCommissionModel()
                {
                    MerchantName = merchant.Name,
                    Mid = merchant.Id,
                    EDC_TerminalDamage = 1500,
                    EDC_PaperRollDamage = 50,
                    CreditCard_Frequency = 3,
                    CreditCard_Currency = currency,
                    MyDebit_Frequency = 1,
                    MyDebit_Currency = currency,
                    FPX_Frequency = 1,
                    FPX_Currency = currency,
                    EWallets_Frequency = 3,
                    EWallets_Currency = currency,
                    DuitNowQR_Frequency = 1,
                    DuitNowQR_Currency = currency
                };
            }
            else
            {
                var commision = JsonSerializer.Deserialize<MerchantCommissionModel>(merchant.MerchantCommission.JsonValue);
                commision.Remark = merchant.MerchantCommission.Remark;
                commision.MerchantName = merchant.Name;
                commision.Mid = merchant.Id;

                return commision;
            }
        }

        public FullResponse UpdateCommission(MerchantCommissionModel domainModel)
        {
            var merchant = _merchantRepository
                .GetById(domainModel.Mid, true, m => m.MerchantCommission, m => m.MerchantInfo);
            if (merchant == null)
            {
                throw new ApplicationException("Merchant doesn't exist or your privilege is insufficient");
            }
            CheckMerchantStatus(GetMerchantInfoByMid(domainModel.Mid).MerchantStatus);
            if (merchant.MerchantCommission == null)
            {
                var commission = new MerchantCommission
                {
                    MerchantId = domainModel.Mid,
                    JsonValue = JsonSerializer.Serialize(domainModel),
                    Remark = domainModel.Remark,
                    Name = merchant.Name
                };
                _merchantCommisionRepo.Add(commission);
                UpdateMerchantStateForCommission(domainModel.Mid);
                _notificationService.Register(ActionType.CommissionProvided, commission.MerchantId, nameof(Merchant));
            }
            else
            {
                var commission = _merchantCommisionRepo.GetById(merchant.MerchantCommission.Id);
                commission.JsonValue = JsonSerializer.Serialize(domainModel);
                commission.Remark = domainModel.Remark;
                commission.Name = merchant.Name;
                _merchantCommisionRepo.Update(commission);
                UpdateMerchantStateForCommission(domainModel.Mid);
            }
            return FullResponse.SuccessWithId(merchant.MerchantInfo.FormNumber);
        }

        public FullResponse AddBankAccount(MerchantRegisterModel_Bank model)
        {
            try
            {
                var merchantInfo = GetMerchantInfoByMid(model.Mid ?? Guid.Empty);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant hasn't registered before");
                }

                CheckMerchantStatus(merchantInfo.MerchantStatus);

                merchantInfo.MerchantState = MerchantRegisterState.BankAccount;
                merchantInfo.BankName = model.BankName;
                merchantInfo.BankAccountName = model.AccountName;
                merchantInfo.BankAccountNo = model.AccountNo;
                merchantInfo.BankAddress = model.BankAddress;
                merchantInfo.BankPromoAgreeable = model.BankPromoAgree == BankPromotionalPremisesType.Yes;
                merchantInfo.PassedStates = GeneratePassedStatesString(merchantInfo.PassedStates, MerchantRegisterState.BankAccount);

                _merchantInfoRepo.Update(merchantInfo);
                return new FullResponse
                {
                    IsSuccessful = true,
                    Id = merchantInfo.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse AddChannel(MerchantRegisterModel_Channel model)
        {
            try
            {
                var merchantInfo = GetMerchantInfoByMid(model.Mid ?? Guid.Empty);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant hasn't registered before");
                }

                CheckMerchantStatus(merchantInfo.MerchantStatus);

                merchantInfo.MerchantState = MerchantRegisterState.Channel;
                merchantInfo.ChannelAddress = model.ChannelAddress;
                merchantInfo.ChannelUrl = model.ChannelUrl;
                merchantInfo.ChannelEmail = model.ChannelEmail;
                merchantInfo.PassedStates = GeneratePassedStatesString(merchantInfo.PassedStates, MerchantRegisterState.Channel);

                _merchantInfoRepo.Update(merchantInfo);
                return new FullResponse
                {
                    IsSuccessful = true,
                    Id = merchantInfo.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse AddMerchantFile(MerchantUploadFileModel model)
        {
            try
            {
                var merchant = _merchantRepository.GetById(model.Mid.Value, true, m => m.MerchantInfo);
                if (merchant == null) throw new ApplicationException("merchant doesn't exist");
                CheckMerchantStatus(merchant.MerchantInfo.MerchantStatus);
                OrganizationFileType filetype = (OrganizationFileType)Enum.Parse(typeof(OrganizationFileType), model.Id);
                if (filetype == OrganizationFileType.CommercialRate)
                {
                    CheckUserPermissionAsSalesPerson(merchant.MerchantInfo?.SalesPersonId);
                }
                var (docId, url) = _dmsService.InsertFile(model.File);

                OrganizationFile merchantFile = GetMerchantFile(merchant.Id, filetype);
                if (merchantFile == null)
                {
                    merchantFile = new OrganizationFile
                    {
                        DmsId = docId,
                        DmsUrl = url,
                        FileType = filetype,
                        Name = model.File.FileName,
                        OrganizationId = merchant.Id
                    };
                    _merchantFileRepo.Add(merchantFile);
                }
                else
                {
                    merchantFile.Name = model.File.Name;
                    merchantFile.DmsId = docId;
                    merchantFile.DmsUrl = url;
                    _merchantFileRepo.Update(merchantFile);
                }
                var fileUrl = _dmsService.GetFileUrl(url);
                return FullResponse.SuccessWithId(fileUrl);
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse AddOutlet(MerchantRegisterModel_Outlet model)
        {
            try
            {
                CheckMerchantStatus(model.Mid);
                if (model.Id.HasValue)
                {
                    var outlet = _merchantOutletRepo.GetById(model.Id.Value);
                    if (outlet == null)
                    {
                        throw new ApplicationException("Outlet Doesn't Exist");
                    }

                    _mapper.Map(model, outlet);

                    _merchantOutletRepo.Update(outlet);
                    return FullResponse.SuccessWithId(outlet.Id.ToString());
                }
                var newOutlet = _mapper.Map<MerchantOutlet>(model);
                _merchantOutletRepo.Add(newOutlet);
                return FullResponse.SuccessWithId(newOutlet.Id.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse AddOwner(MerchantRegisterModel_Owner model)
        {
            try
            {
                CheckMerchantStatus(model.Mid);
                if (model.Id.HasValue)
                {
                    MerchantOwner owner = _merchantOwnerRepo.GetById(model.Id.Value);
                    if (owner == null)
                    {
                        throw new ApplicationException("Owner Doesn't Exist");
                    }
                    owner.Name = model.OwnerName;
                    owner.Mobile = model.OwnerMobile;
                    owner.Designation = model.OwnerDesignation;
                    owner.IdentityNumber = model.OwnerIdentityNo;

                    _merchantOwnerRepo.Update(owner);
                    return FullResponse.SuccessWithId(owner.Id.ToString());
                }

                var newOwner = _mapper.Map<MerchantOwner>(model);
                _merchantOwnerRepo.Add(newOwner);
                return FullResponse.SuccessWithId(newOwner.Id.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse AddServices(MerchantRegisterModel_Services model)
        {
            try
            {
                if (!model.Mid.HasValue)
                {
                    throw new ApplicationException("MID is not provided");
                }
                var merchantInfo = GetMerchantInfoByMid(model.Mid.Value);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant hasn't registered before");
                }
                CheckMerchantStatus(merchantInfo.MerchantStatus);

                merchantInfo.MerchantState = MerchantRegisterState.Services;
                merchantInfo.Services = JsonSerializer.Serialize(model);
                merchantInfo.PassedStates = GeneratePassedStatesString(merchantInfo.PassedStates, MerchantRegisterState.Services);

                _merchantInfoRepo.Update(merchantInfo);
                return new FullResponse
                {
                    IsSuccessful = true,
                    Id = merchantInfo.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse Approve(Guid mid)
        {
            try
            {
                var merchantInfo = GetMerchantInfoByMid(mid);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant Id doesn't exist.");
                }
                //OP1 or OP2 approving
                if (merchantInfo.MerchantStatus == MerchantStatus.Op1)
                {
                    ApproveMerchant(merchantInfo);
                    _notificationService.Register(ActionType.OP1Accepted, mid, nameof(Merchant));
                    return FullResponse.Success;
                }
                else if (merchantInfo.MerchantStatus == MerchantStatus.Op2)
                {
                    ApproveMerchant(merchantInfo);
                    _notificationService.Register(ActionType.OP2Accepted, mid, nameof(Merchant));
                    return FullResponse.Success;
                }
                else
                {
                    throw new ApplicationException("Merchant Status is not correct");
                }
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse Complete(Guid mid)
        {
            try
            {
                var merchantInfo = GetMerchantInfoByMid(mid);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant Id doesn't exist");
                }
                if (merchantInfo.MerchantStatus != MerchantStatus.Incomplete && merchantInfo.MerchantStatus != MerchantStatus.Inadequate)
                {
                    throw new ApplicationException("Merchant Status is not correct");
                }
                var result = ValidatePassedStates(merchantInfo);
                if (!result.IsSuccessful)
                {
                    return result;
                }

                CompleteMerchantState(mid);
                _notificationService.Register(ActionType.MerchantCompleted, mid, nameof(Merchant));
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public void CompleteMerchantState(Guid merchantId)
        {
            MerchantInfo merchantInfo = GetMerchantInfoByMid(merchantId);
            if (merchantInfo == null) throw new ApplicationException("Merchant doesn't exist");
            merchantInfo.RejectRemark = string.Empty;
            merchantInfo.MerchantState = MerchantRegisterState.Complete;
            merchantInfo.PassedStates = GeneratePassedStatesString(merchantInfo.PassedStates, MerchantRegisterState.Complete);
            merchantInfo.MerchantStatus = MerchantStatus.Op1;
            _merchantInfoRepo.Update(merchantInfo);
        }

        public FullResponse CreateUser(UserRegisterModel model)
        {
            try
            {
                model.OrganizationId = BasicOrganizations.Merchant;
                if (string.CompareOrdinal(model.Password, model.PasswordConfirm) != 0)
                {
                    throw new ApplicationException("Password doesn't match");
                }

                var ouser = new OUser
                {
                    OrganizationId = model.OrganizationId,
                    Email = model.Email.Trim(),
                    UserName = model.UserName.Trim(),
                    Name = model.Name.Trim(),
                    PhoneNumber = model.Phone.Trim(),
                    State = StateOfEntity.Complete
                };
                _userService.Create(ouser, model.Password);
                _userService.AddToRole(ouser, BasicRoles.Merchant.name);

                return FullResponse.Success;
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public Merchant GetAsAdmin(Guid id, bool isNoTracking = false, params Expression<Func<Merchant, object>>[] includes)
        {
            return _merchantRepository.GetByIdAsAdmin(id, false, includes);
        }

        public MerchantRegisterModel_Bank GetBankAccount(Guid mid)
        {
            var bankAccount = new MerchantRegisterModel_Bank();
            MerchantInfo merchantInfo = GetMerchantInfoByMid(mid);

            if (merchantInfo == null)
            {
                return bankAccount;
            }

            bankAccount.BankName = merchantInfo.BankName;
            bankAccount.AccountName = merchantInfo.BankAccountName;
            bankAccount.AccountNo = merchantInfo.BankAccountNo;
            bankAccount.BankAddress = merchantInfo.BankAddress;
            bankAccount.Id = merchantInfo.Id;
            bankAccount.Mid = mid;

            return bankAccount;
        }

        public MerchantRegisterModel GetMerchantInfoByFormNo(string id)
        {
            Merchant merchant = GetbyFormNumber(id);
            //return merchant.ToRegisterModel();
            return ToRegisterModel(merchant);
        }

        public MerchantRegisterModel GetNewRegisterModelForCurrentUser()
        {
            var user = _authorizationService.GetCurrentUser();
            return new MerchantRegisterModel
            {
                Info = new MerchantRegisterModel_Info
                {
                    ContactName = user?.Name,
                    Email = user?.Email,
                    MobileNo = user?.PhoneNumber
                }
            };
        }

        public MerchantRegisterModel_Outlet GetOutlet(Guid id)
        {
            MerchantOutlet outlet = _merchantOutletRepo.GetById(id);
            if (outlet == null)
            {
                throw new ApplicationException("Outlet doesn't exist");
            }
            return new MerchantRegisterModel_Outlet
            {
                Id = outlet.Id,
                OAddress = outlet.Address,
                OContactPerson = outlet.ContactPerson,
                OFaxNo = outlet.Fax,
                OLat = outlet.Lat,
                OLng = outlet.Lng,
                OName = outlet.Name,
                OOperatingDaysHours = outlet.OperatingDaysHours,
                OPEmail = outlet.ContactEmail,
                OPMobileNo = outlet.ContactMobile,
                OPostCode = outlet.PostCode,
                OPTelNo = outlet.ContactTel,
                ORemarks = outlet.Remark,
                OState = outlet.RegionState,
                OTelNo = outlet.Tel,
                OTown = outlet.Town,
                OType = outlet.OutletType
            };
        }

        public List<MerchantRegisterModel_Outlet> GetOutlets(Guid mid)
        {
            List<MerchantOutlet> outlets = _merchantOutletRepo.Entities.Where(m => m.Merchant.Id == mid).ToList();
            return outlets.Select(o => new MerchantRegisterModel_Outlet
            {
                Id = o.Id,
                OName = o.Name,
                OTelNo = o.Tel,
                OAddress = o.Address,
                OFaxNo = o.Fax,
                OTown = o.Town,
                OState = o.RegionState,
                OPostCode = o.PostCode,
                OLat = o.Lat,
                OLng = o.Lng,
                ORemarks = o.Remark,
                OOperatingDaysHours = o.OperatingDaysHours,
                OType = o.OutletType,
                OContactPerson = o.ContactPerson,
                OPMobileNo = o.ContactMobile,
                OPEmail = o.ContactEmail,
                OPTelNo = o.ContactTel
            }).ToList();
        }

        public MerchantRegisterModel_Owner GetOwner(Guid id)
        {
            MerchantOwner owner = _merchantOwnerRepo.GetById(id);
            if (owner == null)
            {
                throw new ApplicationException("Owner doesn't exist");
            }

            return new MerchantRegisterModel_Owner
            {
                Id = owner.Id,
                OwnerDesignation = owner.Designation,
                OwnerIdentityNo = owner.IdentityNumber,
                OwnerMobile = owner.Mobile,
                OwnerName = owner.Name
            };
        }

        public List<MerchantRegisterModel_Owner> GetOwners(Guid merchantId)
        {
            var owners = _merchantOwnerRepo.Entities.Where(m => m.Merchant.Id == merchantId).ToList();

            return owners.Select(mo => new MerchantRegisterModel_Owner
            {
                Id = mo.Id,
                OwnerName = mo.Name,
                OwnerIdentityNo = mo.IdentityNumber,
                OwnerDesignation = mo.Designation,
                OwnerMobile = mo.Mobile
            }).ToList();
        }

        public Dictionary<string, string> GetSalesPersonList()
        {
            var salesPersons = _authorizationService.GetUsersInRole(BasicRoles.SalesPerson.name);
            return salesPersons.Where(s => s.State == StateOfEntity.Complete).ToDictionary(s => s.Id.ToString(), s => s.Name);
        }
        public Dictionary<string, string> GetSalesPersonList(Guid? salespersonId)
        {
            if (salespersonId == null)
            {
                var salesPersons = _authorizationService.GetUsersInRole(BasicRoles.SalesPerson.name);
                return salesPersons.Where(s => s.State == StateOfEntity.Complete).ToDictionary(s => s.Id.ToString(), s => s.Name);
            }
            else
            {
                var salesPersons = _authorizationService.GetUsersInRole(BasicRoles.SalesPerson.name);
                return salesPersons.Where(s => s.State == StateOfEntity.Complete && s.Id == salespersonId).ToDictionary(s => s.Id.ToString(), s => s.Name);
            }

        }

        public List<string> GetUserRoles()
        {
            var currentUser = _authorizationService.GetCurrentUser();
            if (currentUser == null) return new List<string>();
            return _authorizationService.GetUserRoles(currentUser).ToList();
        }

        public FullResponse RegisterInfo(MerchantRegisterModel_Info model)
        {
            try
            {
                var merchant = new Merchant()
                {
                    ParentId = BasicOrganizations.Merchant,
                    Name = model.RegisteredBusiness
                };
                //Generate form number
                (int formNo, string formNoStr) = GenerateFormNumber();
                var merchantInfo = _mapper.Map<MerchantInfo>(model);
                merchantInfo.FormNumber = formNoStr;
                merchantInfo.FormNumberBase = formNo;
                merchantInfo.MerchantState = MerchantRegisterState.Info;
                merchantInfo.PassedStates = GeneratePassedStatesString(string.Empty, MerchantRegisterState.Info);
                merchantInfo.MerchantStatus = MerchantStatus.Incomplete;
                merchantInfo.Account = _merchantRepository.CurrentUserId;
                merchant.MerchantInfo = merchantInfo;
                _merchantRepository.Add(merchant);
                _notificationService.Register(ActionType.NewMerchantRegistered, merchant.Id, nameof(Merchant));
                return new FullResponse
                {
                    Id = merchant.Id.ToString(),
                    IsSuccessful = true,
                    Message = formNoStr
                };
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse Reject(MerchantRegisterRejectModel model)
        {
            try
            {
                if (!model.Mid.HasValue)
                {
                    throw new ApplicationException("Merchant Id isn't provided.");
                }
                var merchantInfo = GetMerchantInfoByMid(model.Mid.Value);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant Id doesn't exist.");
                }
                merchantInfo.RejectRemark = model.Remark;
                switch (merchantInfo.MerchantStatus)
                {
                    case MerchantStatus.Op1:
                        RejectMerchant(merchantInfo, ActionType.OP1Rejected, model.IsPermanent);
                        return FullResponse.Success;

                    case MerchantStatus.Op2:
                        RejectMerchant(merchantInfo, ActionType.OP2Rejected, model.IsPermanent);
                        return FullResponse.Success;

                    case MerchantStatus.Risk:
                        RejectMerchant(merchantInfo, ActionType.RiskRejected, model.IsPermanent);
                        return FullResponse.Success;

                    default:
                        throw new ApplicationException("Merchant Status is not correct");
                }
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public void RemoveMerchantFile(string mid, string name)
        {
            if (string.IsNullOrEmpty(mid) || !Guid.TryParse(mid, out _)) throw new ApplicationException("Merchant Id is not valid");
            var merchantId = Guid.Parse(mid);

            var merchant = _merchantRepository.GetById(merchantId, true, m => m.MerchantInfo);
            if (merchant == null) throw new ApplicationException("merchant doesn't exist");
            CheckMerchantStatus(merchant.MerchantInfo.MerchantStatus);
            var filetype = (OrganizationFileType)Enum.Parse(typeof(OrganizationFileType), name);
            if (filetype == OrganizationFileType.CommercialRate)
            {
                CheckUserPermissionAsSalesPerson(merchant.MerchantInfo?.SalesPersonId);
            }
            OrganizationFile merchantFile = GetMerchantFile(merchant.Id, filetype);
            if (merchantFile == null) return;

            _merchantFileRepo.Remove(merchantFile.Id);
        }

        public FullResponse RemoveOutlet(Guid outletId)
        {
            try
            {
                var outlet = _merchantOutletRepo.GetById(outletId);
                if (outlet == null) throw new ApplicationException("The outlet doesn't exist");
                CheckMerchantStatus(outlet.MerchantId);
                _merchantOutletRepo.Remove(outlet.Id);
                return FullResponse.Success;
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public FullResponse RemoveOwner(Guid ownerId)
        {
            try
            {
                var owner = _merchantOwnerRepo.GetById(ownerId);
                if (owner == null) throw new ApplicationException("The owner doesn't exist");
                CheckMerchantStatus(owner.MerchantId);
                _merchantOwnerRepo.Remove(owner.Id);
                return FullResponse.Success;
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public List<MerchantListModel> RetrieveForInbox()
        {
            if (_authorizationService.HasUserRole(BasicRoles.SalesPerson.name))
            {
                return RetrieveListForSalesPersonInbox();
            }

            MerchantStatus status = MerchantStatus.Incomplete;
            if (_authorizationService.HasUserRole(BasicRoles.MerchantOPLvl1.name)) status = MerchantStatus.Op1;
            if (_authorizationService.HasUserRole(BasicRoles.MerchantOPLvl2.name)) status = MerchantStatus.Op2;
            if (_authorizationService.HasUserRole(BasicRoles.MerchantRiskHead.name)) status = MerchantStatus.Risk;
            if (status == MerchantStatus.Incomplete)
            {
                return new List<MerchantListModel>();
            }
            return RetrieveList(status);
        }

        public DtReturn<MerchantListModel> RetrieveForList(DtReceive dtReceive)
        {
            if (_authorizationService.HasUserRole(BasicRoles.Merchant.name))
            {
                return GetMerchantListForUser(dtReceive);
            }
            if (_authorizationService.HasUserRole(BasicRoles.SalesPerson.name))
            {
                return GetMerchantListForSalesPerson(dtReceive);
            }
            return GetMerchantList(dtReceive);
        }

        public FullResponse UpdateInfo(MerchantRegisterModel_Info model)
        {
            try
            {
                var merchantInfo = GetMerchantInfoByMid(model.Mid.Value);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant doesn't exist with this Id: " + model.Mid);
                }
                CheckMerchantStatus(merchantInfo.MerchantStatus);
                model.Id = merchantInfo.Id;
                merchantInfo = _mapper.Map(model, merchantInfo);

                merchantInfo.State = StateOfEntity.InProgress;
                merchantInfo.MerchantState = MerchantRegisterState.Info;
                merchantInfo.PassedStates = GeneratePassedStatesString(string.Empty, MerchantRegisterState.Info);
                _merchantInfoRepo.Update(merchantInfo);
                return new FullResponse
                {
                    Id = merchantInfo.MerchantId.ToString(),
                    IsSuccessful = true,
                    Message = merchantInfo.FormNumber
                };
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        public SimpleResponse UpdateMerchantState(Guid merchantId, MerchantRegisterState state)
        {
            try
            {
                MerchantInfo merchantInfo = GetMerchantInfoByMid(merchantId);
                if (merchantInfo == null)
                {
                    throw new ApplicationException("Merchant doesn't exist");
                }
                if (state == MerchantRegisterState.Terms)
                {
                    var hasCommission = _merchantCommisionRepo.Entities.Any(c => c.MerchantId == merchantId);
                    if (!hasCommission)
                    {
                        throw new ApplicationException("The commercial document hasn't provided, please contact your sales person");
                    }
                }
                merchantInfo.MerchantState = state;
                merchantInfo.PassedStates = GeneratePassedStatesString(merchantInfo.PassedStates, state);
                _merchantInfoRepo.Update(merchantInfo);
                return SimpleResponse.Success();
            }
            catch (Exception ex)
            {
                return ex.ToSimpleResponse();
            }
        }

        internal bool IsDuplicate(MerchantRegisterModel_Info model)
        {
            if (model.Mid == null)
            {
                return _merchantInfoRepo.Entities.Any(m => m.Name == model.RegisteredBusiness.Trim() || m.BusinessNo == model.BusinessNo.Trim());
            }
            else
            {
                return _merchantInfoRepo.Entities.Any(m => m.MerchantId != model.Mid.Value && (m.Name == model.RegisteredBusiness.Trim() || m.BusinessNo == model.BusinessNo.Trim()));
            }
        }

        public bool HasCommission(Guid? merchantId)
        {
            if (!merchantId.HasValue) return false;
            return _merchantCommisionRepo.Entities.Any(c => c.MerchantId == merchantId.Value);
        }

        public Dictionary<string, string> GetMerchantAccountList()
        {
            return _userService.GetMerchantAccountList();
        }
        public FullResponse RegisterMerchantFormBySalesPerson(SalesPersonFormRegistrationModel model)
        {
            var isAnotherMerchantWithSameBusinessNoExist = _merchantRepository.Entities
                .Include(m => m.MerchantInfo)
                .Any(m => m.MerchantInfo.BusinessNo == model.BusinessNo && m.MerchantInfo.Account == model.MerchantAccountGuid);

            return isAnotherMerchantWithSameBusinessNoExist ? CreateMerchantFormClone(model) : CreateMerchantFormNew(model);
        }

        #region Private Methods

        private void RejectMerchant(MerchantInfo merchantInfo, ActionType actionType, bool isPermanent)
        {
            if (isPermanent)
            {
                RejectMerchantPermanently(merchantInfo);
                _notificationService.Register(ActionType.Rejected, merchantInfo.MerchantId, nameof(Merchant));
            }
            else
            {
                InadequateMerchant(merchantInfo);
                _notificationService.Register(actionType, merchantInfo.MerchantId, nameof(Merchant));
            }
        }

        private void UpdateMerchantStateForCommission(Guid merchantId)
        {
            MerchantInfo merchantInfo = GetMerchantInfoByMid(merchantId);
            merchantInfo.PassedStates = GeneratePassedStatesString(merchantInfo.PassedStates, MerchantRegisterState.Commission);
            _merchantInfoRepo.Update(merchantInfo);
        }

        private static MerchantRegisterModel_Services GetServicesFromString(string services)
        {
            if (string.IsNullOrEmpty(services))
            {
                return new MerchantRegisterModel_Services();
            }
            return JsonSerializer.Deserialize<MerchantRegisterModel_Services>(services);
        }

        private void AcceptMerchant(MerchantInfo merchantInfo)
        {
            merchantInfo.MerchantStatus = MerchantStatus.Accept;
            _merchantInfoRepo.Update(merchantInfo);
        }

        private void ApproveMerchant(MerchantInfo merchantInfo)
        {
            merchantInfo.RejectRemark = string.Empty;
            if (merchantInfo.MerchantStatus == MerchantStatus.Op1)
            {
                merchantInfo.MerchantStatus = MerchantStatus.Op2;
            }
            else
            {
                merchantInfo.MerchantStatus = MerchantStatus.Risk;
            }

            _merchantInfoRepo.Update(merchantInfo);
        }

        private FullResponse ValidatePassedStates(MerchantInfo merchantInfo)
        {
            var passedStates = merchantInfo.PassedStates.Split(',');
            var services = new MerchantRegisterModel_Services();
            if (merchantInfo.Services != null)
            {
                services = JsonSerializer.Deserialize<MerchantRegisterModel_Services>(merchantInfo.Services);
            }

            var result = FullResponse.Success;
            if (!passedStates.Contains(MerchantRegisterState.Info.ToKeyString()))
            {
                result.IsSuccessful = false;
                result.Validations.Add(new ValidationModel
                {
                    Field = "collapseInfo",
                    Description = "Application Information"
                });
            }
            if (!passedStates.Contains(MerchantRegisterState.Services.ToKeyString()))
            {
                result.IsSuccessful = false;
                result.Validations.Add(new ValidationModel
                {
                    Field = "collapseServices",
                    Description = "Application Types"
                });
            }
            if (!passedStates.Contains(MerchantRegisterState.Owners.ToKeyString()))
            {
                result.IsSuccessful = false;
                result.Validations.Add(new ValidationModel
                {
                    Field = "collapseOwner",
                    Description = "Company Owner / Directors"
                });
            }
            if (!passedStates.Contains(MerchantRegisterState.Outlets.ToKeyString()))
            {
                result.IsSuccessful = false;
                result.Validations.Add(new ValidationModel
                {
                    Field = "collapseOutlet",
                    Description = "Physical Channel - Main Outlet Details"
                });
            }
            if (!passedStates.Contains(MerchantRegisterState.BankAccount.ToKeyString()))
            {
                result.IsSuccessful = false;
                result.Validations.Add(new ValidationModel
                {
                    Field = "collapseBank",
                    Description = "Application Bank Account Details"
                });
            }
            if (!passedStates.Contains(MerchantRegisterState.Files.ToKeyString()))
            {
                result.IsSuccessful = false;
                result.Validations.Add(new ValidationModel
                {
                    Field = "collapseFile",
                    Description = "Upload Documents"
                });
            }
            if (!passedStates.Contains(MerchantRegisterState.Terms.ToKeyString()))
            {
                result.IsSuccessful = false;
                result.Validations.Add(new ValidationModel
                {
                    Field = "collapseTerms",
                    Description = "Terms and Conditions"
                });
            }
            if (services.M1Pay_CreditCardUms || services.M1Pay_Emonei || services.M1Pay_Ewallets || services.M1Pay_Fpx || services.M1Pay_Alipay)
            {
                if (!passedStates.Contains(MerchantRegisterState.Channel.ToKeyString()))
                {
                    result.IsSuccessful = false;
                    result.Validations.Add(new ValidationModel
                    {
                        Field = "collapseChannel",
                        Description = "Internet Channel / Web Store Details"
                    });
                }
            }
            if (!result.IsSuccessful)
            {
                result.Message = "You need to complete all sections one by one and ensure to press Next button after each section completion";
            }
            return result;
        }

        private void CheckUserPermissionAsSalesPerson(Guid? salesPersonId)
        {
            if (!_authorizationService.HasUserRole(BasicRoles.SalesPerson.name))
            {
                throw new ApplicationException("for Sales person/account manager use only");
            }
            if (_authorizationService.GetCurrentUserId() != salesPersonId)
            {
                throw new ApplicationException("only related sales person can upload this document");
            }
        }

        private DtReturn<MerchantListModel> GetMerchantList(DtReceive dtReceive)
        {
            var result = new DtReturn<MerchantListModel>
            {
                Data = RetrieveList(dtReceive.Search.Value, dtReceive.Start, dtReceive.Length, out int total, out int filteredTotal),
                RecordsTotal = total,
                RecordsFiltered = filteredTotal,
                Draw = dtReceive.Draw
            };
            return result;
        }

        private DtReturn<MerchantListModel> GetMerchantListForSalesPerson(DtReceive dtReceive)
        {
            var dataResult = RetrieveListForSalesPerson(dtReceive.Search.Value, dtReceive.Start, dtReceive.Length, out int total, out int filteredTotal);
            var result = new DtReturn<MerchantListModel>
            {
                Data = dataResult,
                RecordsTotal = total,
                RecordsFiltered = filteredTotal,
                Draw = dtReceive.Draw
            };
            return result;
        }

        private DtReturn<MerchantListModel> GetMerchantListForUser(DtReceive dtReceive)
        {
            var dataResult = RetrieveListForUser(dtReceive.Search.Value, dtReceive.Start, dtReceive.Length, out int total, out int filteredTotal);
            var result = new DtReturn<MerchantListModel>
            {
                Data = dataResult,
                RecordsTotal = total,
                RecordsFiltered = filteredTotal,
                Draw = dtReceive.Draw
            };
            return result;
        }

        private void InadequateMerchant(MerchantInfo merchantInfo)
        {
            if (merchantInfo.MerchantStatus == MerchantStatus.Op1)
            {
                merchantInfo.MerchantStatus = MerchantStatus.Inadequate;
            }
            else if (merchantInfo.MerchantStatus == MerchantStatus.Op2)
            {
                merchantInfo.MerchantStatus = MerchantStatus.Op1;
            }
            else
            {
                merchantInfo.MerchantStatus = MerchantStatus.Op2;
            }
            _merchantInfoRepo.Update(merchantInfo);
        }

        private void RejectMerchantPermanently(MerchantInfo merchantInfo)
        {
            merchantInfo.MerchantStatus = MerchantStatus.Rejected;
            _merchantInfoRepo.Update(merchantInfo);
        }

        private MerchantRegisterModel ToRegisterModel(Merchant merchant)
        {
            return merchant == null ? null : new MerchantRegisterModel
            {
                Id = merchant.Id,
                FormNo = merchant.MerchantInfo.FormNumber,
                MerchantState = merchant.MerchantInfo.MerchantState,
                MerchantStatus = merchant.MerchantInfo.MerchantStatus,
                MerchantStatusUser = merchant.MerchantInfo.MerchantStatusUser,
                Services = GetServicesFromString(merchant.MerchantInfo.Services),
                Info = new MerchantRegisterModel_Info
                {
                    Id = merchant.MerchantInfo.Id,
                    Mid = merchant.Id,
                    RegisteredBusiness = merchant.MerchantInfo.Name,
                    BusinessNo = merchant.MerchantInfo.BusinessNo,
                    Address = merchant.MerchantInfo.Address,
                    Country = merchant.MerchantInfo.Country,
                    BusinessType = merchant.MerchantInfo.BusinessType,
                    Principal = merchant.MerchantInfo.Principal,
                    ProductType = merchant.MerchantInfo.ProductType,
                    TickectSize = merchant.MerchantInfo.TickectSize,
                    MonthlyTurnover = merchant.MerchantInfo.MonthlyTurnover,
                    SstId = merchant.MerchantInfo.SstId,
                    Town = merchant.MerchantInfo.Town,
                    AreaState = merchant.MerchantInfo.AreaState,
                    PostCode = merchant.MerchantInfo.PostCode,
                    OperatingDaysHours = merchant.MerchantInfo.OperatingDaysHours,
                    ContactName = merchant.MerchantInfo.ContactName,
                    MobileNo = merchant.MerchantInfo.MobileNo,
                    Email = merchant.MerchantInfo.Email,
                    Designation = merchant.MerchantInfo.Designation,
                    TelNo = merchant.MerchantInfo.TelNo,
                    FaxNo = merchant.MerchantInfo.FaxNo,
                    SalesPersonId = merchant.MerchantInfo.SalesPersonId,
                    DeliveryTime = merchant.MerchantInfo.DeliveryTime
                },
                Bank = new MerchantRegisterModel_Bank
                {
                    AccountName = merchant.MerchantInfo?.BankAccountName,
                    AccountNo = merchant.MerchantInfo?.BankAccountNo,
                    BankAddress = merchant.MerchantInfo?.BankAddress,
                    BankName = merchant.MerchantInfo?.BankName,
                    BankPromoAgree = merchant.MerchantInfo?.BankPromoAgreeable.ToBankPromoType()
                },
                Channel = new MerchantRegisterModel_Channel
                {
                    ChannelAddress = merchant.MerchantInfo?.ChannelAddress,
                    ChannelUrl = merchant.MerchantInfo?.ChannelUrl,
                    ChannelEmail = merchant.MerchantInfo?.ChannelEmail
                },
                Files = new MerchantRegisterModel_File
                {
                    ApplicantPhoto = merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.ApplicantPhoto)?.DmsId.ToString(),
                    ApplicantPhotoUrl = _dmsService.GetFileUrl(merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.ApplicantPhoto)?.DmsUrl),
                    BankStatement = merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.BankStatement)?.DmsId.ToString(),
                    BankStatementUrl = _dmsService.GetFileUrl(merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.BankStatement)?.DmsUrl),
                    CompanyRegistrationSearch = merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.CompanyRegistrationSearch)?.DmsId.ToString(),
                    CompanyRegistrationSearchUrl = _dmsService.GetFileUrl(merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.CompanyRegistrationSearch)?.DmsUrl),
                    CtosOfBoard = merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.CtosOfBoard)?.DmsId.ToString(),
                    CtosOfBoardUrl = _dmsService.GetFileUrl(merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.CtosOfBoard)?.DmsUrl),
                    IdentificationDocuments = merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.IdentificationDocuments)?.DmsId.ToString(),
                    IdentificationDocumentsUrl = _dmsService.GetFileUrl(merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.IdentificationDocuments)?.DmsUrl),
                    OtherDocument = merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.OtherDocument)?.DmsId.ToString(),
                    OtherDocumentUrl = _dmsService.GetFileUrl(merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.OtherDocument)?.DmsUrl),
                    CommercialRate = merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.CommercialRate)?.DmsId.ToString(),
                    CommercialRateUrl = _dmsService.GetFileUrl(merchant.OrganizationFiles?.SingleOrDefault(f => f.FileType == OrganizationFileType.CommercialRate)?.DmsUrl)
                },
                Reject = new MerchantRegisterRejectModel
                {
                    IsPermanent = merchant.MerchantInfo?.MerchantStatus == MerchantStatus.Rejected,
                    Remark = merchant.MerchantInfo?.RejectRemark
                }
            };
        }

        private List<MerchantListModel> RetrieveListForUser(string searchValue, int start, int take, out int total, out int count)
        {
            var query = _merchantRepository.Entities.AsNoTracking();
            query = query.Include(m => m.MerchantInfo);
            query = query.Where(m => m.MerchantInfo.Account == _merchantRepository.CurrentUserId);
            total = query.Count();

            query = query.Where(m =>
                            string.IsNullOrEmpty(searchValue)
                            || m.Name.Contains(searchValue)
                            || m.MerchantInfo.FormNumber.Contains(searchValue)
                            || m.MerchantInfo.BusinessNo.Contains(searchValue));

            count = query.Count();
            query = query.OrderByDescending(m => m.CreatedAt);
            query = query.Skip(start).Take(take);

            var result = query.AsEnumerable()
                .GroupJoin(_merchantRepository.Context.Users, m => m.MerchantInfo.SalesPersonId, u => u.Id,
                (merchant, user) => new MerchantListModel
                {
                    Id = merchant.Id,
                    BusinessNo = merchant.MerchantInfo.BusinessNo,
                    BusinessType = merchant.MerchantInfo.BusinessType.ToString(),
                    ContactName = merchant.MerchantInfo.ContactName,
                    Salesperson = user.FirstOrDefault()?.Name,
                    Designation = merchant.MerchantInfo.Designation,
                    FormNo = merchant.MerchantInfo.FormNumber,
                    MobileNo = merchant.MerchantInfo.MobileNo,
                    RegisteredBusiness = merchant.MerchantInfo.Name,
                    Status = merchant.MerchantInfo.MerchantStatusUser
                });

            return result.ToList();
        }

        private List<MerchantListModel> RetrieveListForSalesPerson(string searchValue, int start, int take, out int total, out int count)
        {
            var salesPersonId = _merchantRepository.CurrentUserId;
            var query = _merchantRepository.Entities.AsNoTracking();
            query = query.Where(m => m.MerchantInfo.SalesPersonId == salesPersonId);
            total = query.Count();

            query = query.Where(m =>
                            string.IsNullOrEmpty(searchValue)
                            || m.Name.Contains(searchValue)
                            || m.MerchantInfo.FormNumber.Contains(searchValue)
                            || m.MerchantInfo.BusinessNo.Contains(searchValue));

            count = query.Count();
            query = query.OrderByDescending(m => m.CreatedAt);
            query = query.Skip(start).Take(take);
            query = query.Include(m => m.MerchantInfo);

            var result = query.AsEnumerable()
                .GroupJoin(_merchantRepository.Context.Users, m => m.MerchantInfo.SalesPersonId, u => u.Id,
                (merchant, user) => new MerchantListModel
                {
                    Id = merchant.Id,
                    BusinessNo = merchant.MerchantInfo.BusinessNo,
                    BusinessType = merchant.MerchantInfo.BusinessType.ToString(),
                    ContactName = merchant.MerchantInfo.ContactName,
                    Salesperson = user.FirstOrDefault()?.Name,
                    Designation = merchant.MerchantInfo.Designation,
                    FormNo = merchant.MerchantInfo.FormNumber,
                    MobileNo = merchant.MerchantInfo.MobileNo,
                    RegisteredBusiness = merchant.MerchantInfo.Name,
                    Status = merchant.MerchantInfo.MerchantStatus.ToString()
                });
            return result.ToList();
        }

        private List<MerchantListModel> RetrieveList(string searchValue, int start, int take, out int total, out int count)
        {
            var query = _merchantRepository.Entities.AsNoTracking();
            total = query.Count();

            query = query.Where(m =>
                    string.IsNullOrEmpty(searchValue)
                    || m.Name.Contains(searchValue)
                    || m.MerchantInfo.FormNumber.Contains(searchValue)
                    || m.MerchantInfo.BusinessNo.Contains(searchValue));

            count = query.Count();
            query = query.OrderByDescending(m => m.CreatedAt);
            query = query.Skip(start).Take(take);
            query = query.Include(m => m.MerchantInfo);

            var result = query.AsEnumerable()
                .GroupJoin(_merchantRepository.Context.Users, m => m.MerchantInfo.SalesPersonId, u => u.Id,
                (merchant, user) => new MerchantListModel
                {
                    Id = merchant.Id,
                    BusinessNo = merchant.MerchantInfo.BusinessNo,
                    BusinessType = merchant.MerchantInfo.BusinessType.ToString(),
                    ContactName = merchant.MerchantInfo.ContactName,
                    Salesperson = user.FirstOrDefault()?.Name,
                    Designation = merchant.MerchantInfo.Designation,
                    FormNo = merchant.MerchantInfo.FormNumber,
                    MobileNo = merchant.MerchantInfo.MobileNo,
                    RegisteredBusiness = merchant.MerchantInfo.Name,
                    Status = merchant.MerchantInfo.MerchantStatus.ToString()
                });

            return result.ToList();
        }

        private List<MerchantListModel> RetrieveList(MerchantStatus status)
        {
            var result = from merchant in _merchantRepository.Entities
               .Include(m => m.MerchantInfo)
               .Where(m => m.MerchantInfo.MerchantStatus == status)
                         join user in _merchantRepository.Context.Users
                         on merchant.MerchantInfo.SalesPersonId equals user.Id into u
                         from user in u.DefaultIfEmpty()
                         select new MerchantListModel
                         {
                             Id = merchant.Id,
                             BusinessNo = merchant.MerchantInfo.BusinessNo,
                             BusinessType = merchant.MerchantInfo.BusinessType.ToString(),
                             ContactName = merchant.MerchantInfo.ContactName,
                             Salesperson = user.Name,
                             Designation = merchant.MerchantInfo.Designation,
                             FormNo = merchant.MerchantInfo.FormNumber,
                             MobileNo = merchant.MerchantInfo.MobileNo,
                             RegisteredBusiness = merchant.MerchantInfo.Name,
                             Status = merchant.MerchantInfo.MerchantStatus.ToString()
                         };
            return result.ToList();
        }

        private List<MerchantListModel> RetrieveListForSalesPersonInbox()
        {
            var salesPersonId = _merchantRepository.CurrentUserId;
            var result = from merchant in _merchantRepository.Entities
               .Include(m => m.MerchantInfo)
               .Include(m => m.MerchantCommission)
               .Where(m => m.MerchantInfo.SalesPersonId == salesPersonId && m.MerchantCommission == null)
                         join user in _merchantRepository.Context.Users
                         on merchant.MerchantInfo.SalesPersonId equals user.Id into u
                         from user in u.DefaultIfEmpty()
                         select new MerchantListModel
                         {
                             Id = merchant.Id,
                             BusinessNo = merchant.MerchantInfo.BusinessNo,
                             BusinessType = merchant.MerchantInfo.BusinessType.ToString(),
                             ContactName = merchant.MerchantInfo.ContactName,
                             Salesperson = user.Name,
                             Designation = merchant.MerchantInfo.Designation,
                             FormNo = merchant.MerchantInfo.FormNumber,
                             MobileNo = merchant.MerchantInfo.MobileNo,
                             RegisteredBusiness = merchant.MerchantInfo.Name,
                             Status = merchant.MerchantInfo.MerchantStatus.ToString()
                         };
            return result.ToList();
        }

        private Merchant GetbyFormNumber(string id)
        {
            return _merchantRepository.Entities
                .Where(m => m.MerchantInfo.FormNumber == id)
                .Include(m => m.MerchantInfo)
                .Include(m => m.OrganizationFiles)
                .AsNoTracking()
                .SingleOrDefault();
        }

        private OrganizationFile GetMerchantFile(Guid merchantId, OrganizationFileType fileType)
        {
            return _merchantFileRepo.FilteredEntities.SingleOrDefault(f => f.OrganizationId == merchantId && f.FileType == fileType);
        }

        private MerchantInfo GetMerchantInfoByMid(Guid mid)
        {
            return _merchantRepository.Context.MerchantInfos
                .Where(m => m.MerchantId == mid)
                .FirstOrDefault();
        }

        private void CheckMerchantStatus(MerchantStatus status)
        {
            if (_authorizationService.GetCurrentUserId() == BasicUser.AdminId) return;
            if (status != MerchantStatus.Incomplete && status != MerchantStatus.Inadequate)
            {
                throw new ApplicationException("Merchant status is read-only");
            }
        }

        private void CheckMerchantStatus(Guid? merchantId)
        {
            var merchantStatus =
                _merchantRepository.Context.MerchantInfos.AsNoTracking()
                .Where(m => m.MerchantId == merchantId.Value).Select(m => m.MerchantStatus).First();

            CheckMerchantStatus(merchantStatus);
        }

        private MerchantFilePosition FindFilePosition(string name)
        {
            return (MerchantFilePosition)Enum.Parse(typeof(MerchantFilePosition), name);
        }

        private (int num, string str) GenerateFormNumber()
        {
            int sequencer = GetLastFormNumber() + 1;
            var sum = sequencer.ToString().Select(s => char.GetNumericValue(s)).Sum();
            var checksum = sum % 10;

            return (sequencer, sequencer.ToString() + checksum.ToString());
        }

        private int GetLastFormNumber()
        {
            var number = _merchantInfoRepo.Entities.OrderByDescending(m => m.FormNumberBase).Select(m => m.FormNumberBase).FirstOrDefault();
            return number == 0 ? 10000 : number;
        }

        private static string GeneratePassedStatesString(string currentStates, MerchantRegisterState newState)
        {
            if (currentStates == null)
            {
                currentStates = string.Empty;
            }
            var currentStatesArray = currentStates.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var newStateString = ((int)newState).ToString();

            if (currentStates.Contains(newStateString)) return string.Join(',', currentStatesArray);

            var newStatesArray = currentStatesArray.Append(newStateString);
            return string.Join(',', newStatesArray);
        }

        private FullResponse CreateMerchantFormClone(SalesPersonFormRegistrationModel model)
        {
            var merchantAccount = _userService.GetById(model.MerchantAccountGuid, asNoTrack: true);
            if (merchantAccount is null) return FullResponse.FailBecause("Merchant Account does not exist");
            var currentMerchant = _merchantRepository.Entities
                .AsNoTracking()
                .Include(m => m.MerchantInfo)
                .Include(m => m.MerchantOutlets)
                .Include(m => m.MerchantOwners)
                .Include(m => m.OrganizationFiles)
                .OrderBy(m => m.MerchantInfo.FormNumberBase)
                .First(m => m.MerchantInfo.BusinessNo == model.BusinessNo && m.MerchantInfo.Account == model.MerchantAccountGuid);
                
            Merchant newMerchant = new();
             _mapper.Map(currentMerchant,newMerchant);
            newMerchant.Name = model.BusinessName;
            var (formNo, formStr) = GenerateFormNumber();
            newMerchant.MerchantInfo.FormNumber = formStr;
            newMerchant.MerchantInfo.FormNumberBase = formNo;
            newMerchant.MerchantInfo.MerchantState = MerchantRegisterState.New;
            newMerchant.MerchantInfo.PassedStates = GeneratePassedStatesString(string.Empty, MerchantRegisterState.New);
            newMerchant.MerchantInfo.MerchantStatus = MerchantStatus.Incomplete;
            newMerchant.MerchantInfo.Name = model.BusinessName;
            newMerchant.MerchantInfo.SalesPersonId = _merchantRepository.CurrentUserId;
            newMerchant.MerchantInfo.Services = model.ServicesStr;
            newMerchant.MerchantInfo.ContactName = merchantAccount.Name;
            newMerchant.MerchantInfo.MobileNo = merchantAccount.PhoneNumber;
            newMerchant.MerchantInfo.Email = merchantAccount.Email;
            newMerchant.MerchantInfo.Account = merchantAccount.Id;

            _merchantRepository.Add(newMerchant);
            //_notificationService.Register(ActionType.NewMerchantRegistered, merchant.Id, nameof(Merchant));
            return new FullResponse
            {
                Id = newMerchant.Id.ToString(),
                IsSuccessful = true,
                Message = formStr
            };
        }

        private FullResponse CreateMerchantFormNew(SalesPersonFormRegistrationModel model)
        {
            var merchantAccount = _userService.GetById(model.MerchantAccountGuid, asNoTrack: true);
            if (merchantAccount is null) return FullResponse.FailBecause("Merchant Account does not exist");
            Merchant merchant = new()
            {
                ParentId = BasicOrganizations.Merchant,
                Name = model.BusinessName
            };
            var (formNo, formStr) = GenerateFormNumber();
            merchant.MerchantInfo = new()
            {
                FormNumber = formStr,
                FormNumberBase = formNo,
                MerchantState = MerchantRegisterState.New,
                PassedStates = GeneratePassedStatesString(string.Empty, MerchantRegisterState.New),
                MerchantStatus = MerchantStatus.Incomplete,
                Name = model.BusinessName,
                BusinessNo = model.BusinessNo,
                SalesPersonId = _merchantRepository.CurrentUserId,
                Services = model.ServicesStr,
                ContactName = merchantAccount.Name,
                MobileNo = merchantAccount.PhoneNumber,
                Email = merchantAccount.Email,
                Account = merchantAccount.Id
            };

            _merchantRepository.Add(merchant);
            //_notificationService.Register(ActionType.NewMerchantRegistered, merchant.Id, nameof(Merchant));
            return new FullResponse
            {
                Id = merchant.Id.ToString(),
                IsSuccessful = true,
                Message = formStr
            };
        }

        #endregion Private Methods
    }
}