using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using OneRegister.Data.Entities.AgroRegistration;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Model.AgropreneurRegistration;
using OneRegister.Core.Model.ControllerResponse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using static OneRegister.Data.Contract.Constants;
using OneRegister.Core.Model.DataTablesModel;
using OneRegister.Domain.Services.Dms;
using OneRegister.Data.Contract;
using Microsoft.EntityFrameworkCore;

namespace OneRegister.Domain.Services.AgropreneurRegistration
{
    public class AgropreneurService
    {
        private readonly IOrganizedRepository<Agropreneur> _agroRepository;
        private readonly DmsService _dmsService;
        private readonly IMapper _mapper;
        public AgropreneurService(
            IOrganizedRepository<Agropreneur> agroRepository,
            DmsService dmsService,
            IMapper mapper)
        {
            _agroRepository = agroRepository;
            _dmsService = dmsService;
            _mapper = mapper;
        }

        public AGPRegisterModel GetAgroById(Guid id)
        {
            var agropreneur = _agroRepository.GetById(id,true,a=>a.MemberFiles,a=>a.MemberAddresses);
            var model = _mapper.Map<AGPRegisterModel>(agropreneur);
            if (model == null) return model;

            model.PhotoDms = agropreneur.MemberFiles.SingleOrDefault(f => f.FileType == MemberFileType.Photo)?.DmsId;
            model.PhotoUrl = _dmsService.GetFileUrl(agropreneur.MemberFiles.FirstOrDefault(f => f.FileType == MemberFileType.Photo));

            model.IdPhotoDms = agropreneur.MemberFiles.SingleOrDefault(f => f.FileType == MemberFileType.Identity)?.DmsId;
            model.IdPhotoUrl = _dmsService.GetFileUrl(agropreneur.MemberFiles.FirstOrDefault(f => f.FileType == MemberFileType.Identity));

            model.MailingAddress = agropreneur.MemberAddresses.SingleOrDefault(a => a.AddressType == AddressType.MailingAddress)?.Address;

            return model;
        }

        public byte[] GetThumbnail(Guid agroId)
        {
            const string unknownPicBase64Str = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAYEBQYFBAYGBQYHBwYIChAKCgkJChQODwwQFxQYGBcUFhYaHSUfGhsjHBYWICwgIyYnKSopGR8tMC0oMCUoKSj/2wBDAQcHBwoIChMKChMoGhYaKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCj/wAARCABCADIDASIAAhEBAxEB/8QAGgABAAIDAQAAAAAAAAAAAAAAAAUGAQIEA//EACoQAAICAQMCBQMFAAAAAAAAAAABAgMEBRESIWETIjFBgQZCUTRicXKx/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/ALAd2mYEsufKW6pT6v8APZHCWrTKlVg1JbdVyfyB60UVUR2qrjFdvV/Jm+iq+HG6Cku/segAhdR0qEKpW42649XFvfp2IUuVkVOuUH6STRTpLaTXcDAAAFvxP0tP9I/4VAn9ByfEpdMn5odV/AEqAABTrePiz4b8d3tuWTVsl4+I+L2nPyrt3KwAAAA7NJslXn1cfufF/JtRpeTa/NDw1v1cicwcKvEhtHzTfrJoDqAAED9RN+PUt3tx329vUiS3ZWPXk1OFkU/XZv2f5K7k6dkUNtwcor7o9QOMB9Hs+jAF0AAAAAAABo6q223XBt/tQNwAAAAAAAAAAAH/2Q==";
            try
            {
                //byte[] thumbnail = _agroRepository.Get(agroId,new string[] {nameof(Member.MemberFiles)},isNoTracking:true).MemberFiles?.SingleOrDefault(f => f.FileType == MemberFileType.Photo)?.Thumbnail;
                byte[] thumbnail = GetThumbnailFromDatabase(agroId);
                if (thumbnail == null)
                {
                    return Convert.FromBase64String(unknownPicBase64Str);
                }

                return thumbnail;
            }
            catch (Exception ex)
            {
                //log ex
                return Convert.FromBase64String(unknownPicBase64Str);
            }
        }

        public int ImportAcceptedRecords(List<AGPImportModel> records)
        {
            int count = 0;
            foreach (var record in records)
            {
                try
                {
                    var newAgro = new Agropreneur
                    {
                        AgroOrganizationId = BasicOrganizations.Agropreneur
                    };

                    newAgro = MapAgroFromRecord(newAgro, record);
                    _agroRepository.Add(newAgro);
                    count++;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return count;
        }

        public int ImportFile(IFormFile file)
        {
            if (file.Length > 1024 * 1024)
            {
                throw new ApplicationException("file should be less than 1MB");
            }
            List<AGPImportModel> records = ReadFile(file);
            //_AGPImportValidation.Validate(records);
            int importCount = ImportAcceptedRecords(records.Where(r => r.IsAcceptable).ToList());
            return importCount;
        }

        public List<AGPImportModel> ReadFile(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) == ".csv")
            {
                return ReadCsvFile(file);
            }
            else if (Path.GetExtension(file.FileName) == ".xls")
            {
                return ReadXlsFile(file);
            }
            else if (Path.GetExtension(file.FileName) == ".xlsx")
            {
                return ReadXlsxFile(file);
            }
            else
            {
                throw new ApplicationException($"{Path.GetExtension(file.FileName)} extension is not supported");
            }
        }

        public FullResponse Register(AGPRegisterModel model)
        {
            try
            {
                var agropreneur = _mapper.Map<Agropreneur>(model);
                agropreneur.AgroOrganizationId = BasicOrganizations.Agropreneur;
                if (model.PhotoC != null)
                {
                    AddFile(agropreneur, model.PhotoC, MemberFileType.Photo, model.PhotoT.ToByte());
                }
                if (model.IdPhoto != null)
                {
                    AddFile(agropreneur, model.IdPhoto, MemberFileType.Identity);
                }
                if (!string.IsNullOrEmpty(model.MailingAddress))
                {
                    AddAddress(agropreneur, model.MailingAddress, AddressType.MailingAddress);
                }
                _agroRepository.Add(agropreneur);

                return FullResponse.SuccessWithId(agropreneur.Id.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }
        public DtReturn<AGPListModel> RetrieveForList(DtReceive dtReceive)
        {
            IEnumerable<Agropreneur> queryResult = RetrieveForList(dtReceive.Search.Value, dtReceive.Start, dtReceive.Length, out int total, out int filteredTotal);

            var dataResult = queryResult.Select(s => new AGPListModel
            {
                Gender = s.Gender.ToString(),
                Id = s.Id,
                IdentityNumber = s.IdentityNumber,
                Name = s.Name,
                Nationality = s.Nationality,
                IdentityType = s.IdentityType,
                PlotNo = s.PlotNo,
                HasPicture = s.MemberFiles.Any(f => f.FileType == MemberFileType.Photo),
                Company = s.Company,
                Status = s.State.ToString()
            }).ToList();

            var result = new DtReturn<AGPListModel>
            {
                Data = dataResult,
                RecordsTotal = total,
                RecordsFiltered = filteredTotal,
                Draw = dtReceive.Draw
            };
            return result;
        }

        public FullResponse Update(AGPRegisterModel model)
        {
            try
            {
                var agropreneur = _agroRepository.GetById(model.Id,false,a=>a.MemberFiles,a=>a.MemberAddresses);
                if (agropreneur == null) throw new ApplicationException("The Id is not correct");
                UpdateFile(agropreneur, model.PhotoC, MemberFileType.Photo, model.PhotoT.ToByte());
                UpdateFile(agropreneur, model.IdPhoto, MemberFileType.Identity);
                UpdateAddress(agropreneur, model.MailingAddress, AddressType.MailingAddress);
                agropreneur = _mapper.Map(model, agropreneur);
                _agroRepository.Update(agropreneur);

                return FullResponse.SuccessWithId(agropreneur.Id.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToFullResponse();
            }
        }

        internal bool IsDuplicate(AGPRegisterModel model)
        {
            if (model.Id == Guid.Empty)
            {
                return IsPlotNumberExist(model.PlotNo);
            }
            else
            {
                return IsPlotNumberExist(model.PlotNo, model.Id);
            }
        }

        private Agropreneur AddAddress(Agropreneur agropreneur, string address, AddressType addressType)
        {
            agropreneur.MemberAddresses.Add(new MemberAddress
            {
                Name = agropreneur.Name,
                Address = address,
                AddressType = addressType
            });

            return agropreneur;
        }

        private Agropreneur AddFile(Agropreneur agropreneur, IFormFile file, MemberFileType fileType, byte[] thumbnail = null)
        {
            var (docId, url) = _dmsService.InsertFile(file);
            agropreneur.MemberFiles.Add(new MemberFile
            {
                Name = file.Name,
                DmsId = docId,
                Thumbnail = thumbnail,
                DmsUrl = url,
                FileType = fileType
            });

            return agropreneur;
        }

        private Agropreneur MapAgroFromRecord(Agropreneur newAgro, AGPImportModel record)
        {
            newAgro.Name = record.FirstName + " " + record.LastName;
            newAgro.FirstName = record.FirstName;
            newAgro.LastName = record.LastName;
            newAgro.Gender = record.Gender == "F" ? Gender.Female : Gender.Male;
            newAgro.Nationality = record.Nationality;
            newAgro.IdentityType = record.IdentityType;
            newAgro.IdentityNumber = record.IdentityNumber;
            if (!string.IsNullOrEmpty(record.DateOfBirth))
            {
                newAgro.BirthDay = record.DateOfBirth.ToDate();
            }
            newAgro.BankName = record.CompanyBankAccount;
            newAgro.AccountNo = record.AccountNo;
            newAgro.Company = record.Company;
            newAgro.CompanyNo = record.SsmNo;
            newAgro.Designation = record.Designation;
            newAgro.Email = record.EmailAddress;
            newAgro.Industry = record.Industry;
            newAgro.Mobile = record.MobileNo;
            newAgro.PlotNo = record.PlotNo;
            newAgro.MemberAddresses.Add(new MemberAddress
            {
                Name = newAgro.Name,
                Address = record.MailingAddress,
                AddressType = AddressType.MailingAddress
            });
            newAgro.NatureOfBusiness = record.NatureOfBusiness;
            newAgro.PurposeOfTransaction = record.PurposeOfTransaction;
            if (!string.IsNullOrEmpty(record.EntryDate))
            {
                newAgro.EntryDate = record.EntryDate.ToDate();
            }
            if (!string.IsNullOrEmpty(record.VisaExpiry))
            {
                newAgro.VisaExpiry = record.VisaExpiry.ToDate();
            }
            if (!string.IsNullOrEmpty(record.PlksExpiry))
            {
                newAgro.PlksExpiry = record.PlksExpiry.ToDate();
            }
            newAgro.TermOfService = record.TermOfService;
            return newAgro;
        }

        private List<AGPImportModel> ReadCsvFile(IFormFile file)
        {
            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                //csvReader.Configuration.MissingFieldFound = null;
                //csvReader.Configuration.IgnoreBlankLines = true;
                //csvReader.Configuration.RegisterClassMap<AGPCsvFileMapper>();
                return csvReader.GetRecords<AGPImportModel>().ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Only CSV file is acceptable. you can use template file linked into page to fill and convert to CSV" + ex.Message);
            }
        }

        private List<AGPImportModel> ReadXlsFile(IFormFile file)
        {
            try
            {
                var workbook = new HSSFWorkbook(file.OpenReadStream());
                var sheet = workbook.GetSheetAt(0);
                var header = sheet.GetRow(0);
                var importer = new Npoi.Mapper.Mapper(workbook);
                var models = importer.Take<AGPImportModel>(0).ToList();
                return models.Select(m => m.Value).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<AGPImportModel> ReadXlsxFile(IFormFile file)
        {
            try
            {
                var workbook = new XSSFWorkbook(file.OpenReadStream());
                var importer = new Npoi.Mapper.Mapper(workbook);
                var models = importer.Take<AGPImportModel>(0).ToList();
                return models.Select(m => m.Value).Where(m => !string.IsNullOrEmpty(m.PlotNo?.ToString().Trim())).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateFile(Agropreneur agropreneur, IFormFile file, MemberFileType fileType, byte[] thumbnail = null)
        {
            if (file != null)
            {
                var memeberFile = agropreneur.MemberFiles.SingleOrDefault(f => f.FileType == fileType);
                if (memeberFile == null)
                {
                    var (docId, url) = _dmsService.InsertFile(file);
                    //model.PhotoDms = InsertPhotoIntoDms(string.Empty, model.Name, model.PhotoC.ToByte(), model.CreatedBy.ToString());
                    agropreneur.MemberFiles.Add(new MemberFile
                    {
                        Name = file.Name,
                        DmsId = docId,
                        Thumbnail = thumbnail,
                        DmsUrl = url,
                        FileType = fileType
                    });
                }
                else
                {
                    var (editedDocId, editedUrl) = _dmsService.UpdateFile(memeberFile.DmsId.Value, file);
                    memeberFile.Name = file.Name;
                    memeberFile.Thumbnail = thumbnail;
                    memeberFile.DmsId = editedDocId;
                    memeberFile.DmsUrl = editedUrl;
                }
            }
        }
        private Agropreneur UpdateAddress(Agropreneur agropreneur, string address, AddressType addressType)
        {
            if (!string.IsNullOrEmpty(address))
            {
                var currentAddress = agropreneur.MemberAddresses.SingleOrDefault(a => a.AddressType == addressType);
                if (currentAddress == null)
                {
                    agropreneur.MemberAddresses.Add(new MemberAddress
                    {
                        Name = agropreneur.Name,
                        Address = address,
                        AddressType = addressType
                    });
                }
                else
                {
                    currentAddress.Name = agropreneur.Name;
                    currentAddress.Address = address;
                }
            }

            return agropreneur;
        }
        public byte[] GetThumbnailFromDatabase(Guid agroId)
        {
            var file = _agroRepository.Context.MemberFiles.SingleOrDefault(f => f.MemberId == agroId && f.FileType == MemberFileType.Photo);
            return file?.Thumbnail;
        }
        public bool IsPlotNumberExist(string plotNo)
        {
            return _agroRepository.Entities.Any(a => a.PlotNo == plotNo);
        }
        private bool IsPlotNumberExist(string plotNo, Guid id)
        {
            return _agroRepository.Entities.Any(a => a.PlotNo == plotNo && a.Id != id);
        }
        private IEnumerable<Agropreneur> RetrieveForList(string searchValue, int start, int take, out int total, out int count)
        {
            total = _agroRepository.Entities.Count();
            var query = _agroRepository.FilteredEntities.AsNoTracking();
            query = query
                .Where(a =>
                            string.IsNullOrEmpty(searchValue)
                            || a.Name.Contains(searchValue)
                            || a.PlotNo.Contains(searchValue)
                            || a.IdentityNumber.Contains(searchValue));

            count = query.Count();
            query = query.OrderByDescending(a => a.Name);
            query = query.Skip(start).Take(take);
            query = query.Include(a => a.MemberFiles);

            return query;
        }
    }
}