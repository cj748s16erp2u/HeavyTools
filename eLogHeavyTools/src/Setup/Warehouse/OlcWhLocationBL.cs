using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationBL : DefaultBL1<OlcWhLocation, OlcWhLocationRules>
    {
        public static readonly string ID = typeof(OlcWhLocationBL).FullName;

        public static T New<T>()
            where T : OlcWhLocationBL
        {
            return ObjectFactory.New<T>();
        }

        public static OlcWhLocationBL New()
        {
            return New<OlcWhLocationBL>();
        }

        public OlcWhLocationBL() : base(DefaultBLFunctions.Basic)
        {
        }
    }
}
