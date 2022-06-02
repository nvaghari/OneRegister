using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneRegister.Web.Models.Account
{
    public class NinoTreeViewModel
    {
        public NinoTreeViewModel()
        {
            Childs = new List<NinoTreeViewModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public List<NinoTreeViewModel> Childs { get; set; }
    }
}
