using System.Collections.Generic;
using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Masters.Partner
{
    public class EmployeeList3 : EmployeeList
    {
        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            var q = base.GetQuerySql(args);

            //base-ben nincs benne
            q = q.Replace("select e.empid", "select e.empid $$moreFields$$");
            q = q.Replace("ols_partner p (nolock) on p.partnid = e.partnid", "ols_partner p (nolock) on p.partnid = e.partnid $$moreJoins$$");

            if (args.ContainsKey("emptype"))
            {
                if (q.Contains(" where "))
                {
                    q = q.Replace(" where ", $" where e.type={Utils.SqlToString(args["emptype"])} or e.type = '-1' and ");
                }
                else
                {
                    q += $" where e.type={Utils.SqlToString(args["emptype"])} or e.type = '-1'";
                }
            }

            if (args.ContainsKey("cmpidfilter"))
            {
                q = q.Replace("$$moreJoins$$", " join ols_company cmp (nolock) on cmp.partnid = e.partnid $$moreJoins$$ ");

                if (q.Contains(" where "))
                {
                    q = q.Replace(" where ", $" where cmp.cmpid={Utils.SqlToString(args["cmpidfilter"])} and ");
                }
                else
                {
                    q += $" where cmp.cmpid={Utils.SqlToString(args["cmpidfilter"])}";
                }
            }

            q = q.Replace("$$moreFields$$", string.Empty);
            q = q.Replace("$$moreJoins$$", string.Empty);

            // nem tudom ez mi a *** csinal, miert kerul bele, de nem kell
            q = q.Replace("order by 2", string.Empty);

            return q;
        }
    }
}