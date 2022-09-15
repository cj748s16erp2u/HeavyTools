using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common.xcval
{
    public class OfcXcvalBL : OfcXcvalBL<OfcXcval, OfcXcvalRules>
    {
        public static readonly string ID = typeof(OfcXcvalBL).FullName;

        public static T New<T>()
            where T : OfcXcvalBL
        {
            return ObjectFactory.New<T>();
        }

        public static OfcXcvalBL New()
        {
            return New<OfcXcvalBL>();
        }

        public OfcXcvalBL() : base(DefaultBLFunctions.Basic)
        {
        }
    }

    public abstract class OfcXcvalBL<TEntity, TRule> : DefaultBL1<TEntity, TRule>
        where TEntity : OfcXcval
        where TRule : OfcXcvalRules<TEntity>
    {
        protected OfcXcvalBL(DefaultBLFunctions functions = DefaultBLFunctions.Basic) : base(functions)
        {
        }
    }
}
