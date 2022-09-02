using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common.xcval
{
    public class OfcXcvalRules : OfcXcvalRules<OfcXcval>
    {
        public OfcXcvalRules() : base(true, false)
        {
        }
    }

    public abstract class OfcXcvalRules<T> : eLog.Base.Common.TypedBaseRuleSet<T>
        where T : OfcXcval
    {
        public OfcXcvalRules() : base(true, false)
        {
        }

        public OfcXcvalRules(bool useStdRules, bool checkPK = false) : base(useStdRules, checkPK)
        {
        }
    }
}
