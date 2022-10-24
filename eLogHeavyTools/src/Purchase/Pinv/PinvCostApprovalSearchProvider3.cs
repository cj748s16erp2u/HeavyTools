using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvCostApprovalSearchProvider3 : Base.Purchase.Pinv.PinvCostApprovalSearchProvider
    {
        public override string GetNamespaceName()
        {
            return typeof(Base.Purchase.Pinv.PinvCostApprovalSearchProvider).Namespace;
            // vagy this.GetType().BaseType.Namespace;
        }

        protected override string GetSearchXmlFileName()
        {
            return GetNamespaceName() + "." + typeof(Base.Purchase.Pinv.PinvCostApprovalSearchProvider).Name;
            // vagy GetNamespaceName() + "." + this.GetType().BaseType.Name;
        }

        protected override void ModifyQueryString(Dictionary<string, object> args, bool fmtonly, ref string query)
        {
            base.ModifyQueryString(args, fmtonly, ref query);

            query = query.Replace("--morejoins", @"
    cross apply ( select (case when exists(select 0 from ols_attachment a (nolock) where a.src = 'pinvhead' 
        and a.refid = '{\""pinvid\"":'+convert(varchar(10), h.pinvid)+'}' and a.delstat = 0) then 1 else 0 end) as attacheddoc) x01
    left outer join ofc_dochlelm fdhe (nolock) on fdhe.costlineid = cl.costlineid--morejoins")
            .Replace("--morefields", @",
        x01.attacheddoc, fdhe.el1, fdhe.el2, fdhe.el3, fdhe.el4, fdhe.el5, fdhe.el6, fdhe.el7, fdhe.el8--morefields");
        }

    }
}
