using System;
namespace OneRegister.Domain.Model.AgropreneurRegistration
{
    public class AGPListModel
    {
        public Guid Id { get; set; }
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public bool HasPicture { get; set; }
        public string IdentityType { get; set; }
        public string PlotNo { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
    }
}
