using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    class PatternTypeList : TypeListProvider
    {
        public static readonly string ID = typeof(PatternTypeList).FullName;
        public PatternTypeList()
            : base("olc_itemmaingroup.PatternType")
        {
        }
    }
}
