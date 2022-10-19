using eLog.Base.Setup.SordDoc;
using eLog.Base.Setup.StDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.SordDoc
{
    internal class SordDocSearchProvider3 : SordDocSearchProvider
    {
        public SordDocSearchProvider3()
        {
            QueryString = @"select d.*, c.frameordersorddocid,
convert(varchar(2000), null) as cmpcodes_,
(select top 1 lastnum 
	from ols_sordnum n (nolock) 
	join ols_company c (nolock) on c.cmpid = n.cmpid 
   where (d.cmpcodes = $$cmpall$$ or d.cmpcodes like '%' + c.cmpcode + '%') 
     and n.cmpid in ($$cmpids$$) 
	 and n.sorddocid = d.sorddocid 
	 and n.delstat = 0 
	 and $$date$$ between n.startdate and n.enddate) as lastnum 
from ols_sorddoc d (nolock)
left join olc_sorddoc c (nolock) on c.sorddocid=d.sorddocid";
        }
    }
}
