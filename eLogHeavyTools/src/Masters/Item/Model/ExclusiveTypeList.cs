using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Model
{
    class ExclusiveTypeList : TypeListProvider
    {
        public static readonly string ID = typeof(ExclusiveTypeList).FullName;
        public ExclusiveTypeList()
            : base("olc_itemmaingroup.ExclusiveType")
        {
        }
    }
}
