using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    class MaterialTypeList : TypeListProvider
    {
        public static readonly string ID = typeof(MaterialTypeList).FullName;
        public MaterialTypeList()
            : base("olc_itemmaingroup.MaterialType")
        {
        }
    }
}
