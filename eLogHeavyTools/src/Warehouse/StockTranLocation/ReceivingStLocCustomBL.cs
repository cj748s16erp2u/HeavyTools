using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomBL : DefaultBL1<ReceivingStLocCustomDto, ReceivingStLocCustomRules>
    {
        public static readonly string ID = typeof(ReceivingStLocCustomBL).FullName;

        public static T New<T>() where T : ReceivingStLocCustomBL
        {
            return ObjectFactory.New<T>();
        }

        public static ReceivingStLocCustomBL New()
        {
            return New<ReceivingStLocCustomBL>();
        }

        protected ReceivingStLocCustomBL() { }        
    }
}
