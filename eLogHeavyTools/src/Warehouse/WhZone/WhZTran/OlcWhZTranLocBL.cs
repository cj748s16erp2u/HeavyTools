using eProjectWeb.Framework.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranLocBL<TRule> : DefaultBL1<OlcWhZTranLoc, TRule>
         where TRule : OlcWhZTranLocRules
    {
        //public static readonly string ID = typeof(OlcWhZTranBL<TEntity>).FullName;

        public OlcWhZTranLocBL()
        {

        }
    }
}
