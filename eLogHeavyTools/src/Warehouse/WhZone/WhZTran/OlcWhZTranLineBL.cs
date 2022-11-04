using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranLineBL<TRule> : DefaultBL1<OlcWhZTranLine, TRule>
         where TRule : OlcWhZTranLineRules
    {
        //public static readonly string ID = typeof(OlcWhZTranBL<TEntity>).FullName;

        public OlcWhZTranLineBL()
        {

        }
    }
}
