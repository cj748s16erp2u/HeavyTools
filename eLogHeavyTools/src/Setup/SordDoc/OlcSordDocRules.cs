using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.SordDoc
{
    internal class OlcSordDocRules : eLog.Base.Common.TypedBaseRuleSet<OlcSordDoc>
    {
        public OlcSordDocRules()
            : base(true, false)
        {
            ERules["sorddocid"].Mandatory = false;  // PK, FK
        }
    }
}
