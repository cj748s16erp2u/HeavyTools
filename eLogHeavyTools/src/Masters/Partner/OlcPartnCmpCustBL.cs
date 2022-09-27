using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.BL;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnCmpCustBL : eProjectWeb.Framework.BL.DefaultBL1<OlcPartnCmp, OlcPartnCmpRules>
    {
        public static readonly string ID = typeof(OlcPartnCmpCustBL).FullName;

        public static OlcPartnCmpCustBL New()
        {
            return eProjectWeb.Framework.ObjectFactory.New<OlcPartnCmpCustBL>();
        }

        protected OlcPartnCmpCustBL() : base()
        {
        }
    }

}