using eLog.Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranLineRules : TypedBaseRuleSet<OlcWhZTranLine>
    {
        public OlcWhZTranLineRules() : base(true)
        {

        }

        protected static List<string> m_closedFieldsModifyable;
        public static List<string> ClosedFieldsModifyable
        {
            get { return m_closedFieldsModifyable; }
        }
    }
}
