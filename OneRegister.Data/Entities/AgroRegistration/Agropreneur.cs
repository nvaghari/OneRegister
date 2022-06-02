using OneRegister.Data.SuperEntities;
using System;
using System.ComponentModel.DataAnnotations;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.AgroRegistration
{
    public class Agropreneur : Member
    {
        [StringLength(DataLength.IDENTITY)]
        public string PlotNo { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string Company { get; set; }
        [StringLength(DataLength.IDENTITY)]
        public string CompanyNo { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string Industry { get; set; }
        [StringLength(DataLength.SHORTNAME)]
        public string BankName { get; set; }
        [StringLength(DataLength.IDENTITY)]
        public string AccountNo { get; set; }
        [StringLength(DataLength.LONGNAME)]
        public string NatureOfBusiness { get; set; }
        [StringLength(DataLength.LONGNAME)]
        public string PurposeOfTransaction { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? VisaExpiry { get; set; }
        public DateTime? PlksExpiry { get; set; }
        public int? TermOfService { get; set; }

        private Guid agroOrganizationId;
        public AgroOrganization AgroOrganization { get; set; }
        public Guid AgroOrganizationId
        {
            get
            {
                return agroOrganizationId;
            }
            set
            {
                OrganizationId = value;
                agroOrganizationId = value;
            }
        }
    }
}
