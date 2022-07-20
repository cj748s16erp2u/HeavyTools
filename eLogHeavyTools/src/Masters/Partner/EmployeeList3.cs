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

            if (args.ContainsKey("emptype"))
            {
                if (q.Contains(" where "))
                {
                    q = q.Replace(" where ", $" where e.type={Utils.SqlToString(args["emptype"])} and ");
                }
                else
                {
                    q += $" where e.type={Utils.SqlToString(args["emptype"])}";
                }

                q = q.Replace("$$moreFields$$", ", e.type emptype");
            }

            q = q.Replace("$$moreFields$$", string.Empty);

            return q;
        }
    }
}