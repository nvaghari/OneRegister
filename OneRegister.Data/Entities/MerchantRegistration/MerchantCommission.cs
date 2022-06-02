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

namespace OneRegister.Data.Entities.MerchantRegistration
{
    [Table(nameof(MerchantCommission), Schema = SchemaNames.Merchant)]
    public class MerchantCommission : BaseEntity,IGenericEntity
    {
        [StringLength(DataLength.MediumJson)]
        public string JsonValue { get; set; }
        [StringLength(DataLength.Remark)]
        public string Remark { get; set; }

        //navigate properties
        public Guid MerchantId { get; set; }
        public Merchant Merchant { get; set; }
    }
}
