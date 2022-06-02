using OneRegister.Data.Contract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneRegister.Data.SuperEntities
{
    public enum OrganizationFileType
    {
        OtherDocument,
        CompanyRegistrationSearch,
        CertificateOfRegistration,
        IdentificationDocuments,
        BankStatement,
        ApplicantPhoto,
        CtosOfBoard,
        CommercialRate
    }

    [Table(nameof(OrganizationFile), Schema = SchemaNames.Base)]
    public class OrganizationFile : FileTableEntity,IOrganizedEntity
    {
        public OrganizationFileType FileType { get; set; }

        public Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
