using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Data.Contract
{
    public interface IOrganizationEntity : IGenericEntity
    {
        int Sequencer { get; set; }
        string Path { get; set; }
        Guid? ParentId { get; set; }
    }
}
