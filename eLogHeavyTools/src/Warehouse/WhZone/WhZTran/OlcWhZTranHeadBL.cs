using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranHeadBL<TRule> : DefaultBL1<OlcWhZTranHead, TRule>
        where TRule: OlcWhZTranHeadRules
    {
        //public static readonly string ID = typeof(OlcWhZTranBL<TEntity>).FullName;

        public OlcWhZTranHeadBL()
        {
             
        }
    }
}
