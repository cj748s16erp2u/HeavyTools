using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnCmpRules : eLog.Base.Common.TypedBaseRuleSet<OlcPartnCmp>
    {
        public OlcPartnCmpRules() : base(true, false)
        {
            this.ERules[OlcPartnCmp.FieldPartnid.Name].Mandatory = false;
        }
    }
}
