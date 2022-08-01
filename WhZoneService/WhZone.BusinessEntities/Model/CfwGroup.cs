using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class CfwGroup : Base.Entity
    {
        public CfwGroup()
        {
            CfwUsergroups = new HashSet<CfwUsergroup>();
        }

        public int Grpid { get; set; }
        public string Name { get; set; } = null!;
        public int Options { get; set; }
        public int Lev { get; set; }
        public string? Note { get; set; }
        public string? Xmldata { get; set; }
        public string Cs { get; set; } = null!;

        public ICollection<CfwUsergroup> CfwUsergroups { get; set; }
    }
}
