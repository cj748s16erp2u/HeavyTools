using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcEmployeeRules : eLog.Base.Common.TypedBaseRuleSet<OlcEmployee>
    {
        public OlcEmployeeRules() : base(true, false)
        {
            this.ERules[OlcEmployee.FieldEmpid.Name].Mandatory = false;
        }
    }
}
