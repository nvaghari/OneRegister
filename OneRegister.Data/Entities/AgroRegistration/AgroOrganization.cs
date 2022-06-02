using OneRegister.Data.SuperEntities;
using System.Collections.Generic;

namespace OneRegister.Data.Entities.AgroRegistration
{
    public class AgroOrganization : Organization
    {
        public virtual ICollection<Agropreneur> Agropreneurs { get; set; }
    }
}
