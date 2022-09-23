using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvHeadSearchProvider3 : CodaInt.Base.Purchase.Pinv.PinvHeadSearchProvider2
    {

        #region IXmlObjectName

        protected static Type baseType = typeof(CodaInt.Base.Purchase.Pinv.PinvHeadSearchProvider2);

        protected override string GetSearchXmlFileName()
        {
            return $"{baseType.Namespace}.{baseType.Name}";
        }

        protected override string GetColorizerID()
        {
            return baseType.Name;
        }

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        #endregion

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            string sql = base.CreateQueryString(args, fmtonly);

            sql = sql.Replace("--$$morefields$$", @",
       x01.attacheddoc --$$morefields$$");

            sql = sql.Replace("--$$morejoins$$", @"
     cross apply ( select (case when exists(select 0 from ols_attachment a (nolock) where a.src = 'pinvhead' 
and a.refid = '{\""pinvid\"":'+convert(varchar(10), ph.pinvid)+'}' and a.delstat = 0) then 1 else 0 end) as attacheddoc) x01 --$$morejoins$$");

            return sql;
        }

    }
}