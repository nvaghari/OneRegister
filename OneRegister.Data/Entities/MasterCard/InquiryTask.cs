using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Data.Entities.MasterCard
{
    [Table(nameof(InquiryTask), Schema = SchemaNames.MasterCard)]
    public class InquiryTask : BaseEntity,IGenericEntity
    {
        public InquiryType InquiryType { get; set; }

        [StringLength(DataLength.SHORTNAME)]
        public string InquiryName { get; set; }

        [StringLength(DataLength.RefId)]
        public string RefId { get; set; }
        [StringLength(DataLength.RefId)]
        public string RefId2 { get; set; }

        [StringLength(DataLength.LONGNAME)]
        public string Source { get; set; }

        [StringLength(DataLength.MediumJson)]
        public string JsonValue { get; set; }

        [StringLength(DataLength.Remark)]
        public string Result { get; set; }

        [StringLength(DataLength.LONGNAME)]
        public string ErrorSource { get; set; }
        [StringLength(DataLength.LONGNAME)]
        public string ErrorCode { get; set; }
    }
}
