using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnAddrRules : eLog.Base.Common.TypedBaseRuleSet<OlcPartnAddr>
    {
        public OlcPartnAddrRules() : base(true, false)
        {
            this.ERules[OlcPartnAddr.FieldAddrid.Name].Mandatory = false;
        }
    }
}
