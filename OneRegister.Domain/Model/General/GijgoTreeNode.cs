using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Model.General
{
    public class GijgoTreeNode
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<GijgoTreeNode> Children { get; set; }
        public bool Checked { get; set; } = false;
    }
}
