using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvAssignmentSearchProvider3 : Base.Purchase.Pinv.PinvAssignmentSearchProvider
    {
        public override string GetNamespaceName()
        {
            return this.GetType().BaseType.Namespace;
        }

        protected override string GetSearchXmlFileName()
        {
            return GetNamespaceName() + "." + this.GetType().BaseType.Name;
        }

        protected override void ModifyQueryString(Dictionary<string, object> args, bool fmtonly, ref string query3)
        {
            base.ModifyQueryString(args, fmtonly, ref query3);

            query3 = query3.Replace("--$$morefields", @",
     dhe.el1, dhe.el2, dhe.el3, dhe.el4, dhe.el5, dhe.el6, dhe.el7, dhe.el8,
     ocl.othtrlinedocid, otrd.name othtrlinedocname--$$morefields");

            query3 = query3.Replace("--$$morejoins", @"
     left outer join ofc_dochlelm dhe (nolock) on dhe.costlineid = cl.costlineid
     left outer join olc_costline ocl (nolock) on ocl.costlineid = cl.costlineid
     left outer join ofc_othtrlinedoc otrd (nolock) on otrd.othtrlinedocid = ocl.othtrlinedocid--$$morejoins");
        }
    }
}
