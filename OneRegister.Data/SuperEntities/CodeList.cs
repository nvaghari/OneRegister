using OneRegister.Data.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.SuperEntities
{
    [Table(nameof(CodeList), Schema = SchemaNames.Application)]
    public class CodeList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int Key { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }

    }
}
