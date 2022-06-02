using System;
using System.Collections.Generic;

namespace OneRegister.Domain.Model.General
{
    public class OrgNode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public List<OrgNode> Descendants { get; set; }
    }
}
