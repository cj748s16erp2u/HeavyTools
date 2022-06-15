using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    class ColorTypeList : TypeListProvider
    {
        public static readonly string ID = typeof(ColorTypeList).FullName;
        public ColorTypeList()
            : base("olc_itemmaingroup.ColorType")
        {
        }
    }
}
