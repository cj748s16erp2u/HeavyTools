using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodaInt.Base.Bookkeeping.Common;
using eLog.Base.Masters.Item;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Bookkeeping.Common
{
    public class UmbrellaPartnSelectionProvider3 : UmbrellaPartnSelectionProvider
    {
        static string m_queryString3 = @"
select ea.cmpcode, ea.elmlevel, ea.elmcode, ea.defaddr, ea.name, ea.country countryid, ea.postcode, ea.add1, ea.add2, ea.temporaryid, ea.tradernamecode,
     et.vat,
     eb.defbank, eb.acname, eb.sort + eb.acnum sort_acnum,
     ea.cmpcode + '|' + ea.elmcode + '|' + convert(varchar, ea.elmlevel) + '|' + convert(varchar, ea.temporaryid) umbrellaelementchoosekey
from oas_elmaddrlist as ea (nolock)
     join oas_elmtaxes as et (nolock) on et.cmpcode = ea.cmpcode and et.elmcode = ea.elmcode and et.elmlevel = ea.elmlevel and ea.temporaryid = et.temporaryid
     left join oas_elmbanklist as eb (nolock) on eb.cmpcode = ea.cmpcode and eb.elmcode = ea.elmcode and eb.elmlevel = ea.elmlevel and ea.temporaryid = eb.temporaryid
";

        public UmbrellaPartnSelectionProvider3()
            : base()
        {
            this.m_sql = m_queryString3;
        }

        protected override void GetQuery(Dictionary<string, object> args, out string sql, out string argsStr)
        {
            base.GetQuery(args, out sql, out argsStr);
        }
    }
}
