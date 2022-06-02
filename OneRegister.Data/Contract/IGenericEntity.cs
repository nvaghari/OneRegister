using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Contract
{
    public interface IGenericEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        DateTime CreatedAt { get; set; }

        Guid? CreatedBy { get; set; }
        DateTime ModifiedAt { get; set; }

        Guid? ModifiedBy { get; set; }
        StateOfEntity State { get; set; }
    }
}
