using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class CfwUsergroup : Base.Entity
    {
        public CfwUsergroup()
        {
        }

        public string Usrid { get; set; } = null!;
        public int Grpid { get; set; }
        public string Cs { get; set; } = null!;

        public virtual CfwUser CfwUser { get; set; } = null!;
        public virtual CfwGroup CfwGroup { get; set; } = null!;
    }
}
